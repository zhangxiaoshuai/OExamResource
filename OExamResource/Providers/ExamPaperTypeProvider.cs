using Common;
using Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Providers
{
	public class ExamPaperTypeProvider
	{
		private readonly static ExamPaperTypeProvider InstanceObj;

		public static ExamPaperTypeProvider Instance
		{
			get
			{
				return ExamPaperTypeProvider.InstanceObj;
			}
		}

		static ExamPaperTypeProvider()
		{
			ExamPaperTypeProvider.InstanceObj = new ExamPaperTypeProvider();
		}

		private ExamPaperTypeProvider()
		{
		}

		public ExamPaperType Create(ExamPaperType examPaperType)
		{
			ExamPaperType examPaperType1;
			if (examPaperType.Id == Guid.Empty)
			{
				examPaperType.Id = Guid.NewGuid();
			}
			string sql = "INSERT INTO ExamPaperType (Id, Name, SortId, Logo, ParentId, PaperCount, Tier, IsDeleted, CreateTime)  VALUES (@Id, @Name, @SortId, @Logo, @ParentId, @PaperCount, @Tier, @IsDeleted, @CreateTime)";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", SqlHelper.ToDBValue(examPaperType.Id)), new SqlParameter("@Name", SqlHelper.ToDBValue(examPaperType.Name)), new SqlParameter("@SortId", SqlHelper.ToDBValue(examPaperType.SortId)), new SqlParameter("@Logo", SqlHelper.ToDBValue(examPaperType.Logo)), new SqlParameter("@ParentId", SqlHelper.ToDBValue(examPaperType.ParentId)), new SqlParameter("@PaperCount", SqlHelper.ToDBValue(examPaperType.PaperCount)), new SqlParameter("@Tier", SqlHelper.ToDBValue(examPaperType.Tier)), new SqlParameter("@IsDeleted", SqlHelper.ToDBValue(examPaperType.IsDeleted)), new SqlParameter("@CreateTime", SqlHelper.ToDBValue(examPaperType.CreateTime)) };
			if (SqlHelper.ExecuteNonQuery(sql, sqlParameter) > 0)
			{
				examPaperType1 = examPaperType;
			}
			else
			{
				examPaperType1 = null;
			}
			return examPaperType1;
		}

		public int Delete(Guid id)
		{
			string sql = "DELETE ExamPaperType WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}

		public int GetCount()
		{
			return (int)SqlHelper.ExecuteScalar("SELECT COUNT(Id) FROM ExamPaperType WHERE IsDeleted = 0", new SqlParameter[0]);
		}

		public ExamPaperType GetEntity(Guid id)
		{
			string sql = "SELECT * FROM ExamPaperType WHERE Id = @Id AND IsDeleted = 0";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteEntity<ExamPaperType>(sql, sqlParameter);
		}

		public ExamPaperType GetEntityAdmin(Guid id)
		{
			string sql = "SELECT * FROM ExamPaperType WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteEntity<ExamPaperType>(sql, sqlParameter);
		}

		public List<ExamPaperType> GetList(Enums.PaperTier tier)
		{
			string sql = "SELECT * FROM ExamPaperType WHERE Tier = @Tier AND IsDeleted = 0 ORDER BY SortId";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Tier", (object)((int)tier)) };
			return SqlHelper.ExecuteList<ExamPaperType>(sql, sqlParameter);
		}

		public List<ExamPaperType> GetList(Guid parentId, Enums.PaperTier tier)
		{
			string sql = "SELECT * FROM ExamPaperType WHERE ParentId = @ParentId AND Tier = @Tier AND IsDeleted = 0 ORDER BY SortId";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Tier", (object)((int)tier)), new SqlParameter("@ParentId", (object)parentId) };
			return SqlHelper.ExecuteList<ExamPaperType>(sql, sqlParameter);
		}

		public List<ExamPaperType> GetList(Guid parentId, int tier)
		{
			List<ExamPaperType> examPaperTypes;
			examPaperTypes = (tier <= 2 ? this.GetList(parentId, (Enums.PaperTier)tier) : new List<ExamPaperType>());
			return examPaperTypes;
		}

		public List<ExamPaperType> GetList(string key, int year = -1, int mean = -1)
		{
			string sql = "SELECT TypeId as Id,Tier = 2,COUNT(1) as PaperCount FROM ExamPaper WHERE Name LIKE @key AND IsDeleted = 0";
			List<SqlParameter> sqlParameters = new List<SqlParameter>()
			{
				new SqlParameter("@key", string.Concat("%", key, "%"))
			};
			List<SqlParameter> par = sqlParameters;
			if (year != -1)
			{
				sql = string.Concat(sql, " AND Year = @year ");
				par.Add(new SqlParameter("@year", (object)year));
			}
			if (mean != -1)
			{
				sql = string.Concat(sql, " AND Means = @mean ");
				par.Add(new SqlParameter("@mean", (object)mean));
			}
			sql = string.Concat(sql, "GROUP BY TypeId");
			return SqlHelper.ExecuteList<ExamPaperType>(sql, par.ToArray());
		}

		public List<ExamPaperType> GetList(Guid parentId)
		{
			string sql = "WITH CTE AS\r\n                            (\r\n                            SELECT * FROM ExamPaperType WHERE Id = @Id AND IsDeleted = 0\r\n                            UNION ALL\r\n                            SELECT ExamPaperType.* FROM ExamPaperType join CTE ON ExamPaperType.ParentId = CTE.Id WHERE ExamPaperType.IsDeleted = 0\r\n                            )\r\n                            SELECT * FROM CTE ORDER BY SortId ASC,Tier DESC";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)parentId) };
			return SqlHelper.ExecuteList<ExamPaperType>(sql, sqlParameter);
		}

		public List<ExamPaperType> GetList()
		{
			return SqlHelper.ExecuteList<ExamPaperType>("SELECT * FROM ExamPaperType WHERE IsDeleted = 0 order by SortId", new SqlParameter[0]);
		}

		public List<ExamPaperType> GetListAdmin()
		{
			return SqlHelper.ExecuteList<ExamPaperType>("SELECT * FROM ExamPaperType  order by SortId", new SqlParameter[0]);
		}

		public List<ExamPaperType> GetListsAdmin(Enums.PaperTier tier)
		{
			string sql = "SELECT * FROM ExamPaperType WHERE Tier = @Tier   ORDER BY SortId";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Tier", (object)((int)tier)) };
			return SqlHelper.ExecuteList<ExamPaperType>(sql, sqlParameter);
		}

		public List<ExamPaperType> GetPage(int pageIndex, int pageCount)
		{
			string sql = "SELECT * FROM(SELECT *,(ROW_NUMBER() OVER(ORDER BY Id)) rownum FROM ExamPaperType WHERE IsDeleted = 0) t WHERE rownum>=(@pageCount*(@pageIndex-1)+1) AND rownum<=@pageIndex*@pageCount";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@pageIndex", (object)pageIndex), new SqlParameter("@pageCount", (object)pageCount) };
			return SqlHelper.ExecuteList<ExamPaperType>(sql, sqlParameter);
		}

		public List<ExamPaperType> GetSuperList(Guid id)
		{
			string sql = "WITH CTE AS\r\n                            (\r\n                            SELECT * FROM ExamPaperType WHERE Id = @Id\r\n                            UNION ALL\r\n                            SELECT ExamPaperType.* FROM ExamPaperType join CTE ON ExamPaperType.Id = CTE.ParentId\r\n                            )\r\n                            SELECT * FROM CTE ORDER BY Tier,SortId";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteList<ExamPaperType>(sql, sqlParameter);
		}

		public int SoftDelete(Guid id)
		{
			string sql = "UPDATE ExamPaperType SET IsDeleted = 1 WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}

		public int Update(ExamPaperType examPaperType)
		{
			string sql = "UPDATE ExamPaperType SET Name = @Name,SortId = @SortId,Logo = @Logo,ParentId = @ParentId,PaperCount = @PaperCount,Tier = @Tier,IsDeleted = @IsDeleted,CreateTime = @CreateTime WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)examPaperType.Id), new SqlParameter("@Name", SqlHelper.ToDBValue(examPaperType.Name)), new SqlParameter("@SortId", SqlHelper.ToDBValue(examPaperType.SortId)), new SqlParameter("@Logo", SqlHelper.ToDBValue(examPaperType.Logo)), new SqlParameter("@ParentId", SqlHelper.ToDBValue(examPaperType.ParentId)), new SqlParameter("@PaperCount", SqlHelper.ToDBValue(examPaperType.PaperCount)), new SqlParameter("@Tier", SqlHelper.ToDBValue(examPaperType.Tier)), new SqlParameter("@IsDeleted", SqlHelper.ToDBValue(examPaperType.IsDeleted)), new SqlParameter("@CreateTime", SqlHelper.ToDBValue(examPaperType.CreateTime)) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}
	}
}