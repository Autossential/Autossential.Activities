using Autossential.Activities.Properties;
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
            var sharedDrivePath = SharedDrivePath.Get(context)
                ?? throw new InvalidOperationException(ResourcesFn.Common_ErrorMsg_ValueNotSuppliedFormat(Resources.MapDrive_SharedDrivePath_DisplayName));

            var driveLetter = DriveLetter.Get(context) ?? GetAvailableDriveLetter();

            var force = Force.Get(context);
            if (force && IsDriveMapped(driveLetter))
                Disconnect(driveLetter);

            var credential = Credential.Get(context);

            var result = Connect(driveLetter, sharedDrivePath, credential);
            if (result == 1219 && force) // 1219 = "Multiple connections to a server or shared resource by the same user, using more than one user name, are not allowed."
            {
                Disconnect(driveLetter);
                result = Connect(driveLetter, sharedDrivePath, credential);
            }

            ResponseCode.Set(context, result);
            SetResponseMessageFromCode(context, result);
            if (result == 0)
            {
                DriveLetter.Set(context, driveLetter);
                return true;
            }

            return false;
        }
    }
}
