using Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Providers
{
	public class QualitySubject1Provider
	{
		private readonly static QualitySubject1Provider InstanceObj;

		public static QualitySubject1Provider Instance
		{
			get
			{
				return QualitySubject1Provider.InstanceObj;
			}
		}

		static QualitySubject1Provider()
		{
			QualitySubject1Provider.InstanceObj = new QualitySubject1Provider();
		}

		private QualitySubject1Provider()
		{
		}

		public QualitySubject1 Create(QualitySubject1 qualitySubject1)
		{
			QualitySubject1 qualitySubject11;
			if (qualitySubject1.Id == Guid.Empty)
			{
				qualitySubject1.Id = Guid.NewGuid();
			}
			string sql = "INSERT INTO QualitySubject1 (Id, Name, Count, SubjectId, CreateTime, IsDeleted)  VALUES (@Id, @Name, @Count, @SubjectId, @CreateTime, @IsDeleted)";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", SqlHelper.ToDBValue(qualitySubject1.Id)), new SqlParameter("@Name", SqlHelper.ToDBValue(qualitySubject1.Name)), new SqlParameter("@Count", SqlHelper.ToDBValue(qualitySubject1.Count)), new SqlParameter("@SubjectId", SqlHelper.ToDBValue(qualitySubject1.SubjectId)), new SqlParameter("@CreateTime", SqlHelper.ToDBValue(qualitySubject1.CreateTime)), new SqlParameter("@IsDeleted", SqlHelper.ToDBValue(qualitySubject1.IsDeleted)) };
			if (SqlHelper.ExecuteNonQuery(sql, sqlParameter) > 0)
			{
				qualitySubject11 = qualitySubject1;
			}
			else
			{
				qualitySubject11 = null;
			}
			return qualitySubject11;
		}

		public int Delete(Guid id)
		{
			string sql = "DELETE QualitySubject1 WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}

		public int GetCount()
		{
			return (int)SqlHelper.ExecuteScalar("SELECT COUNT(Id) FROM QualitySubject1 WHERE IsDeleted = 0", new SqlParameter[0]);
		}

		public QualitySubject1 GetEntity(Guid id)
		{
			string sql = "SELECT * FROM QualitySubject1 WHERE Id = @Id AND IsDeleted = 0";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteEntity<QualitySubject1>(sql, sqlParameter);
		}

		public List<QualitySubject1> GetList()
		{
			return SqlHelper.ExecuteList<QualitySubject1>("SELECT * FROM QualitySubject1 WHERE IsDeleted = 0", new SqlParameter[0]);
		}

		public List<QualitySubject1> GetPage(int pageIndex, int pageCount)
		{
			string sql = "SELECT * FROM(SELECT *,(ROW_NUMBER() OVER(ORDER BY Id)) rownum FROM QualitySubject1 WHERE IsDeleted = 0) t WHERE rownum>=(@pageCount*(@pageIndex-1)+1) AND rownum<=@pageIndex*@pageCount";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@pageIndex", (object)pageIndex), new SqlParameter("@pageCount", (object)pageCount) };
			return SqlHelper.ExecuteList<QualitySubject1>(sql, sqlParameter);
		}

		public int SoftDelete(Guid id)
		{
			string sql = "UPDATE QualitySubject1 SET IsDeleted = 1 WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}

		public int Update(QualitySubject1 qualitySubject1)
		{
			string sql = "UPDATE QualitySubject1 SET Name = @Name,Count = @Count,SubjectId = @SubjectId,CreateTime = @CreateTime,IsDeleted = @IsDeleted WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)qualitySubject1.Id), new SqlParameter("@Name", SqlHelper.ToDBValue(qualitySubject1.Name)), new SqlParameter("@Count", SqlHelper.ToDBValue(qualitySubject1.Count)), new SqlParameter("@SubjectId", SqlHelper.ToDBValue(qualitySubject1.SubjectId)), new SqlParameter("@CreateTime", SqlHelper.ToDBValue(qualitySubject1.CreateTime)), new SqlParameter("@IsDeleted", SqlHelper.ToDBValue(qualitySubject1.IsDeleted)) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}
	}
}