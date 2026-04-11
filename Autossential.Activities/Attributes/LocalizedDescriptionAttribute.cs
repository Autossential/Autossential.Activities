using Autossential.Activities.Properties;
using System.ComponentModel;

namespace Autossential.Activities.Attributes
{
    public class LocalizedDescriptionAttribute(string description) : DescriptionAttribute
    {
        public override string Description => Resources.ResourceManager.GetString(description) ?? base.Description;
    }
}
