using Autossential.Activities.Properties;
using Autossential.Core.Security;
using Autossential.Shared.Activities.Constraints;
using System.Activities;

namespace Autossential.Activities.Security.Algorithms
{
    public abstract class SymmetricAlgorithmEncryptionBase<T> : CodeActivity<IEncryption> where T : IEncryption, new()
    {
        public int Iterations { get; set; } = EncryptionBase.MINIMUM_ITERATIONS_RECOMMENDED;

        protected SymmetricAlgorithmEncryptionBase()
        {
            Constraints.Add(ActivityConstraints.CreateConstraint<SymmetricAlgorithmEncryptionBase<T>>(activity => activity is TextEncryption || activity is DataTableEncryption,
              Resources.Validation_ScopesErrorFormat($"({Resources.TextEncryption_DisplayName} or {Resources.DataTableEncryption_DisplayName})")));
        }


        protected override IEncryption Execute(CodeActivityContext context)
        {
            return new T() { Iterations = Iterations };
        }
    }
}

