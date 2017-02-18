using Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Providers
{
	public class SpecialityProvider
	{
		private readonly static SpecialityProvider InstanceObj;

		public static SpecialityProvider Instance
		{
			get
			{
				return SpecialityProvider.InstanceObj;
			}
		}

		static SpecialityProvider()
		{
			SpecialityProvider.InstanceObj = new SpecialityProvider();
		}

		private SpecialityProvider()
		{
		}

		public Speciality Create(Speciality speciality)
		{
			Speciality speciality1;
			if (speciality.Id == Guid.Empty)
			{
				speciality.Id = Guid.NewGuid();
			}
			string sql = "INSERT INTO Speciality (Id, Name, SubjectId, CreateTime, IsDeleted)  VALUES (@Id, @Name, @SubjectId, @CreateTime, @IsDeleted)";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", SqlHelper.ToDBValue(speciality.Id)), new SqlParameter("@Name", SqlHelper.ToDBValue(speciality.Name)), new SqlParameter("@SubjectId", SqlHelper.ToDBValue(speciality.SubjectId)), new SqlParameter("@CreateTime", SqlHelper.ToDBValue(speciality.CreateTime)), new SqlParameter("@IsDeleted", SqlHelper.ToDBValue(speciality.IsDeleted)) };
			if (SqlHelper.ExecuteNonQuery(sql, sqlParameter) > 0)
			{
				speciality1 = speciality;
			}
			else
			{
				speciality1 = null;
			}
			return speciality1;
		}

		public int Delete(Guid id)
		{
			string sql = "DELETE Speciality WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}

		public int GetCount()
		{
			return (int)SqlHelper.ExecuteScalar("SELECT COUNT(Id) FROM Speciality WHERE IsDeleted = 0", new SqlParameter[0]);
		}

		public Speciality GetEntity(Guid id)
		{
			string sql = "SELECT * FROM Speciality WHERE Id = @Id AND IsDeleted = 0";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteEntity<Speciality>(sql, sqlParameter);
		}

		public List<Speciality> GetList()
		{
			return SqlHelper.ExecuteList<Speciality>("SELECT * FROM Speciality WHERE IsDeleted = 0", new SqlParameter[0]);
		}

		public List<Speciality> GetList(Guid subid)
		{
			string sql = "SELECT * FROM Speciality WHERE SubjectId = @SubjectId AND IsDeleted = 0";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@SubjectId", (object)subid) };
			return SqlHelper.ExecuteList<Speciality>(sql, sqlParameter);
		}

		public bool GetNameBool(Guid id, string name)
		{
			bool flag;
			string sql = "SELECT * FROM Speciality WHERE Id <> @Id AND IsDeleted = 0 AND Name = @name";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id), new SqlParameter("@Name", name) };
			flag = (SqlHelper.ExecuteList<Speciality>(sql, sqlParameter).Count <= 0 ? false : true);
			return flag;
		}

		public bool GetNameBool(string name)
		{
			bool flag;
			string sql = "SELECT * FROM Speciality WHERE  IsDeleted = 0 AND Name = @name";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Name", name) };
			flag = (SqlHelper.ExecuteList<Speciality>(sql, sqlParameter).Count <= 0 ? false : true);
			return flag;
		}

		public List<Speciality> GetPage(int pageIndex, int pageCount)
		{
			string sql = "SELECT * FROM(SELECT *,(ROW_NUMBER() OVER(ORDER BY Id)) rownum FROM Speciality WHERE IsDeleted = 0) t WHERE rownum>=(@pageCount*(@pageIndex-1)+1) AND rownum<=@pageIndex*@pageCount";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@pageIndex", (object)pageIndex), new SqlParameter("@pageCount", (object)pageCount) };
			return SqlHelper.ExecuteList<Speciality>(sql, sqlParameter);
		}

		public int SoftDelete(Guid id)
		{
			string sql = "UPDATE Speciality SET IsDeleted = 1 WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}

		public int Update(Speciality speciality)
		{
			string sql = "UPDATE Speciality SET Name = @Name,SubjectId = @SubjectId,CreateTime = @CreateTime,IsDeleted = @IsDeleted WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)speciality.Id), new SqlParameter("@Name", SqlHelper.ToDBValue(speciality.Name)), new SqlParameter("@SubjectId", SqlHelper.ToDBValue(speciality.SubjectId)), new SqlParameter("@CreateTime", SqlHelper.ToDBValue(speciality.CreateTime)), new SqlParameter("@IsDeleted", SqlHelper.ToDBValue(speciality.IsDeleted)) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}
	}
}