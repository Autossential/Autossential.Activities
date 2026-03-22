using Autossential.Activities.Base;
using Autossential.Activities.Models;
using System.Activities.DesignViewModels;

namespace Autossential.Activities.ViewModels
{
    internal class MergeDataViewModel(IDesignServices services) : BaseViewModel(services)
    {
        public DesignInArgument<DataNode> Source { get; set; }
        public DesignInOutArgument<DataNode> Destination { get; set; }
        protected override void InitializeModel()
        {
            base.InitializeModel();
            PersistValuesChangedDuringInit();

            var orderIndex = 0;
            Source.IsPrincipal = true;
            Source.OrderIndex = orderIndex++;

            Destination.IsPrincipal = true;
            Destination.OrderIndex = orderIndex++;
        }
    }
}
