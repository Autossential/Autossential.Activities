using System.Activities;
using System.Activities.Statements;
using System.ComponentModel;
using System.Diagnostics;

namespace Autossential.Activities
{
    public sealed class TimeLoop : NativeActivity<bool>
    {
        public InArgument<TimeSpan> Timeout { get; set; }
        public InArgument<double> IntervalSeconds { get; set; }
        public OutArgument<int> IterationIndex { get; set; }

        private TimeSpan _timeout;
        private double _intervalSeconds;
        private int _iterationIndex;
        private Stopwatch _sw;
        private bool _stop;

        [Browsable(false)]
        public ActivityAction Body { get; set; }
        protected override bool CanInduceIdle => true;

        public TimeLoop()
        {
            Body = new ActivityAction
            {
                Handler = new Sequence { DisplayName = "Do" }
            };
        }

        protected override void Execute(NativeActivityContext context)
        {
            _timeout = Timeout.Get(context);
            _intervalSeconds = IntervalSeconds.Get(context);
            _iterationIndex = IterationIndex.Get(context);
            _sw = Stopwatch.StartNew();

            CreateExitBookmark(context);

            ExecuteInternal(context);
        }

        private void ExecuteInternal(NativeActivityContext context)
        {
            if (context.IsCancellationRequested)
            {
                context.MarkCanceled();
                return;
            }

            var timeout = _sw.Elapsed > _timeout;
            if (timeout || _stop)
            {
                Result.Set(context, timeout);
                return;
            }

            IterationIndex.Set(context, _iterationIndex);
            _iterationIndex++;
            context.ScheduleAction(Body, OnIterationCompleted, OnIterationFaulted);
        }

        private void OnIterationFaulted(NativeActivityFaultContext faultContext, Exception propagatedException, ActivityInstance propagatedFrom)
        {
            faultContext.CancelChildren();
            throw propagatedException;
        }

        private void OnIterationCompleted(NativeActivityContext context, ActivityInstance completedInstance)
        {
            if (_intervalSeconds > 0 && !_stop)
                Thread.Sleep(TimeSpan.FromSeconds(_intervalSeconds));

            ExecuteInternal(context);
        }

        private void CreateExitBookmark(NativeActivityContext context)
        {
            var exitBookmark = context.CreateBookmark(OnExit, BookmarkOptions.NonBlocking);
            context.Properties.Add(Exit.BOOKMARK_NAME, exitBookmark);
        }

        private void OnExit(NativeActivityContext context, Bookmark bookmark, object value)
        {
            _stop = true;
            context.CancelChildren();
            if (value is Bookmark b)
                context.ResumeBookmark(b, value);
        }
    }
}