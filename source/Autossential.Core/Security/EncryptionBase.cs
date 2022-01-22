using System;
using System.IO;
using System.Security.Cryptography;

namespace Autossential.Core.Security
{
    public abstract class EncryptionBase : IEncryption
    {
        protected readonly RandomNumberGenerator RnGen = RandomNumberGenerator.Create();

        public const int SALT_SIZE = 8;
        public const int MINIMUM_ITERATIONS_RECOMMENDED = 1000;

        public int Iterations { get; set; }

        protected EncryptionBase(int iterations = MINIMUM_ITERATIONS_RECOMMENDED)
        {
            Iterations = iterations;
        }

        public abstract byte[] Decrypt(byte[] data, byte[] password);

        public abstract byte[] Encrypt(byte[] data, byte[] password);

        /// <summary>
        /// Encrypts the specified data.
        /// </summary>
        /// <param name="alg">The SymmetricAlgorithm to be used</param>
        /// <param name="data">The bytes to be encrypted.</param>
        /// <param name="password">The password bytes to be used on encryption.</param>
        /// <returns>A byte array that represents the encrypted input bytes.</returns>
        protected byte[] SymmetricEncrypt(SymmetricAlgorithm alg, byte[] data, byte[] password)
        {
            var saltBytes = new byte[SALT_SIZE];
            RnGen.GetBytes(saltBytes);

            using (var r = new Rfc2898DeriveBytes(password, saltBytes, Iterations))
            {
                var key = r.GetBytes(alg.KeySize >> 3);
                var IV = r.GetBytes(alg.BlockSize >> 3);

                byte[] encryptedBytes;

                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, alg.CreateEncryptor(key, IV), CryptoStreamMode.Write))
                    {
                        cs.Write(data, 0, data.Length);
                        cs.FlushFinalBlock();
                    }

                    encryptedBytes = ms.ToArray();
                }

                var result = new byte[saltBytes.Length + IV.Length + encryptedBytes.Length];
                Array.Copy(saltBytes, 0, result, 0, saltBytes.Length);
                Array.Copy(IV, 0, result, saltBytes.Length, IV.Length);
                Array.Copy(encryptedBytes, 0, result, saltBytes.Length + IV.Length, encryptedBytes.Length);

                return result;
            }
        }

        /// <summary>
        /// Decrypts the specified byte array.
        /// </summary>
        /// <param name="alg">The SymmetricAlgorithm to be used</param>
        /// <param name="data">The bytes to be decrypted.</param>
        /// <param name="password">The password bytes to be used on decryption.</param>
        /// <returns>A byte array that represents the decrypted input bytes.</returns>
        public byte[] SymmetricDecrypt(SymmetricAlgorithm alg, byte[] data, byte[] password)
        {
            byte[] decryptedBytes;
            var saltBytes = new byte[SALT_SIZE];
            var IV = new byte[alg.BlockSize >> 3];

            var encryptedData = new byte[data.Length - IV.Length - saltBytes.Length];
            Array.Copy(data, 0, saltBytes, 0, saltBytes.Length);
            Array.Copy(data, saltBytes.Length, IV, 0, IV.Length);
            Array.Copy(data, saltBytes.Length + IV.Length, encryptedData, 0, encryptedData.Length);
            using (var r = new Rfc2898DeriveBytes(password, saltBytes, Iterations))
            {
                var key = r.GetBytes(alg.KeySize >> 3);
                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, alg.CreateDecryptor(key, IV), CryptoStreamMode.Write))
                    {
                        cs.Write(encryptedData, 0, encryptedData.Length);
                        cs.FlushFinalBlock();
                    }

                    decryptedBytes = ms.ToArray();
                }
            }
            return decryptedBytes;
        }

        public virtual void Dispose()
        {
            RnGen.Dispose();
        }
    }
}