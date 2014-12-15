using System.Collections.Generic;
using System.Linq;

namespace Arcmedia.PrefCom.WebFrontEnd.Model.Preferences
{
	public class PreferencesFactory
	{
		public IDictionary<string, PreferenceSet> CreateDefaultSet()
		{
			var ret = new List<PreferenceSet> { CreatePriceOptions(), CreateColorOptions(), CreateBodyOptions() };
			return ret.ToDictionary(set => set.Name);
		}

		public PreferenceSet CreatePriceOptions()
		{
			var options = new List<PreferenceOption>
			{
				new PreferenceOption
				{
					Label = "Low",
					Value = "LOW CARSTABLE.price",
					Active = true
				},
				new PreferenceOption
				{
					Label = "High",
					Value = "HIGH CARSTABLE.price"
				},
				new PreferenceOption
				{
					Label = "Around 10'000",
					Value = "CARSTABLE.price AROUND 10000"
				}
			};

			return new PreferenceSet {
				Name = "price",
				Label = "Price",
				Options = options
			};
		}

		public PreferenceSet CreateColorOptions()
		{
			var options = new List<PreferenceOption>
			{
				new PreferenceOption
				{
					Label = "Red > all others equal",
					Value = "HIGH colors.name {'rot' >> OTHERSEQUAL}",
					Active = true
				},
				new PreferenceOption
				{
					Label = "Red > others",
					Value = "HIGH colors.name {'rot' >> OTHERS}"
				},
				new PreferenceOption
				{
					Label = "Pink > {Red, Black} > Beige > others",
					Value = "HIGH colors.name {'pink' >> {'rot', 'schwarz'} >> 'beige' >> OTHERS}"
				}
			};

			return new PreferenceSet {
				Name = "color",
				Label = "Color",
				Options = options
			};
		}

		public PreferenceSet CreateBodyOptions()
		{
			var options = new List<PreferenceOption>
			{
				new PreferenceOption
				{
					Label = "Van > Compact Car",
					Value = "HIGH bodies.name {'Bus' >> 'Kleinwagen'}",
					Active = true
				},
				new PreferenceOption
				{
					Label = "Van > Compact Car > all others equal",
					Value = "HIGH bodies.name {'Bus' >> 'Kleinwagen' >> OTHERSEQUAL}"
				},
				new PreferenceOption
				{
					Label = "Compact Car > Van > Station Wagon > Scooter > all others > Pick Up",
					Value = "HIGH bodies.name {'Kleinwagen' >> 'Bus' >> 'Kombi' >> 'Roller' >> OTHERSEQUAL >> 'Pick-Up'}"
				}
			};

			return new PreferenceSet {
				Name = "body",
				Label = "Body",
				Options = options
			};
		}
	}
}