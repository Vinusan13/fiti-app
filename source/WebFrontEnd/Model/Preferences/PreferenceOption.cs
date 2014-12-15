namespace Arcmedia.PrefCom.WebFrontEnd.Model.Preferences
{
	public class PreferenceOption
	{
		public string Value { get; set; }
		public string Label { get; set; }
		public bool Active { get; set; }

		public PreferenceOption()
		{
			Active = false;
		}
	}
}