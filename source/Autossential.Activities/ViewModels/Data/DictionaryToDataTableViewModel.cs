using Autossential.Activities.Properties;
using System.Activities.DesignViewModels;
using System.Collections.Generic;
using System.Data;

namespace Autossential.Activities.ViewModels.Data
{
    public class DictionaryToDataTableViewModel : BaseViewModel
    {
        public DictionaryToDataTableViewModel(IDesignServices services) : base(services)
        {
        }

        public DesignInArgument<Dictionary<string, object>> InputDictionary { get; set; }

        public DesignOutArgument<DataTable> Result { get; set; }

        protected override void InitializeModel()
        {
            base.InitializeModel();
            PersistValuesChangedDuringInit();

            int orderIndex = 0;
            InputDictionary.IsPrincipal = true;
            InputDictionary.IsRequired = true;
            InputDictionary.Category = Resources.Input_Category;
            InputDictionary.DisplayName = Resources.DictionaryToDataTable_InputDictionary_DisplayName;
            InputDictionary.Placeholder = Resources.DictionaryToDataTable_InputDictionary_Description;
            InputDictionary.OrderIndex = orderIndex++;

            Result.IsPrincipal = false;
            Result.IsRequired = false;
            Result.Category = Resources.Output_Category;
            Result.DisplayName = Resources.DictionaryToDataTable_Result_DisplayName;
            Result.Placeholder = Resources.DictionaryToDataTable_Result_Description;
            Result.OrderIndex = orderIndex++;
        }
    }
}