using Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Providers
{
	public class SubjectProvider
	{
		private readonly static SubjectProvider InstanceObj;

		public static SubjectProvider Instance
		{
			get
			{
				return SubjectProvider.InstanceObj;
			}
		}

		static SubjectProvider()
		{
			SubjectProvider.InstanceObj = new SubjectProvider();
		}

		private SubjectProvider()
		{
		}

		public Subject Create(Subject subject)
		{
			Subject subject1;
			if (subject.Id == Guid.Empty)
			{
				subject.Id = Guid.NewGuid();
			}
			string sql = "INSERT INTO Subject (Id, Name, CategoryId, CreateTime, IsDeleted)  VALUES (@Id, @Name, @CategoryId, @CreateTime, @IsDeleted)";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", SqlHelper.ToDBValue(subject.Id)), new SqlParameter("@Name", SqlHelper.ToDBValue(subject.Name)), new SqlParameter("@CategoryId", SqlHelper.ToDBValue(subject.CategoryId)), new SqlParameter("@CreateTime", SqlHelper.ToDBValue(subject.CreateTime)), new SqlParameter("@IsDeleted", SqlHelper.ToDBValue(subject.IsDeleted)) };
			if (SqlHelper.ExecuteNonQuery(sql, sqlParameter) > 0)
			{
				subject1 = subject;
			}
			else
			{
				subject1 = null;
			}
			return subject1;
		}

		public int Delete(Guid id)
		{
			string sql = "DELETE Subject WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}

		public int GetCount()
		{
			return (int)SqlHelper.ExecuteScalar("SELECT COUNT(Id) FROM Subject WHERE IsDeleted = 0", new SqlParameter[0]);
		}

		public Subject GetEntity(Guid id)
		{
			string sql = "SELECT * FROM Subject WHERE Id = @Id AND IsDeleted = 0";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteEntity<Subject>(sql, sqlParameter);
		}

		public List<Subject> GetList(Guid cateid)
		{
			string sql = "SELECT * FROM Subject WHERE IsDeleted = 0 AND CategoryId = @CategoryId ORDER BY SortId";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@CategoryId", (object)cateid) };
			return SqlHelper.ExecuteList<Subject>(sql, sqlParameter);
		}

		public List<Subject> GetList()
		{
			return SqlHelper.ExecuteList<Subject>("SELECT * FROM Subject WHERE IsDeleted = 0 and count>0 order by sortid", new SqlParameter[0]);
		}

		public bool GetNameBool(Guid id, string name)
		{
			bool flag;
			string sql = "SELECT * FROM Subject WHERE Id <> @Id AND IsDeleted = 0 AND Name = @name";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id), new SqlParameter("@Name", name) };
			flag = (SqlHelper.ExecuteList<Subject>(sql, sqlParameter).Count <= 0 ? false : true);
			return flag;
		}

		public bool GetNameBool(string name)
		{
			bool flag;
			string sql = "SELECT * FROM Subject WHERE  IsDeleted = 0 AND Name = @name";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Name", name) };
			flag = (SqlHelper.ExecuteList<Subject>(sql, sqlParameter).Count <= 0 ? false : true);
			return flag;
		}

		public List<Subject> GetPage(int pageIndex, int pageCount)
		{
			string sql = "SELECT * FROM(SELECT *,(ROW_NUMBER() OVER(ORDER BY Id)) rownum FROM Subject WHERE IsDeleted = 0) t WHERE rownum>=(@pageCount*(@pageIndex-1)+1) AND rownum<=@pageIndex*@pageCount";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@pageIndex", (object)pageIndex), new SqlParameter("@pageCount", (object)pageCount) };
			return SqlHelper.ExecuteList<Subject>(sql, sqlParameter);
		}

		public int SoftDelete(Guid id)
		{
			string sql = "UPDATE Subject SET IsDeleted = 1 WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}

		public int Update(Subject subject)
		{
			string sql = "UPDATE Subject SET Name = @Name,CategoryId = @CategoryId,CreateTime = @CreateTime,IsDeleted = @IsDeleted WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)subject.Id), new SqlParameter("@Name", SqlHelper.ToDBValue(subject.Name)), new SqlParameter("@CategoryId", SqlHelper.ToDBValue(subject.CategoryId)), new SqlParameter("@CreateTime", SqlHelper.ToDBValue(subject.CreateTime)), new SqlParameter("@IsDeleted", SqlHelper.ToDBValue(subject.IsDeleted)) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}
	}
}