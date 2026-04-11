using Autossential.Activities.Base;
using System.Activities.DesignViewModels;

namespace Autossential.Activities.ViewModels
{
    internal class IncrementViewModel(IDesignServices services) : BaseViewModel(services)
    {
        public DesignInArgument<int> Value { get; set; }
        public DesignInOutArgument<int> Variable { get; set; }

        protected override void InitializeModel()
        {
            base.InitializeModel();
            PersistValuesChangedDuringInit();

            var orderIndex = 0;

            Variable.IsRequired = true;
            Variable.IsPrincipal = true;
            Variable.OrderIndex = orderIndex++;

            Value.IsRequired = true;
            Value.IsPrincipal = true;
            Value.OrderIndex = orderIndex++;
        }
    }
}
