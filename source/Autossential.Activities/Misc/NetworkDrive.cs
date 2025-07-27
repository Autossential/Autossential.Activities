using Autossential.Activities.Properties;
using System;
using System.Activities;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;


namespace Autossential.Activities
{
    public abstract class NetworkDrive : CodeActivity<bool>
    {
        public InArgument<string> DriveLetter { get; set; }
        public OutArgument<int> ResponseCode { get; set; }
        public OutArgument<string> ResponseMessage { get; set; }

        protected bool Fail(CodeActivityContext context, int responseCode)
        {
            ResponseMessage.Set(context, new Win32Exception(responseCode).Message);
            return false;
        }

        public static bool IsDriveMapped(string driveLetter)
        {
            foreach (var drive in Environment.GetLogicalDrives())
            {
                if (drive[0].ToString().Equals(driveLetter[0].ToString(), StringComparison.InvariantCultureIgnoreCase))
                    return true;
            }

            return false;
        }

        protected static void UnmapNetworkDrives()
        {
            foreach (var drive in DriveInfo.GetDrives())
            {
                if (drive.IsReady && drive.DriveType == DriveType.Network)
                {
                    DisconnectDrive(drive.Name.TrimEnd('\\'));
                }
            }
        }

        protected string GetNormalizedDriveLetter(CodeActivityContext context, bool allowAutoSelection)
        {
            var name = DriveLetter.Get(context);

            if (string.IsNullOrEmpty(name) && allowAutoSelection)
            {
                name = GetAvailableDriveLetter();
            }
            else
            {
                // despite this is not truly necessary
                // it keeps a consistency for the inputs
                ValidateName(name);
            }

            return char.ToUpper(name[0]) + ":";
        }

        private void ValidateName(string name)
        {
            var exception = new ArgumentException(Resources.NetworkDrive_ErrorMsg_InvalidDriveLetter, nameof(DriveLetter));

            if (name.Length > 3)
                throw exception;

            if (!ALPHABET.Contains(name[0]))
                throw exception;

            if (name.Length > 1 && name[1] != ':')
                throw exception;

            if (name.Length > 2 && name[2] != '\\')
                throw exception;
        }

        private const string ALPHABET = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        private string GetAvailableDriveLetter()
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
            return random ?? throw new InvalidOperationException(Resources.NetworkDrive_ErrorMsg_NoDriversAvailable);
        }

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
    }
}