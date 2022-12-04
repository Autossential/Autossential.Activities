using Autossential.Activities.Files;
using Autossential.Shared.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;

namespace Autossential.Activities.Test
{
    [TestClass]
    public class DownloadFileTests
    {
        private readonly string DestinationFolder;

        public DownloadFileTests()
        {
            DestinationFolder = Path.Combine(Environment.ExpandEnvironmentVariables("%USERPROFILE%"), "Downloads");
        }

        [TestMethod]
        //[DataRow("https://file-examples.com/storage/fe352586866388d59a8918d/2017/10/file-example_PDF_1MB.pdf")]
        //[DataRow("https://www.stats.govt.nz/assets/Uploads/Annual-enterprise-survey/Annual-enterprise-survey-2021-financial-year-provisional/Download-data/annual-enterprise-survey-2021-financial-year-provisional-csv.csv")]
        public void DownloadTest(string url)
        {
            var dest = GetDestinationPath(url.Split('/').Last());
            WorkflowTester.Invoke(new DownloadFile(), GetArgs(url, dest.FullName));
            Assert.IsTrue(IsFileDownloaded(dest));
        }

        [TestMethod]
        [DataRow(false)]
        [DataRow(true)]
        [DataRow(false)]
        [DataRow(true)]
        public void InvalidCert(bool allowUntrustedSSLCert)
        {
            var dest = GetDestinationPath("badssl.html");
            var action = new Action(() =>
            {
                WorkflowTester.Invoke(new DownloadFile()
                {
                    AllowUntrustedSSLCertificate = allowUntrustedSSLCert
                }, GetArgs("https://expired.badssl.com/", dest.FullName));
            });            

            if (allowUntrustedSSLCert)
            {
                action();
                Assert.IsTrue(IsFileDownloaded(dest));                
            }
            else
            {
                Assert.ThrowsException<HttpRequestException>(action);
            }
        }

        private bool IsFileDownloaded(FileInfo info) => info.Exists && info.Length > 0;

        private FileInfo GetDestinationPath(string fileName)
        {
            return new FileInfo(Path.Combine(DestinationFolder, fileName));
        }

        private IDictionary<string, object> GetArgs(string url, string finalPath)
        {
            return new Dictionary<string, object>
            {
                { nameof(DownloadFile.RequestURI), url },
                { nameof(DownloadFile.DestinationFilePath), finalPath }
            };
        }
    }
}
