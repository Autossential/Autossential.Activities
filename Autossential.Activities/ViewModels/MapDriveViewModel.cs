using Autossential.Activities.Base;
using Autossential.Activities.Properties;
using System.Activities.DesignViewModels;
using System.Activities.ViewModels;
using System.Net;

namespace Autossential.Activities.ViewModels
{
    internal class MapDriveViewModel : BaseViewModel
    {
        public MapDriveViewModel(IDesignServices services) : base(services)
        {
            var letters = ALPHABET.ToCharArray().ToList();
            foreach (var di in DriveInfo.GetDrives())
            {
                char drive = di.Name[0];
                if (char.IsLower(drive))
                    drive = char.ToUpper(drive);

                letters.Remove(drive);
            }

            AvailableDrivers = DataSourceBuilder<string>
                .WithId(p => p)
                .WithLabel(p => p)
                .WithData([.. letters.Select(p => p.ToString() + ':')]).Build();
        }
        private const string ALPHABET = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        public DesignInArgument<string> SharedDrivePath { get; set; }
        public DesignInArgument<NetworkCredential> Credential { get; set; }
        public DesignInArgument<bool> Force { get; set; }
        public DesignInArgument<string> DriveLetter { get; set; }
        public DesignOutArgument<int> ResponseCode { get; set; }
        public DesignOutArgument<string> ResponseMessage { get; set; }
        public DesignOutArgument<bool> Result { get; set; }
        public DesignOutArgument<string> MappedDrive { get; set; }

        private DataSource<string> AvailableDrivers { get; }

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
            DriveLetter.DataSource = AvailableDrivers;
            AddWidget(DriveLetter, ViewModelWidgetType.Dropdown);

            AddWidget(Force, ViewModelWidgetType.Toggle);
            Force.OrderIndex = orderIndex++;

            MappedDrive.OrderIndex = orderIndex++;
            ResponseCode.OrderIndex = orderIndex++;
            ResponseMessage.OrderIndex = orderIndex++;
            Result.OrderIndex = orderIndex;
        }
    }
}
