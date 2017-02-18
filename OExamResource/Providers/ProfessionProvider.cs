using Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Providers
{
	public class ProfessionProvider
	{
		private readonly static ProfessionProvider InstanceObj;

		public static ProfessionProvider Instance
		{
			get
			{
				return ProfessionProvider.InstanceObj;
			}
		}

		static ProfessionProvider()
		{
			ProfessionProvider.InstanceObj = new ProfessionProvider();
		}

		private ProfessionProvider()
		{
		}

		public Profession Create(Profession profession)
		{
			Profession profession1;
			if (profession.Id == Guid.Empty)
			{
				profession.Id = Guid.NewGuid();
			}
			string sql = "INSERT INTO Profession (Id, Name, CreateTime, IsDeleted)  VALUES (@Id, @Name, @CreateTime, @IsDeleted)";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", SqlHelper.ToDBValue(profession.Id)), new SqlParameter("@Name", SqlHelper.ToDBValue(profession.Name)), new SqlParameter("@CreateTime", SqlHelper.ToDBValue(profession.CreateTime)), new SqlParameter("@IsDeleted", SqlHelper.ToDBValue(profession.IsDeleted)) };
			if (SqlHelper.ExecuteNonQuery(sql, sqlParameter) > 0)
			{
				profession1 = profession;
			}
			else
			{
				profession1 = null;
			}
			return profession1;
		}

		public int Delete(Guid id)
		{
			string sql = "DELETE Profession WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}

		public int GetCount()
		{
			return (int)SqlHelper.ExecuteScalar("SELECT COUNT(Id) FROM Profession WHERE IsDeleted = 0", new SqlParameter[0]);
		}

		public Profession GetEntity(string name)
		{
			string sql = "SELECT * FROM Profession WHERE Name = @Name AND IsDeleted = 0";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Name", name) };
			return SqlHelper.ExecuteEntity<Profession>(sql, sqlParameter);
		}

		public Profession GetEntity(Guid id)
		{
			string sql = "SELECT * FROM Profession WHERE Id = @Id AND IsDeleted = 0";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteEntity<Profession>(sql, sqlParameter);
		}

		public List<Profession> GetList()
		{
			return SqlHelper.ExecuteList<Profession>("SELECT * FROM Profession WHERE IsDeleted = 0 ORDER BY LEN([Name])", new SqlParameter[0]);
		}

		public List<Profession> GetPage(int pageIndex, int pageCount)
		{
			string sql = "SELECT * FROM(SELECT *,(ROW_NUMBER() OVER(ORDER BY Id)) rownum FROM Profession WHERE IsDeleted = 0) t WHERE rownum>=(@pageCount*(@pageIndex-1)+1) AND rownum<=@pageIndex*@pageCount";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@pageIndex", (object)pageIndex), new SqlParameter("@pageCount", (object)pageCount) };
			return SqlHelper.ExecuteList<Profession>(sql, sqlParameter);
		}

		public int SoftDelete(Guid id)
		{
			string sql = "UPDATE Profession SET IsDeleted = 1 WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}

		public int Update(Profession profession)
		{
			string sql = "UPDATE Profession SET Name = @Name,CreateTime = @CreateTime,IsDeleted = @IsDeleted WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)profession.Id), new SqlParameter("@Name", SqlHelper.ToDBValue(profession.Name)), new SqlParameter("@CreateTime", SqlHelper.ToDBValue(profession.CreateTime)), new SqlParameter("@IsDeleted", SqlHelper.ToDBValue(profession.IsDeleted)) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}
	}
}