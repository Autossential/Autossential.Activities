using Autossential.Activities.Base;
using System.Activities;
using System.Activities.DesignViewModels;

namespace Autossential.Activities.ViewModels
{
    internal class CheckPointViewModel(IDesignServices services) : BaseViewModel(services)
    {
        public DesignInArgument<bool> Expression { get; set; }
        public DesignInArgument<Exception> Exception { get; set; }
        public DesignInArgument<Dictionary<string, string>> Data { get; set; }
        protected override void InitializeModel()
        {
            base.InitializeModel();
            PersistValuesChangedDuringInit();

            int orderIndex = 0;
            Expression.IsPrincipal = true;
            Expression.OrderIndex = orderIndex++;

            Exception.IsPrincipal = true;
            Exception.OrderIndex = orderIndex++;
        }
    }
}
