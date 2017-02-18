using Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Providers
{
	public class LanCollectProvider
	{
		private readonly static LanCollectProvider InstanceObj;

		public static LanCollectProvider Instance
		{
			get
			{
				return LanCollectProvider.InstanceObj;
			}
		}

		static LanCollectProvider()
		{
			LanCollectProvider.InstanceObj = new LanCollectProvider();
		}

		private LanCollectProvider()
		{
		}

		public LanCollect Create(LanCollect lanCollect)
		{
			LanCollect lanCollect1;
			if (lanCollect.Id == Guid.Empty)
			{
				lanCollect.Id = Guid.NewGuid();
			}
			string sql = "INSERT INTO LanCollect (Id, UserId, ResourceId, CreateTime, IsDeleted)  VALUES (@Id, @UserId, @ResourceId, @CreateTime, @IsDeleted)";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", SqlHelper.ToDBValue(lanCollect.Id)), new SqlParameter("@UserId", SqlHelper.ToDBValue(lanCollect.UserId)), new SqlParameter("@ResourceId", SqlHelper.ToDBValue(lanCollect.ResourceId)), new SqlParameter("@CreateTime", SqlHelper.ToDBValue(lanCollect.CreateTime)), new SqlParameter("@IsDeleted", SqlHelper.ToDBValue(lanCollect.IsDeleted)) };
			if (SqlHelper.ExecuteNonQuery(sql, sqlParameter) > 0)
			{
				lanCollect1 = lanCollect;
			}
			else
			{
				lanCollect1 = null;
			}
			return lanCollect1;
		}

		public int Delete(Guid id)
		{
			string sql = "DELETE LanCollect WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}

		public int Delete(Guid userId, Guid resourceId)
		{
			string sql = "delete LanCollect where UserId = @UserId and ResourceId = @ResourceId";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@UserId", (object)userId), new SqlParameter("@ResourceId", (object)resourceId) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}

		public int GetCount()
		{
			return (int)SqlHelper.ExecuteScalar("SELECT COUNT(Id) FROM LanCollect WHERE IsDeleted = 0", new SqlParameter[0]);
		}

		public int GetCount(Guid userId)
		{
			string sql = "select count(1) from LanCollect where UserId = @UserId and IsDeleted = 0";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@UserId", (object)userId) };
			return (int)SqlHelper.ExecuteScalar(sql, sqlParameter);
		}

		public LanCollect GetEntity(Guid id)
		{
			string sql = "SELECT * FROM LanCollect WHERE Id = @Id AND IsDeleted = 0";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteEntity<LanCollect>(sql, sqlParameter);
		}

		public LanCollect GetEntity(Guid userId, Guid resourceId)
		{
			string sql = "select * from LanCollect where UserId = @UserId and ResourceId = @ResourceId and IsDeleted = 0";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@UserId", (object)userId), new SqlParameter("@ResourceId", (object)resourceId) };
			return SqlHelper.ExecuteEntity<LanCollect>(sql, sqlParameter);
		}

		public List<LanCollect> GetList()
		{
			return SqlHelper.ExecuteList<LanCollect>("SELECT * FROM LanCollect WHERE IsDeleted = 0", new SqlParameter[0]);
		}

		public List<LanCollect> GetPage(int pageIndex, int pageCount)
		{
			string sql = "SELECT * FROM(SELECT *,(ROW_NUMBER() OVER(ORDER BY Id)) rownum FROM LanCollect WHERE IsDeleted = 0) t WHERE rownum>=(@pageCount*(@pageIndex-1)+1) AND rownum<=@pageIndex*@pageCount";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@pageIndex", (object)pageIndex), new SqlParameter("@pageCount", (object)pageCount) };
			return SqlHelper.ExecuteList<LanCollect>(sql, sqlParameter);
		}

		public List<LanCollectInfo> GetPageInfo(Guid userId, int pageIndex, int pageCount)
		{
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@PageIndex", (object)pageIndex), new SqlParameter("@PageCount", (object)pageCount), new SqlParameter("@UserId", (object)userId) };
			return SqlHelper.ProcedureList<LanCollectInfo>("LanCollectPage", sqlParameter);
		}

		public int SoftDelete(Guid id)
		{
			string sql = "UPDATE LanCollect SET IsDeleted = 1 WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}

		public int Update(LanCollect lanCollect)
		{
			string sql = "UPDATE LanCollect SET UserId = @UserId,ResourceId = @ResourceId,CreateTime = @CreateTime,IsDeleted = @IsDeleted WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)lanCollect.Id), new SqlParameter("@UserId", SqlHelper.ToDBValue(lanCollect.UserId)), new SqlParameter("@ResourceId", SqlHelper.ToDBValue(lanCollect.ResourceId)), new SqlParameter("@CreateTime", SqlHelper.ToDBValue(lanCollect.CreateTime)), new SqlParameter("@IsDeleted", SqlHelper.ToDBValue(lanCollect.IsDeleted)) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}
	}
}