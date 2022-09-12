using System.Activities;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Autossential.Activities
{
    public sealed class Container : NativeActivity
    {
        private Collection<Activity> _activities;
        private Collection<Variable> _variables;
        private bool _exitRequested;
        private readonly Variable<int> _lastIndexHint;
        private readonly CompletionCallback _onCompletionCallback;
        protected override bool CanInduceIdle => true;

        public Container()
        {
            _lastIndexHint = new Variable<int>();
            _onCompletionCallback = new CompletionCallback(InternalExecute);
        }

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

        [Browsable(false)]
        public Collection<Activity> Activities
        {
            get
            {
                if (_activities == null)
                    _activities = new Collection<Activity>();

                return _activities;
            }
        }

        protected override void CacheMetadata(NativeActivityMetadata metadata)
        {
            metadata.SetChildrenCollection(Activities);
            metadata.SetVariablesCollection(Variables);
            metadata.AddImplementationVariable(_lastIndexHint);
        }

        protected override void Execute(NativeActivityContext context)
        {
            _exitRequested = false;

            if (Activities.Count > 0)
            {
                var next = Activities[0];

                var exit = context.CreateBookmark(OnExit, BookmarkOptions.NonBlocking);
                context.Properties.Add(Exit.BOOKMARK_NAME, exit);

                context.ScheduleActivity(next, _onCompletionCallback);
            }
        }

        private void OnExit(NativeActivityContext context, Bookmark bookmark, object value)
        {
            context.CancelChildren();
            _exitRequested = true;
        }

        private void InternalExecute(NativeActivityContext context, ActivityInstance completedInstance)
        {
            if (_exitRequested) return;

            var nextIndex = _lastIndexHint.Get(context) + 1;
            if (nextIndex == Activities.Count) return;

            context.ScheduleActivity(Activities[nextIndex], _onCompletionCallback);
            _lastIndexHint.Set(context, nextIndex);
        }
    }
}