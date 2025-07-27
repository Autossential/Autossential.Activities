using Autossential.Activities.Properties;
using System.Activities;

namespace Autossential.Activities
{
    public sealed class UnmapDrive : NetworkDrive
    {
        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            base.CacheMetadata(metadata);
            if (DriveLetter == null)
            {
                metadata.AddValidationError(Resources.Validation_ValueErrorFormat(nameof(DriveLetter)));
            }
        }

        protected override bool Execute(CodeActivityContext context)
        {
            var drive = GetNormalizedDriveLetter(context, false);
            var result = DisconnectDrive(drive);
            ResponseCode.Set(context, result);

            if (result == 0)
            {
                ResponseMessage.Set(context, "Success");
                return true;
            }

            return Fail(context, result);
        }
    }
}