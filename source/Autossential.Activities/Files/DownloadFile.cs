using Autossential.Activities.Properties;
using Autossential.Shared;
using Autossential.Shared.Activities.Base;
using System;
using System.Activities;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Autossential.Activities.Files
{
    public sealed class DownloadFile : ContinuableAsyncTaskCodeActivity
    {
        public InArgument<string> RequestURI { get; set; }
        public InArgument<string> DestinationFilePath { get; set; }
        public InArgument<int> Timeout { get; set; } = 30000;
        public bool AllowUntrustedSSLCertificate { get; set; }

        private static HttpClient _httpClient;
        private static HttpClient _unsafeHttpClient;

        private static HttpClient GetClient(bool useUnsafeClient)
        {
            if (useUnsafeClient)
            {
                if (_unsafeHttpClient != null)
                    return _unsafeHttpClient;
#if NET461
                var handler = new WebRequestHandler
                {
                    ServerCertificateValidationCallback = delegate { return true; },
                };
#else
                var handler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = delegate { return true; }
                };
#endif
                _unsafeHttpClient = new HttpClient(handler);
                return _unsafeHttpClient;
            }

            if (_httpClient == null)
                _httpClient = new HttpClient();

            return _httpClient;
        }

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            base.CacheMetadata(metadata);

            if (RequestURI == null)
                metadata.AddValidationError(Resources.Validation_ValueErrorFormat(nameof(RequestURI)));

            if (DestinationFilePath == null)
                metadata.AddValidationError(Resources.Validation_ValueErrorFormat(nameof(DestinationFilePath)));
        }

        protected override async Task<Action<AsyncCodeActivityContext>> ExecuteAsync(AsyncCodeActivityContext context, CancellationToken token)
        {
            var time = Timeout.Get(context);

            await ExecuteWithTimeoutAsync(context, token, DownloadFileAsync(context, token), time);
            return new Action<AsyncCodeActivityContext>(_ => { });
        }

        private async Task<bool> DownloadFileAsync(AsyncCodeActivityContext context, CancellationToken token)
        {
            var requestURI = RequestURI.Get(context);
            var destFilePath = DestinationFilePath.Get(context);

            Directory.CreateDirectory(Path.GetDirectoryName(destFilePath));
#if NET461
            using (var response = await GetClient(AllowUntrustedSSLCertificate).GetAsync(requestURI, token))
            using (var stream = await response.Content.ReadAsStreamAsync())
#else
            using (var stream = await GetClient(AllowUntrustedSSLCertificate).GetStreamAsync(requestURI, token))
#endif
            using (var fs = new FileStream(destFilePath, FileMode.Create))
            {
                stream.CopyTo(fs);
            }

            return true;
        }
    }
}