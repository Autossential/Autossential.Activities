using Autossential.Core.Enums;
using Autossential.Core.Security;
using System;
using System.Activities;

namespace Autossential.Activities
{
    public sealed class TextEncryption : EncryptionBase<string>
    {
        protected override void Execute(NativeActivityContext context)
        {
            context.ScheduleFunc(Algorithm, OnComplete);
        }

        private void OnComplete(NativeActivityContext context, ActivityInstance completedInstance, IEncryption result)
        {
            if (result == null)
                throw new ArgumentNullException(nameof(result));

            var encoding = TextEncoding.Get(context);
            var input = Input.Get(context);
            var pass = GetRawKey(context);

            if (Action == CryptoActions.Encrypt)
            {
                var encrypted = result.Encrypt(encoding.GetBytes(input), encoding.GetBytes(pass));
                Result.Set(context, Convert.ToBase64String(encrypted));
            }
            else
            {
                var decrypted = result.Decrypt(Convert.FromBase64String(input), encoding.GetBytes(pass));
                Result.Set(context, encoding.GetString(decrypted));
            }
        }
    }
}