using Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Providers
{
	public class CategoryProvider
	{
		private readonly static CategoryProvider InstanceObj;

		public static CategoryProvider Instance
		{
			get
			{
				return CategoryProvider.InstanceObj;
			}
		}

		static CategoryProvider()
		{
			CategoryProvider.InstanceObj = new CategoryProvider();
		}

		private CategoryProvider()
		{
		}

		public Category Create(Category category)
		{
			Category category1;
			if (category.Id == Guid.Empty)
			{
				category.Id = Guid.NewGuid();
			}
			string sql = "INSERT INTO Category (Id, Name, Ico, Count, CreateTime, IsDeleted)  VALUES (@Id, @Name, @Ico, @Count, @CreateTime, @IsDeleted)";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", SqlHelper.ToDBValue(category.Id)), new SqlParameter("@Name", SqlHelper.ToDBValue(category.Name)), new SqlParameter("@Ico", SqlHelper.ToDBValue(category.Ico)), new SqlParameter("@Count", SqlHelper.ToDBValue(category.Count)), new SqlParameter("@CreateTime", SqlHelper.ToDBValue(category.CreateTime)), new SqlParameter("@IsDeleted", SqlHelper.ToDBValue(category.IsDeleted)) };
			if (SqlHelper.ExecuteNonQuery(sql, sqlParameter) > 0)
			{
				category1 = category;
			}
			else
			{
				category1 = null;
			}
			return category1;
		}

		public int Delete(Guid id)
		{
			string sql = "DELETE Category WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}

		public int GetCount()
		{
			return (int)SqlHelper.ExecuteScalar("SELECT COUNT(Id) FROM Category WHERE IsDeleted = 0", new SqlParameter[0]);
		}

		public Category GetEntity(Guid id)
		{
			string sql = "SELECT * FROM Category WHERE Id = @Id AND IsDeleted = 0";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteEntity<Category>(sql, sqlParameter);
		}

		public List<Category> GetList()
		{
			return SqlHelper.ExecuteList<Category>("SELECT * FROM Category WHERE IsDeleted = 0 order by sortId ", new SqlParameter[0]);
		}

		public bool GetNameBool(Guid id, string name)
		{
			bool flag;
			string sql = "SELECT * FROM Category WHERE Id <> @Id AND IsDeleted = 0 AND Name = @name";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id), new SqlParameter("@Name", name) };
			flag = (SqlHelper.ExecuteList<Category>(sql, sqlParameter).Count <= 0 ? false : true);
			return flag;
		}

		public bool GetNameBool(string name)
		{
			bool flag;
			string sql = "SELECT * FROM Category WHERE  IsDeleted = 0 AND Name = @name";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Name", name) };
			flag = (SqlHelper.ExecuteList<Category>(sql, sqlParameter).Count <= 0 ? false : true);
			return flag;
		}

		public List<Category> GetPage(int pageIndex, int pageCount)
		{
			string sql = "SELECT * FROM(SELECT *,(ROW_NUMBER() OVER(ORDER BY Id)) rownum FROM Category WHERE IsDeleted = 0) t WHERE rownum>=(@pageCount*(@pageIndex-1)+1) AND rownum<=@pageIndex*@pageCount";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@pageIndex", (object)pageIndex), new SqlParameter("@pageCount", (object)pageCount) };
			return SqlHelper.ExecuteList<Category>(sql, sqlParameter);
		}

		public int SoftDelete(Guid id)
		{
			string sql = "UPDATE Category SET IsDeleted = 1 WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}

		public int Update(Category category)
		{
			string sql = "UPDATE Category SET Name = @Name,Ico = @Ico,Count = @Count,CreateTime = @CreateTime,IsDeleted = @IsDeleted WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)category.Id), new SqlParameter("@Name", SqlHelper.ToDBValue(category.Name)), new SqlParameter("@Ico", SqlHelper.ToDBValue(category.Ico)), new SqlParameter("@Count", SqlHelper.ToDBValue(category.Count)), new SqlParameter("@CreateTime", SqlHelper.ToDBValue(category.CreateTime)), new SqlParameter("@IsDeleted", SqlHelper.ToDBValue(category.IsDeleted)) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}
	}
}