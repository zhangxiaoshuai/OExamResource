using Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Providers
{
	public class SystemInfoProvider
	{
		private readonly static SystemInfoProvider InstanceObj;

		public static SystemInfoProvider Instance
		{
			get
			{
				return SystemInfoProvider.InstanceObj;
			}
		}

		static SystemInfoProvider()
		{
			SystemInfoProvider.InstanceObj = new SystemInfoProvider();
		}

		private SystemInfoProvider()
		{
		}

		public SystemInfo Create(SystemInfo systemInfo)
		{
			SystemInfo systemInfo1;
			if (systemInfo.Id == Guid.Empty)
			{
				systemInfo.Id = Guid.NewGuid();
			}
			string sql = "INSERT INTO SystemInfo (Id, IsReg, IsValid, CreatTime, IsDeleted)  VALUES (@Id, @IsReg, @IsValid, @CreatTime, @IsDeleted)";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", SqlHelper.ToDBValue(systemInfo.Id)), new SqlParameter("@IsReg", SqlHelper.ToDBValue(systemInfo.IsReg)), new SqlParameter("@IsValid", SqlHelper.ToDBValue(systemInfo.IsValid)), new SqlParameter("@CreatTime", SqlHelper.ToDBValue(systemInfo.CreatTime)), new SqlParameter("@IsDeleted", SqlHelper.ToDBValue(systemInfo.IsDeleted)) };
			if (SqlHelper.ExecuteNonQuery(sql, sqlParameter) > 0)
			{
				systemInfo1 = systemInfo;
			}
			else
			{
				systemInfo1 = null;
			}
			return systemInfo1;
		}

		public int Delete(Guid id)
		{
			string sql = "DELETE SystemInfo WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}

		public int GetCount()
		{
			return (int)SqlHelper.ExecuteScalar("SELECT COUNT(Id) FROM SystemInfo WHERE IsDeleted = 0", new SqlParameter[0]);
		}

		public SystemInfo GetEntity(Guid id)
		{
			string sql = "SELECT * FROM SystemInfo WHERE Id = @Id AND IsDeleted = 0";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteEntity<SystemInfo>(sql, sqlParameter);
		}

		public List<SystemInfo> GetList()
		{
			return SqlHelper.ExecuteList<SystemInfo>("SELECT * FROM SystemInfo WHERE IsDeleted = 0", new SqlParameter[0]);
		}

		public List<SystemInfo> GetPage(int pageIndex, int pageCount)
		{
			string sql = "SELECT * FROM(SELECT *,(ROW_NUMBER() OVER(ORDER BY Id)) rownum FROM SystemInfo WHERE IsDeleted = 0) t WHERE rownum>=(@pageCount*(@pageIndex-1)+1) AND rownum<=@pageIndex*@pageCount";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@pageIndex", (object)pageIndex), new SqlParameter("@pageCount", (object)pageCount) };
			return SqlHelper.ExecuteList<SystemInfo>(sql, sqlParameter);
		}

		public int SoftDelete(Guid id)
		{
			string sql = "UPDATE SystemInfo SET IsDeleted = 1 WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}

		public int Update(SystemInfo systemInfo)
		{
			string sql = "UPDATE SystemInfo SET IsReg = @IsReg,IsValid = @IsValid,CreatTime = @CreatTime,IsDeleted = @IsDeleted WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)systemInfo.Id), new SqlParameter("@IsReg", SqlHelper.ToDBValue(systemInfo.IsReg)), new SqlParameter("@IsValid", SqlHelper.ToDBValue(systemInfo.IsValid)), new SqlParameter("@CreatTime", SqlHelper.ToDBValue(systemInfo.CreatTime)), new SqlParameter("@IsDeleted", SqlHelper.ToDBValue(systemInfo.IsDeleted)) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}
	}
}