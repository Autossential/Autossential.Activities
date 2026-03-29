using Autossential.Activities.Base;
using Autossential.Activities.Extensions;
using System.Activities;
using System.Activities.DesignViewModels;
using System.Activities.ViewModels;

namespace Autossential.Activities.ViewModels
{
    internal class IfActivityViewModel(IDesignServices services) : BaseViewModel(services)
    {
        public DesignProperty<ActivityFunc<bool>> Condition { get; set; }

        public DesignProperty<Activity> Then { get; set; }
        public DesignProperty<Activity> Else { get; set; }

        protected override void InitializeModel()
        {
            base.InitializeModel();
            PersistValuesChangedDuringInit();

            int orderIndex = 0;

            Condition.IsPrincipal = true;
            Condition.OrderIndex = orderIndex++;
            Then.IsPrincipal = true;
            Then.OrderIndex = orderIndex++;
            Else.IsPrincipal = true;
            Else.OrderIndex = orderIndex++;

            if (IsWidgetSupported(ViewModelWidgetType.Container))
            {
                Condition.AddWidget(ViewModelWidgetType.Container);
                Then.AddWidget(ViewModelWidgetType.Container);
                Else.AddWidget(ViewModelWidgetType.Container);
            }
        }
    }
}
