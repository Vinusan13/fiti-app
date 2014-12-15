using System.Collections.Generic;

namespace Arcmedia.PrefCom.WebFrontEnd.Model.QueryTargets
{
	public class QueryTargetFactory
	{
		public QueryTarget CreateDefaultSet()
		{
			var tables = new List<QueryMainTable>
			{
				new QueryMainTable
				{
					Label = "Small",
					Description = "~500",
					TableName = "Cars_small",
					Active = true
				},
				new QueryMainTable
				{
					Label = "Medium",
					Description = "~1'000",
					TableName = "Cars_medium"
				},
				new QueryMainTable
				{
					Label = "Large",
					Description = "~5'000",
					TableName = "Cars_large"
				},
				new QueryMainTable
				{
					Label = "Super Large",
					Description = "~50'000",
					TableName = "Cars_superlarge"
				}
			};

			return new QueryTarget {
				Tables = tables,
				Name = "querytarget"
			};
		}

	}
}