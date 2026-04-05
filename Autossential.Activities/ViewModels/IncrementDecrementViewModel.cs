using Autossential.Activities.Base;
using Autossential.Activities.Extensions;
using Autossential.Activities.Properties;
using System.Activities.DesignViewModels;

namespace Autossential.Activities.ViewModels
{
    internal class IncrementDecrementViewModel(IDesignServices services) : BaseViewModel(services)
    {
        public DesignInArgument<int> Value { get; set; }
        public DesignInOutArgument<int> Variable { get; set; }
        public DesignProperty<IncrementDecrement.ArithmeticOperation> Operation { get; set; }

        protected override void InitializeModel()
        {
            base.InitializeModel();
            PersistValuesChangedDuringInit();

            var orderIndex = 0;

            Variable.IsPrincipal = true;
            Variable.OrderIndex = orderIndex++;

            Operation.IsPrincipal = true;
            Operation.OrderIndex = orderIndex;
            Operation.ColumnOrder = 0;

            Value.IsPrincipal = true;
            Value.OrderIndex = orderIndex;
            Value.ColumnOrder = 1;
        }

        protected override void InitializeRules()
        {
            Rule(nameof(Operation), OperationChanged, false);
        }

        private void OperationChanged()
        {
            DisplayName.Value = ResourcesFn.IncrementDecrement_ViewModel_DynamicDisplayNameFormat(Operation.Value, Variable.Value.GetExpressionText<int>(), Value.Value.GetExpressionText<int>());
        }
    }
}
