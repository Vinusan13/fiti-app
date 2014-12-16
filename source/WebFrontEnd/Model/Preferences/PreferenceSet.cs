using System.Collections.Generic;
using System.Linq;
using WebGrease.Css.Extensions;

namespace Fiti.WebFrontEnd.Model.Preferences
{
	public class PreferenceSet
	{
		public string Name { get; set; }
		public string Label { get; set; }
		public IEnumerable<PreferenceOption> Options { get; set; }

		public PreferenceOption this[string value]
		{
			get { return Options.FirstOrDefault(o => o.Value.Equals(value)); }
		}

		public string ActiveValue
		{
			get
			{
				var active = Options.FirstOrDefault(o => o.Active);
				return (active == null) ? null : active.Value;
			}
			set
			{
				var option = this[value];
				if (option == null) {
					return;
				}
				Options.ForEach(o => o.Active = false);
				option.Active = true;
			}
		}
	}
}