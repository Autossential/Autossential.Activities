using Autossential.Activities.Properties;
using System.Activities.DesignViewModels;

namespace Autossential.Activities.ViewModels.Misc
{
    public class TerminateProcessViewModel : BaseViewModel
    {
        public TerminateProcessViewModel(IDesignServices services) : base(services)
        {
        }

        public DesignInArgument Timeout { get; set; }
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

            Timeout.IsRequired = false;
            Timeout.IsPrincipal = false;
            Timeout.Category = Resources.Input_Category;
            Timeout.DisplayName = Resources.Common_Timeout_DisplayName;
            Timeout.Tooltip = Resources.Common_Timeout_Description;
            Timeout.OrderIndex = orderIndex++;
        }
    }
}
