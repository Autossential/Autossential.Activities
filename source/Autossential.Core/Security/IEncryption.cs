using System;

namespace Autossential.Core.Security
{
    public interface IEncryption : IDisposable
    {
        int Iterations { get; set; }
        byte[] Encrypt(byte[] data, byte[] password);
        byte[] Decrypt(byte[] data, byte[] password);
    }
}