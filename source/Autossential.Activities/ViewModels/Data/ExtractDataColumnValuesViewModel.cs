using Autossential.Activities.Properties;
using Autossential.Core.Enums;
using System;
using System.Activities.DesignViewModels;
using System.Activities.ViewModels;
using System.Activities.ViewModels.Interfaces;
using System.Data;
using UiPath.Studio.Activities.Api;

namespace Autossential.Activities.ViewModels.Data
{
    public class ExtractDataColumnValuesViewModel<T> : BaseViewModel
    {
        private readonly IWorkflowDesignApi _workflowDesignAPI;
        private bool _typePickerWidgetAvailable;

        public ExtractDataColumnValuesViewModel(IDesignServices services) : base(services)
        {
            _workflowDesignAPI = services.GetService<IWorkflowDesignApi>();
            _typePickerWidgetAvailable = IsWidgetSupported(ViewModelWidgetType.TypePicker);
        }

        public DesignInArgument<DataTable> InputDataTable { get; set; }
        public DesignInArgument Column { get; set; }
        public DesignInArgument<T> DefaultValue { get; set; }
        public DesignInArgument<char[]> Trim { get; set; }
        public DesignInArgument<bool> Sanitize { get; set; }
        public DesignInArgument<bool> Unique { get; set; }
        public DesignProperty<TextCase> TextCase { get; set; }
        public DesignOutArgument<T[]> Result { get; set; }

        [NotMappedProperty]
        public DesignProperty<Type> ArgumentType { get; set; }

        protected override void InitializeModel()
        {
            base.InitializeModel();
            PersistValuesChangedDuringInit();

            int orderIndex = 0;

            ArgumentType.IsVisible = true;
            ArgumentType.Category = Resources.Options_Category;
            ArgumentType.OrderIndex = orderIndex++;

            InputDataTable.IsPrincipal = true;
            InputDataTable.IsRequired = true;
            InputDataTable.Category = Resources.Input_Category;
            InputDataTable.DisplayName = Resources.ExtractDataColumnValues_InputDataTable_DisplayName;
            InputDataTable.Placeholder = Resources.ExtractDataColumnValues_InputDataTable_Description;
            InputDataTable.Tooltip = Resources.ExtractDataColumnValues_InputDataTable_Description;
            InputDataTable.OrderIndex = orderIndex++;

            Column.IsPrincipal = true;
            Column.IsRequired = true;
            Column.Category = Resources.Input_Category;
            Column.DisplayName = Resources.ExtractDataColumnValues_Column_DisplayName;
            Column.Placeholder = Resources.ExtractDataColumnValues_Column_Description;
            Column.Tooltip = Resources.ExtractDataColumnValues_Column_Description;
            Column.OrderIndex = orderIndex++;

            DefaultValue.IsPrincipal = false;
            DefaultValue.IsRequired = false;
            DefaultValue.Category = Resources.Options_Category;
            DefaultValue.DisplayName = Resources.ExtractDataColumnValues_DefaultValue_DisplayName;
            DefaultValue.Placeholder = Resources.ExtractDataColumnValues_DefaultValue_Description;
            DefaultValue.Tooltip = Resources.ExtractDataColumnValues_DefaultValue_Description;
            DefaultValue.OrderIndex = orderIndex++;

            Trim.IsPrincipal = false;
            Trim.IsRequired = false;
            Trim.Category = Resources.Options_Category;
            Trim.DisplayName = Resources.ExtractDataColumnValues_Trim_DisplayName;
            Trim.Placeholder = Resources.ExtractDataColumnValues_Trim_Description;
            Trim.Tooltip = Resources.ExtractDataColumnValues_Trim_Description;
            Trim.OrderIndex = orderIndex++;

            Sanitize.IsPrincipal = false;
            Sanitize.IsRequired = false;
            Sanitize.Category = Resources.Options_Category;
            Sanitize.DisplayName = Resources.ExtractDataColumnValues_Sanitize_DisplayName;
            Sanitize.Placeholder = Resources.ExtractDataColumnValues_Sanitize_Description;
            Sanitize.Tooltip = Resources.ExtractDataColumnValues_Sanitize_Description;
            Sanitize.Widget = new DefaultWidget
            {
                Type = ViewModelWidgetType.Toggle
            };
            Sanitize.OrderIndex = orderIndex++;

            Unique.IsPrincipal = false;
            Unique.IsRequired = false;
            Unique.Category = Resources.Options_Category;
            Unique.DisplayName = Resources.ExtractDataColumnValues_Unique_DisplayName;
            Unique.Placeholder = Resources.ExtractDataColumnValues_Unique_Description;
            Unique.Tooltip = Resources.ExtractDataColumnValues_Unique_Description;
            Unique.Widget = new DefaultWidget
            {
                Type = ViewModelWidgetType.Toggle
            };
            Unique.OrderIndex = orderIndex++;

            TextCase.IsPrincipal = false;
            TextCase.IsRequired = false;
            TextCase.Category = Resources.Options_Category;
            TextCase.DisplayName = Resources.ExtractDataColumnValues_TextCase_DisplayName;
            TextCase.Placeholder = Resources.ExtractDataColumnValues_TextCase_Description;
            TextCase.Tooltip = Resources.ExtractDataColumnValues_TextCase_Description;
            TextCase.OrderIndex = orderIndex++;

            Result.IsPrincipal = false;
            Result.IsRequired = false;
            Result.Category = Resources.Output_Category;
            Result.DisplayName = Resources.ExtractDataColumnValues_Result_DisplayName;
            Result.Placeholder = Resources.ExtractDataColumnValues_Result_Description;
            Result.Tooltip = Resources.ExtractDataColumnValues_Result_Description;
            Result.OrderIndex = orderIndex++;

            if (_typePickerWidgetAvailable)
            {
                AddTypePicker();
            }
        }

        private void AddTypePicker()
        {
            ArgumentType.Widget = new TypePickerWidget();
            ArgumentType.Value = ModelItem.ItemType.GenericTypeArguments[0];
        }

        protected override void ManualRegisterDependencies()
        {
            base.ManualRegisterDependencies();
            if (_typePickerWidgetAvailable)
                RegisterDependency(ArgumentType, nameof(ArgumentType.Value), nameof(ArgumentType));
        }

        protected override void InitializeRules()
        {
            base.InitializeRules();
            if (_typePickerWidgetAvailable)
                Rule(nameof(ArgumentType), MorphArgumentTypeAsync, false);
        }

        private async void MorphArgumentTypeAsync()
        {
            if (!ArgumentType.HasValue || ArgumentType.Value == ModelItem.ItemType.GenericTypeArguments[0])
                return;

            var service = Services.GetService<IDesignerStaticTypesService>();
            if (service == null)
                return;

            await service.UseTypeAsync(ArgumentType.Value);
        }
    }
}