using Autossential.Activities.Properties;
using System.Activities.DesignViewModels;

namespace Autossential.Activities.ViewModels.Misc
{
    public class UnmapDriveViewModel : BaseViewModel
    {
        public UnmapDriveViewModel(IDesignServices services) : base(services)
        {
        }

        public DesignInArgument<string> DriveLetter { get; set; }
        public DesignOutArgument<int> ResponseCode { get; set; }
        public DesignOutArgument<string> ResponseMessage { get; set; }

        protected override void InitializeModel()
        {
            base.InitializeModel();
            PersistValuesChangedDuringInit();

            int orderIndex = 0;

            DriveLetter.IsRequired = true;
            DriveLetter.IsPrincipal = true;
            DriveLetter.Category = Resources.Input_Category;
            DriveLetter.DisplayName = Resources.UnmapDrive_DriveLetter_DisplayName;
            DriveLetter.Tooltip = Resources.UnmapDrive_DriveLetter_Description;
            DriveLetter.OrderIndex = orderIndex++;

            ResponseCode.IsPrincipal = false;
            ResponseCode.Category = Resources.Output_Category;
            ResponseCode.DisplayName = Resources.UnmapDrive_ResponseCode_DisplayName;
            ResponseCode.Tooltip = Resources.UnmapDrive_ResponseCode_Description;
            ResponseCode.OrderIndex = orderIndex++;

            ResponseMessage.IsPrincipal = false;
            ResponseMessage.Category = Resources.Output_Category;
            ResponseMessage.DisplayName = Resources.UnmapDrive_ResponseMessage_DisplayName;
            ResponseMessage.Tooltip = Resources.UnmapDrive_ResponseMessage_Description;
            ResponseMessage.OrderIndex = orderIndex++;
        }
    }
}
