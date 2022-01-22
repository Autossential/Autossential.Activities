using Autossential.Core.Security;
using System.Security.Cryptography;

namespace Autossential.Core.Security.Algorithms
{
    public class AesEncryption : EncryptionBase
    {
        public AesEncryption() : this(MINIMUM_ITERATIONS_RECOMMENDED)
        {

        }
        public AesEncryption(int iterations) : base(iterations)
        {
        }

        public override byte[] Decrypt(byte[] data, byte[] password)
        {
            using (var alg = new AesCryptoServiceProvider())
                return SymmetricDecrypt(alg, data, password);
        }

        public override byte[] Encrypt(byte[] data, byte[] password)
        {
            using (var alg = new AesCryptoServiceProvider())
                return SymmetricEncrypt(alg, data, password);
        }
    }
}
