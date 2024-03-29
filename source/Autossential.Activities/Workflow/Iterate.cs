﻿using Autossential.Activities.Properties;
using Autossential.Shared.Activities.Base;
using System;
using System.Activities;
using System.Activities.Expressions;

namespace Autossential.Activities
{
    public sealed class Iterate : ScopeActivity<int>
    {
        private int _iterations;
        private int _index;
        private bool _break;

        public InArgument<int> Iterations { get; set; }

        public bool Reverse { get; set; }

        protected override void InitializeBody()
        {
            Index = new Variable<int>();

            base.InitializeBody();
            Body.Argument = new DelegateInArgument<int>("index");
        }
        protected override void CacheMetadata(NativeActivityMetadata metadata)
        {
            base.CacheMetadata(metadata);

            if (Iterations == null)
            {
                metadata.AddValidationError(Resources.Validation_ValueErrorFormat(nameof(Iterations)));
            }
            else if (Iterations.Expression is Literal<int> expr && expr.Value < 1)
            {
                metadata.AddValidationError(Resources.Iterate_ErrorMsg_IterationsMinValue);
            }
            metadata.AddImplementationVariable(Index);
        }

        protected override void Execute(NativeActivityContext context)
        {
            _iterations = 0;
            _break = false;
            _index = 0;

            CreateBookmarks(context);

            _iterations = Iterations.Get(context);
            if (_iterations <= 0)
                throw new InvalidOperationException(Resources.Iterate_ErrorMsg_IterationsMinValue);

            ExecuteNext(context);
        }

        private void CreateBookmarks(NativeActivityContext context)
        {
            var exitBookmark = context.CreateBookmark(OnExit, BookmarkOptions.NonBlocking);
            context.Properties.Add(Exit.BOOKMARK_NAME, exitBookmark);

            var nextBookmark = context.CreateBookmark(OnNext, BookmarkOptions.MultipleResume | BookmarkOptions.NonBlocking);
            context.Properties.Add(Next.BOOKMARK_NAME, nextBookmark);
        }

        private Variable<int> Index { get; set; }

        private void ExecuteNext(NativeActivityContext context)
        {
            var value = Reverse ? _iterations - 1 - _index : _index;
            Index.Set(context, value);
            context.ScheduleAction(Body, value, OnIterateCompleted);
        }

        private void OnIterateCompleted(NativeActivityContext context, ActivityInstance completedInstance)
        {
            if (context.IsCancellationRequested)
            {
                context.MarkCanceled();
                _break = true;
            }

            if (!_break && ++_index < _iterations)
                ExecuteNext(context);
        }

        private void OnNext(NativeActivityContext context, Bookmark bookmark, object value)
        {
            context.CancelChildren();
            if (value is Bookmark b)
                context.ResumeBookmark(b, value);
        }

        private void OnExit(NativeActivityContext context, Bookmark bookmark, object value)
        {
            _break = true;
            OnNext(context, bookmark, value);
        }
    }
}