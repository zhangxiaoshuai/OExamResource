using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Dynamic;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Providers
{
	public class SqlHelper
	{
		private readonly static string connstr;

		static SqlHelper()
		{
			SqlHelper.connstr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
		}

		public SqlHelper()
		{
		}

		private static List<T> ConvertToList<T>(DataTable dt)
		where T : new()
		{
			T t1;
			Type type = typeof(T);
			List<T> list = new List<T>();
			PropertyInfo[] propertys = type.GetProperties();
			string name = string.Empty;
			foreach (DataRow dr in dt.Rows)
			{
				T t2 = default(T);
				if (t2 == null)
				{
					t1 = Activator.CreateInstance<T>();
				}
				else
				{
					t2 = default(T);
					t1 = t2;
				}
				T t = t1;
				PropertyInfo[] propertyInfoArray = propertys;
				for (int i = 0; i < (int)propertyInfoArray.Length; i++)
				{
					PropertyInfo pi = propertyInfoArray[i];
					name = pi.Name;
					if (dt.Columns.Contains(name))
					{
						if (!pi.CanWrite)
						{
                            continue;
						}
						object value = SqlHelper.ToCSharpValue(dr[name]);
						if (null != value)
						{
							pi.SetValue(t, value, null);
						}
					}
				
				}
				list.Add(t);
			}
			return list;
		}

		private static List<dynamic> ConvertToList(DataTable dt)
		{
			List<object> list = new List<object>();
			foreach (DataRow dr in dt.Rows)
			{
				IDictionary<string, object> t = new ExpandoObject();
				foreach (DataColumn dc in dt.Columns)
				{
					string name = dc.ColumnName;
					object value = SqlHelper.ToCSharpValue(dr[name]);
					t.Add(name, value);
				}
				list.Add((dynamic)t);
			}
			return list;
		}

		public static DataTable ExcelToDataTable(string pathName, string sheetName)
		{
			OleDbConnection cnnxls;
			OleDbDataAdapter oda;
			DataSet ds;
			DataTable dataTable;
			int num;
			DataTable tbContainer = new DataTable();
			string strConn = string.Empty;
			if (string.IsNullOrEmpty(sheetName))
			{
				sheetName = "Sheet1";
			}
			FileInfo file = new FileInfo(pathName);
			if (!file.Exists)
			{
				throw new Exception("文件不存在");
			}
			string extension = file.Extension;
			if (extension != null)
			{
				if (extension == ".xls")
				{
					strConn = string.Concat("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=", pathName, ";Extended Properties='Excel 8.0;HDR=Yes;IMEX=1;'");
					cnnxls = new OleDbConnection(strConn);
					oda = new OleDbDataAdapter(string.Format("select * from [{0}$]", sheetName), cnnxls);
					ds = new DataSet();
					num = oda.Fill(tbContainer);
					dataTable = tbContainer;
					return dataTable;
				}
				else
				{
					if (extension != ".xlsx")
					{
						strConn = string.Concat("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=", pathName, ";Extended Properties='Excel 8.0;HDR=Yes;IMEX=1;'");
						cnnxls = new OleDbConnection(strConn);
						oda = new OleDbDataAdapter(string.Format("select * from [{0}$]", sheetName), cnnxls);
						ds = new DataSet();
						num = oda.Fill(tbContainer);
						dataTable = tbContainer;
						return dataTable;
					}
					strConn = string.Concat("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=", pathName, ";Extended Properties='Excel 12.0;HDR=Yes;IMEX=1;'");
					cnnxls = new OleDbConnection(strConn);
					oda = new OleDbDataAdapter(string.Format("select * from [{0}$]", sheetName), cnnxls);
					ds = new DataSet();
					num = oda.Fill(tbContainer);
					dataTable = tbContainer;
					return dataTable;
				}
			}
			strConn = string.Concat("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=", pathName, ";Extended Properties='Excel 8.0;HDR=Yes;IMEX=1;'");
			cnnxls = new OleDbConnection(strConn);
			oda = new OleDbDataAdapter(string.Format("select * from [{0}$]", sheetName), cnnxls);
			ds = new DataSet();
			num = oda.Fill(tbContainer);
			dataTable = tbContainer;
			return dataTable;
		}

		private static DataTable ExecuteDataReader(string sql, params SqlParameter[] parameters)
		{
			DataTable dt = new DataTable();
			SqlConnection conn = new SqlConnection(SqlHelper.connstr);
			try
			{
				conn.Open();
				SqlCommand cmd = conn.CreateCommand();
				try
				{
					cmd.CommandText = sql;
					cmd.Parameters.AddRange(parameters);
					SqlDataReader reader = cmd.ExecuteReader();
					try
					{
						dt.Load(reader);
					}
					finally
					{
						if (reader != null)
						{
							((IDisposable)reader).Dispose();
						}
					}
				}
				finally
				{
					if (cmd != null)
					{
						((IDisposable)cmd).Dispose();
					}
				}
			}
			finally
			{
				if (conn != null)
				{
					((IDisposable)conn).Dispose();
				}
			}
			return dt;
		}

		public static DataTable ExecuteDataTable(string sql, params SqlParameter[] parameters)
		{
			DataTable dataTable;
			SqlConnection cn = new SqlConnection(SqlHelper.connstr);
			try
			{
				cn.Open();
				SqlCommand cmd = cn.CreateCommand();
				try
				{
					cmd.CommandText = sql;
					cmd.Parameters.AddRange(parameters);
					SqlDataAdapter adapter = new SqlDataAdapter(cmd);
					try
					{
						DataTable dt = new DataTable();
						adapter.Fill(dt);
						dataTable = dt;
					}
					finally
					{
						if (adapter != null)
						{
							((IDisposable)adapter).Dispose();
						}
					}
				}
				finally
				{
					if (cmd != null)
					{
						((IDisposable)cmd).Dispose();
					}
				}
			}
			finally
			{
				if (cn != null)
				{
					((IDisposable)cn).Dispose();
				}
			}
			return dataTable;
		}

		internal static dynamic ExecuteEntity(string sql, params SqlParameter[] parameters)
		{
			List<object> list = SqlHelper.ConvertToList(SqlHelper.ExecuteDataTable(sql, parameters));
			return (list.Count > 0 ? list[0] : null);
		}

		internal static T ExecuteEntity<T>(string sql, params SqlParameter[] parameters)
		where T : new()
		{
			List<T> list = SqlHelper.ConvertToList<T>(SqlHelper.ExecuteDataTable(sql, parameters));
			return (list.Count > 0 ? list[0] : default(T));
		}

		internal static List<dynamic> ExecuteList(string sql, params SqlParameter[] parameters)
		{
			return SqlHelper.ConvertToList(SqlHelper.ExecuteDataReader(sql, parameters));
		}

		internal static List<T> ExecuteList<T>(string sql, params SqlParameter[] parameters)
		where T : new()
		{
			return SqlHelper.ConvertToList<T>(SqlHelper.ExecuteDataReader(sql, parameters));
		}

		public static int ExecuteNonQuery(string sql, params SqlParameter[] parameters)
		{
			int num;
			SqlConnection conn = new SqlConnection(SqlHelper.connstr);
			try
			{
				conn.Open();
				SqlCommand cmd = conn.CreateCommand();
				try
				{
					cmd.CommandText = sql;
					cmd.Parameters.AddRange(parameters);
					num = cmd.ExecuteNonQuery();
				}
				finally
				{
					if (cmd != null)
					{
						((IDisposable)cmd).Dispose();
					}
				}
			}
			finally
			{
				if (conn != null)
				{
					((IDisposable)conn).Dispose();
				}
			}
			return num;
		}

		internal static object ExecuteScalar(string sql, params SqlParameter[] parameters)
		{
			object obj;
			SqlConnection conn = new SqlConnection(SqlHelper.connstr);
			try
			{
				conn.Open();
				SqlCommand cmd = conn.CreateCommand();
				try
				{
					cmd.CommandText = sql;
					cmd.Parameters.AddRange(parameters);
					obj = cmd.ExecuteScalar();
				}
				finally
				{
					if (cmd != null)
					{
						((IDisposable)cmd).Dispose();
					}
				}
			}
			finally
			{
				if (conn != null)
				{
					((IDisposable)conn).Dispose();
				}
			}
			return obj;
		}

		private static DataTable ProcedureDataReader(string proc, params SqlParameter[] parameters)
		{
			DataTable dataTable;
			SqlConnection conn = new SqlConnection(SqlHelper.connstr);
			try
			{
				conn.Open();
				SqlCommand cmd = conn.CreateCommand();
				try
				{
					cmd.CommandText = proc;
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.AddRange(parameters);
					SqlDataReader reader = cmd.ExecuteReader();
					try
					{
						DataTable dt = new DataTable();
						dt.Load(reader);
						dataTable = dt;
					}
					finally
					{
						if (reader != null)
						{
							((IDisposable)reader).Dispose();
						}
					}
				}
				finally
				{
					if (cmd != null)
					{
						((IDisposable)cmd).Dispose();
					}
				}
			}
			finally
			{
				if (conn != null)
				{
					((IDisposable)conn).Dispose();
				}
			}
			return dataTable;
		}

		private static DataTable ProcedureDataTable(string proc, params SqlParameter[] parameters)
		{
			DataTable dataTable;
			SqlConnection conn = new SqlConnection(SqlHelper.connstr);
			try
			{
				conn.Open();
				SqlCommand cmd = conn.CreateCommand();
				try
				{
					cmd.CommandText = proc;
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.AddRange(parameters);
					SqlDataAdapter adapter = new SqlDataAdapter(cmd);
					try
					{
						DataTable dt = new DataTable();
						adapter.Fill(dt);
						dataTable = dt;
					}
					finally
					{
						if (adapter != null)
						{
							((IDisposable)adapter).Dispose();
						}
					}
				}
				finally
				{
					if (cmd != null)
					{
						((IDisposable)cmd).Dispose();
					}
				}
			}
			finally
			{
				if (conn != null)
				{
					((IDisposable)conn).Dispose();
				}
			}
			return dataTable;
		}

		internal static T ProcedureEntity<T>(string proc, params SqlParameter[] parameters)
		where T : new()
		{
			List<T> list = SqlHelper.ConvertToList<T>(SqlHelper.ProcedureDataTable(proc, parameters));
			return (list.Count > 0 ? list[0] : default(T));
		}

		internal static dynamic ProcedureEntity(string proc, params SqlParameter[] parameters)
		{
			List<object> list = SqlHelper.ConvertToList(SqlHelper.ProcedureDataTable(proc, parameters));
			return (list.Count > 0 ? list[0] : null);
		}

		internal static List<T> ProcedureList<T>(string proc, params SqlParameter[] parameters)
		where T : new()
		{
			return SqlHelper.ConvertToList<T>(SqlHelper.ProcedureDataReader(proc, parameters));
		}

		internal static List<dynamic> ProcedureList(string proc, params SqlParameter[] parameters)
		{
			return SqlHelper.ConvertToList(SqlHelper.ProcedureDataReader(proc, parameters));
		}

		internal static int ProcedureNonQuery(string proc, params SqlParameter[] parameters)
		{
			int num;
			SqlConnection conn = new SqlConnection(SqlHelper.connstr);
			try
			{
				conn.Open();
				SqlCommand cmd = conn.CreateCommand();
				try
				{
					cmd.CommandText = proc;
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.AddRange(parameters);
					num = cmd.ExecuteNonQuery();
				}
				finally
				{
					if (cmd != null)
					{
						((IDisposable)cmd).Dispose();
					}
				}
			}
			finally
			{
				if (conn != null)
				{
					((IDisposable)conn).Dispose();
				}
			}
			return num;
		}

		internal static object ProcedureScalar(string proc, params SqlParameter[] parameters)
		{
			object obj;
			SqlConnection conn = new SqlConnection(SqlHelper.connstr);
			try
			{
				conn.Open();
				SqlCommand cmd = conn.CreateCommand();
				try
				{
					cmd.CommandText = proc;
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.AddRange(parameters);
					obj = cmd.ExecuteScalar();
				}
				finally
				{
					if (cmd != null)
					{
						((IDisposable)cmd).Dispose();
					}
				}
			}
			finally
			{
				if (conn != null)
				{
					((IDisposable)conn).Dispose();
				}
			}
			return obj;
		}

		internal static object ToCSharpValue(object value)
		{
			return (value != DBNull.Value ? value : null);
		}

		internal static object ToDBValue(object value)
		{
			object obj;
			if (value != null)
			{
				obj = value;
			}
			else
			{
				obj = DBNull.Value;
			}
			return obj;
		}
	}
}