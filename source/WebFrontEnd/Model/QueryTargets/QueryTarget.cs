using System.Collections.Generic;
using System.Linq;
using WebGrease.Css.Extensions;

namespace Arcmedia.PrefCom.WebFrontEnd.Model.QueryTargets
{
	public class QueryTarget
	{
		public IEnumerable<QueryMainTable> Tables { get; set; }
		public string Name { get; set; }

		public QueryMainTable this[string tableName]
		{
			get { return Tables.FirstOrDefault(o => o.TableName.Equals(tableName)); }
		}

		public string MainTable
		{
			get
			{
				var active = Tables.FirstOrDefault(o => o.Active);
				return (active == null) ? null : active.TableName;
			}
			set
			{
				var table = this[value];
				if (table == null) {
					return;
				}
				Tables.ForEach(o => o.Active = false);
				table.Active = true;
			}
		}
	}
}