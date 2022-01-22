using Autossential.Core.Enums;
using Autossential.Shared.Utils;

namespace Autossential.Activities.Design.Designers
{
    // Interaction logic for DataTableToTextFormatDesigner.xaml
    public partial class DataTableToTextDesigner
    {
        public DataTableToTextDesigner()
        {
            InitializeComponent();

            cbTextFormat.ItemsSource = EnumUtil.EnumAsDictionary<TextFormat>();
            cbTextFormat.DisplayMemberPath = "Key";
            cbTextFormat.SelectedValuePath = "Value";
        }
    }
}
