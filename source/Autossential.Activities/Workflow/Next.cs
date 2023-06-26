using Autossential.Activities.Properties;
using Autossential.Shared.Activities.Constraints;
using System.Activities;

namespace Autossential.Activities
{
    public sealed class Next : NativeActivity
    {
        protected override bool CanInduceIdle => true;

        public InArgument<bool> Condition { get; set; }

        public Next()
        {
            Constraints.Add(
                ActivityConstraints.CreateConstraint<Next>(activity => activity is Iterate || activity is TimeLoop,
                Resources.Validation_ScopesErrorFormat($"({nameof(Iterate)} or {nameof(TimeLoop)})")));
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

        public const string BOOKMARK_NAME = "Autossential_NextBookmark";
    }
}