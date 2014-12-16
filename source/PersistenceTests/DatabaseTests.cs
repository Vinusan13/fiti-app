
using Fiti.Persistence;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PersistenceTests
{
	[TestClass]
	public class DatabaseTests
	{
		[TestMethod]
		public void TestConnectivity()
		{
			var db = new Database();
			var result = db.Connect();
			Assert.IsTrue(result);
		}

		[TestMethod]
		public void TestPureQuery()
		{
			var db = new Database();
			var reader = db.RunQuery("SELECT TOP 1 cars.Id, cars.Title FROM cars");
			while (reader.Read()) {
				System.Diagnostics.Debug.WriteLine(reader["Id"].ToString());
				System.Diagnostics.Debug.WriteLine(reader["Title"].ToString());
			}
		}

		[TestMethod]
		public void TestPreferenceQuery()
		{
			var db = new Database();
			var reader = db.RunQuery("SELECT cars.Id, cars.Title FROM cars PREFERENCE LOW cars.Id");
			while (reader.Read()) {
				System.Diagnostics.Debug.WriteLine(reader["Id"].ToString());
				System.Diagnostics.Debug.WriteLine(reader["Title"].ToString());
			}
		}

	}
}
