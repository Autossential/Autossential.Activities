using Autossential.Activities.Properties;
using System.Activities.DesignViewModels;

namespace Autossential.Activities.ViewModels.Data
{
    public class AddRangeToCollectionViewModel : BaseViewModel
    {
        public AddRangeToCollectionViewModel(IDesignServices services) : base(services)
        {
        }

        public DesignInOutArgument Collection { get; set; }

        public DesignInArgument Items { get; set; }
        public DesignProperty<bool> AutoInstantiate { get; set; }

        protected override void InitializeModel()
        {
            base.InitializeModel();
            PersistValuesChangedDuringInit();

            int orderIndex = 0;

            Collection.IsRequired = true;
            Collection.IsPrincipal = true;
            Collection.Category = Resources.Input_Category;
            Collection.Placeholder = Resources.AddRangeToCollection_Collection_Description;
            Collection.Tooltip = Resources.AddRangeToCollection_Collection_Description;
            Collection.DisplayName = Resources.AddRangeToCollection_Collection_DisplayName;
            Collection.OrderIndex = orderIndex++;

            Items.IsRequired = true;
            Items.IsPrincipal = true;
            Items.Category = Resources.Input_Category;
            Items.Placeholder = Resources.AddRangeToCollection_Items_Description;
            Items.Tooltip = Resources.AddRangeToCollection_Items_Description;
            Items.DisplayName = Resources.AddRangeToCollection_Items_DisplayName;
            Items.OrderIndex = orderIndex++;

            AutoInstantiate.IsRequired = false;
            AutoInstantiate.IsPrincipal = false;
            AutoInstantiate.DisplayName = Resources.AddRangeToCollection_AutoInstantiate_DisplayName;
            AutoInstantiate.Tooltip = Resources.AddRangeToCollection_AutoInstantiate_Description;
            AutoInstantiate.Category = Resources.Options_Category;
            AutoInstantiate.OrderIndex = orderIndex++;
        }
    }
}
