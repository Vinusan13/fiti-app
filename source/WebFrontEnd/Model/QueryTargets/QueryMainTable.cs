namespace Arcmedia.PrefCom.WebFrontEnd.Model.QueryTargets
{
	public class QueryMainTable
	{
		public string TableName { get; set; }
		public string Label { get; set; }
		public string Description { get; set; }
		public bool Active { get; set; }

		public QueryMainTable()
		{
			Active = false;
		}
	}
}