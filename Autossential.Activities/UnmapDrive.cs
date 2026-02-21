using System.Activities;

namespace Autossential.Activities
{
    public sealed class UnmapDrive : NetworkDrive
    {
        [RequiredArgument]
        public InArgument<string> DriveLetter { get; set; }

        protected override bool Execute(CodeActivityContext context)
        {
            var driveLetter = DriveLetter.Get(context) ?? throw new NullReferenceException(nameof(DriveLetter));
            ValidateDriveLetterFormat(driveLetter);

            var result = DisconnectDrive(driveLetter);
            ResponseCode.Set(context, result);

            SetResponseMessageFromCode(context, result);
            return result == 0;
        }
    }
}