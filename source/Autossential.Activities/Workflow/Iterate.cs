using Autossential.Activities.Properties;
using Autossential.Shared.Activities.Base;

using System;
using System.Activities;
using System.Activities.Expressions;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Autossential.Activities
{
    public sealed class Iterate : ScopeActivity
    {
        private Collection<Variable> _variables;

        [Browsable(false)]
        public Collection<Variable> Variables
        {
            get
            {
                if (_variables == null)
                    _variables = new Collection<Variable>();

                return _variables;
            }
        }

        public InArgument<int> Iterations { get; set; }

        public OutArgument<int> Index { get; set; }

        public bool Reverse { get; set; }

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
        }

        protected override void Execute(NativeActivityContext context)
        {
            var exitBookmark = context.CreateBookmark(OnExit, BookmarkOptions.NonBlocking);
            context.Properties.Add(Exit.BOOKMARK_NAME, exitBookmark);

            var nextBookmark = context.CreateBookmark(OnNext, BookmarkOptions.MultipleResume | BookmarkOptions.NonBlocking);
            context.Properties.Add(Next.BOOKMARK_NAME, nextBookmark);

            _iterations = Iterations.Get(context);
            if (_iterations <= 0)
                throw new InvalidOperationException(Resources.Iterate_ErrorMsg_IterationsMinValue);

            ExecuteNext(context);
        }

        private void ExecuteNext(NativeActivityContext context)
        {
            Index.Set(context, Reverse ? _iterations - 1 - _index : _index);
            context.ScheduleAction(Body, OnIterateCompleted);
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
            context.CancelChildren();
            _break = true;
            if (value is Bookmark b)
                context.ResumeBookmark(b, value);
        }

        private int _iterations = 0;
        private int _index = 0;
        private bool _break;
    }
}