using Autossential.Activities.Properties;
using System.Activities.DesignViewModels;

namespace Autossential.Activities.ViewModels.Misc
{
    internal class TerminateProcessViewModel : BaseViewModel
    {
        public TerminateProcessViewModel(IDesignServices services) : base(services)
        {
        }

        public DesignInArgument<double> TimeoutSeconds { get; set; }
        public DesignInArgument ProcessName { get; set; }

        protected override void InitializeModel()
        {
            base.InitializeModel();
            PersistValuesChangedDuringInit();

            int orderIndex = 0;

            ProcessName.IsRequired = true;
            ProcessName.IsPrincipal = true;
            ProcessName.Category = Resources.Input_Category;
            ProcessName.DisplayName = Resources.TerminateProcess_ProcessName_DisplayName;
            ProcessName.Tooltip = Resources.TerminateProcess_ProcessName_Description;
            ProcessName.OrderIndex = orderIndex++;

            TimeoutSeconds.IsRequired = false;
            TimeoutSeconds.IsPrincipal = false;
            TimeoutSeconds.Category = Resources.Input_Category;
            TimeoutSeconds.DisplayName = Resources.Common_TimeoutSeconds_DisplayName;
            TimeoutSeconds.Tooltip = Resources.Common_TimeoutSeconds_Description;
            TimeoutSeconds.OrderIndex = orderIndex++;
        }
    }
}
