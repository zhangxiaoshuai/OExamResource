using Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Providers
{
	public class ExamUserLogProvider
	{
		private readonly static ExamUserLogProvider InstanceObj;

		public static ExamUserLogProvider Instance
		{
			get
			{
				return ExamUserLogProvider.InstanceObj;
			}
		}

		static ExamUserLogProvider()
		{
			ExamUserLogProvider.InstanceObj = new ExamUserLogProvider();
		}

		private ExamUserLogProvider()
		{
		}

		public ExamUserLog Create(ExamUserLog examUserLog)
		{
			ExamUserLog examUserLog1;
			if (examUserLog.Id == Guid.Empty)
			{
				examUserLog.Id = Guid.NewGuid();
			}
			string sql = "INSERT INTO ExamUserLog (Id, PaperId, UserId, Score, CreateTime, IsDeleted)  VALUES (@Id, @PaperId, @UserId, @Score, @CreateTime, @IsDeleted)";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", SqlHelper.ToDBValue(examUserLog.Id)), new SqlParameter("@PaperId", SqlHelper.ToDBValue(examUserLog.PaperId)), new SqlParameter("@UserId", SqlHelper.ToDBValue(examUserLog.UserId)), new SqlParameter("@Score", SqlHelper.ToDBValue(examUserLog.Score)), new SqlParameter("@CreateTime", SqlHelper.ToDBValue(examUserLog.CreateTime)), new SqlParameter("@IsDeleted", SqlHelper.ToDBValue(examUserLog.IsDeleted)) };
			if (SqlHelper.ExecuteNonQuery(sql, sqlParameter) > 0)
			{
				examUserLog1 = examUserLog;
			}
			else
			{
				examUserLog1 = null;
			}
			return examUserLog1;
		}

		public int Delete(Guid id)
		{
			string sql = "DELETE ExamUserLog WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}

		public int GetCount()
		{
			return (int)SqlHelper.ExecuteScalar("SELECT COUNT(Id) FROM ExamUserLog WHERE IsDeleted = 0", new SqlParameter[0]);
		}

		public ExamUserLog GetEntity(Guid id)
		{
			string sql = "SELECT * FROM ExamUserLog WHERE Id = @Id AND IsDeleted = 0";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteEntity<ExamUserLog>(sql, sqlParameter);
		}

		public List<ExamUserLog> GetList(Guid userId, Guid paperId, int top)
		{
			string sql = "select top(@top) * from ExamUserLog where UserId = @UserId and PaperId = @PaperId and IsDeleted = 0 order by CreateTime DESC";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@top", (object)top), new SqlParameter("@UserId", (object)userId), new SqlParameter("@PaperId", (object)paperId) };
			return SqlHelper.ExecuteList<ExamUserLog>(sql, sqlParameter);
		}

		public List<ExamUserLog> GetList()
		{
			return SqlHelper.ExecuteList<ExamUserLog>("SELECT * FROM ExamUserLog WHERE IsDeleted = 0", new SqlParameter[0]);
		}

		public List<ExamUserLog> GetPage(int pageIndex, int pageCount)
		{
			string sql = "SELECT * FROM(SELECT *,(ROW_NUMBER() OVER(ORDER BY Id)) rownum FROM ExamUserLog WHERE IsDeleted = 0) t WHERE rownum>=(@pageCount*(@pageIndex-1)+1) AND rownum<=@pageIndex*@pageCount";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@pageIndex", (object)pageIndex), new SqlParameter("@pageCount", (object)pageCount) };
			return SqlHelper.ExecuteList<ExamUserLog>(sql, sqlParameter);
		}

		public int SoftDelete(Guid id)
		{
			string sql = "UPDATE ExamUserLog SET IsDeleted = 1 WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}

		public int Update(ExamUserLog examUserLog)
		{
			string sql = "UPDATE ExamUserLog SET PaperId = @PaperId,UserId = @UserId,Score = @Score,CreateTime = @CreateTime,IsDeleted = @IsDeleted WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)examUserLog.Id), new SqlParameter("@PaperId", SqlHelper.ToDBValue(examUserLog.PaperId)), new SqlParameter("@UserId", SqlHelper.ToDBValue(examUserLog.UserId)), new SqlParameter("@Score", SqlHelper.ToDBValue(examUserLog.Score)), new SqlParameter("@CreateTime", SqlHelper.ToDBValue(examUserLog.CreateTime)), new SqlParameter("@IsDeleted", SqlHelper.ToDBValue(examUserLog.IsDeleted)) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}
	}
}