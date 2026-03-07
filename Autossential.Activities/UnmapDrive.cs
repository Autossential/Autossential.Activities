using Autossential.Activities.Properties;
using System.Activities;

namespace Autossential.Activities
{
    public sealed class UnmapDrive : NetworkDrive
    {
        [RequiredArgument]
        public InArgument<string> DriveLetter { get; set; }

        protected override bool Execute(CodeActivityContext context)
        {
            var driveLetter = DriveLetter.Get(context) ?? throw new InvalidOperationException(ResourcesFn.Common_ErrorMsg_ValueNotSuppliedFormat(Resources.UnmapDrive_DriveLetter_DisplayName));
            ValidateDriveLetterFormat(driveLetter);

            var result = DisconnectDrive(driveLetter);
            ResponseCode.Set(context, result);

            SetResponseMessageFromCode(context, result);
            return result == 0;
        }
    }
}