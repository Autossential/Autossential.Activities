using Autossential.Activities.Properties;
using Autossential.Core.Enums;
using Autossential.Core.Security;
using Autossential.Shared;
using System.Activities;
using System.ComponentModel;
using System.Net;
using System.Security;
using System.Text;

namespace Autossential.Activities
{
    public abstract class EncryptionBase<T> : NativeActivity<T>
    {
        [Browsable(false)]
        public ActivityFunc<IEncryption> Algorithm { get; set; }

        public InArgument Key { get; set; }

        public InArgument<T> Input { get; set; }

        public InArgument<Encoding> TextEncoding { get; set; }

        public CryptoActions Action { get; set; }

        protected string GetRawKey(ActivityContext context)
        {
            var key = Key.Get(context);
            if (key is SecureString sKey) return new NetworkCredential("", sKey).Password;
            if (key is string rawKey) return rawKey;
            return null;
        }

        protected EncryptionBase()
        {
            Algorithm = new ActivityFunc<IEncryption>();
        }

        protected override void CacheMetadata(NativeActivityMetadata metadata)
        {
            metadata.AddImplementationDelegate(Algorithm);
            metadata.AddRuntimeArgument(Input, typeof(T), nameof(Input), true);
            metadata.AddRuntimeArgument(TextEncoding, nameof(TextEncoding), false);

            if (Key == null)
            {
                metadata.AddRuntimeArgument(Key, typeof(string), nameof(Key), true);
            }
            else if (Key.IsArgumentTypeAnyCompatible<string, SecureString>())
            {
                metadata.AddRuntimeArgument(Key, Key.ArgumentType, nameof(Key), true);
            }

            if (Algorithm == null || Algorithm.Handler == null)
            {
                metadata.AddValidationError(Resources.EncryptionBase_ErrorMsg_AlgorithmMissing);
            }
        }
    }
}