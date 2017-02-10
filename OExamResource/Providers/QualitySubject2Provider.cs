using Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Providers
{
	public class QualitySubject2Provider
	{
		private readonly static QualitySubject2Provider InstanceObj;

		public static QualitySubject2Provider Instance
		{
			get
			{
				return QualitySubject2Provider.InstanceObj;
			}
		}

		static QualitySubject2Provider()
		{
			QualitySubject2Provider.InstanceObj = new QualitySubject2Provider();
		}

		private QualitySubject2Provider()
		{
		}

		public QualitySubject2 Create(QualitySubject2 qualitySubject2)
		{
			QualitySubject2 qualitySubject21;
			if (qualitySubject2.Id == Guid.Empty)
			{
				qualitySubject2.Id = Guid.NewGuid();
			}
			string sql = "INSERT INTO QualitySubject2 (Id, Name, Subject1id, Count, CreateTime, IsDeleted)  VALUES (@Id, @Name, @Subject1id, @Count, @CreateTime, @IsDeleted)";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", SqlHelper.ToDBValue(qualitySubject2.Id)), new SqlParameter("@Name", SqlHelper.ToDBValue(qualitySubject2.Name)), new SqlParameter("@Subject1id", SqlHelper.ToDBValue(qualitySubject2.Subject1id)), new SqlParameter("@Count", SqlHelper.ToDBValue(qualitySubject2.Count)), new SqlParameter("@CreateTime", SqlHelper.ToDBValue(qualitySubject2.CreateTime)), new SqlParameter("@IsDeleted", SqlHelper.ToDBValue(qualitySubject2.IsDeleted)) };
			if (SqlHelper.ExecuteNonQuery(sql, sqlParameter) > 0)
			{
				qualitySubject21 = qualitySubject2;
			}
			else
			{
				qualitySubject21 = null;
			}
			return qualitySubject21;
		}

		public int Delete(Guid id)
		{
			string sql = "DELETE QualitySubject2 WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}

		public int GetCount()
		{
			return (int)SqlHelper.ExecuteScalar("SELECT COUNT(Id) FROM QualitySubject2 WHERE IsDeleted = 0", new SqlParameter[0]);
		}

		public QualitySubject2 GetEntity(Guid id)
		{
			string sql = "SELECT * FROM QualitySubject2 WHERE Id = @Id AND IsDeleted = 0";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteEntity<QualitySubject2>(sql, sqlParameter);
		}

		public List<QualitySubject2> GetList()
		{
			return SqlHelper.ExecuteList<QualitySubject2>("SELECT * FROM QualitySubject2 WHERE IsDeleted = 0", new SqlParameter[0]);
		}

		public List<QualitySubject2> GetListBySub1(Guid sub1)
		{
			string sql = "SELECT * FROM QualitySubject2 WHERE Subject1Id = @sub1 AND IsDeleted = 0";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@sub1", (object)sub1) };
			return SqlHelper.ExecuteList<QualitySubject2>(sql, sqlParameter);
		}

		public List<QualitySubject2> GetPage(int pageIndex, int pageCount)
		{
			string sql = "SELECT * FROM(SELECT *,(ROW_NUMBER() OVER(ORDER BY Id)) rownum FROM QualitySubject2 WHERE IsDeleted = 0) t WHERE rownum>=(@pageCount*(@pageIndex-1)+1) AND rownum<=@pageIndex*@pageCount";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@pageIndex", (object)pageIndex), new SqlParameter("@pageCount", (object)pageCount) };
			return SqlHelper.ExecuteList<QualitySubject2>(sql, sqlParameter);
		}

		public int SoftDelete(Guid id)
		{
			string sql = "UPDATE QualitySubject2 SET IsDeleted = 1 WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}

		public int Update(QualitySubject2 qualitySubject2)
		{
			string sql = "UPDATE QualitySubject2 SET Name = @Name,Subject1id = @Subject1id,Count = @Count,CreateTime = @CreateTime,IsDeleted = @IsDeleted WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)qualitySubject2.Id), new SqlParameter("@Name", SqlHelper.ToDBValue(qualitySubject2.Name)), new SqlParameter("@Subject1id", SqlHelper.ToDBValue(qualitySubject2.Subject1id)), new SqlParameter("@Count", SqlHelper.ToDBValue(qualitySubject2.Count)), new SqlParameter("@CreateTime", SqlHelper.ToDBValue(qualitySubject2.CreateTime)), new SqlParameter("@IsDeleted", SqlHelper.ToDBValue(qualitySubject2.IsDeleted)) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}
	}
}