using Autossential.Activities.Helpers;
using Autossential.Activities.Properties;
using System.Activities;

namespace Autossential.Activities
{
    public sealed class Exit : NativeActivity
    {
        public const string BOOKMARK_NAME = "AutossentialExitBookmark";
        protected override bool CanInduceIdle => true;
        public InArgument<bool> Condition { get; set; }
        public Exit()
        {
            Constraints.Add(ActivityConstraints.ParentValidationConstraint<Exit>(parent => parent is Container || parent is TimeLoop, Resources.Exit_ErrorMsg_InvalidParent));
        }
        protected override void Execute(NativeActivityContext context)
        {
            if (Condition.Expression != null && !Condition.Get(context))
                return;

            var bookmark = (Bookmark)context.Properties.Find(BOOKMARK_NAME);
            if (bookmark != null)
            {
                var value = context.CreateBookmark();
                _ = context.ResumeBookmark(bookmark, value);
            }
        }
    }
}