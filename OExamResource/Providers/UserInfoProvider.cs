using Common;
using Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Providers
{
	public class UserInfoProvider
	{
		private readonly static UserInfoProvider InstanceObj;

		public static UserInfoProvider Instance
		{
			get
			{
				return UserInfoProvider.InstanceObj;
			}
		}

		static UserInfoProvider()
		{
			UserInfoProvider.InstanceObj = new UserInfoProvider();
		}

		private UserInfoProvider()
		{
		}

		public UserInfo Create(UserInfo userInfo)
		{
			UserInfo userInfo1;
			if (userInfo.Id == Guid.Empty)
			{
				userInfo.Id = Guid.NewGuid();
			}
			string sql = "INSERT INTO UserInfo (Id, Name, LoginName, LoginPwd, Sex, Birthday, SchoolId, FacultyId, DepartmentId, JobId, ProfessionId, PostId, Email, Phone, Type, CreateTime, IsDeleted)  VALUES (@Id, @Name, @LoginName, @LoginPwd, @Sex, @Birthday, @SchoolId, @FacultyId, @DepartmentId, @JobId, @ProfessionId, @PostId, @Email, @Phone, @Type, @CreateTime, @IsDeleted)";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", SqlHelper.ToDBValue(userInfo.Id)), new SqlParameter("@Name", SqlHelper.ToDBValue(userInfo.Name)), new SqlParameter("@LoginName", SqlHelper.ToDBValue(userInfo.LoginName)), new SqlParameter("@LoginPwd", SqlHelper.ToDBValue(userInfo.LoginPwd)), new SqlParameter("@Sex", SqlHelper.ToDBValue(userInfo.Sex)), new SqlParameter("@Birthday", SqlHelper.ToDBValue(userInfo.Birthday)), new SqlParameter("@SchoolId", SqlHelper.ToDBValue(userInfo.SchoolId)), new SqlParameter("@FacultyId", SqlHelper.ToDBValue(userInfo.FacultyId)), new SqlParameter("@DepartmentId", SqlHelper.ToDBValue(userInfo.DepartmentId)), new SqlParameter("@JobId", SqlHelper.ToDBValue(userInfo.JobId)), new SqlParameter("@ProfessionId", SqlHelper.ToDBValue(userInfo.ProfessionId)), new SqlParameter("@PostId", SqlHelper.ToDBValue(userInfo.PostId)), new SqlParameter("@Email", SqlHelper.ToDBValue(userInfo.Email)), new SqlParameter("@Phone", SqlHelper.ToDBValue(userInfo.Phone)), new SqlParameter("@Type", SqlHelper.ToDBValue(userInfo.Type)), new SqlParameter("@CreateTime", SqlHelper.ToDBValue(userInfo.CreateTime)), new SqlParameter("@IsDeleted", SqlHelper.ToDBValue(userInfo.IsDeleted)) };
			if (SqlHelper.ExecuteNonQuery(sql, sqlParameter) > 0)
			{
				userInfo1 = userInfo;
			}
			else
			{
				userInfo1 = null;
			}
			return userInfo1;
		}

		public int Delete(Guid id)
		{
			string sql = "DELETE UserInfo WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}

		public int DeleteBySchoolId(Guid schoolid)
		{
			string sql = "delete UserInfo where SchoolId = @schoolid";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@schoolid", (object)schoolid) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}

		public int GetCount()
		{
			return (int)SqlHelper.ExecuteScalar("SELECT COUNT(Id) FROM UserInfo WHERE IsDeleted = 0", new SqlParameter[0]);
		}

		public int GetCountByFacDepId(Guid id)
		{
			string sql = "SELECT COUNT(Id) FROM UserInfo WHERE IsDeleted = 0 AND (FacultyId = @Id OR DepartmentId = @Id)";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return (int)SqlHelper.ExecuteScalar(sql, sqlParameter);
		}

		public UserInfo GetEntity(string loginName, string passWord)
		{
			string sql = "SELECT * FROM [UserInfo] WHERE LoginName = @LoginName AND LoginPwd = @LoginPwd";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@LoginName", loginName), new SqlParameter("@LoginPwd", passWord) };
			return SqlHelper.ExecuteEntity<UserInfo>(sql, sqlParameter);
		}

		public UserInfo GetEntity(string loginName)
		{
			string sql = "SELECT * FROM [UserInfo] WHERE LoginName = @LoginName ";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@LoginName", loginName) };
			return SqlHelper.ExecuteEntity<UserInfo>(sql, sqlParameter);
		}

		public UserInfo GetEntity(Guid id)
		{
			string sql = "SELECT * FROM UserInfo WHERE Id = @Id AND IsDeleted = 0";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteEntity<UserInfo>(sql, sqlParameter);
		}

		public List<UserInfo> GetList(Guid schoolid, int type)
		{
			string sql = "SELECT * FROM [UserInfo] WHERE Type=@Type AND SchoolId = @SchoolId AND IsDeleted = 0 ";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@SchoolId", (object)schoolid), new SqlParameter("@Type", (object)type) };
			return SqlHelper.ExecuteList<UserInfo>(sql, sqlParameter);
		}

		public List<UserInfo> GetList(Guid guid, Enums.SchoolTier tier, Enums.UserType type)
		{
			string sql = "";
			switch (tier)
			{
				case Enums.SchoolTier.School:
				{
					sql = "SELECT * FROM [UserInfo] WHERE Type=@Type AND SchoolId = @SchoolId AND IsDeleted = 0 ";
					break;
				}
				case Enums.SchoolTier.Faculty:
				{
					sql = "SELECT * FROM [UserInfo] WHERE Type=@Type AND FacultyId = @SchoolId AND IsDeleted = 0 ";
					break;
				}
				case Enums.SchoolTier.Department:
				{
					sql = "SELECT * FROM [UserInfo] WHERE Type=@Type AND DepartmentId = @SchoolId AND IsDeleted = 0 ";
					break;
				}
			}
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@SchoolId", (object)guid), new SqlParameter("@Type", (object)((int)type)) };
			return SqlHelper.ExecuteList<UserInfo>(sql, sqlParameter);
		}

		public List<UserInfo> GetList(Guid schoolid, Guid facultyid, int type)
		{
			string sql = "SELECT * FROM [UserInfo] WHERE Type=@Type AND SchoolId = @SchoolId  AND IsDeleted = 0 ";
			if (facultyid != Guid.Empty)
			{
				sql = string.Concat(sql, " AND FacultyId = @FacultyId");
			}
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@SchoolId", (object)schoolid), new SqlParameter("@FacultyId", (object)facultyid), new SqlParameter("@Type", (object)type) };
			return SqlHelper.ExecuteList<UserInfo>(sql, sqlParameter);
		}

		public List<UserInfo> GetList()
		{
			return SqlHelper.ExecuteList<UserInfo>("SELECT * FROM UserInfo WHERE IsDeleted = 0", new SqlParameter[0]);
		}

		public List<UserInfo> GetListBySchoolId(Guid id)
		{
			string sql = "SELECT * FROM UserInfo WHERE SchoolId = @SchoolId";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@SchoolId", (object)id) };
			return SqlHelper.ExecuteList<UserInfo>(sql, sqlParameter);
		}

		public List<UserInfo> GetPage(int pageIndex, int pageCount)
		{
			string sql = "SELECT * FROM(SELECT *,(ROW_NUMBER() OVER(ORDER BY Id)) rownum FROM UserInfo WHERE IsDeleted = 0) t WHERE rownum>=(@pageCount*(@pageIndex-1)+1) AND rownum<=@pageIndex*@pageCount";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@pageIndex", (object)pageIndex), new SqlParameter("@pageCount", (object)pageCount) };
			return SqlHelper.ExecuteList<UserInfo>(sql, sqlParameter);
		}

		public List<UserInfo> GetSeller()
		{
			return SqlHelper.ExecuteList<UserInfo>("SELECT UserInfo.Id,UserInfo.[Name] from UserInfo\r\n                                LEFT JOIN User_Role\r\n                                ON User_Role.UserId=UserInfo.Id\r\n                                LEFT JOIN Role\r\n                                ON User_Role.RoleId=Role.Id\r\n                                WHERE Role.Type = 3 AND UserInfo.IsDeleted = 0 ", new SqlParameter[0]);
		}

		public UserInfo GetUser(Guid id)
		{
			string sql = "SELECT * FROM UserInfo WHERE Id = @Id ";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteEntity<UserInfo>(sql, sqlParameter);
		}

		public int GetUserBySchool(Guid id, Guid schoolid)
		{
			string sql = "SELECT COUNT(Id) FROM UserInfo WHERE Id = @Id and SchoolId = @SchoolId ";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id), new SqlParameter("@SchoolId", (object)schoolid) };
			return (int)SqlHelper.ExecuteScalar(sql, sqlParameter);
		}

		public List<UserInfoList> GetUserList(Guid schoolid, Guid facultyid, int state, string name, string type, int pageIndex, int pageCount)
		{
			string sql = " SELECT * FROM( \r\n                                SELECT \r\n\t\t\t\t\t\t            UserInfo.Id , \r\n\t\t\t\t\t\t            UserInfo.LoginName, \r\n\t\t\t\t\t\t            UserInfo.[Name], \r\n\t\t\t\t\t\t            UserInfo.[Type] UserType, \r\n\t\t\t\t\t\t            (select School.[Name] from School where UserInfo.SchoolId = School.Id) SchoolName, \r\n\t\t\t\t\t\t            (select School.[Name] from School where UserInfo.FacultyId = School.Id) FacultyName,\r\n                                    (select School.[Name] from School where UserInfo.DepartmentId = School.Id) DepartmentName,\r\n\t\t\t\t\t\t            UserInfo.IsDeleted State, \r\n                                    ROW_NUMBER() OVER(ORDER BY UserInfo.CreateTime DESC,UserInfo.FacultyId) rownum ";
			sql = (!(type == "-1") ? string.Concat(sql, " FROM UserInfo ") : string.Concat(sql, " ,Role.Name RoleName \r\n                        FROM UserInfo \r\n                        INNER JOIN User_Role\r\n                        ON User_Role.UserId = UserInfo.Id\r\n                        INNER JOIN Role\r\n                        ON Role.Id = User_Role.RoleId  "));
			sql = string.Concat(sql, " WHERE UserInfo.LoginName IS NOT NULL ");
			if (schoolid != Guid.Empty)
			{
				sql = string.Concat(sql, " AND UserInfo.SchoolId=@schoolid ");
			}
			if (facultyid != Guid.Empty)
			{
				sql = string.Concat(sql, " AND UserInfo.FacultyId=@facultyid ");
			}
			if (state >= 0)
			{
				sql = string.Concat(sql, " AND UserInfo.IsDeleted=@isdeleted ");
			}
			if (!string.IsNullOrEmpty(name))
			{
				sql = string.Concat(sql, " AND ( UserInfo.[Name] LIKE @name OR UserInfo.LoginName LIKE @name ) ");
			}
			if (!string.IsNullOrEmpty(type))
			{
				sql = string.Concat(sql, " AND UserInfo.[Type] IN (", type, ") ");
			}
			sql = string.Concat(sql, " ) as t WHERE rownum>=(@pageCount*(@pageIndex-1)+1) AND rownum<=@pageIndex*@pageCount");
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@pageIndex", (object)pageIndex), new SqlParameter("@pageCount", (object)pageCount), new SqlParameter("@isdeleted", (object)state), new SqlParameter("@schoolid", (object)schoolid), new SqlParameter("@facultyid", (object)facultyid), new SqlParameter("@name", string.Concat("%", name, "%")) };
			return SqlHelper.ExecuteList<UserInfoList>(sql, sqlParameter);
		}

		public int GetUserListCount(Guid schoolid, Guid facultyid, int state, string type, string name)
		{
			string sql = " SELECT COUNT(Id) FROM UserInfo WHERE LoginName IS NOT NULL ";
			if (schoolid != Guid.Empty)
			{
				sql = string.Concat(sql, " AND SchoolId=@schoolid ");
			}
			if (facultyid != Guid.Empty)
			{
				sql = string.Concat(sql, " AND FacultyId=@facultyid ");
			}
			if (state >= 0)
			{
				sql = string.Concat(sql, " AND IsDeleted=@isdeleted ");
			}
			if (!string.IsNullOrEmpty(name))
			{
				sql = string.Concat(sql, " AND ([Name] LIKE @name OR UserInfo.LoginName LIKE @name) ");
			}
			if (!string.IsNullOrEmpty(type))
			{
				sql = string.Concat(sql, " AND UserInfo.[Type] IN (", type, ") ");
			}
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@isdeleted", (object)state), new SqlParameter("@schoolid", (object)schoolid), new SqlParameter("@facultyid", (object)facultyid), new SqlParameter("@name", string.Concat("%", name, "%")) };
			return (int)SqlHelper.ExecuteScalar(sql, sqlParameter);
		}

		public UserView GetViewEntity(Guid id)
		{
			string sql = "select\r\n                        UserInfo.Id,\r\n                        UserInfo.Name,\r\n                        UserInfo.LoginName,\r\n                        UserInfo.LoginPwd,\r\n                        UserInfo.Sex,\r\n                        UserInfo.Birthday,\r\n                        s1.Name as School,\r\n                        s2.Name as Faculty,\r\n                        s3.Name as Department,\r\n                        Job.Name as Job,\r\n                        Profession.Name as Profession,\r\n                        Post.Name as Post,\r\n                        UserInfo.Email,\r\n                        UserInfo.Phone,\r\n                        UserInfo.[Type],\r\n                        UserInfo.CreateTime,\r\n                        UserInfo.IsDeleted\r\n                        from UserInfo\r\n                        left join School as s1 on UserInfo.SchoolId = s1.Id\r\n                        left join School as s2 on UserInfo.FacultyId = s2.Id\r\n                        left join School as s3 on UserInfo.DepartmentId = s3.Id\r\n                        left join Job on UserInfo.JobId = Job.Id\r\n                        left join Profession on UserInfo.ProfessionId = Profession.Id\r\n                        left join Post on UserInfo.PostId = Post.Id\r\n                        where UserInfo.Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteEntity<UserView>(sql, sqlParameter);
		}

		public int SoftDelete(Guid id)
		{
			string sql = "UPDATE UserInfo SET IsDeleted = 1 WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}

		public int Update(UserInfo userInfo)
		{
			string sql = "UPDATE UserInfo SET Name = @Name,LoginName = @LoginName,LoginPwd = @LoginPwd,Sex = @Sex,Birthday = @Birthday,SchoolId = @SchoolId,FacultyId = @FacultyId,DepartmentId = @DepartmentId,JobId = @JobId,ProfessionId = @ProfessionId,PostId = @PostId,Email = @Email,Phone = @Phone,Type = @Type,CreateTime = @CreateTime,IsDeleted = @IsDeleted WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)userInfo.Id), new SqlParameter("@Name", SqlHelper.ToDBValue(userInfo.Name)), new SqlParameter("@LoginName", SqlHelper.ToDBValue(userInfo.LoginName)), new SqlParameter("@LoginPwd", SqlHelper.ToDBValue(userInfo.LoginPwd)), new SqlParameter("@Sex", SqlHelper.ToDBValue(userInfo.Sex)), new SqlParameter("@Birthday", SqlHelper.ToDBValue(userInfo.Birthday)), new SqlParameter("@SchoolId", SqlHelper.ToDBValue(userInfo.SchoolId)), new SqlParameter("@FacultyId", SqlHelper.ToDBValue(userInfo.FacultyId)), new SqlParameter("@DepartmentId", SqlHelper.ToDBValue(userInfo.DepartmentId)), new SqlParameter("@JobId", SqlHelper.ToDBValue(userInfo.JobId)), new SqlParameter("@ProfessionId", SqlHelper.ToDBValue(userInfo.ProfessionId)), new SqlParameter("@PostId", SqlHelper.ToDBValue(userInfo.PostId)), new SqlParameter("@Email", SqlHelper.ToDBValue(userInfo.Email)), new SqlParameter("@Phone", SqlHelper.ToDBValue(userInfo.Phone)), new SqlParameter("@Type", SqlHelper.ToDBValue(userInfo.Type)), new SqlParameter("@CreateTime", SqlHelper.ToDBValue(userInfo.CreateTime)), new SqlParameter("@IsDeleted", SqlHelper.ToDBValue(userInfo.IsDeleted)) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}

		public int UpdateJob(Guid id)
		{
			string sql = "UPDATE UserInfo SET JobId = NULL WHERE JobId = @JobId";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@JobId", (object)id) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}

		public int UpdatePost(Guid id)
		{
			string sql = "UPDATE UserInfo SET PostId = NULL WHERE PostId = @PostId";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@PostId", (object)id) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}

		public int UpdatePro(Guid id)
		{
			string sql = "UPDATE UserInfo SET ProfessionId = NULL WHERE ProfessionId = @ProfessionId";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@ProfessionId", (object)id) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}
	}
}