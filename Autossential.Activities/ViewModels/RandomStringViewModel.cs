using Autossential.Activities.Base;
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
            Instruction.DisplayName = "Instructions";
            Instruction.Widget = new TextBlockWidget
            {
                Level = TextBlockWidgetLevel.Info,
                Multiline = true
            };
            Instruction.OrderIndex = orderIndex++;
            Instruction.Value =
@"You can use the following placeholders:
- a: for lowercase letters
- A: for uppercase letters
- 0: for digits
- ?: for characters from Custom property
- *: for any of the placeholders above

To escape a character, use the backslash '\'.

Any other character will be treated as a literal";

            Custom.IsPrincipal = true;
            Custom.OrderIndex = orderIndex++;

            if (IsWidgetSupported(ViewModelWidgetType.ActionButton))
            {
                Format.AddMenuAction(new MenuAction
                {
                    DisplayName = "Show instructions",
                    IsVisible = true,
                    IsMain = true,
                    Handler = menu => Task.Run(() =>
                    {
                        Instruction.IsVisible = !Instruction.IsVisible;
                        menu.DisplayName = Instruction.IsVisible ? "Hide instructions" : "Show instructions";
                    })
                });

                Custom.AddMenuAction(new MenuAction
                {
                    DisplayName = "Generate",
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
