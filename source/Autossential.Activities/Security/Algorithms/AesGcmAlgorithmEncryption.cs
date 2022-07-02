using Autossential.Core.Security.Algorithms;

namespace Autossential.Activities.Security.Algorithms
{
#if NET6_0
    public sealed class AesGcmAlgorithmEncryption : SymmetricAlgorithmEncryptionBase<AesGcmEncryption> { }
#endif
}
