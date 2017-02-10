using Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Providers
{
	public class IpProvider
	{
		private readonly static IpProvider InstanceObj;

		public static IpProvider Instance
		{
			get
			{
				return IpProvider.InstanceObj;
			}
		}

		static IpProvider()
		{
			IpProvider.InstanceObj = new IpProvider();
		}

		private IpProvider()
		{
		}

		public Ip Create(Ip ip)
		{
			Ip ip1;
			if (ip.Id == Guid.Empty)
			{
				ip.Id = Guid.NewGuid();
			}
			string sql = "INSERT INTO Ip (Id, SchoolId, IpStart, IpEnd, Tier, CreateTime, IsDeleted)  VALUES (@Id, @SchoolId, @IpStart, @IpEnd, @Tier, @CreateTime, @IsDeleted)";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", SqlHelper.ToDBValue(ip.Id)), new SqlParameter("@SchoolId", SqlHelper.ToDBValue(ip.SchoolId)), new SqlParameter("@IpStart", SqlHelper.ToDBValue(ip.IpStart)), new SqlParameter("@IpEnd", SqlHelper.ToDBValue(ip.IpEnd)), new SqlParameter("@Tier", SqlHelper.ToDBValue(ip.Tier)), new SqlParameter("@CreateTime", SqlHelper.ToDBValue(ip.CreateTime)), new SqlParameter("@IsDeleted", SqlHelper.ToDBValue(ip.IsDeleted)) };
			if (SqlHelper.ExecuteNonQuery(sql, sqlParameter) > 0)
			{
				ip1 = ip;
			}
			else
			{
				ip1 = null;
			}
			return ip1;
		}

		public int Delete(Guid id)
		{
			string sql = "DELETE Ip WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}

		public int DeleteBySchoolId(Guid id)
		{
			string sql = "DELETE Ip WHERE SchoolId = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}

		public int GetCount()
		{
			return (int)SqlHelper.ExecuteScalar("SELECT COUNT(Id) FROM Ip WHERE IsDeleted = 0", new SqlParameter[0]);
		}

		public Ip GetEntity(Guid id)
		{
			string sql = "SELECT * FROM Ip WHERE Id = @Id AND IsDeleted = 0";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteEntity<Ip>(sql, sqlParameter);
		}

		public Ip GetEntity(byte[] ip)
		{
			string sql = "select * from Ip where IpStart <= @ip and IpEnd >= @ip and IsDeleted = 0";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@ip", ip) };
			return SqlHelper.ExecuteEntity<Ip>(sql, sqlParameter);
		}

		public int GetEntity(byte[] ip, Guid schoolid)
		{
			string sql = "select count(id) from Ip where IpStart <= @ip and IpEnd >= @ip and IsDeleted = 0 and SchoolId = @SchoolId";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@ip", ip), new SqlParameter("@SchoolId", (object)schoolid) };
			return (int)SqlHelper.ExecuteScalar(sql, sqlParameter);
		}

		public List<Ip> GetList()
		{
			return SqlHelper.ExecuteList<Ip>("SELECT * FROM Ip WHERE IsDeleted = 0", new SqlParameter[0]);
		}

		public List<Ip> GetList(Guid id)
		{
			string sql = "SELECT * FROM Ip WHERE IsDeleted = 0 AND SchoolId =@schoolid";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@schoolid", (object)id) };
			return SqlHelper.ExecuteList<Ip>(sql, sqlParameter);
		}

		public List<Ip> GetPage(int pageIndex, int pageCount)
		{
			string sql = "SELECT * FROM(SELECT *,(ROW_NUMBER() OVER(ORDER BY Id)) rownum FROM Ip WHERE IsDeleted = 0) t WHERE rownum>=(@pageCount*(@pageIndex-1)+1) AND rownum<=@pageIndex*@pageCount";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@pageIndex", (object)pageIndex), new SqlParameter("@pageCount", (object)pageCount) };
			return SqlHelper.ExecuteList<Ip>(sql, sqlParameter);
		}

		public int SoftDelete(Guid id)
		{
			string sql = "UPDATE Ip SET IsDeleted = 1 WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}

		public int Update(Ip ip)
		{
			string sql = "UPDATE Ip SET SchoolId = @SchoolId,IpStart = @IpStart,IpEnd = @IpEnd,Tier = @Tier,CreateTime = @CreateTime,IsDeleted = @IsDeleted WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)ip.Id), new SqlParameter("@SchoolId", SqlHelper.ToDBValue(ip.SchoolId)), new SqlParameter("@IpStart", SqlHelper.ToDBValue(ip.IpStart)), new SqlParameter("@IpEnd", SqlHelper.ToDBValue(ip.IpEnd)), new SqlParameter("@Tier", SqlHelper.ToDBValue(ip.Tier)), new SqlParameter("@CreateTime", SqlHelper.ToDBValue(ip.CreateTime)), new SqlParameter("@IsDeleted", SqlHelper.ToDBValue(ip.IsDeleted)) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}
	}
}