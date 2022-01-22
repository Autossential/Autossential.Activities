using Autossential.Core.Enums;
using Autossential.Shared.Utils;

namespace Autossential.Activities.Design.Designers
{
    public partial class AggregateDesigner
    {
        public AggregateDesigner()
        {
            InitializeComponent();

            cbFunction.ItemsSource = EnumUtil.EnumAsDictionary<AggregateFunction>();
            cbFunction.DisplayMemberPath = "Key";
            cbFunction.SelectedValuePath = "Value";
        }
    }
}