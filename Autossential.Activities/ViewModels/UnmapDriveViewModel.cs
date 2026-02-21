using Autossential.Activities.Base;
using System.Activities.DesignViewModels;

namespace Autossential.Activities.ViewModels
{
    internal class UnmapDriveViewModel(IDesignServices services) : BaseViewModel(services)
    {
        public DesignInArgument<string> DriveLetter { get; set; }
        public DesignOutArgument<int> ResponseCode { get; set; }
        public DesignOutArgument<string> ResponseMessage { get; set; }
        protected override void InitializeModel()
        {
            base.InitializeModel();
            PersistValuesChangedDuringInit();

            int orderIndex = 0;

            DriveLetter.IsPrincipal = true;
            DriveLetter.OrderIndex = orderIndex++;

            ResponseCode.OrderIndex = orderIndex++;
            ResponseMessage.OrderIndex = orderIndex++;
        }
    }
}
