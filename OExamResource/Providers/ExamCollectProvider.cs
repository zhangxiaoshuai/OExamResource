using Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Providers
{
	public class ExamCollectProvider
	{
		private readonly static ExamCollectProvider InstanceObj;

		public static ExamCollectProvider Instance
		{
			get
			{
				return ExamCollectProvider.InstanceObj;
			}
		}

		static ExamCollectProvider()
		{
			ExamCollectProvider.InstanceObj = new ExamCollectProvider();
		}

		private ExamCollectProvider()
		{
		}

		public ExamCollect Create(ExamCollect examCollect)
		{
			ExamCollect examCollect1;
			if (examCollect.Id == Guid.Empty)
			{
				examCollect.Id = Guid.NewGuid();
			}
			string sql = "INSERT INTO ExamCollect (Id, UserId, ExamPaperId, CreateTime, IsDeleted)  VALUES (@Id, @UserId, @ExamPaperId, @CreateTime, @IsDeleted)";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", SqlHelper.ToDBValue(examCollect.Id)), new SqlParameter("@UserId", SqlHelper.ToDBValue(examCollect.UserId)), new SqlParameter("@ExamPaperId", SqlHelper.ToDBValue(examCollect.ExamPaperId)), new SqlParameter("@CreateTime", SqlHelper.ToDBValue(examCollect.CreateTime)), new SqlParameter("@IsDeleted", SqlHelper.ToDBValue(examCollect.IsDeleted)) };
			if (SqlHelper.ExecuteNonQuery(sql, sqlParameter) > 0)
			{
				examCollect1 = examCollect;
			}
			else
			{
				examCollect1 = null;
			}
			return examCollect1;
		}

		public int Delete(Guid userId, Guid paperId)
		{
			string sql = "DELETE ExamCollect WHERE UserId = @UserId AND ExamPaperId = @ExamPaperId";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@UserId", (object)userId), new SqlParameter("@ExamPaperId", (object)paperId) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}

		public int Delete(Guid id)
		{
			string sql = "DELETE ExamCollect WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}

		public int GetCounByUser(Guid userId)
		{
			string sql = "select count(1) from ExamCollect where UserId = @UserId and IsDeleted = 0";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@UserId", (object)userId) };
			return (int)SqlHelper.ExecuteScalar(sql, sqlParameter);
		}

		public int GetCount(Guid paperId)
		{
			string sql = "SELECT COUNT(1) FROM ExamCollect WHERE ExamPaperId = @ExamPaperId";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@ExamPaperId", (object)paperId) };
			return (int)SqlHelper.ExecuteScalar(sql, sqlParameter);
		}

		public int GetCount()
		{
			return (int)SqlHelper.ExecuteScalar("SELECT COUNT(Id) FROM ExamCollect WHERE IsDeleted = 0", new SqlParameter[0]);
		}

		public ExamCollect GetEntity(Guid id)
		{
			string sql = "SELECT * FROM ExamCollect WHERE Id = @Id AND IsDeleted = 0";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteEntity<ExamCollect>(sql, sqlParameter);
		}

		public List<ExamCollect> GetList()
		{
			return SqlHelper.ExecuteList<ExamCollect>("SELECT * FROM ExamCollect WHERE IsDeleted = 0", new SqlParameter[0]);
		}

		public List<ExamCollect> GetPage(int pageIndex, int pageCount)
		{
			string sql = "SELECT * FROM(SELECT *,(ROW_NUMBER() OVER(ORDER BY Id)) rownum FROM ExamCollect WHERE IsDeleted = 0) t WHERE rownum>=(@pageCount*(@pageIndex-1)+1) AND rownum<=@pageIndex*@pageCount";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@pageIndex", (object)pageIndex), new SqlParameter("@pageCount", (object)pageCount) };
			return SqlHelper.ExecuteList<ExamCollect>(sql, sqlParameter);
		}

		public List<ExamCollectInfo> GetPageInfo(Guid userId, int pageIndex, int pageCount)
		{
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@UserId", (object)userId), new SqlParameter("@pageIndex", (object)pageIndex), new SqlParameter("@pageCount", (object)pageCount) };
			return SqlHelper.ProcedureList<ExamCollectInfo>("[ExamCollectPage]", sqlParameter);
		}

		public bool IsCollect(Guid userId, Guid paperId)
		{
			string sql = "SELECT COUNT(1) FROM ExamCollect WHERE UserId = @UserId AND ExamPaperId = @ExamPaperId";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@UserId", (object)userId), new SqlParameter("@ExamPaperId", (object)paperId) };
			return ((int)SqlHelper.ExecuteScalar(sql, sqlParameter) > 0 ? true : false);
		}

		public int SoftDelete(Guid id)
		{
			string sql = "UPDATE ExamCollect SET IsDeleted = 1 WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}

		public int Update(ExamCollect examCollect)
		{
			string sql = "UPDATE ExamCollect SET UserId = @UserId,ExamPaperId = @ExamPaperId,CreateTime = @CreateTime,IsDeleted = @IsDeleted WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)examCollect.Id), new SqlParameter("@UserId", SqlHelper.ToDBValue(examCollect.UserId)), new SqlParameter("@ExamPaperId", SqlHelper.ToDBValue(examCollect.ExamPaperId)), new SqlParameter("@CreateTime", SqlHelper.ToDBValue(examCollect.CreateTime)), new SqlParameter("@IsDeleted", SqlHelper.ToDBValue(examCollect.IsDeleted)) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}
	}
}