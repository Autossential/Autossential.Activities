using Autossential.Activities.Properties;
using Autossential.Core.Enums;
using System.Activities.DesignViewModels;

namespace Autossential.Activities.ViewModels.Misc
{
    public class StopwatchViewModel : BaseViewModel
    {
        public StopwatchViewModel(IDesignServices services) : base(services)
        {
        }

        public DesignInOutArgument<System.Diagnostics.Stopwatch> ReferenceStopwatch { get; set; }
        public DesignProperty<StopwatchMethods> Method { get; set; }

        protected override void InitializeModel()
        {
            base.InitializeModel();
            PersistValuesChangedDuringInit();

            int orderIndex = 0;

            ReferenceStopwatch.IsRequired = true;
            ReferenceStopwatch.IsPrincipal = true;
            ReferenceStopwatch.Category = Resources.Input_Category;
            ReferenceStopwatch.DisplayName = Resources.Stopwatch_ReferenceStopwatch_DisplayName;
            ReferenceStopwatch.Tooltip = Resources.Stopwatch_ReferenceStopwatch_Description;
            ReferenceStopwatch.OrderIndex = orderIndex++;

            Method.IsRequired = true;
            Method.IsPrincipal = true;
            Method.Category = Resources.Input_Category;
            Method.DisplayName = Resources.Stopwatch_Method_DisplayName;
            Method.Tooltip = Resources.Stopwatch_Method_Description;
            Method.OrderIndex = orderIndex++;
        }
    }
}
