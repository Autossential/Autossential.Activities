using Autossential.Shared;
using System.Activities;
using System.Text;

namespace Autossential.Activities.Design.Designers
{
    // Interaction logic for ZipDesigner.xaml
    public partial class ZipDesigner
    {
        public ZipDesigner()
        {
            InitializeComponent();
            Loaded += ZipDesigner_Loaded;
        }

        private void ZipDesigner_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            var encoding = ModelItem.Properties[nameof(Zip.TextEncoding)];
            if (encoding.Value == null)
                encoding.SetValue(new InArgument<Encoding>(ExpressionServiceLanguage.CreateExpression<Encoding>(ModelItem, $"{typeof(Encoding).FullName}.{nameof(Encoding.UTF8)}")));
        }
    }
}