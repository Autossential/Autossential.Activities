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
            var result = WNetCancelConnection2A(drive, 0, true);
            return result == 0;
        }
    }
}