using System.Activities.Presentation.Model;

namespace Autossential.Activities.Design.Helpers
{
    public static class CryptographyBaseActivityHelper
    {
        public static void NormalizeCryptoKeys(ModelItem modelItem)
        {
            //var value = (bool)modelItem.Properties[nameof(ICryptographyBaseActivity.UseSecureKey)].ComputedValue;
            //modelItem.Properties[value ? nameof(ICryptographyBaseActivity.Key) : nameof(ICryptographyBaseActivity.SecureKey)].ClearValue();
        }
    }
}