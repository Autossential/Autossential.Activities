using Autossential.Activities.Base;
using System.Activities.DesignViewModels;
using System.Data;

namespace Autossential.Activities.ViewModels
{
    internal class DataTableToTextViewModel(IDesignServices services) : BaseViewModel(services)
    {
        public DesignInArgument<DataTable> DataTable { get; set; }
        public DesignProperty<DataTableToText.OutputTextFormat> OutputFormat { get; set; }
        public DesignInArgument<string> DateTimeFormat { get; set; }
        public DesignOutArgument<string> Result { get; set; }

        protected override void InitializeModel()
        {
            base.InitializeModel();
            PersistValuesChangedDuringInit();

            int orderIndex = 0;
            DataTable.IsPrincipal = true;
            DataTable.OrderIndex = orderIndex++;

            OutputFormat.IsPrincipal = true;
            OutputFormat.OrderIndex = orderIndex++;
        }
    }
}
