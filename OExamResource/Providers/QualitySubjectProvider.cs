using Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Providers
{
	public class QualitySubjectProvider
	{
		private readonly static QualitySubjectProvider InstanceObj;

		public static QualitySubjectProvider Instance
		{
			get
			{
				return QualitySubjectProvider.InstanceObj;
			}
		}

		static QualitySubjectProvider()
		{
			QualitySubjectProvider.InstanceObj = new QualitySubjectProvider();
		}

		private QualitySubjectProvider()
		{
		}

		public QualitySubject Create(QualitySubject qualitySubject)
		{
			QualitySubject qualitySubject1;
			if (qualitySubject.Id == Guid.Empty)
			{
				qualitySubject.Id = Guid.NewGuid();
			}
			string sql = "INSERT INTO QualitySubject (Id, Name, Ico, Count, Createtime, IsDeleted)  VALUES (@Id, @Name, @Ico, @Count, @Createtime, @IsDeleted)";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", SqlHelper.ToDBValue(qualitySubject.Id)), new SqlParameter("@Name", SqlHelper.ToDBValue(qualitySubject.Name)), new SqlParameter("@Ico", SqlHelper.ToDBValue(qualitySubject.Ico)), new SqlParameter("@Count", SqlHelper.ToDBValue(qualitySubject.Count)), new SqlParameter("@Createtime", SqlHelper.ToDBValue(qualitySubject.Createtime)), new SqlParameter("@IsDeleted", SqlHelper.ToDBValue(qualitySubject.IsDeleted)) };
			if (SqlHelper.ExecuteNonQuery(sql, sqlParameter) > 0)
			{
				qualitySubject1 = qualitySubject;
			}
			else
			{
				qualitySubject1 = null;
			}
			return qualitySubject1;
		}

		public int Delete(Guid id)
		{
			string sql = "DELETE QualitySubject WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}

		public int GetCount()
		{
			return (int)SqlHelper.ExecuteScalar("SELECT COUNT(Id) FROM QualitySubject WHERE IsDeleted = 0", new SqlParameter[0]);
		}

		public QualitySubject GetEntity(Guid id)
		{
			string sql = "SELECT * FROM QualitySubject WHERE Id = @Id AND IsDeleted = 0";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteEntity<QualitySubject>(sql, sqlParameter);
		}

		public List<QualitySubject> GetList()
		{
			return SqlHelper.ExecuteList<QualitySubject>("SELECT * FROM QualitySubject WHERE IsDeleted = 0 and count>0", new SqlParameter[0]);
		}

		public List<QualitySubject> GetPage(int pageIndex, int pageCount)
		{
			string sql = "SELECT * FROM(SELECT *,(ROW_NUMBER() OVER(ORDER BY Id)) rownum FROM QualitySubject WHERE IsDeleted = 0) t WHERE rownum>=(@pageCount*(@pageIndex-1)+1) AND rownum<=@pageIndex*@pageCount";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@pageIndex", (object)pageIndex), new SqlParameter("@pageCount", (object)pageCount) };
			return SqlHelper.ExecuteList<QualitySubject>(sql, sqlParameter);
		}

		public int SoftDelete(Guid id)
		{
			string sql = "UPDATE QualitySubject SET IsDeleted = 1 WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}

		public int Update(QualitySubject qualitySubject)
		{
			string sql = "UPDATE QualitySubject SET Name = @Name,Ico = @Ico,Count = @Count,Createtime = @Createtime,IsDeleted = @IsDeleted WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)qualitySubject.Id), new SqlParameter("@Name", SqlHelper.ToDBValue(qualitySubject.Name)), new SqlParameter("@Ico", SqlHelper.ToDBValue(qualitySubject.Ico)), new SqlParameter("@Count", SqlHelper.ToDBValue(qualitySubject.Count)), new SqlParameter("@Createtime", SqlHelper.ToDBValue(qualitySubject.Createtime)), new SqlParameter("@IsDeleted", SqlHelper.ToDBValue(qualitySubject.IsDeleted)) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}
	}
}