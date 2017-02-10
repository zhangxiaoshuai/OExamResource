using Common;
using Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Providers
{
	public class LanCourseProvider
	{
		private readonly static LanCourseProvider InstanceObj;

		public static LanCourseProvider Instance
		{
			get
			{
				return LanCourseProvider.InstanceObj;
			}
		}

		static LanCourseProvider()
		{
			LanCourseProvider.InstanceObj = new LanCourseProvider();
		}

		private LanCourseProvider()
		{
		}

		public int AddResourceCount(Guid id)
		{
			string sql = "update LanCourse set ResourceCount=ISNULL(ResourceCount,0)+1 where Id = @id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@id", (object)id) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}

		public LanCourse Create(LanCourse lanCourse)
		{
			LanCourse lanCourse1;
			if (lanCourse.Id == Guid.Empty)
			{
				lanCourse.Id = Guid.NewGuid();
			}
			string sql = "INSERT INTO LanCourse (Id, Name, Remark, IsLab, ImageUrl, UserId, SchoolId, FacultyId, DepartmentId, ResourceCount, ClickCount, K4Id, CreateTime, IsDeleted, Tag1, Tag2)  VALUES (@Id, @Name, @Remark, @IsLab, @ImageUrl, @UserId, @SchoolId, @FacultyId, @DepartmentId, @ResourceCount, @ClickCount, @K4Id, @CreateTime, @IsDeleted, @Tag1, @Tag2)";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", SqlHelper.ToDBValue(lanCourse.Id)), new SqlParameter("@Name", SqlHelper.ToDBValue(lanCourse.Name)), new SqlParameter("@Remark", SqlHelper.ToDBValue(lanCourse.Remark)), new SqlParameter("@IsLab", SqlHelper.ToDBValue(lanCourse.IsLab)), new SqlParameter("@ImageUrl", SqlHelper.ToDBValue(lanCourse.ImageUrl)), new SqlParameter("@UserId", SqlHelper.ToDBValue(lanCourse.UserId)), new SqlParameter("@SchoolId", SqlHelper.ToDBValue(lanCourse.SchoolId)), new SqlParameter("@FacultyId", SqlHelper.ToDBValue(lanCourse.FacultyId)), new SqlParameter("@DepartmentId", SqlHelper.ToDBValue(lanCourse.DepartmentId)), new SqlParameter("@ResourceCount", SqlHelper.ToDBValue(lanCourse.ResourceCount)), new SqlParameter("@ClickCount", SqlHelper.ToDBValue(lanCourse.ClickCount)), new SqlParameter("@K4Id", SqlHelper.ToDBValue(lanCourse.K4Id)), new SqlParameter("@CreateTime", SqlHelper.ToDBValue(lanCourse.CreateTime)), new SqlParameter("@IsDeleted", SqlHelper.ToDBValue(lanCourse.IsDeleted)), new SqlParameter("@Tag1", SqlHelper.ToDBValue(lanCourse.Tag1)), new SqlParameter("@Tag2", SqlHelper.ToDBValue(lanCourse.Tag2)) };
			if (SqlHelper.ExecuteNonQuery(sql, sqlParameter) > 0)
			{
				lanCourse1 = lanCourse;
			}
			else
			{
				lanCourse1 = null;
			}
			return lanCourse1;
		}

		public int Delete(Guid id)
		{
			string sql = "DELETE LanCourse WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}

		public int GetCount()
		{
			return (int)SqlHelper.ExecuteScalar("SELECT COUNT(Id) FROM LanCourse WHERE IsDeleted = 0", new SqlParameter[0]);
		}

		public List<LanCourse> GetCourseList(Guid sfdid)
		{
			string sql = "select LanCourse.Id,LanCourse.[Name],UserInfo.[Name] Tag1\r\n                            from LanCourse\r\n                            join UserInfo\r\n                            on UserInfo.Id = LanCourse.UserId\r\n                            where (LanCourse.SchoolId = @SFD\r\n                            or LanCourse.FacultyId = @SFD\r\n                            or LanCourse.DepartmentId =  @SFD) and LanCourse.IsDeleted = 0 order by LanCourse.[Name]";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@SFD", (object)sfdid) };
			return SqlHelper.ExecuteList<LanCourse>(sql, sqlParameter);
		}

		public List<LanCourse> GetCourseList()
		{
			return SqlHelper.ExecuteList<LanCourse>("select LanCourse.Id,LanCourse.[Name],UserInfo.[Name] Tag1\r\n                            from LanCourse\r\n                            join UserInfo\r\n                            on UserInfo.Id = LanCourse.UserId\r\n                            where LanCourse.IsDeleted = 0 order by LanCourse.[Name]", new SqlParameter[0]);
		}

		public LanCourse GetEntity(Guid id)
		{
			string sql = "SELECT * FROM LanCourse WHERE Id = @Id AND IsDeleted = 0";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteEntity<LanCourse>(sql, sqlParameter);
		}

		public LanCourse GetEntityAndTest(Guid id)
		{
			string sql = "select LanCourse.[Id]\r\n                          ,LanCourse.[Name]\r\n                          ,LanCourse.[Remark]\r\n                          ,LanCourse.[IsLab]\r\n                          ,LanCourse.[ImageUrl]\r\n                          ,LanCourse.[UserId]\r\n                          ,LanCourse.[SchoolId]\r\n                          ,LanCourse.[FacultyId]\r\n                          ,LanCourse.[DepartmentId]\r\n                          ,LanCourse.[ResourceCount]\r\n                          ,LanCourse.[ClickCount]\r\n                          ,LanCourse.[K4Id]\r\n                          ,LanCourse.[CreateTime]\r\n                          ,LanCourse.[IsDeleted]\r\n                          ,Test.Name as [Tag1]\r\n                          ,CAST(Test.ItemCount as nvarchar(50)) as [Tag2]\r\n            from LanCourse left join Test on LanCourse.K4Id = Test.Id and Test.IsDeleted = 0 where LanCourse.Id = @id and LanCourse.IsDeleted = 0";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@id", (object)id) };
			return SqlHelper.ExecuteEntity<LanCourse>(sql, sqlParameter);
		}

		public List<LanCourse> GetList()
		{
			return SqlHelper.ExecuteList<LanCourse>("SELECT * FROM LanCourse WHERE IsDeleted = 0", new SqlParameter[0]);
		}

		public List<LanCourse> GetList(Guid guid, Enums.SchoolTier tier)
		{
			string sql = "SELECT LanCourse.[Id]\r\n                                  ,LanCourse.[Name]\r\n                                  ,LanCourse.[Remark]\r\n                                  ,LanCourse.[IsLab]\r\n                                  ,LanCourse.[ImageUrl]\r\n                                  ,LanCourse.[UserId]\r\n                                  ,LanCourse.[SchoolId]\r\n                                  ,LanCourse.[FacultyId]\r\n                                  ,LanCourse.[DepartmentId]\r\n                                  ,LanCourse.[ResourceCount]\r\n                                  ,LanCourse.[ClickCount]\r\n                                  ,LanCourse.[K4Id]\r\n                                  ,LanCourse.[CreateTime]\r\n                                  ,LanCourse.[IsDeleted]\r\n                                  ,LanCourse.[Tag1]\r\n                                ,UserInfo.[Name] as Tag2\r\n                                FROM [LanCourse]\r\n                                left join UserInfo\r\n                                on LanCourse.UserId = UserInfo.Id\r\n                                WHERE LanCourse.{0} = @Id AND LanCourse.IsDeleted = 0";
			switch (tier)
			{
				case Enums.SchoolTier.School:
				{
					sql = string.Format(sql, "SchoolId");
					break;
				}
				case Enums.SchoolTier.Faculty:
				{
					sql = string.Format(sql, "FacultyId");
					break;
				}
				case Enums.SchoolTier.Department:
				{
					sql = string.Format(sql, "DepartmentId");
					break;
				}
			}
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)guid) };
			return SqlHelper.ExecuteList<LanCourse>(sql, sqlParameter);
		}

		public List<LanCourse> GetList(Guid userId)
		{
			string sql = "select * from LanCourse where UserId = @userId and IsDeleted = 0 order by CreateTime";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@userId", (object)userId) };
			return SqlHelper.ExecuteList<LanCourse>(sql, sqlParameter);
		}

		public List<LanCourse> GetPage(int pageIndex, int pageCount)
		{
			string sql = "SELECT * FROM(SELECT *,(ROW_NUMBER() OVER(ORDER BY Id)) rownum FROM LanCourse WHERE IsDeleted = 0) t WHERE rownum>=(@pageCount*(@pageIndex-1)+1) AND rownum<=@pageIndex*@pageCount";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@pageIndex", (object)pageIndex), new SqlParameter("@pageCount", (object)pageCount) };
			return SqlHelper.ExecuteList<LanCourse>(sql, sqlParameter);
		}

		public List<LanCourse> GetSearchList(Guid schoolId, string key)
		{
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@SchoolId", (object)schoolId), new SqlParameter("@Key", string.Concat("%", key, "%")) };
			return SqlHelper.ProcedureList<LanCourse>("LanSearch", sqlParameter);
		}

		public int SoftDelete(Guid id)
		{
			string sql = "UPDATE LanCourse SET IsDeleted = 1 WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}

		public int SubtractResourceCount(Guid id)
		{
			string sql = "update LanCourse set ResourceCount=ISNULL(ResourceCount,1)-1 where Id = @id and (ResourceCount>0 or ResourceCount is null)";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@id", (object)id) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}

		public int Update(LanCourse lanCourse)
		{
			string sql = "UPDATE LanCourse SET Name = @Name,Remark = @Remark,IsLab = @IsLab,ImageUrl = @ImageUrl,UserId = @UserId,SchoolId = @SchoolId,FacultyId = @FacultyId,DepartmentId = @DepartmentId,ResourceCount = @ResourceCount,ClickCount = @ClickCount,K4Id = @K4Id,CreateTime = @CreateTime,IsDeleted = @IsDeleted,Tag1 = @Tag1,Tag2 = @Tag2 WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)lanCourse.Id), new SqlParameter("@Name", SqlHelper.ToDBValue(lanCourse.Name)), new SqlParameter("@Remark", SqlHelper.ToDBValue(lanCourse.Remark)), new SqlParameter("@IsLab", SqlHelper.ToDBValue(lanCourse.IsLab)), new SqlParameter("@ImageUrl", SqlHelper.ToDBValue(lanCourse.ImageUrl)), new SqlParameter("@UserId", SqlHelper.ToDBValue(lanCourse.UserId)), new SqlParameter("@SchoolId", SqlHelper.ToDBValue(lanCourse.SchoolId)), new SqlParameter("@FacultyId", SqlHelper.ToDBValue(lanCourse.FacultyId)), new SqlParameter("@DepartmentId", SqlHelper.ToDBValue(lanCourse.DepartmentId)), new SqlParameter("@ResourceCount", SqlHelper.ToDBValue(lanCourse.ResourceCount)), new SqlParameter("@ClickCount", SqlHelper.ToDBValue(lanCourse.ClickCount)), new SqlParameter("@K4Id", SqlHelper.ToDBValue(lanCourse.K4Id)), new SqlParameter("@CreateTime", SqlHelper.ToDBValue(lanCourse.CreateTime)), new SqlParameter("@IsDeleted", SqlHelper.ToDBValue(lanCourse.IsDeleted)), new SqlParameter("@Tag1", SqlHelper.ToDBValue(lanCourse.Tag1)), new SqlParameter("@Tag2", SqlHelper.ToDBValue(lanCourse.Tag2)) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}

		public int UpdateClick(Guid id)
		{
			string sql = "update LanCourse set ClickCount=ISNULL(ClickCount,0)+1 where Id = @id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@id", (object)id) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}

		public void UpdateLanCourseCount(Guid schoolId, int appendCount)
		{
			string sql = "with CTE AS\r\n                        (\r\n                        SELECT Id,ParentId FROM School WHERE Id = @Id\r\n                        UNION ALL\r\n                        SELECT School.Id,School.ParentId FROM School join CTE ON School.Id = CTE.ParentId\r\n                        )\r\n                        update School set LanCourseCount=ISNULL(LanCourseCount,0) + @c where Id in (select Id from CTE)\r\n                        and (ISNULL(LanCourseCount,0) + @c)>=0";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)schoolId), new SqlParameter("@c", (object)appendCount) };
			SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}

		public void UpdateLanResourceCount(Guid schoolId, int appendCount)
		{
			string sql = "with CTE AS\r\n                        (\r\n                        SELECT Id,ParentId FROM School WHERE Id = @Id\r\n                        UNION ALL\r\n                        SELECT School.Id,School.ParentId FROM School join CTE ON School.Id = CTE.ParentId\r\n                        )\r\n                        update School set LanResourceCount=ISNULL(LanResourceCount,0) + @c where Id in (select Id from CTE)\r\n                        and (ISNULL(LanResourceCount,0) + @c)>=0";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)schoolId), new SqlParameter("@c", (object)appendCount) };
			SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}
	}
}