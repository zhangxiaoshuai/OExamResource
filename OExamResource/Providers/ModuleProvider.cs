using Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Providers
{
	public class ModuleProvider
	{
		private readonly static ModuleProvider InstanceObj;

		public static ModuleProvider Instance
		{
			get
			{
				return ModuleProvider.InstanceObj;
			}
		}

		static ModuleProvider()
		{
			ModuleProvider.InstanceObj = new ModuleProvider();
		}

		private ModuleProvider()
		{
		}

		public Module Create(Module module)
		{
			Module module1;
			if (module.Id == Guid.Empty)
			{
				module.Id = Guid.NewGuid();
			}
			string sql = "INSERT INTO Module (Id, Name, Code, CreateTime, IsDeleted)  VALUES (@Id, @Name, @Code, @CreateTime, @IsDeleted)";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", SqlHelper.ToDBValue(module.Id)), new SqlParameter("@Name", SqlHelper.ToDBValue(module.Name)), new SqlParameter("@Code", SqlHelper.ToDBValue(module.Code)), new SqlParameter("@CreateTime", SqlHelper.ToDBValue(module.CreateTime)), new SqlParameter("@IsDeleted", SqlHelper.ToDBValue(module.IsDeleted)) };
			if (SqlHelper.ExecuteNonQuery(sql, sqlParameter) > 0)
			{
				module1 = module;
			}
			else
			{
				module1 = null;
			}
			return module1;
		}

		public int Delete(Guid id)
		{
			string sql = "DELETE Module WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}

		public int GetCount()
		{
			return (int)SqlHelper.ExecuteScalar("SELECT COUNT(Id) FROM Module WHERE IsDeleted = 0", new SqlParameter[0]);
		}

		public Module GetEntity(Guid id)
		{
			string sql = "SELECT * FROM Module WHERE Id = @Id AND IsDeleted = 0";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteEntity<Module>(sql, sqlParameter);
		}

		public List<Module> GetList(Guid schoolId)
		{
			string sql = "select Module.* from School_Module left join Module on School_Module.ModuleId = Module.Id where SchoolId = @schoolId And School_Module.IsDeleted = 0 order by Module.Tag1 ";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@schoolId", (object)schoolId) };
			return SqlHelper.ExecuteList<Module>(sql, sqlParameter);
		}

		public List<Module> GetList()
		{
			return SqlHelper.ExecuteList<Module>("SELECT * FROM Module WHERE IsDeleted = 0 ORDER BY CreateTime", new SqlParameter[0]);
		}

		public List<Module> GetModuleList()
		{
			return SqlHelper.ExecuteList<Module>("SELECT * FROM Module WHERE IsDeleted = 0 ORDER BY CreateTime", new SqlParameter[0]);
		}

		public List<Module> GetPage(int pageIndex, int pageCount)
		{
			string sql = "SELECT * FROM(SELECT *,(ROW_NUMBER() OVER(ORDER BY Id)) rownum FROM Module WHERE IsDeleted = 0) t WHERE rownum>=(@pageCount*(@pageIndex-1)+1) AND rownum<=@pageIndex*@pageCount";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@pageIndex", (object)pageIndex), new SqlParameter("@pageCount", (object)pageCount) };
			return SqlHelper.ExecuteList<Module>(sql, sqlParameter);
		}

		public List<Province> GetProvinceList()
		{
			return SqlHelper.ExecuteList<Province>("SELECT * FROM Province WHERE IsDeleted = 0 ORDER BY keys", new SqlParameter[0]);
		}

		public int SoftDelete(Guid id)
		{
			string sql = "UPDATE Module SET IsDeleted = 1 WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}

		public int Update(Module module)
		{
			string sql = "UPDATE Module SET Name = @Name,Code = @Code,CreateTime = @CreateTime,IsDeleted = @IsDeleted WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)module.Id), new SqlParameter("@Name", SqlHelper.ToDBValue(module.Name)), new SqlParameter("@Code", SqlHelper.ToDBValue(module.Code)), new SqlParameter("@CreateTime", SqlHelper.ToDBValue(module.CreateTime)), new SqlParameter("@IsDeleted", SqlHelper.ToDBValue(module.IsDeleted)) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}
	}
}