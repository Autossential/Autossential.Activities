using Autossential.Core.Security;
using System.Security.Cryptography;


namespace Autossential.Core.Security.Algorithms
{
    public class DESEncryption : EncryptionBase
    {
        public DESEncryption() : this(MINIMUM_ITERATIONS_RECOMMENDED)
        {

        }

        public DESEncryption(int iterations) : base(iterations)
        {
        }

        public override byte[] Decrypt(byte[] data, byte[] password)
        {
            using (var alg = new DESCryptoServiceProvider())
                return SymmetricDecrypt(alg, data, password);
        }

        public override byte[] Encrypt(byte[] data, byte[] password)
        {
            using (var alg = new DESCryptoServiceProvider())
                return SymmetricEncrypt(alg, data, password);
        }
    }
}
