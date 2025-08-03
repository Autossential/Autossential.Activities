using Autossential.Activities.Properties;
using System;
using System.Activities.DesignViewModels;

namespace Autossential.Activities.ViewModels.Workflow
{
    internal class TimeLoopViewModel : BaseViewModel
    {
        public TimeLoopViewModel(IDesignServices services) : base(services) { }

        public DesignInArgument<TimeSpan> Timer { get; set; }
        public DesignInArgument<bool> ExitOnException { get; set; }
        public DesignInArgument<bool> PropagateException { get; set; }
        public DesignInArgument<TimeSpan> LoopInterval { get; set; }
        public DesignOutArgument<Exception> OutputException { get; set; }
        public DesignOutArgument<int> Index { get; set; }
        public DesignOutArgument<bool> IsTimeout { get; set; }

        protected override void InitializeModel()
        {
            base.InitializeModel();
            PersistValuesChangedDuringInit();
            int orderIndex = 0;
            Timer.IsPrincipal = true;
            Timer.IsRequired = true;
            Timer.Category = Resources.Input_Category;
            Timer.DisplayName = Resources.TimeLoop_Timer_DisplayName;
            Timer.Placeholder = Resources.TimeLoop_Timer_Description;
            Timer.Tooltip = Resources.TimeLoop_Timer_Description;
            Timer.OrderIndex = orderIndex++;

            ExitOnException.IsPrincipal = false;
            ExitOnException.IsRequired = false;
            ExitOnException.Category = Resources.Options_Category;
            ExitOnException.DisplayName = Resources.TimeLoop_ExitOnException_DisplayName;
            ExitOnException.Placeholder = Resources.TimeLoop_ExitOnException_Description;
            ExitOnException.Tooltip = Resources.TimeLoop_ExitOnException_Description;
            ExitOnException.OrderIndex = orderIndex++;

            PropagateException.IsPrincipal = false;
            PropagateException.IsRequired = false;
            PropagateException.Category = Resources.Options_Category;
            PropagateException.DisplayName = Resources.TimeLoop_PropagateException_DisplayName;
            PropagateException.Placeholder = Resources.TimeLoop_PropagateException_Description;
            PropagateException.Tooltip = Resources.TimeLoop_PropagateException_Description;
            PropagateException.OrderIndex = orderIndex++;

            LoopInterval.IsPrincipal = false;
            LoopInterval.IsRequired = false;
            LoopInterval.Category = Resources.Options_Category;
            LoopInterval.DisplayName = Resources.TimeLoop_LoopInterval_DisplayName;
            LoopInterval.Placeholder = Resources.TimeLoop_LoopInterval_Description;
            LoopInterval.Tooltip = Resources.TimeLoop_LoopInterval_Description;
            LoopInterval.OrderIndex = orderIndex++;

            OutputException.IsPrincipal = false;
            OutputException.IsRequired = false;
            OutputException.Category = Resources.Output_Category;
            OutputException.DisplayName = Resources.TimeLoop_OutputException_DisplayName;
            OutputException.Placeholder = Resources.TimeLoop_OutputException_Description;
            OutputException.Tooltip = Resources.TimeLoop_OutputException_Description;
            OutputException.OrderIndex = orderIndex++;

            Index.IsPrincipal = false;
            Index.IsRequired = false;
            Index.Category = Resources.Output_Category;
            Index.DisplayName = Resources.TimeLoop_Index_DisplayName;
            Index.Placeholder = Resources.TimeLoop_Index_Description;
            Index.Tooltip = Resources.TimeLoop_Index_Description;
            Index.OrderIndex = orderIndex++;

            IsTimeout.IsPrincipal = false;
            IsTimeout.IsRequired = false;
            IsTimeout.Category = Resources.Output_Category;
            IsTimeout.DisplayName = Resources.TimeLoop_IsTimeout_DisplayName;
            IsTimeout.Placeholder = Resources.TimeLoop_IsTimeout_Description;
            IsTimeout.Tooltip = Resources.TimeLoop_IsTimeout_Description;
            IsTimeout.OrderIndex = orderIndex++;
        }
    }
}
