using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Autossential.Shared.Activities.Design.Controls
{
    public class CheckBoxControl : CheckBox
    {
		private readonly static Dictionary<int, bool?> _states = new Dictionary<int, bool?>
		{
			{ 0, null },
			{ 1, true },
			{ 2, false }
		};

        protected override void OnToggle()
        {
			var index = _states.First(p => p.Value == IsChecked).Key;
			if (++index >= _states.Count)
				index = 0;

			IsChecked = _states[index];
        }
    }
}