using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Arcmedia.PrefCom.Persistence;
using Arcmedia.PrefCom.WebFrontEnd.Model.Preferences;
using Arcmedia.PrefCom.WebFrontEnd.Model.QueryTargets;

namespace Arcmedia.PrefCom.WebFrontEnd
{
	public partial class Default : Page
	{
		private readonly IDictionary<string, PreferenceSet> _preferences;
		private readonly QueryTarget _queryTarget;

		protected TimeSpan? QueryExecutionTime { get; set; }

		public Default()
		{
			_preferences = new PreferencesFactory().CreateDefaultSet();
			_queryTarget = new QueryTargetFactory().CreateDefaultSet();
		}

		public override bool EnableViewState
		{
			get { return false; }
		}

		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);
			RetrieveActiveOptions();
		}
		
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			SetupDebugInfo();
			RenderTargetOptions();
			RenderPreferenceSets();
			var query = BuildQuery();
			RunQuery(query);
			ShowQuery(query);
		}

		#region Rendering

		protected string ImageView(DataRow row)
		{
			var imageName = row["reference"];
			var imageUrl = ConfigurationManager.AppSettings["ImageUrl"];
			return string.Format(imageUrl, imageName);
		}

		private void ShowQuery(string prefQuery)
		{
			var db = new Database();
			PreferenceQueryBox.Text = prefQuery;
			SqlQueryBox.Text = db.ParsePreferenceQuery(prefQuery);
		}
		private void RenderTargetOptions()
		{
			QueryTargetRepeater.DataSource = CreateTargetsSource();
			QueryTargetRepeater.DataBind();
		}

		private void RenderPreferenceSets()
		{
			PreferenceSetsRepeater.ItemDataBound += PreferenceSetsRepeaterItemDataBound;
			PreferenceSetsRepeater.DataSource = CreatePreferenceSetsSource();
			PreferenceSetsRepeater.DataBind();
		}

		void PreferenceSetsRepeaterItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			// check item type
			var item = e.Item;
			if (!(item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)) {
				return;
			}

			// retrieve tariff cost
			var view = (DataRowView) item.DataItem;
			var setName = view.Row["name"] as String;
			var set = _preferences[setName];

			// create view
			var repeater = (Repeater) item.FindControl("PreferenceOptionsRepeater");
			repeater.DataSource = CreatePreferenceOptionsSource(set);
			repeater.DataBind();
		}

		private DataTable CreateTargetsSource()
		{
			// prepare table
			var table = new DataTable();
			table.Columns.Add("classes");
			table.Columns.Add("set");
			table.Columns.Add("value");
			table.Columns.Add("label");
			table.Columns.Add("description");

			// iterate sets
			foreach (var target in _queryTarget.Tables) {
				var row = table.NewRow();
				row["set"] = _queryTarget.Name;
				row["label"] = target.Label;
				row["description"] = target.Description;
				row["value"] = target.TableName;
				row["classes"] += target.Active ? " active" : string.Empty;
				table.Rows.Add(row);
			}

			return table;
		}

		private DataTable CreatePreferenceSetsSource()
		{
			// prepare table
			var table = new DataTable();
			table.Columns.Add("name");
			table.Columns.Add("label");
			table.Columns.Add("cols");

			// auto layout
			var width = GetColWidths(_preferences.Values);

			// iterate sets
			foreach (var pair in _preferences) {
				var set = pair.Value;
				var row = table.NewRow();
				row["name"] = set.Name;
				row["label"] = set.Label;
				row["cols"] = width[set];
				table.Rows.Add(row);
			}

			return table;
		}

		private IDictionary<PreferenceSet, int> GetColWidths(ICollection<PreferenceSet> sets)
		{
			// determine text length
			var textLengths = new Dictionary<PreferenceSet, int>();
			foreach (var set in sets) {
				var length = set.Options.Max(o => o.Label.Length);
				textLengths[set] = length;
			}

			// set col size according to text size
			var colWidths = new Dictionary<PreferenceSet, int>();
			var maxDiff = 0.0;
			var maxAdjuted = sets.First();
			var totalColWidths = textLengths.Sum(p => p.Value);
			const int minWidth = 2;
			foreach (var set in sets) {
				var exactWidth = 12.0 / totalColWidths * textLengths[set];
				var ceiledWidth = (int) Math.Ceiling(exactWidth);
				colWidths[set] = ceiledWidth;
				var diff = ceiledWidth - exactWidth;
				if ((ceiledWidth > minWidth) && diff >= maxDiff) {
					maxDiff = diff;
					maxAdjuted = set;
				}
			}

			// adjust to exact 12
			var totalSize = colWidths.Sum(p => p.Value);
			if (totalSize > 12) {
				var correctBy = totalSize - 12;
				colWidths[maxAdjuted] -= correctBy;
			}

			return colWidths;

		}

		private DataTable CreatePreferenceOptionsSource(PreferenceSet set)
		{
			// prepare table
			var table = new DataTable();
			table.Columns.Add("set");
			table.Columns.Add("value");
			table.Columns.Add("classes");
			table.Columns.Add("label");

			// iterate sets
			foreach (var option in set.Options) {
				var row = table.NewRow();
				row["set"] = set.Name;
				row["value"] = option.Value;
				row["classes"] += option.Active ? " active" : string.Empty;
				row["label"] = option.Label;
				table.Rows.Add(row);
			}

			return table;
		}

		private void SetupDebugInfo()
		{
#if DEBUG
			DebugInfo.Visible = true;
#else
			DebugInfo.Visible = false;
#endif
		}

		private void ShowResults(DataTable dataTable)
		{
			//dataTable.Columns["reference"].
			ResultsGrid.DataSource = dataTable;
			ResultsGrid.DataBind();
			ResultsGrid.Visible = true;
			ResultsGrid.HeaderRow.TableSection = TableRowSection.TableHeader;
			MessageLabel.Visible = false;
		}

		private void ShowError(Exception ex)
		{
			ResultsGrid.Visible = false;
			MessageLabel.Text = ex.Message;
			MessageLabel.Visible = true;
		}

		#endregion

		#region Logic

		private void RetrieveActiveOptions()
		{
			if (!IsPostBack) {
				return;
			}
			var database = Request.Params[_queryTarget.Name];
			_queryTarget.MainTable = database;
			foreach (var pair in _preferences) {
				var postBack = Request.Params[pair.Value.Name];
				pair.Value.ActiveValue = postBack;
			}
		}

		private string BuildQuery()
		{
			var prefList = _preferences.Values.Select(set => set.ActiveValue).ToList();
			var prefSql = String.Join(" AND ", prefList);
			var baseQuery = ConfigurationManager.AppSettings["BaseQuery"];
			var preferenceQuery = baseQuery + " " + prefSql;
			var finalQuery = preferenceQuery.Replace("CARSTABLE", _queryTarget.MainTable);
			return finalQuery;
		}

		private void RunQuery(string sql)
		{

			try {
				var start = DateTime.Now;
				var db = new Database();
				var reader = db.RunQuery(sql);
				var dt = new DataTable();
				dt.Load(reader);
				QueryExecutionTime = DateTime.Now - start;
				ShowResults(dt);
			} catch (Exception ex) {
				QueryExecutionTime = null;
				ShowError(ex);
			}

		}

		#endregion


	}
}