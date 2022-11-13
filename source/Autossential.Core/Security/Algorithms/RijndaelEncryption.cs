using Autossential.Core.Security;
using System.Security.Cryptography;


namespace Autossential.Core.Security.Algorithms
{
    public class RijndaelEncryption : EncryptionBase
    {
        public RijndaelEncryption() : this(MINIMUM_ITERATIONS_RECOMMENDED)
        {

        }
        protected RijndaelEncryption(int iterations) : base(iterations)
        {
        }

        public override byte[] Decrypt(byte[] data, byte[] password)
        {
            using (var alg = Aes.Create("AesManaged"))
                return SymmetricDecrypt(alg, data, password);
        }

        public override byte[] Encrypt(byte[] data, byte[] password)
        {
            using (var alg = Aes.Create("AesManaged"))
                return SymmetricEncrypt(alg, data, password);
        }
    }
}
