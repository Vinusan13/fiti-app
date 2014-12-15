using System.Configuration;
using prefSQL.SQLParser;
using System;
using System.Data.SqlClient;

namespace Fiti.Persistence
{
	public class Database : IDisposable
	{
		private SqlConnection _connection;

		public bool IsConnected()
		{
			return (_connection != null);
		}

		public bool Connect()
		{
			// check if a connection already exists
			if (IsConnected()) {
				return true;
			}

			// create connection according string from app configuraion
			var connectionString = ConfigurationManager.ConnectionStrings["AutoScoutDB"].ConnectionString;
			_connection = new SqlConnection(connectionString);

			// connect to database server
			try {
				_connection.Open();
			} catch (Exception e) {
				Console.WriteLine(e.ToString());
				_connection = null;
				return false;
			}
			return true;

		}

		private void Disconnect()
		{
			if (_connection == null) {
				return;
			}
			_connection.Close();
			_connection = null;
		}

		public string ParsePreferenceQuery(string sql)
		{
			// only parse if preferences are defined (standart sql is currently not supported by library)
			if (!sql.Contains(" PREFERENCE ")) {
				return sql;
			}
			var common = new SQLCommon();
			return common.parsePreferenceSQL(sql);
		}

		public SqlDataReader RunQuery(String sql)
		{
			var parsedSql = ParsePreferenceQuery(sql);
			Connect();
			var myCommand = new SqlCommand(parsedSql, _connection);
			return myCommand.ExecuteReader();
		}

		public void Dispose()
		{
			Disconnect();
		}
	}

}
