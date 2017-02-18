using Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Providers
{
	public class KnowledgeProvider
	{
		private readonly static KnowledgeProvider InstanceObj;

		public static KnowledgeProvider Instance
		{
			get
			{
				return KnowledgeProvider.InstanceObj;
			}
		}

		static KnowledgeProvider()
		{
			KnowledgeProvider.InstanceObj = new KnowledgeProvider();
		}

		private KnowledgeProvider()
		{
		}

		public Knowledge Create(Knowledge knowledge)
		{
			Knowledge knowledge1;
			if (knowledge.Id == Guid.Empty)
			{
				knowledge.Id = Guid.NewGuid();
			}
			string sql = "INSERT INTO Knowledge (Id, Name, SpecialityId, CreateTime, IsDeleted)  VALUES (@Id, @Name, @SpecialityId, @CreateTime, @IsDeleted)";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", SqlHelper.ToDBValue(knowledge.Id)), new SqlParameter("@Name", SqlHelper.ToDBValue(knowledge.Name)), new SqlParameter("@SpecialityId", SqlHelper.ToDBValue(knowledge.SpecialityId)), new SqlParameter("@CreateTime", SqlHelper.ToDBValue(knowledge.CreateTime)), new SqlParameter("@IsDeleted", SqlHelper.ToDBValue(knowledge.IsDeleted)) };
			if (SqlHelper.ExecuteNonQuery(sql, sqlParameter) > 0)
			{
				knowledge1 = knowledge;
			}
			else
			{
				knowledge1 = null;
			}
			return knowledge1;
		}

		public int Delete(Guid id)
		{
			string sql = "DELETE Knowledge WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}

		public int GetCount()
		{
			return (int)SqlHelper.ExecuteScalar("SELECT COUNT(Id) FROM Knowledge WHERE IsDeleted = 0", new SqlParameter[0]);
		}

		public Knowledge GetEntity(Guid id)
		{
			string sql = "SELECT * FROM Knowledge WHERE Id = @Id AND IsDeleted = 0";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteEntity<Knowledge>(sql, sqlParameter);
		}

		public List<Knowledge> GetList()
		{
			return SqlHelper.ExecuteList<Knowledge>("SELECT * FROM Knowledge WHERE IsDeleted = 0", new SqlParameter[0]);
		}

		public List<Knowledge> GetList(Guid speid)
		{
			string sql = "SELECT * FROM Knowledge WHERE IsDeleted = 0 AND SpecialityId = @SpecialityId ";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@SpecialityId", (object)speid) };
			return SqlHelper.ExecuteList<Knowledge>(sql, sqlParameter);
		}

		public bool GetNameBool(Guid id, string name)
		{
			bool flag;
			string sql = "SELECT * FROM Knowledge WHERE Id <> @Id AND IsDeleted = 0 AND Name = @name";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id), new SqlParameter("@Name", name) };
			flag = (SqlHelper.ExecuteList<Knowledge>(sql, sqlParameter).Count <= 0 ? false : true);
			return flag;
		}

		public bool GetNameBool(string name)
		{
			bool flag;
			string sql = "SELECT * FROM Knowledge WHERE  IsDeleted = 0 AND Name = @name";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Name", name) };
			flag = (SqlHelper.ExecuteList<Knowledge>(sql, sqlParameter).Count <= 0 ? false : true);
			return flag;
		}

		public List<Knowledge> GetPage(int pageIndex, int pageCount)
		{
			string sql = "SELECT * FROM(SELECT *,(ROW_NUMBER() OVER(ORDER BY Id)) rownum FROM Knowledge WHERE IsDeleted = 0) t WHERE rownum>=(@pageCount*(@pageIndex-1)+1) AND rownum<=@pageIndex*@pageCount";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@pageIndex", (object)pageIndex), new SqlParameter("@pageCount", (object)pageCount) };
			return SqlHelper.ExecuteList<Knowledge>(sql, sqlParameter);
		}

		public int SoftDelete(Guid id)
		{
			string sql = "UPDATE Knowledge SET IsDeleted = 1 WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}

		public int Update(Knowledge knowledge)
		{
			string sql = "UPDATE Knowledge SET Name = @Name,SpecialityId = @SpecialityId,CreateTime = @CreateTime,IsDeleted = @IsDeleted WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)knowledge.Id), new SqlParameter("@Name", SqlHelper.ToDBValue(knowledge.Name)), new SqlParameter("@SpecialityId", SqlHelper.ToDBValue(knowledge.SpecialityId)), new SqlParameter("@CreateTime", SqlHelper.ToDBValue(knowledge.CreateTime)), new SqlParameter("@IsDeleted", SqlHelper.ToDBValue(knowledge.IsDeleted)) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}
	}
}