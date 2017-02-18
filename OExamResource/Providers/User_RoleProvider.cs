using Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Providers
{
	public class User_RoleProvider
	{
		private readonly static User_RoleProvider InstanceObj;

		public static User_RoleProvider Instance
		{
			get
			{
				return User_RoleProvider.InstanceObj;
			}
		}

		static User_RoleProvider()
		{
			User_RoleProvider.InstanceObj = new User_RoleProvider();
		}

		private User_RoleProvider()
		{
		}

		public User_Role Create(User_Role user_Role)
		{
			User_Role userRole;
			if (user_Role.Id == Guid.Empty)
			{
				user_Role.Id = Guid.NewGuid();
			}
			string sql = "INSERT INTO User_Role (Id, UserId, RoleId, IsCustom, CreateTime, IsDeleted)  VALUES (@Id, @UserId, @RoleId, @IsCustom, @CreateTime, @IsDeleted)";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", SqlHelper.ToDBValue(user_Role.Id)), new SqlParameter("@UserId", SqlHelper.ToDBValue(user_Role.UserId)), new SqlParameter("@RoleId", SqlHelper.ToDBValue(user_Role.RoleId)), new SqlParameter("@IsCustom", SqlHelper.ToDBValue(user_Role.IsCustom)), new SqlParameter("@CreateTime", SqlHelper.ToDBValue(user_Role.CreateTime)), new SqlParameter("@IsDeleted", SqlHelper.ToDBValue(user_Role.IsDeleted)) };
			if (SqlHelper.ExecuteNonQuery(sql, sqlParameter) > 0)
			{
				userRole = user_Role;
			}
			else
			{
				userRole = null;
			}
			return userRole;
		}

		public int Delete(Guid id)
		{
			string sql = "DELETE User_Role WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}

		public int DeleteByUserId(Guid id)
		{
			string sql = "DELETE User_Role WHERE UserId = @UserId ";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@UserId", (object)id) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}

		public int GetCount()
		{
			return (int)SqlHelper.ExecuteScalar("SELECT COUNT(Id) FROM User_Role WHERE IsDeleted = 0", new SqlParameter[0]);
		}

		public User_Role GetEntity(Guid id)
		{
			string sql = "SELECT * FROM User_Role WHERE Id = @Id AND IsDeleted = 0";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteEntity<User_Role>(sql, sqlParameter);
		}

		public User_Role GetEntityByUserId(Guid id)
		{
			string sql = "SELECT * FROM User_Role WHERE UserId = @UserId ";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@UserId", (object)id) };
			return SqlHelper.ExecuteEntity<User_Role>(sql, sqlParameter);
		}

		public List<User_Role> GetList()
		{
			return SqlHelper.ExecuteList<User_Role>("SELECT * FROM User_Role WHERE IsDeleted = 0", new SqlParameter[0]);
		}

		public List<User_Role> GetPage(int pageIndex, int pageCount)
		{
			string sql = "SELECT * FROM(SELECT *,(ROW_NUMBER() OVER(ORDER BY Id)) rownum FROM User_Role WHERE IsDeleted = 0) t WHERE rownum>=(@pageCount*(@pageIndex-1)+1) AND rownum<=@pageIndex*@pageCount";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@pageIndex", (object)pageIndex), new SqlParameter("@pageCount", (object)pageCount) };
			return SqlHelper.ExecuteList<User_Role>(sql, sqlParameter);
		}

		public int SoftDelete(Guid id)
		{
			string sql = "UPDATE User_Role SET IsDeleted = 1 WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}

		public int Update(User_Role user_Role)
		{
			string sql = "UPDATE User_Role SET UserId = @UserId,RoleId = @RoleId,IsCustom = @IsCustom,CreateTime = @CreateTime,IsDeleted = @IsDeleted WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)user_Role.Id), new SqlParameter("@UserId", SqlHelper.ToDBValue(user_Role.UserId)), new SqlParameter("@RoleId", SqlHelper.ToDBValue(user_Role.RoleId)), new SqlParameter("@IsCustom", SqlHelper.ToDBValue(user_Role.IsCustom)), new SqlParameter("@CreateTime", SqlHelper.ToDBValue(user_Role.CreateTime)), new SqlParameter("@IsDeleted", SqlHelper.ToDBValue(user_Role.IsDeleted)) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}
	}
}