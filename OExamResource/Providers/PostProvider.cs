using Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Providers
{
	public class PostProvider
	{
		private readonly static PostProvider InstanceObj;

		public static PostProvider Instance
		{
			get
			{
				return PostProvider.InstanceObj;
			}
		}

		static PostProvider()
		{
			PostProvider.InstanceObj = new PostProvider();
		}

		private PostProvider()
		{
		}

		public Post Create(Post post)
		{
			Post post1;
			if (post.Id == Guid.Empty)
			{
				post.Id = Guid.NewGuid();
			}
			string sql = "INSERT INTO Post (Id, Name, CreateTime, IsDeleted)  VALUES (@Id, @Name, @CreateTime, @IsDeleted)";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", SqlHelper.ToDBValue(post.Id)), new SqlParameter("@Name", SqlHelper.ToDBValue(post.Name)), new SqlParameter("@CreateTime", SqlHelper.ToDBValue(post.CreateTime)), new SqlParameter("@IsDeleted", SqlHelper.ToDBValue(post.IsDeleted)) };
			if (SqlHelper.ExecuteNonQuery(sql, sqlParameter) > 0)
			{
				post1 = post;
			}
			else
			{
				post1 = null;
			}
			return post1;
		}

		public int Delete(Guid id)
		{
			string sql = "DELETE Post WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}

		public int GetCount()
		{
			return (int)SqlHelper.ExecuteScalar("SELECT COUNT(Id) FROM Post WHERE IsDeleted = 0", new SqlParameter[0]);
		}

		public Post GetEntity(Guid id)
		{
			string sql = "SELECT * FROM Post WHERE Id = @Id AND IsDeleted = 0";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteEntity<Post>(sql, sqlParameter);
		}

		public Post GetEntity(string name)
		{
			string sql = "SELECT * FROM Post WHERE Name = @Name AND IsDeleted = 0";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Name", name) };
			return SqlHelper.ExecuteEntity<Post>(sql, sqlParameter);
		}

		public List<Post> GetList()
		{
			return SqlHelper.ExecuteList<Post>("SELECT * FROM Post WHERE IsDeleted = 0 ORDER BY LEN([Name]) ", new SqlParameter[0]);
		}

		public List<Post> GetPage(int pageIndex, int pageCount)
		{
			string sql = "SELECT * FROM(SELECT *,(ROW_NUMBER() OVER(ORDER BY Id)) rownum FROM Post WHERE IsDeleted = 0) t WHERE rownum>=(@pageCount*(@pageIndex-1)+1) AND rownum<=@pageIndex*@pageCount";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@pageIndex", (object)pageIndex), new SqlParameter("@pageCount", (object)pageCount) };
			return SqlHelper.ExecuteList<Post>(sql, sqlParameter);
		}

		public int SoftDelete(Guid id)
		{
			string sql = "UPDATE Post SET IsDeleted = 1 WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}

		public int Update(Post post)
		{
			string sql = "UPDATE Post SET Name = @Name,CreateTime = @CreateTime,IsDeleted = @IsDeleted WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)post.Id), new SqlParameter("@Name", SqlHelper.ToDBValue(post.Name)), new SqlParameter("@CreateTime", SqlHelper.ToDBValue(post.CreateTime)), new SqlParameter("@IsDeleted", SqlHelper.ToDBValue(post.IsDeleted)) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}
	}
}