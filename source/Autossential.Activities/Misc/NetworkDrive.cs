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

        protected string GetNormalizedDriveLetter(CodeActivityContext context, bool allowAutoSelection)
        {
            var name = DriveLetter.Get(context);
            if (string.IsNullOrEmpty(name) && allowAutoSelection)
            {
                name = GetAvailableDriveLetter();
            }
            else if (name.Trim().Length == 0 || !ALPHABET.Contains(char.ToUpper(name[0])) || name.Length > 2 || (name.Length > 1 && name[1] != ':'))
            {
                throw new ArgumentException(Resources.NetworkDrive_ErrorMsg_InvalidDriveLetter, nameof(DriveLetter));
            }

            return char.ToUpper(name[0]) + ":";
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