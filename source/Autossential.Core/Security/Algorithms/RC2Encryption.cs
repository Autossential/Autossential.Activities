using Autossential.Core.Security;
using System.Security.Cryptography;


namespace Autossential.Core.Security.Algorithms
{
    public class RC2Encryption : EncryptionBase
    {
        public RC2Encryption() : this(MINIMUM_ITERATIONS_RECOMMENDED)
        {

        }
        protected RC2Encryption(int iterations) : base(iterations)
        {
        }

        public override byte[] Decrypt(byte[] data, byte[] password)
        {
            using (var alg = RC2.Create())
                return SymmetricDecrypt(alg, data, password);
        }

        public override byte[] Encrypt(byte[] data, byte[] password)
        {
            using (var alg = RC2.Create())
                return SymmetricEncrypt(alg, data, password);
        }
    }
}
