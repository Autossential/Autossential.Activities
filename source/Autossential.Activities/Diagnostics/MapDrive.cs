using Autossential.Activities.Properties;
using System.Activities;
using System.IO;
using System.Net;

namespace Autossential.Activities
{
    public sealed class MapDrive : NetworkDrive
    {
        public InArgument<string> SharedDrivePath { get; set; }
        public InArgument<NetworkCredential> Credential { get; set; }
        public OutArgument<string> MappedDrive { get; set; }
        public InArgument<bool> Force { get; set; }

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            base.CacheMetadata(metadata);

            if (SharedDrivePath == null)
            {
                metadata.AddValidationError(Resources.Validation_ValueErrorFormat(nameof(SharedDrivePath)));
            }
        }

        protected override bool Execute(CodeActivityContext context)
        {
            var resource = new NetResource
            {
                dwType = 1,         // disk
                dwScope = 2,        // global network
                dwDisplayType = 3,  // share
                lpLocalName = GetNormalizedDriveLetter(context, true),
                lpRemoteName = SharedDrivePath.Get(context)
            };

            if (Force.Get(context) && IsDriveMapped(resource.lpLocalName))
            {
                _ = WNetCancelConnection2A(resource.lpLocalName, 0, true);
            }

            string username = null;
            string password = null;

            if (Credential != null)
            {
                var c = Credential.Get(context);
                if (c != null)
                {
                    username = string.IsNullOrEmpty(c.Domain) ? c.UserName : Path.Combine(c.Domain, c.UserName);
                    password = c.Password;
                }
            }

            var result = WNetAddConnection2A(ref resource, password, username, 0);
            ResponseCode.Set(context, result);

            if (result == 0)
            {
                MappedDrive.Set(context, resource.lpLocalName + "\\");
                return true;
            }

            return false;
        }
    }
}