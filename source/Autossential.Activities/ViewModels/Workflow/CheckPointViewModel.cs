using Autossential.Activities.Properties;
using System;
using System.Activities.DesignViewModels;
using System.Collections.Generic;

namespace Autossential.Activities.ViewModels.Workflow
{
    internal class CheckPointViewModel : BaseViewModel
    {
        public CheckPointViewModel(IDesignServices services) : base(services) { }

        public DesignInArgument<bool> Expression { get; set; }
        public DesignInArgument<Exception> Exception { get; set; }
        public DesignProperty<Dictionary<string, object>> Data { get; set; }

        protected override void InitializeModel()
        {
            base.InitializeModel();
            PersistValuesChangedDuringInit();

            int orderIndex = 0;
            Expression.IsPrincipal = true;
            Expression.IsRequired = true;
            Expression.Category = Resources.Input_Category;
            Expression.DisplayName = Resources.CheckPoint_Expression_DisplayName;
            Expression.Placeholder = Resources.CheckPoint_Expression_Description;
            Expression.Tooltip = Resources.CheckPoint_Expression_Description;
            Expression.OrderIndex = orderIndex++;

            Exception.IsPrincipal = true;
            Exception.IsRequired = true;
            Exception.Category = Resources.Input_Category;
            Exception.DisplayName = Resources.CheckPoint_Exception_DisplayName;
            Exception.Placeholder = Resources.CheckPoint_Exception_Description;
            Exception.Tooltip = Resources.CheckPoint_Exception_Description;
            Exception.OrderIndex = orderIndex++;

            Data.IsPrincipal = false;
            Data.IsRequired = false;
            Data.Category = Resources.Input_Category;
            Data.DisplayName = Resources.CheckPoint_Data_DisplayName;
            Data.Placeholder = Resources.CheckPoint_Data_Description;
            Data.Tooltip = Resources.CheckPoint_Data_Description;
            Data.OrderIndex = orderIndex++;
        }
    }
}
