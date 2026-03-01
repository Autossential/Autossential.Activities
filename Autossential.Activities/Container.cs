using System.Activities;
using System.Activities.Statements;
using System.ComponentModel;

namespace Autossential.Activities
{
    public sealed class Container : NativeActivity
    {

        [Browsable(false)]
        public ActivityAction Body { get; set; }

        protected override bool CanInduceIdle => true;

        public Container()
        {
            Body = new ActivityAction
            {
                Handler = new Sequence { DisplayName = "Do" }
            };
        }
        protected override void Execute(NativeActivityContext context)
        {
            CreateExitBookmark(context);
            context.ScheduleAction(Body);
        }

        private void CreateExitBookmark(NativeActivityContext context)
        {
            var exitBookmark = context.CreateBookmark(OnExit, BookmarkOptions.NonBlocking);
            context.Properties.Add(Exit.BOOKMARK_NAME, exitBookmark);
        }

        private void OnExit(NativeActivityContext context, Bookmark bookmark, object value)
        {
            context.CancelChildren();
            if (value is Bookmark b)
                context.ResumeBookmark(b, value);
        }
    }
}