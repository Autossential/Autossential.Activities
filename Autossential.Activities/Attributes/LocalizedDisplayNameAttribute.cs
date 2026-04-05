using Autossential.Activities.Properties;
using System.ComponentModel;

namespace Autossential.Activities.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class LocalizedDisplayNameAttribute(string displayName) : DisplayNameAttribute
    {
        public override string DisplayName => Resources.ResourceManager.GetString(displayName) ?? base.DisplayName;
    }
}
