using Autossential.Core.Enums;
using Autossential.Core.Security;
using Autossential.Shared;
using Autossential.Shared.Utils;
using System.Activities;
using System.Activities.Presentation.Expressions;
using System.Text;

namespace Autossential.Activities.Design.Designers
{
    public partial class EncryptionDesigner
    {
        public EncryptionDesigner()
        {
            InitializeComponent();

            Algorithm.AllowedItemType = typeof(Activity<IEncryption>);
            cbActions.ItemsSource = EnumUtil.EnumAsDictionary<CryptoActions>();
            cbActions.DisplayMemberPath = "Key";
            cbActions.SelectedValuePath = "Value";

            Loaded += EncryptionDesigner_Loaded;
        }

        private void EncryptionDesigner_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            var encoding = ModelItem.Properties[nameof(EncryptionBase<object>.TextEncoding)];
            if (encoding.Value == null)
            {
                var value = ExpressionServiceLanguage.CreateExpression<Encoding>(ModelItem, $"{typeof(Encoding).FullName}.{nameof(Encoding.UTF8)}");
                encoding.SetValue(new InArgument<Encoding>(value));
            }
        }
    }
}