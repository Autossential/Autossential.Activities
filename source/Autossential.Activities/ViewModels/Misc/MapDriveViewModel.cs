using Autossential.Activities.Properties;
using System.Activities.DesignViewModels;
using System.Activities.ViewModels;
using System.Net;

namespace Autossential.Activities.ViewModels.Misc
{
    internal class MapDriveViewModel : BaseViewModel
    {
        public MapDriveViewModel(IDesignServices services) : base(services)
        {
        }

        public DesignInArgument<string> SharedDrivePath { get; set; }
        public DesignInArgument<NetworkCredential> Credential { get; set; }
        public DesignOutArgument<string> MappedDrive { get; set; }
        public DesignInArgument<bool> Force { get; set; }
        public DesignInArgument<string> DriveLetter { get; set; }
        public DesignOutArgument<int> ResponseCode { get; set; }
        public DesignOutArgument<string> ResponseMessage { get; set; }

        protected override void InitializeModel()
        {
            base.InitializeModel();
            PersistValuesChangedDuringInit();

            int orderIndex = 0;

            SharedDrivePath.IsRequired = true;
            SharedDrivePath.IsPrincipal = true;
            SharedDrivePath.Category = Resources.Input_Category;
            SharedDrivePath.DisplayName = Resources.MapDrive_SharedDrivePath_DisplayName;
            SharedDrivePath.Tooltip = Resources.MapDrive_SharedDrivePath_Description;
            SharedDrivePath.OrderIndex = orderIndex++;

            DriveLetter.IsRequired = false;
            DriveLetter.IsPrincipal = false;
            DriveLetter.Category = Resources.Input_Category;
            DriveLetter.DisplayName = Resources.MapDrive_DriveLetter_DisplayName;
            DriveLetter.Tooltip = Resources.MapDrive_DriveLetter_Description;
            DriveLetter.OrderIndex = orderIndex++;

            Credential.IsRequired = false;
            Credential.IsPrincipal = false;
            Credential.Category = Resources.Input_Category;
            Credential.DisplayName = Resources.MapDrive_Credential_DisplayName;
            Credential.Tooltip = Resources.MapDrive_Credential_Description;
            Credential.OrderIndex = orderIndex++;

            Force.IsRequired = false;
            Force.IsPrincipal = false;
            Force.Category = Resources.Options_Category;
            Force.DisplayName = Resources.MapDrive_Force_DisplayName;
            Force.Tooltip = Resources.MapDrive_Force_Description;
            Force.OrderIndex = orderIndex++;

            MappedDrive.IsPrincipal = false;
            MappedDrive.Category = Resources.Output_Category;
            MappedDrive.DisplayName = Resources.MapDrive_MappedDrive_DisplayName;
            MappedDrive.Tooltip = Resources.MapDrive_MappedDrive_Description;
            MappedDrive.OrderIndex = orderIndex++;

            ResponseCode.IsPrincipal = false;
            ResponseCode.Category = Resources.Output_Category;
            ResponseCode.DisplayName = Resources.MapDrive_ResponseCode_DisplayName;
            ResponseCode.Tooltip = Resources.MapDrive_ResponseCode_Description;
            ResponseCode.OrderIndex = orderIndex++;

            ResponseMessage.IsPrincipal = false;
            ResponseMessage.Category = Resources.Output_Category;
            ResponseMessage.DisplayName = Resources.MapDrive_ResponseMessage_DisplayName;
            ResponseMessage.Tooltip = Resources.MapDrive_ResponseMessage_Description;
            ResponseMessage.OrderIndex = orderIndex++;

            if (IsWidgetSupported(ViewModelWidgetType.Toggle))
            {
                Force.Widget = new DefaultWidget
                {
                    Type = ViewModelWidgetType.Toggle
                };
            }
        }
    }
}
