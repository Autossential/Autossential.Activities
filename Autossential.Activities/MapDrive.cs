using System.Activities;
using System.Net;

namespace Autossential.Activities
{
    public sealed class MapDrive : NetworkDrive
    {
        [RequiredArgument]
        public InArgument<string> SharedDrivePath { get; set; }
        public InArgument<NetworkCredential> Credential { get; set; }
        public InArgument<bool> Force { get; set; }
        public InOutArgument<string> DriveLetter { get; set; }

        protected override bool Execute(CodeActivityContext context)
        {
            var driveLetter = DriveLetter.Get(context) ?? GetAvailableDriveLetter();
            ValidateDriveLetterFormat(driveLetter);

            var force = Force.Get(context);
            if (force && IsDriveMapped(driveLetter))
                DisconnectDrive(driveLetter);

            string username = null;
            string password = null;

            var resource = new NetResource
            {
                dwType = 1,         // disk
                dwScope = 2,        // global network
                dwDisplayType = 3,  // share
                lpLocalName = driveLetter,
                lpRemoteName = SharedDrivePath.Get(context)
            };

            var credential = Credential?.Get(context);
            if (credential != null)
            {
                username = string.IsNullOrEmpty(credential.Domain) ? credential.UserName : Path.Combine(credential.Domain, credential.UserName);
                password = credential.Password;
            }

            var result = WNetAddConnection2A(ref resource, password, username, 0);
            if (result == 1219 && force) // 1219 = "Multiple connections to a server or shared resource by the same user, using more than one user name, are not allowed."
            {
                DisconnectDrive(driveLetter);
                result = WNetAddConnection2A(ref resource, password, username, 0);
            }

            ResponseCode.Set(context, result);
            SetResponseMessageFromCode(context, result);
            if (result == 0) 
            {
                DriveLetter.Set(context, resource.lpLocalName + "\\");
                return true;
            }

            return false;
        }
    }
}
