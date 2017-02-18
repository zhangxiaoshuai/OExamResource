using Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Providers
{
	public class MedicalKnowledgeProvider
	{
		private readonly static MedicalKnowledgeProvider InstanceObj;

		public static MedicalKnowledgeProvider Instance
		{
			get
			{
				return MedicalKnowledgeProvider.InstanceObj;
			}
		}

		static MedicalKnowledgeProvider()
		{
			MedicalKnowledgeProvider.InstanceObj = new MedicalKnowledgeProvider();
		}

		private MedicalKnowledgeProvider()
		{
		}

		public MedicalKnowledge Create(MedicalKnowledge medicalKnowledge)
		{
			MedicalKnowledge medicalKnowledge1;
			if (medicalKnowledge.Id == Guid.Empty)
			{
				medicalKnowledge.Id = Guid.NewGuid();
			}
			string sql = "INSERT INTO MedicalKnowledge (Id, Name, SortId, CreateTime, IsDeleted)  VALUES (@Id, @Name, @SortId, @CreateTime, @IsDeleted)";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", SqlHelper.ToDBValue(medicalKnowledge.Id)), new SqlParameter("@Name", SqlHelper.ToDBValue(medicalKnowledge.Name)), new SqlParameter("@SortId", SqlHelper.ToDBValue(medicalKnowledge.SortId)), new SqlParameter("@CreateTime", SqlHelper.ToDBValue(medicalKnowledge.CreateTime)), new SqlParameter("@IsDeleted", SqlHelper.ToDBValue(medicalKnowledge.IsDeleted)) };
			if (SqlHelper.ExecuteNonQuery(sql, sqlParameter) > 0)
			{
				medicalKnowledge1 = medicalKnowledge;
			}
			else
			{
				medicalKnowledge1 = null;
			}
			return medicalKnowledge1;
		}

		public int Delete(Guid id)
		{
			string sql = "DELETE MedicalKnowledge WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}

		public int GetCount()
		{
			return (int)SqlHelper.ExecuteScalar("SELECT COUNT(Id) FROM MedicalKnowledge WHERE IsDeleted = 0", new SqlParameter[0]);
		}

		public MedicalKnowledge GetEntity(Guid id)
		{
			string sql = "SELECT * FROM MedicalKnowledge WHERE Id = @Id AND IsDeleted = 0";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteEntity<MedicalKnowledge>(sql, sqlParameter);
		}

		public List<MedicalKnowledge> GetKnowledgeList()
		{
			return SqlHelper.ExecuteList<MedicalKnowledge>("  SELECT Id,[Name] FROM MedicalKnowledge WHERE IsDeleted = 0  ORDER BY [Name] ", new SqlParameter[0]);
		}

		public List<MedicalKnowledge> GetKnowledgeList(Guid speid)
		{
			string sql = "  SELECT Id,[Name] FROM MedicalKnowledge WHERE IsDeleted = 0  \r\n                                AND Id IN \r\n                                (SELECT KnowledgeId FROM  Medical WHERE SpecialityId = @specialityid GROUP BY KnowledgeId ) \r\n                                ORDER BY [Name]";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@specialityid", (object)speid) };
			return SqlHelper.ExecuteList<MedicalKnowledge>(sql, sqlParameter);
		}

		public List<MedicalKnowledge> GetList()
		{
			return SqlHelper.ExecuteList<MedicalKnowledge>("SELECT * FROM MedicalKnowledge WHERE IsDeleted = 0", new SqlParameter[0]);
		}

		public List<MedicalKnowledge> GetPage(int pageIndex, int pageCount)
		{
			string sql = "SELECT * FROM(SELECT *,(ROW_NUMBER() OVER(ORDER BY Id)) rownum FROM MedicalKnowledge WHERE IsDeleted = 0) t WHERE rownum>=(@pageCount*(@pageIndex-1)+1) AND rownum<=@pageIndex*@pageCount";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@pageIndex", (object)pageIndex), new SqlParameter("@pageCount", (object)pageCount) };
			return SqlHelper.ExecuteList<MedicalKnowledge>(sql, sqlParameter);
		}

		public int SoftDelete(Guid id)
		{
			string sql = "UPDATE MedicalKnowledge SET IsDeleted = 1 WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}

		public int Update(MedicalKnowledge medicalKnowledge)
		{
			string sql = "UPDATE MedicalKnowledge SET Name = @Name,SortId = @SortId,CreateTime = @CreateTime,IsDeleted = @IsDeleted WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)medicalKnowledge.Id), new SqlParameter("@Name", SqlHelper.ToDBValue(medicalKnowledge.Name)), new SqlParameter("@SortId", SqlHelper.ToDBValue(medicalKnowledge.SortId)), new SqlParameter("@CreateTime", SqlHelper.ToDBValue(medicalKnowledge.CreateTime)), new SqlParameter("@IsDeleted", SqlHelper.ToDBValue(medicalKnowledge.IsDeleted)) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}
	}
}