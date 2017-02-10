using Common;
using Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Providers
{
	public class ExamPaperProvider
	{
		private readonly static ExamPaperProvider InstanceObj;

		public static ExamPaperProvider Instance
		{
			get
			{
				return ExamPaperProvider.InstanceObj;
			}
		}

		static ExamPaperProvider()
		{
			ExamPaperProvider.InstanceObj = new ExamPaperProvider();
		}

		private ExamPaperProvider()
		{
		}

		public ExamPaper Create(ExamPaper examPaper)
		{
			ExamPaper examPaper1;
			if (examPaper.Id == Guid.Empty)
			{
				examPaper.Id = Guid.NewGuid();
			}
			string sql = "INSERT INTO ExamPaper (Id, Name, SortId, Means, TimeOut, Year, Month, TypeId, CreateTime, IsDeleted)  VALUES (@Id, @Name, @SortId, @Means, @TimeOut, @Year, @Month, @TypeId, @CreateTime, @IsDeleted)";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", SqlHelper.ToDBValue(examPaper.Id)), new SqlParameter("@Name", SqlHelper.ToDBValue(examPaper.Name)), new SqlParameter("@SortId", SqlHelper.ToDBValue(examPaper.SortId)), new SqlParameter("@Means", SqlHelper.ToDBValue(examPaper.Means)), new SqlParameter("@TimeOut", SqlHelper.ToDBValue(examPaper.TimeOut)), new SqlParameter("@Year", SqlHelper.ToDBValue(examPaper.Year)), new SqlParameter("@Month", SqlHelper.ToDBValue(examPaper.Month)), new SqlParameter("@TypeId", SqlHelper.ToDBValue(examPaper.TypeId)), new SqlParameter("@CreateTime", SqlHelper.ToDBValue(examPaper.CreateTime)), new SqlParameter("@IsDeleted", SqlHelper.ToDBValue(examPaper.IsDeleted)) };
			if (SqlHelper.ExecuteNonQuery(sql, sqlParameter) > 0)
			{
				examPaper1 = examPaper;
			}
			else
			{
				examPaper1 = null;
			}
			return examPaper1;
		}

		public int Delete(Guid id)
		{
			string sql = "DELETE ExamPaper WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}

		public List<int> GetAllYear()
		{
			List<int> list = (
				from p in SqlHelper.ExecuteList<ExamPaper>("select distinct Year from ExamPaper where Year is not null order by Year", new SqlParameter[0])
				select p.Year.Value).ToList<int>();
			return list;
		}

		public int GetCount()
		{
			return (int)SqlHelper.ExecuteScalar("SELECT COUNT(Id) FROM ExamPaper WHERE IsDeleted = 0", new SqlParameter[0]);
		}

		public int GetCount(Guid typeId, Enums.PaperMeans type)
		{
			string sql = " with CTE AS\r\n\t                    (\r\n\t                    SELECT * FROM ExamPaperType WHERE Id = @Id\r\n\t                    UNION ALL\r\n\t                    SELECT ExamPaperType.* FROM ExamPaperType join CTE ON ExamPaperType.ParentId = CTE.Id\r\n\t                    )\r\n\t                    select count(1) from ExamPaper where TypeId in (SELECT Id from CTE WHERE Tier = 2) AND Means = @Means AND IsDeleted = 0";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)typeId), new SqlParameter("@Means", (object)((int)type)) };
			return (int)SqlHelper.ExecuteScalar(sql, sqlParameter);
		}

		public int GetCount(string key, int year = -1, int mean = -1)
		{
			string sql = "SELECT COUNT(1) FROM ExamPaper WHERE IsDeleted = 0";
			List<SqlParameter> par = new List<SqlParameter>();
			if (!string.IsNullOrWhiteSpace(key))
			{
				sql = string.Concat(sql, " AND Name LIKE @key");
				par.Add(new SqlParameter("@key", string.Concat("%", key, "%")));
			}
			if (year != -1)
			{
				sql = string.Concat(sql, " AND Year = @year");
				par.Add(new SqlParameter("@year", (object)year));
			}
			if (mean != -1)
			{
				sql = string.Concat(sql, " AND Means = @mean");
				par.Add(new SqlParameter("@mean", (object)mean));
			}
			return (int)SqlHelper.ExecuteScalar(sql, par.ToArray());
		}

		public int GetCount(Guid typeId, string key, int year = -1, int mean = -1)
		{
			string sql = "with CTE AS\r\n\t                    (\r\n\t                    SELECT * FROM ExamPaperType WHERE Id = @Id AND IsDeleted = 0\r\n\t                    UNION ALL\r\n\t                    SELECT ExamPaperType.* FROM ExamPaperType join CTE ON ExamPaperType.ParentId = CTE.Id AND ExamPaperType.IsDeleted = 0\r\n\t                    )\r\n\t                    select count(1) from ExamPaper where TypeId in (SELECT Id from CTE WHERE Tier = 2 AND IsDeleted = 0) AND IsDeleted = 0";
			List<SqlParameter> par = new List<SqlParameter>()
			{
				new SqlParameter("@Id", (object)typeId)
			};
			if (!string.IsNullOrWhiteSpace(key))
			{
				sql = string.Concat(sql, " AND Name LIKE @key");
				par.Add(new SqlParameter("@key", string.Concat("%", key, "%")));
			}
			if (year != -1)
			{
				sql = string.Concat(sql, " AND Year = @year");
				par.Add(new SqlParameter("@year", (object)year));
			}
			if (mean != -1)
			{
				sql = string.Concat(sql, " AND Means = @mean");
				par.Add(new SqlParameter("@mean", (object)mean));
			}
			return (int)SqlHelper.ExecuteScalar(sql, par.ToArray());
		}

		public ExamPaper GetEntity(Guid id)
		{
			string sql = "SELECT * FROM ExamPaper WHERE Id = @Id AND IsDeleted = 0";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteEntity<ExamPaper>(sql, sqlParameter);
		}

		public List<ExamPaper> GetList()
		{
			return SqlHelper.ExecuteList<ExamPaper>("SELECT * FROM ExamPaper WHERE IsDeleted = 0", new SqlParameter[0]);
		}

		public List<ExamPaper> GetList(Guid typeId, int pageIndex, int pageCount)
		{
			string sql = "with CTE AS\r\n                        (\r\n                        SELECT * FROM ExamPaperType WHERE Id = @Id\r\n                        UNION ALL\r\n                        SELECT ExamPaperType.* FROM ExamPaperType join CTE ON ExamPaperType.ParentId = CTE.Id\r\n                        )\r\n                        SELECT * FROM (SELECT ExamPaper.*,(ROW_NUMBER() OVER(ORDER BY ExamPaper.SortId DESC,ExamPaper.Year desc)) rownum FROM ExamPaper JOIN CTE \r\n                        ON ExamPaper.TypeId = CTE.Id\r\n                        WHERE CTE.Tier = @Tier AND ExamPaper.IsDeleted = 0  ) t  WHERE rownum>=(@pageCount*(@pageIndex-1)+1) AND rownum<=@pageIndex*@pageCount";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)typeId), new SqlParameter("@Tier", (object)2), new SqlParameter("@pageCount", (object)pageCount), new SqlParameter("@pageIndex", (object)pageIndex) };
			return SqlHelper.ExecuteList<ExamPaper>(sql, sqlParameter);
		}

		public List<ExamPaper> GetList(Guid typeId, int pageIndex, int pageCount, Enums.PaperMeans means)
		{
			string sql = "with CTE AS\r\n                        (\r\n                        SELECT * FROM ExamPaperType WHERE Id = @Id\r\n                        UNION ALL\r\n                        SELECT ExamPaperType.* FROM ExamPaperType join CTE ON ExamPaperType.ParentId = CTE.Id\r\n                        )\r\n                        SELECT * FROM (SELECT ExamPaper.*,(ROW_NUMBER() OVER(ORDER BY ExamPaper.Year DESC)) rownum FROM ExamPaper JOIN CTE \r\n                        ON ExamPaper.TypeId = CTE.Id\r\n                        WHERE CTE.Tier = @Tier AND ExamPaper.Means = @Meams AND ExamPaper.IsDeleted = 0 ) t  WHERE rownum>=(@pageCount*(@pageIndex-1)+1) AND rownum<=@pageIndex*@pageCount";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)typeId), new SqlParameter("@Tier", (object)2), new SqlParameter("@Meams", (object)((int)means)), new SqlParameter("@pageCount", (object)pageCount), new SqlParameter("@pageIndex", (object)pageIndex) };
			return SqlHelper.ExecuteList<ExamPaper>(sql, sqlParameter);
		}

		public List<ExamPaper> GetListByTypeId2(Guid typeid, string name, int pageIndex, int pageCount)
		{
			string sql = "SELECT * FROM (\r\n                                SELECT ExamPaper.*,(ROW_NUMBER() OVER(ORDER BY ExamPaper.SortId DESC,ExamPaper.Year DESC)) rownum FROM ExamPaper \r\n                                WHERE ExamPaper.TypeId in (\r\n                                    SELECT Id FROM ExamPaperType\r\n                                WHERE ParentId= @typeid) AND ExamPaper.IsDeleted = 0 ";
			if (!string.IsNullOrWhiteSpace(name))
			{
				sql = string.Concat(sql, " AND ExamPaper.Name LIKE @name");
			}
			sql = string.Concat(sql, ") t WHERE rownum>=(@pageCount*(@pageIndex-1)+1) AND rownum<=@pageIndex*@pageCount ");
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@typeid", (object)typeid), new SqlParameter("@pageCount", (object)pageCount), new SqlParameter("@pageIndex", (object)pageIndex), new SqlParameter("@name", string.Concat("%", name, "%")) };
			return SqlHelper.ExecuteList<ExamPaper>(sql, sqlParameter);
		}

		public List<ExamPaper> GetListByTypeId3(Guid typeid, string name, int pageIndex, int pageCount)
		{
			string sql = "SELECT * FROM (\r\n                                SELECT ExamPaper.*,(ROW_NUMBER() OVER(ORDER BY ExamPaper.SortId DESC,ExamPaper.Year DESC)) rownum FROM ExamPaper \r\n                                WHERE ExamPaper.TypeId = @typeid AND ExamPaper.IsDeleted = 0 ";
			if (!string.IsNullOrWhiteSpace(name))
			{
				sql = string.Concat(sql, " AND ExamPaper.Name LIKE @name");
			}
			sql = string.Concat(sql, ") t WHERE rownum>=(@pageCount*(@pageIndex-1)+1) AND rownum<=@pageIndex*@pageCount ");
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@typeid", (object)typeid), new SqlParameter("@pageCount", (object)pageCount), new SqlParameter("@pageIndex", (object)pageIndex), new SqlParameter("@name", string.Concat("%", name, "%")) };
			return SqlHelper.ExecuteList<ExamPaper>(sql, sqlParameter);
		}

		public int GetListCountByTypeId2(Guid typeid, string name)
		{
			string sql = "SELECT Count(Id) FROM ExamPaper\r\n                                WHERE  TypeId in (\r\n                                    SELECT Id FROM ExamPaperType\r\n                                    WHERE ParentId= @typeid\r\n                                ) AND IsDeleted = 0 ";
			if (!string.IsNullOrWhiteSpace(name))
			{
				sql = string.Concat(sql, " AND ExamPaper.Name LIKE @name");
			}
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@typeid", (object)typeid), new SqlParameter("@name", string.Concat("%", name, "%")) };
			return (int)SqlHelper.ExecuteScalar(sql, sqlParameter);
		}

		public int GetListCountByTypeId3(Guid typeid, string name)
		{
			string sql = "SELECT Count(Id) FROM ExamPaper WHERE TypeId = @typeid AND IsDeleted = 0 ";
			if (!string.IsNullOrWhiteSpace(name))
			{
				sql = string.Concat(sql, " AND ExamPaper.Name LIKE @name");
			}
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@typeid", (object)typeid), new SqlParameter("@name", string.Concat("%", name, "%")) };
			return (int)SqlHelper.ExecuteScalar(sql, sqlParameter);
		}

		public List<ExamPaper> GetPage(int pageIndex, int pageCount)
		{
			string sql = "SELECT * FROM(SELECT *,(ROW_NUMBER() OVER(ORDER BY Id)) rownum FROM ExamPaper WHERE IsDeleted = 0) t WHERE rownum>=(@pageCount*(@pageIndex-1)+1) AND rownum<=@pageIndex*@pageCount";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@pageIndex", (object)pageIndex), new SqlParameter("@pageCount", (object)pageCount) };
			return SqlHelper.ExecuteList<ExamPaper>(sql, sqlParameter);
		}

		public List<ExamPaper> SearchPage(Guid typeId, string key, int pageIndex, int pageCount, int year = -1, int mean = -1)
		{
			string sql = "with CTE AS\r\n                        (\r\n                        SELECT * FROM ExamPaperType WHERE Id = @Id AND IsDeleted = 0\r\n                        UNION ALL\r\n                        SELECT ExamPaperType.* FROM ExamPaperType join CTE ON ExamPaperType.ParentId = CTE.Id WHERE ExamPaperType.IsDeleted = 0\r\n                        )\r\n                        SELECT * FROM (SELECT ExamPaper.*,(ROW_NUMBER() OVER(ORDER BY ExamPaper.SortId DESC,ExamPaper.Year DESC)) rownum FROM ExamPaper JOIN CTE \r\n                        ON ExamPaper.TypeId = CTE.Id\r\n                        WHERE ExamPaper.IsDeleted = 0 AND CTE.Tier = @Tier";
			List<SqlParameter> sqlParameters = new List<SqlParameter>()
			{
				new SqlParameter("@Id", (object)typeId),
				new SqlParameter("@Tier", (object)2),
				new SqlParameter("@pageCount", (object)pageCount),
				new SqlParameter("@pageIndex", (object)pageIndex)
			};
			List<SqlParameter> par = sqlParameters;
			if (!string.IsNullOrWhiteSpace(key))
			{
				sql = string.Concat(sql, " AND ExamPaper.Name LIKE @Key");
				par.Add(new SqlParameter("@key", string.Concat("%", key, "%")));
			}
			if (year != -1)
			{
				sql = string.Concat(sql, " AND ExamPaper.Year = @year ");
				par.Add(new SqlParameter("@year", (object)year));
			}
			if (mean != -1)
			{
				sql = string.Concat(sql, " AND ExamPaper.Means = @mean ");
				par.Add(new SqlParameter("@mean", (object)mean));
			}
			sql = string.Concat(sql, ") t  WHERE rownum>=(@pageCount*(@pageIndex-1)+1) AND rownum<=@pageIndex*@pageCount");
			return SqlHelper.ExecuteList<ExamPaper>(sql, par.ToArray());
		}

		public List<ExamPaper> SearchPage(string key, int pageIndex, int pageCount, int year = -1, int mean = -1)
		{
			string sql = "SELECT * FROM (SELECT ExamPaper.*,(ROW_NUMBER() OVER(ORDER BY ExamPaper.SortId DESC,ExamPaper.Year DESC)) rownum FROM ExamPaper\r\n                        WHERE IsDeleted = 0";
			List<SqlParameter> sqlParameters = new List<SqlParameter>()
			{
				new SqlParameter("@pageCount", (object)pageCount),
				new SqlParameter("@pageIndex", (object)pageIndex)
			};
			List<SqlParameter> par = sqlParameters;
			if (!string.IsNullOrWhiteSpace(key))
			{
				sql = string.Concat(sql, " AND Name LIKE @Key");
				par.Add(new SqlParameter("@key", string.Concat("%", key, "%")));
			}
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
			sql = string.Concat(sql, ") t  WHERE rownum>=(@pageCount*(@pageIndex-1)+1) AND rownum<=@pageIndex*@pageCount");
			return SqlHelper.ExecuteList<ExamPaper>(sql, par.ToArray());
		}

		public int SoftDelete(Guid id)
		{
			string sql = "UPDATE ExamPaper SET IsDeleted = 1 WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}

		public int Update(ExamPaper examPaper)
		{
			string sql = "UPDATE ExamPaper SET Name = @Name,SortId = @SortId,Means = @Means,TimeOut = @TimeOut,Year = @Year,Month = @Month,TypeId = @TypeId,CreateTime = @CreateTime,IsDeleted = @IsDeleted WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)examPaper.Id), new SqlParameter("@Name", SqlHelper.ToDBValue(examPaper.Name)), new SqlParameter("@SortId", SqlHelper.ToDBValue(examPaper.SortId)), new SqlParameter("@Means", SqlHelper.ToDBValue(examPaper.Means)), new SqlParameter("@TimeOut", SqlHelper.ToDBValue(examPaper.TimeOut)), new SqlParameter("@Year", SqlHelper.ToDBValue(examPaper.Year)), new SqlParameter("@Month", SqlHelper.ToDBValue(examPaper.Month)), new SqlParameter("@TypeId", SqlHelper.ToDBValue(examPaper.TypeId)), new SqlParameter("@CreateTime", SqlHelper.ToDBValue(examPaper.CreateTime)), new SqlParameter("@IsDeleted", SqlHelper.ToDBValue(examPaper.IsDeleted)) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}
	}
}