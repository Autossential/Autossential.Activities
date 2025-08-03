using Autossential.Shared;
using System;
using System.Activities.DesignViewModels;
using System.Activities.ViewModels;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Autossential.Activities.ViewModels.Programming
{
    internal class RandomStringViewModel : BaseViewModel
    {
        public RandomStringViewModel(IDesignServices services) : base(services)
        {

        }

        public DesignInArgument<string> Format { get; set; }
        public DesignInArgument<string> Custom { get; set; }
        public DesignOutArgument<string> Result { get; set; }

        [NotMappedProperty]
        public DesignProperty<string> Instruction { get; set; }

        private readonly ThreadLocal<Random> _rng = new ThreadLocal<Random>(() => new Random());

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
            Format.Category = "Input";
            Format.Placeholder = "Enter the desired pattern";
            Format.DisplayName = "Format";
            Format.Tooltip = "Enter the desired pattern";
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

            Custom.IsRequired = false;
            Custom.IsPrincipal = true;
            Custom.OrderIndex = orderIndex++;
            Custom.Category = "Input";
            Custom.Placeholder = "Enter any additional characters";
            Custom.DisplayName = "Custom";
            Custom.Tooltip = "Enter any additional characters";
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

                    Custom.Value = string.Join("", hash).Replace("\"", ExpressionServiceLanguage.IsCSharpEnv(ModelItem) ? "\\\"" : "\"\"");
                })
            });

            Result.IsRequired = false;
            Result.IsPrincipal = false;
            Result.OrderIndex = orderIndex++;
            Result.Category = "Output";
            Result.Placeholder = "The generated string";
            Result.DisplayName = "Result";
            Result.Tooltip = "The generated string";
        }
    }
}
