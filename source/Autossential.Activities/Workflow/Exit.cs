using Autossential.Activities.Properties;
using Autossential.Shared.Activities.Constraints;
using System.Activities;

namespace Autossential.Activities
{
    public sealed class Exit : NativeActivity
    {
        protected override bool CanInduceIdle => true;

        public InArgument<bool> Condition { get; set; }

        public Exit()
        {
            Constraints.Add(ActivityConstraints.CreateConstraint<Exit>(activity => activity is Container || activity is Iterate,
                Resources.Validation_ScopesErrorFormat($"({nameof(Container)} or {nameof(Iterate)})")));
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

        public const string BOOKMARK_NAME = "Autossential_ExitBookmark";
    }
}