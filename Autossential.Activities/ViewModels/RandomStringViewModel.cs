using Autossential.Activities.Base;
using Autossential.Activities.Properties;
using System.Activities.DesignViewModels;
using System.Activities.ViewModels;

namespace Autossential.Activities.ViewModels
{
    internal class RandomStringViewModel(IDesignServices services) : BaseViewModel(services)
    {
        public DesignInArgument<string> Format { get; set; }
        public DesignInArgument<string> Custom { get; set; }
        public DesignOutArgument<string> Result { get; set; }

        [NotMappedProperty]
        public DesignProperty<string> Instruction { get; set; }

        private readonly ThreadLocal<Random> _rng = new(() => new Random());

        private const string specialChars = @"!@#$%^&*()_+{}|:""<>?`-=[]\;',./";

        protected override void InitializeModel()
        {
            base.InitializeModel();
            PersistValuesChangedDuringInit();

            int orderIndex = 0;
            var rng = _rng.Value;

            Format.IsRequired = true;
            Format.IsPrincipal = true;
            Format.OrderIndex = orderIndex++;

            Instruction.IsPrincipal = true;
            Instruction.IsVisible = false;
            Instruction.Widget = new TextBlockWidget
            {
                Level = TextBlockWidgetLevel.Info,
                Multiline = true
            };
            Instruction.OrderIndex = orderIndex++;
            Instruction.Value = Resources.RandomString_ViewModel_Instructions;

            Custom.IsPrincipal = true;
            Custom.OrderIndex = orderIndex++;

            if (IsWidgetSupported(ViewModelWidgetType.ActionButton))
            {
                Format.AddMenuAction(new MenuAction
                {
                    DisplayName = Resources.RandomString_ViewModel_ShowInstructions,
                    IsVisible = true,
                    IsMain = true,
                    Handler = menu => Task.Run(() =>
                    {
                        Instruction.IsVisible = !Instruction.IsVisible;
                        menu.DisplayName = Instruction.IsVisible ? Resources.RandomString_ViewModel_HideInstructions : Resources.RandomString_ViewModel_ShowInstructions;
                    })
                });

                Custom.AddMenuAction(new MenuAction
                {
                    DisplayName = Resources.RandomString_ViewModel_Generate,
                    IsVisible = true,
                    IsMain = true,
                    Handler = _ => Task.Run(() =>
                    {
                        var hash = new HashSet<char>();
                        for (int i = 0; i < rng.Next(4, specialChars.Length); i++)
                            hash.Add(specialChars[rng.Next(0, specialChars.Length)]);

                        Custom.Value = string.Join("", hash).Replace("\"", IsCSharpProject() ? "\\\"" : "\"\"");
                    })
                });
            }
        }
    }
}
