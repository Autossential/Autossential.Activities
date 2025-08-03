using Autossential.Activities.Properties;
using System.Activities.DesignViewModels;
using System.Collections.Generic;
using System.Data;

namespace Autossential.Activities.ViewModels.Data
{
    internal class DataRowToDictionaryViewModel : BaseViewModel
    {
        public DataRowToDictionaryViewModel(IDesignServices services) : base(services)
        {
        }

        public DesignInArgument<DataRow> InputDataRow { get; set; }

        public DesignOutArgument<Dictionary<string, object>> Result { get; set; }

        protected override void InitializeModel()
        {
            base.InitializeModel();
            PersistValuesChangedDuringInit();

            int orderIndex = 0;
            InputDataRow.IsPrincipal = true;
            InputDataRow.IsRequired = true;
            InputDataRow.Category = Resources.Input_Category;
            InputDataRow.DisplayName = Resources.DataRowToDictionary_InputDataRow_DisplayName;
            InputDataRow.Placeholder = Resources.DataRowToDictionary_InputDataRow_Description;
            InputDataRow.Tooltip = Resources.DataRowToDictionary_InputDataRow_Description;
            InputDataRow.OrderIndex = orderIndex++;

            Result.IsPrincipal = false;
            Result.IsRequired = false;
            Result.Category = Resources.Output_Category;
            Result.DisplayName = Resources.DataRowToDictionary_Result_DisplayName;
            Result.Placeholder = Resources.DataRowToDictionary_Result_Description;
            Result.Tooltip = Resources.DataRowToDictionary_Result_Description;
            Result.OrderIndex = orderIndex++;
        }
    }
}
