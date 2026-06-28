using Autossential.Activities.Base;
using System.Activities.DesignViewModels;
using System.Activities.ViewModels;

namespace Autossential.Activities.ViewModels
{
    internal class UnmapDriveViewModel : BaseViewModel
    {
        public UnmapDriveViewModel(IDesignServices services) : base(services) { }

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

            ResponseCode.OrderIndex = orderIndex++;
            ResponseMessage.OrderIndex = orderIndex++;
            Result.OrderIndex = orderIndex;
        }
    }
}
