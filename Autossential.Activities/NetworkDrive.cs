using Autossential.Activities.Properties;
using System.Activities;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Autossential.Activities
{
    public abstract class NetworkDrive : CodeActivity<bool>
    {
        public OutArgument<int> ResponseCode { get; set; }
        public OutArgument<string> ResponseMessage { get; set; }

        [DllImport("mpr.dll")]
        protected static extern int WNetAddConnection2A(ref NetResource lpNetResource, string lpPassword, string lpUserName, uint dwFlags);

        [DllImport("mpr.dll")]
        protected static extern int WNetCancelConnection2A(string lpName, int dwFlags, bool fForce);

        protected static int DisconnectDrive(string driveLetter) =>
            WNetCancelConnection2A(driveLetter, 0, true);

        [StructLayout(LayoutKind.Sequential)]
        protected struct NetResource
        {
            public uint dwScope;
            public uint dwType;
            public uint dwDisplayType;
            public uint dwUsage;
            public string lpLocalName;
            public string lpRemoteName;
            public string lpComment;
            public string lpProvider;
        }

        protected static bool IsDriveMapped(string driveLetter)
        {
            foreach (var drive in Environment.GetLogicalDrives())
            {
                if (drive[0].ToString().Equals(driveLetter[0].ToString(), StringComparison.OrdinalIgnoreCase))
                    return true;
            }

            return false;
        }

        protected static void UnmapDrives()
        {
            foreach (var drive in DriveInfo.GetDrives())
            {
                if (drive.IsReady && drive.DriveType == DriveType.Network)
                {
                    DisconnectDrive(drive.Name.TrimEnd('\\'));
                }
            }
        }

        private const string ALPHABET = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        protected static string GetAvailableDriveLetter()
        {
            var letters = ALPHABET.ToCharArray().Select(p => p.ToString()).ToList();
            foreach (var di in DriveInfo.GetDrives())
            {
                char drive = di.Name[0];
                if (char.IsLower(drive))
                    drive = char.ToUpper(drive);

                letters.Remove(drive.ToString());
            }

            var random = letters.OrderBy(_ => Guid.NewGuid()).FirstOrDefault();
            return NormalizeDriveLetter(random) ?? throw new Exception(Resources.NetworkDrive_ErrorMsg_NoDriversAvailable);
        }

        protected static void ValidateDriveLetterFormat(string driveLetter)
        {
            if (driveLetter.Length > 3 ||
                !ALPHABET.Contains(driveLetter[0]) ||
                (driveLetter.Length > 1 && driveLetter[1] != ':') ||
                (driveLetter.Length > 2 && driveLetter[2] != '\\'))
                throw new ArgumentException(Resources.NetworkDrive_ErrorMsg_InvalidDriveLetter, nameof(driveLetter)); ;
        }

        protected static string NormalizeDriveLetter(string driveLetter) => char.ToUpper(driveLetter[0]) + ":";

        protected void SetResponseMessageFromCode(CodeActivityContext context, int responseCode)
        {
            ResponseMessage.Set(context,
                responseCode == 0
                ? Resources.NetworkDrive_ResponseMsg_Success
                : new Win32Exception(responseCode).Message);
        }
    }
}
