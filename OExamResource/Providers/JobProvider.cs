using Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Providers
{
	public class JobProvider
	{
		private readonly static JobProvider InstanceObj;

		public static JobProvider Instance
		{
			get
			{
				return JobProvider.InstanceObj;
			}
		}

		static JobProvider()
		{
			JobProvider.InstanceObj = new JobProvider();
		}

		private JobProvider()
		{
		}

		public Job Create(Job job)
		{
			Job job1;
			if (job.Id == Guid.Empty)
			{
				job.Id = Guid.NewGuid();
			}
			string sql = "INSERT INTO Job (Id, Name, IsDeleted, CreateTime)  VALUES (@Id, @Name, @IsDeleted, @CreateTime)";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", SqlHelper.ToDBValue(job.Id)), new SqlParameter("@Name", SqlHelper.ToDBValue(job.Name)), new SqlParameter("@IsDeleted", SqlHelper.ToDBValue(job.IsDeleted)), new SqlParameter("@CreateTime", SqlHelper.ToDBValue(job.CreateTime)) };
			if (SqlHelper.ExecuteNonQuery(sql, sqlParameter) > 0)
			{
				job1 = job;
			}
			else
			{
				job1 = null;
			}
			return job1;
		}

		public int Delete(Guid id)
		{
			string sql = "DELETE Job WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}

		public int GetCount()
		{
			return (int)SqlHelper.ExecuteScalar("SELECT COUNT(Id) FROM Job WHERE IsDeleted = 0", new SqlParameter[0]);
		}

		public Job GetEntity(Guid id)
		{
			string sql = "SELECT * FROM Job WHERE Id = @Id AND IsDeleted = 0";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteEntity<Job>(sql, sqlParameter);
		}

		public Job GetEntity(string name)
		{
			string sql = "SELECT * FROM Job WHERE Name = @Name AND IsDeleted = 0";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Name", name) };
			return SqlHelper.ExecuteEntity<Job>(sql, sqlParameter);
		}

		public List<Job> GetList()
		{
			return SqlHelper.ExecuteList<Job>("SELECT * FROM Job WHERE IsDeleted = 0 ORDER BY LEN([Name])", new SqlParameter[0]);
		}

		public List<Job> GetPage(int pageIndex, int pageCount)
		{
			string sql = "SELECT * FROM(SELECT *,(ROW_NUMBER() OVER(ORDER BY Id)) rownum FROM Job WHERE IsDeleted = 0) t WHERE rownum>=(@pageCount*(@pageIndex-1)+1) AND rownum<=@pageIndex*@pageCount";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@pageIndex", (object)pageIndex), new SqlParameter("@pageCount", (object)pageCount) };
			return SqlHelper.ExecuteList<Job>(sql, sqlParameter);
		}

		public int SoftDelete(Guid id)
		{
			string sql = "UPDATE Job SET IsDeleted = 1 WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}

		public int Update(Job job)
		{
			string sql = "UPDATE Job SET Name = @Name,IsDeleted = @IsDeleted,CreateTime = @CreateTime WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)job.Id), new SqlParameter("@Name", SqlHelper.ToDBValue(job.Name)), new SqlParameter("@IsDeleted", SqlHelper.ToDBValue(job.IsDeleted)), new SqlParameter("@CreateTime", SqlHelper.ToDBValue(job.CreateTime)) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}
	}
}