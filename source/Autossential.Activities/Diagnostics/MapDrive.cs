using Autossential.Activities.Properties;
using System;
using System.Activities;
using System.Net;

namespace Autossential.Activities
{
    public sealed class MapDrive : NetworkDrive
    {
        private const uint ResourceType_Disk = 0x1;
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
                dwType = ResourceType_Disk,
                lpLocalName = GetNormalizedDriveLetter(context, true),
                lpRemoteName = SharedDrivePath.Get(context)
            };

            if (Force.Get(context) && IsDriveMapped(resource.lpLocalName))
            {
                WNetCancelConnection2A(resource.lpLocalName, 0, true);
            }

            string username = null;
            string password = null;

            if (Credential != null)
            {
                var c = Credential.Get(context);
                if (c != null)
                {
                    username = c.UserName;
                    password = c.Password;
                }
            }

            var result = WNetAddConnection3(IntPtr.Zero, ref resource, password, username, 0);
            if (result == 0)
            {
                MappedDrive.Set(context, resource.lpLocalName + "\\");
                return true;
            }

            return false;
        }
    }
}
