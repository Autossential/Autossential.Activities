using Autossential.Activities.Properties;
using System.Activities.DesignViewModels;

namespace Autossential.Activities.ViewModels.Workflow
{
    internal class IterateViewModel : BaseViewModel
    {
        public IterateViewModel(IDesignServices services) : base(services) { }

        public DesignInArgument<int> Iterations { get; set; }
        public DesignProperty<bool> Reverse { get; set; }

        protected override void InitializeModel()
        {
            base.InitializeModel();
            PersistValuesChangedDuringInit();
            int orderIndex = 0;
            Iterations.IsPrincipal = true;
            Iterations.IsRequired = true;
            Iterations.Category = Resources.Input_Category;
            Iterations.DisplayName = Resources.Iterate_Iterations_DisplayName;
            Iterations.Placeholder = Resources.Iterate_Iterations_Description;
            Iterations.Tooltip = Resources.Iterate_Iterations_Description;
            Iterations.OrderIndex = orderIndex++;

            Reverse.IsPrincipal = false;
            Reverse.IsRequired = false;
            Reverse.Category = Resources.Options_Category;
            Reverse.DisplayName = Resources.Iterate_Reverse_DisplayName;
            Reverse.Placeholder = Resources.Iterate_Reverse_Description;
            Reverse.Tooltip = Resources.Iterate_Reverse_Description;
            Reverse.OrderIndex = orderIndex++;
        }
    }
}
