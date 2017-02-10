using Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Providers
{
	public class LanResourceTypeProvider
	{
		private readonly static LanResourceTypeProvider InstanceObj;

		public static LanResourceTypeProvider Instance
		{
			get
			{
				return LanResourceTypeProvider.InstanceObj;
			}
		}

		static LanResourceTypeProvider()
		{
			LanResourceTypeProvider.InstanceObj = new LanResourceTypeProvider();
		}

		private LanResourceTypeProvider()
		{
		}

		public LanResourceType Create(LanResourceType lanResourceType)
		{
			LanResourceType lanResourceType1;
			if (lanResourceType.Id == Guid.Empty)
			{
				lanResourceType.Id = Guid.NewGuid();
			}
			string sql = "INSERT INTO LanResourceType (Id, Name, TypeEnum, SchoolId, UserId, SortId, CreateTime, IsDeleted)  VALUES (@Id, @Name, @TypeEnum, @SchoolId, @UserId, @SortId, @CreateTime, @IsDeleted)";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", SqlHelper.ToDBValue(lanResourceType.Id)), new SqlParameter("@Name", SqlHelper.ToDBValue(lanResourceType.Name)), new SqlParameter("@TypeEnum", SqlHelper.ToDBValue(lanResourceType.TypeEnum)), new SqlParameter("@SchoolId", SqlHelper.ToDBValue(lanResourceType.SchoolId)), new SqlParameter("@UserId", SqlHelper.ToDBValue(lanResourceType.UserId)), new SqlParameter("@SortId", SqlHelper.ToDBValue(lanResourceType.SortId)), new SqlParameter("@CreateTime", SqlHelper.ToDBValue(lanResourceType.CreateTime)), new SqlParameter("@IsDeleted", SqlHelper.ToDBValue(lanResourceType.IsDeleted)) };
			if (SqlHelper.ExecuteNonQuery(sql, sqlParameter) > 0)
			{
				lanResourceType1 = lanResourceType;
			}
			else
			{
				lanResourceType1 = null;
			}
			return lanResourceType1;
		}

		public int Delete(Guid id)
		{
			string sql = "DELETE LanResourceType WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}

		public int GetCount()
		{
			return (int)SqlHelper.ExecuteScalar("SELECT COUNT(Id) FROM LanResourceType WHERE IsDeleted = 0", new SqlParameter[0]);
		}

		public List<ViewLanResourceType> GetCustomListBySchoolId(Guid schoolid)
		{
			string sql = "select LanResourceType.Id,\r\n                                LanResourceType.[Name],\r\n                                School.[Name] SchoolName,\r\n                                UserInfo.[Name] UserName,\r\n                                SortId,\r\n                                (select count(1) from LanResource where TypeId = LanResourceType.Id ) Count\r\n                            from LanResourceType \r\n                            join School \r\n                            on LanResourceType.SchoolId = School.id \r\n                            join UserInfo \r\n                            on LanResourceType.UserId = UserInfo.id \r\n                            where LanResourceType.TypeEnum = 2 \r\n                                    and LanResourceType.SchoolId = @schoolid  \r\n                                    and LanResourceType.IsDeleted = 0 \r\n                            order by LanResourceType.SortId";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@schoolid", (object)schoolid) };
			return SqlHelper.ExecuteList<ViewLanResourceType>(sql, sqlParameter);
		}

		public LanResourceType GetEntity(Guid id)
		{
			string sql = "SELECT * FROM LanResourceType WHERE Id = @Id AND IsDeleted = 0";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteEntity<LanResourceType>(sql, sqlParameter);
		}

		public List<LanResourceType> GetList()
		{
			return SqlHelper.ExecuteList<LanResourceType>("SELECT * FROM LanResourceType WHERE IsDeleted = 0", new SqlParameter[0]);
		}

		public List<LanResourceType> GetList(Guid schoolId)
		{
			string sql = "select * from LanResourceType WHERE (TypeEnum = 0 or TypeEnum = 1) or (TypeEnum = 2 and SchoolId = @schoolId) and IsDeleted = 0 order by SortId";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@schoolId", (object)schoolId) };
			return SqlHelper.ExecuteList<LanResourceType>(sql, sqlParameter);
		}

		public List<ViewLanResourceType> GetListByType(int type)
		{
			string sql = "select Id,Name,TypeEnum,SortId,(select count(1) from LanResource where TypeId = LanResourceType.Id ) Count\r\n                            from LanResourceType \r\n                            where TypeEnum = @type \r\n                            AND IsDeleted = 0 \r\n                            order by SortId";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@type", (object)type) };
			return SqlHelper.ExecuteList<ViewLanResourceType>(sql, sqlParameter);
		}

		public int GetMaxCustomSort(Guid schoolId)
		{
			string sql = "select isnull(max(SortId),-1) from LanResourceType where IsDeleted = 0 and SchoolId = @id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@id", (object)schoolId) };
			return (int)SqlHelper.ExecuteScalar(sql, sqlParameter);
		}

		public List<LanResourceType> GetPage(int pageIndex, int pageCount)
		{
			string sql = "SELECT * FROM(SELECT *,(ROW_NUMBER() OVER(ORDER BY Id)) rownum FROM LanResourceType WHERE IsDeleted = 0) t WHERE rownum>=(@pageCount*(@pageIndex-1)+1) AND rownum<=@pageIndex*@pageCount";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@pageIndex", (object)pageIndex), new SqlParameter("@pageCount", (object)pageCount) };
			return SqlHelper.ExecuteList<LanResourceType>(sql, sqlParameter);
		}

		public List<ViewLanResourceType> GetSearchViewList(Guid schoolId, Guid sid, string key, Guid cid)
		{
			List<ViewLanResourceType> viewLanResourceTypes;
			SqlParameter[] sqlParameter;
			if (!(cid != Guid.Empty))
			{
				sqlParameter = new SqlParameter[] { new SqlParameter("@RootId", (object)schoolId), new SqlParameter("@SchoolId", (object)sid), new SqlParameter("@Key", string.Concat("%", key, "%")) };
				viewLanResourceTypes = SqlHelper.ProcedureList<ViewLanResourceType>("LanSearchResourceType_01", sqlParameter);
			}
			else
			{
				sqlParameter = new SqlParameter[] { new SqlParameter("@RootId", (object)schoolId), new SqlParameter("@CourseId", (object)cid), new SqlParameter("@Key", string.Concat("%", key, "%")) };
				viewLanResourceTypes = SqlHelper.ProcedureList<ViewLanResourceType>("LanSearchResourceType_02", sqlParameter);
			}
			return viewLanResourceTypes;
		}

		public List<ViewLanResourceType> GetViewList(Guid schoolId, Guid sid)
		{
			string sql = "select Id,Name,TypeEnum,SortId,UserId,(select count(1) from LanResource join LanCourse \r\n                                    on LanResource.CourseId = LanCourse.Id \r\n                                    where LanResource.typeid = LanResourceType.id \r\n                                    and (LanCourse.SchoolId = @sid or LanCourse.FacultyId = @sid or LanCourse.DepartmentId = @sid)\r\n                                    and LanResource.IsDeleted = 0 and LanCourse.IsDeleted = 0)\r\n                                    as Count from lanresourcetype where (SchoolId = @schoolId or SchoolId is null) AND IsDeleted = 0";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@schoolId", (object)schoolId), new SqlParameter("@sid", (object)sid) };
			return SqlHelper.ExecuteList<ViewLanResourceType>(sql, sqlParameter);
		}

		public List<ViewLanResourceType> GetViewListByCourse(Guid schoolId, Guid sid, Guid cid)
		{
			string sql = "select Id,Name,TypeEnum,SortId,UserId,(select count(1) from LanResource join LanCourse \r\n                                    on LanResource.CourseId = LanCourse.Id \r\n                                    where LanResource.CourseId = @cid and LanResource.typeid = LanResourceType.id \r\n                                    and (LanCourse.SchoolId = @sid or LanCourse.FacultyId = @sid or LanCourse.DepartmentId = @sid)\r\n                                    and LanResource.IsDeleted = 0 and LanCourse.IsDeleted = 0)\r\n                                    as Count from lanresourcetype where (SchoolId = @schoolId or SchoolId is null) and IsDeleted = 0";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@schoolId", (object)schoolId), new SqlParameter("@sid", (object)sid), new SqlParameter("@cid", (object)cid) };
			return SqlHelper.ExecuteList<ViewLanResourceType>(sql, sqlParameter);
		}

		public List<ViewLanResourceType> GetViewListByTeacher(Guid schoolId, Guid sid, Guid tid)
		{
			string sql = "select Id,Name,TypeEnum,SortId,UserId,(select count(1) from LanResource join LanCourse \r\n                                    on LanResource.CourseId = LanCourse.Id \r\n                                    where LanCourse.UserId = @tid and LanResource.typeid = LanResourceType.id \r\n                                    and (LanCourse.SchoolId = @sid or LanCourse.FacultyId = @sid or LanCourse.DepartmentId = @sid)\r\n                                    and LanResource.IsDeleted = 0 and LanCourse.IsDeleted = 0)\r\n                                    as Count from lanresourcetype where (SchoolId = @schoolId or SchoolId is null) AND IsDeleted = 0";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@schoolId", (object)schoolId), new SqlParameter("@sid", (object)sid), new SqlParameter("@tid", (object)tid) };
			return SqlHelper.ExecuteList<ViewLanResourceType>(sql, sqlParameter);
		}

		public int SoftDelete(Guid id)
		{
			string sql = "UPDATE LanResourceType SET IsDeleted = 1 WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}

		public int Update(LanResourceType lanResourceType)
		{
			string sql = "UPDATE LanResourceType SET Name = @Name,TypeEnum = @TypeEnum,SchoolId = @SchoolId,UserId = @UserId,SortId = @SortId,CreateTime = @CreateTime,IsDeleted = @IsDeleted WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)lanResourceType.Id), new SqlParameter("@Name", SqlHelper.ToDBValue(lanResourceType.Name)), new SqlParameter("@TypeEnum", SqlHelper.ToDBValue(lanResourceType.TypeEnum)), new SqlParameter("@SchoolId", SqlHelper.ToDBValue(lanResourceType.SchoolId)), new SqlParameter("@UserId", SqlHelper.ToDBValue(lanResourceType.UserId)), new SqlParameter("@SortId", SqlHelper.ToDBValue(lanResourceType.SortId)), new SqlParameter("@CreateTime", SqlHelper.ToDBValue(lanResourceType.CreateTime)), new SqlParameter("@IsDeleted", SqlHelper.ToDBValue(lanResourceType.IsDeleted)) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}
	}
}