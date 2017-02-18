using Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Providers
{
	public class LanResourceProvider
	{
		private readonly static LanResourceProvider InstanceObj;

		public static LanResourceProvider Instance
		{
			get
			{
				return LanResourceProvider.InstanceObj;
			}
		}

		static LanResourceProvider()
		{
			LanResourceProvider.InstanceObj = new LanResourceProvider();
		}

		private LanResourceProvider()
		{
		}

		public LanResource Create(LanResource lanResource)
		{
			LanResource lanResource1;
			if (lanResource.Id == Guid.Empty)
			{
				lanResource.Id = Guid.NewGuid();
			}
			string sql = "INSERT INTO LanResource (Id, Name, TypeId, CourseId, ClickCount, NameEx, DownloadUrl, ViewUrl, ImageUrl, IsShare, CreateTime, IsDeleted, Tag1, Tag2)  VALUES (@Id, @Name, @TypeId, @CourseId, @ClickCount, @NameEx, @DownloadUrl, @ViewUrl, @ImageUrl, @IsShare, @CreateTime, @IsDeleted, @Tag1, @Tag2)";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", SqlHelper.ToDBValue(lanResource.Id)), new SqlParameter("@Name", SqlHelper.ToDBValue(lanResource.Name)), new SqlParameter("@TypeId", SqlHelper.ToDBValue(lanResource.TypeId)), new SqlParameter("@CourseId", SqlHelper.ToDBValue(lanResource.CourseId)), new SqlParameter("@ClickCount", SqlHelper.ToDBValue(lanResource.ClickCount)), new SqlParameter("@NameEx", SqlHelper.ToDBValue(lanResource.NameEx)), new SqlParameter("@DownloadUrl", SqlHelper.ToDBValue(lanResource.DownloadUrl)), new SqlParameter("@ViewUrl", SqlHelper.ToDBValue(lanResource.ViewUrl)), new SqlParameter("@ImageUrl", SqlHelper.ToDBValue(lanResource.ImageUrl)), new SqlParameter("@IsShare", SqlHelper.ToDBValue(lanResource.IsShare)), new SqlParameter("@CreateTime", SqlHelper.ToDBValue(lanResource.CreateTime)), new SqlParameter("@IsDeleted", SqlHelper.ToDBValue(lanResource.IsDeleted)), new SqlParameter("@Tag1", SqlHelper.ToDBValue(lanResource.Tag1)), new SqlParameter("@Tag2", SqlHelper.ToDBValue(lanResource.Tag2)) };
			if (SqlHelper.ExecuteNonQuery(sql, sqlParameter) > 0)
			{
				lanResource1 = lanResource;
			}
			else
			{
				lanResource1 = null;
			}
			return lanResource1;
		}

		public int Delete(Guid id)
		{
			string sql = "DELETE LanResource WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}

		public int GetCount(Guid typeId)
		{
			string sql = "select count(1) from LanResource WHERE TypeId = @id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@id", (object)typeId) };
			return (int)SqlHelper.ExecuteScalar(sql, sqlParameter);
		}

		public int GetCount(Guid schoolId, Guid courseId, Guid typeId, string key)
		{
			string sql = "SELECT COUNT(1) FROM LanResource \r\n                            JOIN LanCourse \r\n                            ON LanResource.CourseId = LanCourse.Id \r\n\t\t\t                JOIN LanResourceType \r\n                            ON LanResource.TypeId = LanResourceType.Id \r\n\t\t\t                WHERE LanResource.IsDeleted = 0 AND LanCourse.IsDeleted = 0 AND LanResourceType.IsDeleted = 0 ";
			if (schoolId != Guid.Empty)
			{
				sql = string.Concat(sql, " AND (LanCourse.SchoolId = @schoolId OR LanCourse.FacultyId = @schoolId OR LanCourse.DepartmentId = @schoolId)");
			}
			if (courseId != Guid.Empty)
			{
				sql = string.Concat(sql, " AND LanResource.CourseId = @courseId ");
			}
			if (typeId != Guid.Empty)
			{
				sql = string.Concat(sql, " AND LanResource.TypeId = @typeId ");
			}
			if (!string.IsNullOrWhiteSpace(key))
			{
				sql = string.Concat(sql, " AND LanResource.Name LIKE @key ");
			}
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@schoolId", (object)schoolId), new SqlParameter("@courseId", (object)courseId), new SqlParameter("@typeId", (object)typeId), new SqlParameter("@key", string.Concat("%", key, "%")) };
			return (int)SqlHelper.ExecuteScalar(sql, sqlParameter);
		}

		public int GetCount()
		{
			return (int)SqlHelper.ExecuteScalar("SELECT COUNT(Id) FROM LanResource WHERE IsDeleted = 0", new SqlParameter[0]);
		}

		public LanResource GetEntity(Guid id)
		{
			string sql = "SELECT * FROM LanResource WHERE Id = @Id AND IsDeleted = 0";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteEntity<LanResource>(sql, sqlParameter);
		}

		public LanResource GetEntityInfo(Guid id)
		{
			string sql = "select LanResource.[Id]\r\n      ,LanResource.[Name]\r\n      ,LanResource.[TypeId]\r\n      ,LanResource.[CourseId]\r\n      ,LanResource.[ClickCount]\r\n      ,LanResource.[NameEx]\r\n      ,LanResource.[IsShare]\r\n      ,LanResource.[DownloadUrl]\r\n      ,LanResource.[ViewUrl]\r\n      ,LanResource.[ImageUrl]\r\n      ,LanResource.[CreateTime]\r\n      ,LanResource.[IsDeleted]\r\n      ,LanResourceType.Name as [Tag1]\r\n      ,LanCourse.Name as [Tag2] from LanResource left join LanResourceType on Lanresource.TypeId = LanResourceType.Id and LanResourceType.IsDeleted = 0 \r\n       left join LanCourse on LanResource.CourseId=LanCourse.Id and LanCourse.IsDeleted = 0        \r\nwhere LanResource.Id = @Id and LanResource.IsDeleted = 0";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteEntity<LanResource>(sql, sqlParameter);
		}

		public List<LanResource> GetLanResource(int pageIndex, int pageCount, Guid schoolId, Guid courseId, Guid typeId, string key)
		{
			string sql = "SELECT * FROM (\r\n\t\t\tSELECT \r\n\t\t\tLanResource.[Id],\r\n\t\t\tLanResource.[Name],\r\n\t\t\tLanResource.[TypeId],\r\n\t\t\tLanResource.[CourseId],\r\n\t\t\tLanResource.[CreateTime],\r\n\t\t\tLanResourceType.Name AS [Tag1],\r\n\t\t\tLanCourse.Name AS [Tag2],\r\n            USerInfo.Name AS [ViewUrl],\r\n\t\t\tROW_NUMBER() OVER(ORDER BY LanResource.CreateTime DESC) AS rownum    \r\n\t\t\tFROM LanResource \r\n            JOIN LanCourse ON LanResource.CourseId = LanCourse.Id \r\n\t\t\tJOIN LanResourceType ON LanResource.TypeId = LanResourceType.Id\r\n            JOIN UserInfo ON LanCourse.UserId = UserInfo.Id \r\n\t\t\tWHERE LanResource.IsDeleted = 0 AND LanCourse.IsDeleted = 0 AND LanResourceType.IsDeleted = 0 ";
			if (schoolId != Guid.Empty)
			{
				sql = string.Concat(sql, " AND (LanCourse.SchoolId = @schoolId OR LanCourse.FacultyId = @schoolId OR LanCourse.DepartmentId = @schoolId)");
			}
			if (courseId != Guid.Empty)
			{
				sql = string.Concat(sql, " AND LanResource.CourseId = @courseId ");
			}
			if (typeId != Guid.Empty)
			{
				sql = string.Concat(sql, " AND LanResource.TypeId = @typeId ");
			}
			if (!string.IsNullOrWhiteSpace(key))
			{
				sql = string.Concat(sql, " AND LanResource.Name LIKE @key ");
			}
			sql = string.Concat(sql, ") t WHERE rownum>=(@pageCount*(@pageIndex-1)+1) AND rownum<=@pageIndex*@pageCount");
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@pageIndex", (object)pageIndex), new SqlParameter("@pageCount", (object)pageCount), new SqlParameter("@schoolId", (object)schoolId), new SqlParameter("@key", string.Concat("%", key, "%")), new SqlParameter("@courseId", (object)courseId), new SqlParameter("@typeId", (object)typeId) };
			return SqlHelper.ExecuteList<LanResource>(sql, sqlParameter);
		}

		public List<LanResource> GetList()
		{
			return SqlHelper.ExecuteList<LanResource>("SELECT * FROM LanResource WHERE IsDeleted = 0", new SqlParameter[0]);
		}

		public List<LanResource> GetListAndType(Guid courseId)
		{
			string sql = "select LanResource.[Id]\r\n      ,LanResource.[Name]\r\n      ,LanResource.[TypeId]\r\n      ,LanResource.[CourseId]\r\n      ,LanResource.[ClickCount]\r\n      ,LanResource.[NameEx]\r\n      ,LanResource.[IsShare]\r\n      ,LanResource.[DownloadUrl]\r\n      ,LanResource.[ViewUrl]\r\n      ,LanResource.[ImageUrl]\r\n      ,LanResource.[CreateTime]\r\n      ,LanResource.[IsDeleted]\r\n      ,LanResourceType.Name as [Tag1]\r\n      ,LanResource.[Tag2] from LanResource left join LanResourceType on Lanresource.TypeId = LanResourceType.Id and LanResourceType.IsDeleted = 0 where CourseId = @courseId and LanResource.IsDeleted = 0 order by LanResource.CreateTime";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@courseId", (object)courseId) };
			return SqlHelper.ExecuteList<LanResource>(sql, sqlParameter);
		}

		public List<LanResource> GetPage(int pageIndex, int pageCount, Guid schoolId, Guid userId, Guid courseId, Guid typeId)
		{
			string where;
			StringBuilder sb = new StringBuilder("SELECT * FROM(");
			sb.Append("select LanResource.[Id],LanResource.[Name],LanResource.[TypeId],LanResource.[CourseId],LanResource.[ClickCount],LanResource.[IsShare],LanResource.[ImageUrl],LanResource.[ViewUrl],LanResource.[CreateTime],LanResource.[IsDeleted],");
			sb.Append("LanResourceType.Name as [Tag1],LanCourse.Name as [Tag2] ,row_number() over(order by LanResource.ClickCount DESC,LanResource.CreateTime) as rownum ");
			sb.Append("from LanResource join LanCourse on LanResource.CourseId = LanCourse.Id join LanResourceType on LanResource.TypeId = LanResourceType.Id ");
			sb.Append("where {0} and LanResource.IsDeleted = 0 and LanCourse.IsDeleted = 0 and LanResourceType.IsDeleted = 0");
			sb.Append(") t WHERE rownum>=(@pageCount*(@pageIndex-1)+1) AND rownum<=@pageIndex*@pageCount");
			List<SqlParameter> par = new List<SqlParameter>()
			{
				new SqlParameter("@pageIndex", (object)pageIndex),
				new SqlParameter("@pageCount", (object)pageCount)
			};
			if (!(courseId != Guid.Empty))
			{
				where = "(LanCourse.SchoolId = @sid or LanCourse.FacultyId = @sid or LanCourse.DepartmentId = @sid) ";
				par.Add(new SqlParameter("@sid", (object)schoolId));
				if (userId != Guid.Empty)
				{
					where = string.Concat(where, "and LanCourse.UserId = @uid ");
					par.Add(new SqlParameter("@uid", (object)userId));
				}
			}
			else
			{
				where = "LanResource.CourseId = @cid ";
				par.Add(new SqlParameter("@cid", (object)courseId));
			}
			if (typeId != Guid.Empty)
			{
				where = string.Concat(where, "and LanResource.TypeId = @tid ");
				par.Add(new SqlParameter("@tid", (object)typeId));
			}
			List<LanResource> lanResources = SqlHelper.ExecuteList<LanResource>(string.Format(sb.ToString(), where), par.ToArray());
			return lanResources;
		}

		public List<LanResource> GetPage(int pageIndex, int pageCount)
		{
			string sql = "SELECT * FROM(SELECT *,(ROW_NUMBER() OVER(ORDER BY Id)) rownum FROM LanResource WHERE IsDeleted = 0) t WHERE rownum>=(@pageCount*(@pageIndex-1)+1) AND rownum<=@pageIndex*@pageCount";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@pageIndex", (object)pageIndex), new SqlParameter("@pageCount", (object)pageCount) };
			return SqlHelper.ExecuteList<LanResource>(sql, sqlParameter);
		}

		public List<LanResource> GetSearchPage(int pageIndex, int pageCount, Guid schoolId, Guid courseId, Guid typeId, string key)
		{
			List<SqlParameter> sqlParameters = new List<SqlParameter>()
			{
				new SqlParameter("@pageIndex", (object)pageIndex),
				new SqlParameter("@pageCount", (object)pageCount),
				new SqlParameter("@schoolId", (object)schoolId),
				new SqlParameter("@key", string.Concat("%", key, "%"))
			};
			List<SqlParameter> par = sqlParameters;
			if (courseId != Guid.Empty)
			{
				par.Add(new SqlParameter("@courseId", (object)courseId));
			}
			if (typeId != Guid.Empty)
			{
				par.Add(new SqlParameter("@typeId", (object)typeId));
			}
			return SqlHelper.ProcedureList<LanResource>("LanSearchPage", par.ToArray());
		}

		public List<LanResource> GetTopByCreateTime(Guid guid, int top)
		{
			string sql = string.Concat("select top(", top, ") \r\nLanResource.[Id],LanResource.[Name],LanResource.[TypeId],LanResource.[CourseId],LanResource.[ClickCount],LanResource.[ImageUrl],LanResource.[IsShare],LanResource.[ViewUrl],LanResource.[CreateTime],LanResource.[IsDeleted],\r\nLanResourceType.Name as [Tag1],LanCourse.Name as [Tag2] \r\nfrom LanResource join LanResourceType on LanResource.TypeId = LanResourceType.Id join LanCourse on LanResource.CourseId = LanCourse.Id where (LanCourse.SchoolId = @Id or LanCourse.FacultyId = @Id or LanCourse.DepartmentId = @Id) and LanResource.IsDeleted = 0 order by LanResource.CreateTime desc");
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)guid) };
			return SqlHelper.ExecuteList<LanResource>(sql, sqlParameter);
		}

		public int SoftDelete(Guid id)
		{
			string sql = "UPDATE LanResource SET IsDeleted = 1 WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}

		public int Update(LanResource lanResource)
		{
			string sql = "UPDATE LanResource SET Name = @Name,TypeId = @TypeId,CourseId = @CourseId,ClickCount = @ClickCount,NameEx = @NameEx,DownloadUrl = @DownloadUrl,ViewUrl = @ViewUrl,ImageUrl = @ImageUrl,IsShare = @IsShare,CreateTime = @CreateTime,IsDeleted = @IsDeleted,Tag1 = @Tag1,Tag2 = @Tag2 WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)lanResource.Id), new SqlParameter("@Name", SqlHelper.ToDBValue(lanResource.Name)), new SqlParameter("@TypeId", SqlHelper.ToDBValue(lanResource.TypeId)), new SqlParameter("@CourseId", SqlHelper.ToDBValue(lanResource.CourseId)), new SqlParameter("@ClickCount", SqlHelper.ToDBValue(lanResource.ClickCount)), new SqlParameter("@NameEx", SqlHelper.ToDBValue(lanResource.NameEx)), new SqlParameter("@DownloadUrl", SqlHelper.ToDBValue(lanResource.DownloadUrl)), new SqlParameter("@ViewUrl", SqlHelper.ToDBValue(lanResource.ViewUrl)), new SqlParameter("@ImageUrl", SqlHelper.ToDBValue(lanResource.ImageUrl)), new SqlParameter("@IsShare", SqlHelper.ToDBValue(lanResource.IsShare)), new SqlParameter("@CreateTime", SqlHelper.ToDBValue(lanResource.CreateTime)), new SqlParameter("@IsDeleted", SqlHelper.ToDBValue(lanResource.IsDeleted)), new SqlParameter("@Tag1", SqlHelper.ToDBValue(lanResource.Tag1)), new SqlParameter("@Tag2", SqlHelper.ToDBValue(lanResource.Tag2)) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}

		public int UpdateClick(Guid id)
		{
			string sql = "update LanResource set ClickCount=ISNULL(ClickCount,0)+1 where Id = @id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@id", (object)id) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}
	}
}