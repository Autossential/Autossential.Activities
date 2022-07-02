using Autossential.Activities.Security.Algorithms;
using Autossential.Core.Security;
using Autossential.Shared.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Activities;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Security;
using System.Text;

namespace Autossential.Activities.Test
{
    [TestClass]
    public class TextEncryptionTests
    {
        private const string CryptoKey = "test-A3B3EAC5-F4E6-4B03-9796-A329DC1F860B";

        [TestMethod]
        [DataRow(nameof(AesAlgorithmEncryption))]
        [DataRow(nameof(DESAlgorithmEncryption))]
        [DataRow(nameof(RC2AlgorithmEncryption))]
        [DataRow(nameof(RijndaelAlgorithmEncryption))]
        [DataRow(nameof(TripleDESAlgorithmEncryption))]
#if NET6_0
        [DataRow(nameof(AesGcmAlgorithmEncryption))]
#endif
        public void Default(string alg)
        {
            CodeActivity<IEncryption> handler = null;

            switch (alg)
            {
                case nameof(AesAlgorithmEncryption):
                    handler = new AesAlgorithmEncryption();
                    break;
                case nameof(DESAlgorithmEncryption):
                    handler = new DESAlgorithmEncryption();
                    break;
                case nameof(RC2AlgorithmEncryption):
                    handler = new RC2AlgorithmEncryption();
                    break;
                case nameof(RijndaelAlgorithmEncryption):
                    handler = new RijndaelAlgorithmEncryption();
                    break;
                case nameof(TripleDESAlgorithmEncryption):
                    handler = new TripleDESAlgorithmEncryption();
                    break;
#if NET6_0
                case nameof(AesGcmAlgorithmEncryption):
                    handler = new AesGcmAlgorithmEncryption();
                    break;
#endif
            }

            var text = Guid.NewGuid().ToString();
            var secureString = new NetworkCredential("", CryptoKey).SecurePassword;

            var activity = new TextEncryption()
            {
                Algorithm = new ActivityFunc<IEncryption>()
                {
                    Handler = handler
                },
                Key = new InArgument<SecureString>(_ => secureString)
            };

            var encrypted = WorkflowTester.CompileAndInvoke(activity, GetArgs(text));
            Assert.IsNotNull(encrypted);
            Assert.AreNotEqual(text, encrypted);

            activity.Action = Core.Enums.CryptoActions.Decrypt;
            var decrypted = WorkflowTester.CompileAndInvoke(activity, GetArgs(encrypted));
            Assert.IsNotNull(decrypted);
            Assert.AreNotEqual(encrypted, decrypted);
            Assert.AreEqual(text, decrypted);
        }

        private static IDictionary<string, object> GetArgs(string input)
        {
            var dic = new Dictionary<string, object>
            {
                { nameof(TextEncryption.Input), input },
                { nameof(TextEncryption.TextEncoding), Encoding.UTF8 }
            };

            return dic;
        }
    }
}