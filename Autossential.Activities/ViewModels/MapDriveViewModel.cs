using Autossential.Activities.Base;
using Autossential.Activities.Properties;
using System.Activities.DesignViewModels;
using System.Activities.ViewModels;
using System.Net;

namespace Autossential.Activities.ViewModels
{
    internal class MapDriveViewModel(IDesignServices services) : BaseViewModel(services)
    {
        public DesignInArgument<string> SharedDrivePath { get; set; }
        public DesignInArgument<NetworkCredential> Credential { get; set; }
        public DesignInArgument<bool> Force { get; set; }
        public DesignInArgument<string> DriveLetter { get; set; }
        public DesignOutArgument<int> ResponseCode { get; set; }
        public DesignOutArgument<string> ResponseMessage { get; set; }
        public DesignOutArgument<bool> Result { get; set; }
        public DesignOutArgument<string> MappedDrive { get; set; }

        protected override void InitializeModel()
        {
            base.InitializeModel();
            PersistValuesChangedDuringInit();

            int orderIndex = 0;

            SharedDrivePath.IsPrincipal = true;
            SharedDrivePath.OrderIndex = orderIndex++;

            Credential.OrderIndex = orderIndex++;
            Credential.Placeholder = Resources.MapDrive_Credential_Placeholder;

            DriveLetter.OrderIndex = orderIndex++;

            AddWidget(Force, ViewModelWidgetType.Toggle);
            Force.OrderIndex = orderIndex++;

            MappedDrive.OrderIndex = orderIndex++;
            ResponseCode.OrderIndex = orderIndex++;
            ResponseMessage.OrderIndex = orderIndex++;
            Result.OrderIndex = orderIndex;
        }
    }
}
