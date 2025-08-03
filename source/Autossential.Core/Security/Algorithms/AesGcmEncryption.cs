using System;
using System.Security.Cryptography;

namespace Autossential.Core.Security.Algorithms
{
    public class AesGcmEncryption : IEncryption
    {
        private const int SALT_SIZE = 8;
        private const int IV_SIZE = 12;
        private const int TAG_SIZE = 16;
        public const int MINIMUM_ITERATIONS_RECOMMENDED = 1000;
        private const int KEY_SIZE = 32;
        protected readonly RandomNumberGenerator _rng = RandomNumberGenerator.Create();

        public AesGcmEncryption() : this(MINIMUM_ITERATIONS_RECOMMENDED)
        {

        }

        public AesGcmEncryption(int iterations)
        {
            Iterations = iterations;
        }

        public int Iterations { get; set; }

        public byte[] Decrypt(byte[] data, byte[] password)
        {
            var saltBytes = new byte[SALT_SIZE];
            var IV = new byte[IV_SIZE];
            var tag = new byte[TAG_SIZE];

            var encryptedData = new byte[data.Length - SALT_SIZE - IV_SIZE - TAG_SIZE];
            Array.Copy(data, 0, saltBytes, 0, SALT_SIZE);
            Array.Copy(data, SALT_SIZE, IV, 0, IV_SIZE);
            Array.Copy(data, SALT_SIZE + IV_SIZE, encryptedData, 0, encryptedData.Length);
            Array.Copy(data, SALT_SIZE + IV_SIZE + encryptedData.Length, tag, 0, TAG_SIZE);

            byte[] decryptedBytes;

            using var r = new Rfc2898DeriveBytes(password, saltBytes, Iterations);
            using var aes = new AesGcm(r.GetBytes(KEY_SIZE));

            decryptedBytes = new byte[encryptedData.Length];
            aes.Decrypt(IV, encryptedData, tag, decryptedBytes);
            return decryptedBytes;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }


        public byte[] Encrypt(byte[] data, byte[] password)
        {
            var saltBytes = new byte[SALT_SIZE];
            var tag = new byte[TAG_SIZE];
            var IV = new byte[IV_SIZE];

            _rng.GetBytes(saltBytes);
            _rng.GetBytes(IV);

            using var r = new Rfc2898DeriveBytes(password, saltBytes, Iterations);
            using var aes = new AesGcm(r.GetBytes(KEY_SIZE));

            var encryptedBytes = new byte[data.Length];
            aes.Encrypt(IV, data, encryptedBytes, tag);

            var result = new byte[SALT_SIZE + IV_SIZE + TAG_SIZE + encryptedBytes.Length];
            Array.Copy(saltBytes, 0, result, 0, SALT_SIZE);
            Array.Copy(IV, 0, result, SALT_SIZE, IV_SIZE);
            Array.Copy(encryptedBytes, 0, result, SALT_SIZE + IV_SIZE, encryptedBytes.Length);
            Array.Copy(tag, 0, result, SALT_SIZE + IV_SIZE + encryptedBytes.Length, TAG_SIZE);

            return result;
        }
    }
}