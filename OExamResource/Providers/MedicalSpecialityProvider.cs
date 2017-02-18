using Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Providers
{
	public class MedicalSpecialityProvider
	{
		private readonly static MedicalSpecialityProvider InstanceObj;

		public static MedicalSpecialityProvider Instance
		{
			get
			{
				return MedicalSpecialityProvider.InstanceObj;
			}
		}

		static MedicalSpecialityProvider()
		{
			MedicalSpecialityProvider.InstanceObj = new MedicalSpecialityProvider();
		}

		private MedicalSpecialityProvider()
		{
		}

		public MedicalSpeciality Create(MedicalSpeciality medicalSpeciality)
		{
			MedicalSpeciality medicalSpeciality1;
			if (medicalSpeciality.Id == Guid.Empty)
			{
				medicalSpeciality.Id = Guid.NewGuid();
			}
			string sql = "INSERT INTO MedicalSpeciality (Id, Name, SortId, Count, CreateTime, IsDeleted)  VALUES (@Id, @Name, @SortId, @Count, @CreateTime, @IsDeleted)";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", SqlHelper.ToDBValue(medicalSpeciality.Id)), new SqlParameter("@Name", SqlHelper.ToDBValue(medicalSpeciality.Name)), new SqlParameter("@SortId", SqlHelper.ToDBValue(medicalSpeciality.SortId)), new SqlParameter("@Count", SqlHelper.ToDBValue(medicalSpeciality.Count)), new SqlParameter("@CreateTime", SqlHelper.ToDBValue(medicalSpeciality.CreateTime)), new SqlParameter("@IsDeleted", SqlHelper.ToDBValue(medicalSpeciality.IsDeleted)) };
			if (SqlHelper.ExecuteNonQuery(sql, sqlParameter) > 0)
			{
				medicalSpeciality1 = medicalSpeciality;
			}
			else
			{
				medicalSpeciality1 = null;
			}
			return medicalSpeciality1;
		}

		public int Delete(Guid id)
		{
			string sql = "DELETE MedicalSpeciality WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}

		public int GetCount()
		{
			return (int)SqlHelper.ExecuteScalar("SELECT COUNT(Id) FROM MedicalSpeciality WHERE IsDeleted = 0", new SqlParameter[0]);
		}

		public MedicalSpeciality GetEntity(Guid id)
		{
			string sql = "SELECT * FROM MedicalSpeciality WHERE Id = @Id AND IsDeleted = 0";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteEntity<MedicalSpeciality>(sql, sqlParameter);
		}

		public List<MedicalSpeciality> GetList()
		{
			return SqlHelper.ExecuteList<MedicalSpeciality>("SELECT * FROM MedicalSpeciality WHERE IsDeleted = 0", new SqlParameter[0]);
		}

		public List<MedicalSpeciality> GetPage(int pageIndex, int pageCount)
		{
			string sql = "SELECT * FROM(SELECT *,(ROW_NUMBER() OVER(ORDER BY Id)) rownum FROM MedicalSpeciality WHERE IsDeleted = 0) t WHERE rownum>=(@pageCount*(@pageIndex-1)+1) AND rownum<=@pageIndex*@pageCount";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@pageIndex", (object)pageIndex), new SqlParameter("@pageCount", (object)pageCount) };
			return SqlHelper.ExecuteList<MedicalSpeciality>(sql, sqlParameter);
		}

		public List<MedicalSpeciality> GetSpecialityList()
		{
			return SqlHelper.ExecuteList<MedicalSpeciality>("  SELECT Id,[Name] FROM MedicalSpeciality WHERE IsDeleted = 0 ORDER BY [Name] ", new SqlParameter[0]);
		}

		public List<MedicalSpeciality> GetSpecialityList(Guid cateid)
		{
			string sql = "  SELECT Id,[Name] FROM MedicalSpeciality WHERE IsDeleted = 0  \r\n                                AND Id IN\r\n                                (SELECT SpecialityId FROM  Medical WHERE CategoryId = @categoryid GROUP BY SpecialityId ) \r\n                                ORDER BY [Name]";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@categoryid", (object)cateid) };
			return SqlHelper.ExecuteList<MedicalSpeciality>(sql, sqlParameter);
		}

		public int SoftDelete(Guid id)
		{
			string sql = "UPDATE MedicalSpeciality SET IsDeleted = 1 WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}

		public int Update(MedicalSpeciality medicalSpeciality)
		{
			string sql = "UPDATE MedicalSpeciality SET Name = @Name,SortId = @SortId,Count = @Count,CreateTime = @CreateTime,IsDeleted = @IsDeleted WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)medicalSpeciality.Id), new SqlParameter("@Name", SqlHelper.ToDBValue(medicalSpeciality.Name)), new SqlParameter("@SortId", SqlHelper.ToDBValue(medicalSpeciality.SortId)), new SqlParameter("@Count", SqlHelper.ToDBValue(medicalSpeciality.Count)), new SqlParameter("@CreateTime", SqlHelper.ToDBValue(medicalSpeciality.CreateTime)), new SqlParameter("@IsDeleted", SqlHelper.ToDBValue(medicalSpeciality.IsDeleted)) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}
	}
}