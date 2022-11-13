using Autossential.Core.Security;
using System.Security.Cryptography;


namespace Autossential.Core.Security.Algorithms
{
    public class TripleDESEncryption : EncryptionBase
    {
        public TripleDESEncryption() : this(MINIMUM_ITERATIONS_RECOMMENDED)
        {

        }
        public TripleDESEncryption(int iterations) : base(iterations)
        {
        }

        public override byte[] Decrypt(byte[] data, byte[] password)
        {
            using (var alg = TripleDES.Create())
                return SymmetricDecrypt(alg, data, password);
        }

        public override byte[] Encrypt(byte[] data, byte[] password)
        {
            using (var alg = TripleDES.Create())
                return SymmetricEncrypt(alg, data, password);
        }
    }
}
