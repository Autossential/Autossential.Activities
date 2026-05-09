using Autossential.Activities.Base;
using System.Activities.DesignViewModels;
using System.Activities.ViewModels;

namespace Autossential.Activities.ViewModels
{
    internal class UnmapDriveViewModel : BaseViewModel
    {
        public UnmapDriveViewModel(IDesignServices services) : base(services)
        {
            var letters = new List<string>();
            foreach (var di in DriveInfo.GetDrives())
            {
                if (di.DriveType == DriveType.Network)
                {
                    letters.Add(di.Name[0].ToString() + ':');
                }
            }

            AvailableDrivers = DataSourceBuilder<string>
                .WithId(p => p)
                .WithLabel(p => p)
                .WithData(letters).Build();
        }

        public DesignInArgument<string> DriveLetter { get; set; }
        public DesignOutArgument<int> ResponseCode { get; set; }
        public DesignOutArgument<string> ResponseMessage { get; set; }
        public DesignOutArgument<bool> Result { get; set; }
        public DataSource<string> AvailableDrivers { get; }
        protected override void InitializeModel()
        {
            base.InitializeModel();
            PersistValuesChangedDuringInit();

            int orderIndex = 0;

            DriveLetter.IsPrincipal = true;
            DriveLetter.OrderIndex = orderIndex++;
            DriveLetter.DataSource = AvailableDrivers;
            AddWidget(DriveLetter, ViewModelWidgetType.Dropdown);

            ResponseCode.OrderIndex = orderIndex++;
            ResponseMessage.OrderIndex = orderIndex++;
            Result.OrderIndex = orderIndex;
        }
    }
}
