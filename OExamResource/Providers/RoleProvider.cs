using Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Providers
{
	public class RoleProvider
	{
		private readonly static RoleProvider InstanceObj;

		public static RoleProvider Instance
		{
			get
			{
				return RoleProvider.InstanceObj;
			}
		}

		static RoleProvider()
		{
			RoleProvider.InstanceObj = new RoleProvider();
		}

		private RoleProvider()
		{
		}

		public Role Create(Role role)
		{
			Role role1;
			if (role.Id == Guid.Empty)
			{
				role.Id = Guid.NewGuid();
			}
			string sql = "INSERT INTO Role (Id, Name, Type, SchoolId, DepartmentId, CreateTime, IsDeleted)  VALUES (@Id, @Name, @Type, @SchoolId, @DepartmentId, @CreateTime, @IsDeleted)";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", SqlHelper.ToDBValue(role.Id)), new SqlParameter("@Name", SqlHelper.ToDBValue(role.Name)), new SqlParameter("@Type", SqlHelper.ToDBValue(role.Type)), new SqlParameter("@SchoolId", SqlHelper.ToDBValue(role.SchoolId)), new SqlParameter("@DepartmentId", SqlHelper.ToDBValue(role.DepartmentId)), new SqlParameter("@CreateTime", SqlHelper.ToDBValue(role.CreateTime)), new SqlParameter("@IsDeleted", SqlHelper.ToDBValue(role.IsDeleted)) };
			if (SqlHelper.ExecuteNonQuery(sql, sqlParameter) > 0)
			{
				role1 = role;
			}
			else
			{
				role1 = null;
			}
			return role1;
		}

		public int Delete(Guid id)
		{
			string sql = "DELETE Role WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}

		public int GetCount()
		{
			return (int)SqlHelper.ExecuteScalar("SELECT COUNT(Id) FROM Role WHERE IsDeleted = 0", new SqlParameter[0]);
		}

		public Role GetEntity(Guid id)
		{
			string sql = "SELECT * FROM Role WHERE Id = @Id AND IsDeleted = 0";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteEntity<Role>(sql, sqlParameter);
		}

		public List<Role> GetList()
		{
			return SqlHelper.ExecuteList<Role>("SELECT * FROM Role WHERE IsDeleted = 0", new SqlParameter[0]);
		}

		public List<Role> GetListBySchoolId(Guid schoolId)
		{
			string sql = "SELECT * FROM Role WHERE SchoolId = @SchoolId AND IsDeleted = 0";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@SchoolId", (object)schoolId) };
			return SqlHelper.ExecuteList<Role>(sql, sqlParameter);
		}

		public List<Role> GetPage(int pageIndex, int pageCount)
		{
			string sql = "SELECT * FROM(SELECT *,(ROW_NUMBER() OVER(ORDER BY Id)) rownum FROM Role WHERE IsDeleted = 0) t WHERE rownum>=(@pageCount*(@pageIndex-1)+1) AND rownum<=@pageIndex*@pageCount";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@pageIndex", (object)pageIndex), new SqlParameter("@pageCount", (object)pageCount) };
			return SqlHelper.ExecuteList<Role>(sql, sqlParameter);
		}

		public Guid GetRoleid(Guid schoolid)
		{
			string sql = "SELECT id FROM Role WHERE SchoolId = @SchoolId AND  Type=3 and  IsDeleted = 0";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@SchoolId", (object)schoolid) };
			SqlParameter[] para = sqlParameter;
			Guid guid = Guid.Parse(SqlHelper.ExecuteScalar(sql, para).ToString());
			return guid;
		}

		public int SoftDelete(Guid id)
		{
			string sql = "UPDATE Role SET IsDeleted = 1 WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}

		public int Update(Role role)
		{
			string sql = "UPDATE Role SET Name = @Name,Type = @Type,SchoolId = @SchoolId,DepartmentId = @DepartmentId,CreateTime = @CreateTime,IsDeleted = @IsDeleted WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)role.Id), new SqlParameter("@Name", SqlHelper.ToDBValue(role.Name)), new SqlParameter("@Type", SqlHelper.ToDBValue(role.Type)), new SqlParameter("@SchoolId", SqlHelper.ToDBValue(role.SchoolId)), new SqlParameter("@DepartmentId", SqlHelper.ToDBValue(role.DepartmentId)), new SqlParameter("@CreateTime", SqlHelper.ToDBValue(role.CreateTime)), new SqlParameter("@IsDeleted", SqlHelper.ToDBValue(role.IsDeleted)) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}
	}
}