using Autossential.Activities.Properties;
using Autossential.Shared.Activities.Base;
using System;
using System.Activities;
using System.Threading;

namespace Autossential.Activities
{
    public sealed class TimeLoop : ScopeActivity
    {
        private TimeSpan _timer;
        private TimeSpan _interval;
        private int _index;
        private bool _stop;
        private System.Diagnostics.Stopwatch _sw;

        public InArgument<TimeSpan> Timer { get; set; }
        public InArgument<bool> ExitOnException { get; set; }
        public InArgument<bool> PropagateException { get; set; }
        public InArgument<TimeSpan> LoopInterval { get; set; }
        public OutArgument<Exception> OutputException { get; set; }
        public OutArgument<int> Index { get; set; }

        protected override void CacheMetadata(NativeActivityMetadata metadata)
        {
            if (Timer == null)
                metadata.AddValidationError(Resources.Validation_ValueErrorFormat(nameof(Timer)));

            base.CacheMetadata(metadata);
        }

        protected override void Execute(NativeActivityContext context)
        {
            _timer = Timer.Get(context);
            if (_timer <= TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(Timer), _timer, "The value need to be greater than zero");

            _interval = LoopInterval.Get(context);
            _index = Index.Get(context);
            _stop = false;

            CreateBookmarks(context);

            _sw = System.Diagnostics.Stopwatch.StartNew();
            ExecuteNext(context);
        }

        private void ApplyDelay()
        {
            if (_interval > TimeSpan.Zero && !_stop)
                Thread.Sleep(_interval);
        }

        private void ExecuteNext(NativeActivityContext context)
        {
            if (context.IsCancellationRequested)
            {
                context.MarkCanceled();
                return;
            }

            if (_sw.Elapsed > _timer || _stop)
                return;

            Index.Set(context, _index);
            context.ScheduleAction(Body, OnIterationCompleted, OnIterationFaulted);
            _index++;
        }

        private void OnIterationCompleted(NativeActivityContext context, ActivityInstance completedInstance)
        {
            ApplyDelay();
            ExecuteNext(context);
        }

        private void OnIterationFaulted(NativeActivityFaultContext faultContext, Exception propagatedException, ActivityInstance propagatedFrom)
        {
            faultContext.HandleFault();
            faultContext.CancelChild(propagatedFrom);

            if (ExitOnException.Get(faultContext))
            {
                _stop = true;
                faultContext.CancelChildren();
                OutputException.Set(faultContext, propagatedException);
                if (PropagateException.Get(faultContext))
                    throw propagatedException;
            }
        }

        private void CreateBookmarks(NativeActivityContext context)
        {
            var exitBookmark = context.CreateBookmark(OnExit, BookmarkOptions.NonBlocking);
            context.Properties.Add(Exit.BOOKMARK_NAME, exitBookmark);

            var nextBookmark = context.CreateBookmark(OnNext, BookmarkOptions.MultipleResume | BookmarkOptions.NonBlocking);
            context.Properties.Add(Next.BOOKMARK_NAME, nextBookmark);
        }

        private void OnNext(NativeActivityContext context, Bookmark bookmark, object value)
        {
            context.CancelChildren();
            if (value is Bookmark b)
                context.ResumeBookmark(b, value);
        }

        private void OnExit(NativeActivityContext context, Bookmark bookmark, object value)
        {
            _stop = true;
            OnNext(context, bookmark, value);
        }
    }
}