using Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Providers
{
	public class School_ModuleProvider
	{
		private readonly static School_ModuleProvider InstanceObj;

		public static School_ModuleProvider Instance
		{
			get
			{
				return School_ModuleProvider.InstanceObj;
			}
		}

		static School_ModuleProvider()
		{
			School_ModuleProvider.InstanceObj = new School_ModuleProvider();
		}

		private School_ModuleProvider()
		{
		}

		public School_Module Create(School_Module school_Module)
		{
			School_Module schoolModule;
			if (school_Module.Id == Guid.Empty)
			{
				school_Module.Id = Guid.NewGuid();
			}
			string sql = "INSERT INTO School_Module (Id, SchoolId, ModuleId, CreateTime, IsDeleted,TrialTag)  VALUES (@Id, @SchoolId, @ModuleId, @CreateTime, @IsDeleted,@TrialTag)";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", SqlHelper.ToDBValue(school_Module.Id)), new SqlParameter("@SchoolId", SqlHelper.ToDBValue(school_Module.SchoolId)), new SqlParameter("@ModuleId", SqlHelper.ToDBValue(school_Module.ModuleId)), new SqlParameter("@CreateTime", SqlHelper.ToDBValue(school_Module.CreateTime)), new SqlParameter("@IsDeleted", SqlHelper.ToDBValue(school_Module.IsDeleted)), new SqlParameter("@TrialTag", SqlHelper.ToDBValue(school_Module.TrialTag)) };
			if (SqlHelper.ExecuteNonQuery(sql, sqlParameter) > 0)
			{
				schoolModule = school_Module;
			}
			else
			{
				schoolModule = null;
			}
			return schoolModule;
		}

		public int Delete(Guid id)
		{
			string sql = "DELETE School_Module WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}

		public int DeleteBySchoolId(Guid id)
		{
			string sql = "DELETE School_Module WHERE SchoolId = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}

		public void EditBySchoolId(Guid schoolId)
		{
			string sql = "UPDATE School_Module SET IsDeleted = 1 WHERE SchoolId =@SchoolId ";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@SchoolId", (object)schoolId) };
			SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}

		public void EditBySchoolIdModuleId(Guid schoolId, Guid moduleId)
		{
			string sql = "UPDATE School_Module SET IsDeleted = 0 WHERE SchoolId =@SchoolId AND ModuleId = @ModuleId";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@SchoolId", (object)schoolId), new SqlParameter("@ModuleId", (object)moduleId) };
			SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}

		public int GetCount()
		{
			return (int)SqlHelper.ExecuteScalar("SELECT COUNT(Id) FROM School_Module WHERE IsDeleted = 0", new SqlParameter[0]);
		}

		public School_Module GetEntity(Guid id)
		{
			string sql = "SELECT * FROM School_Module WHERE Id = @Id AND IsDeleted = 0";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteEntity<School_Module>(sql, sqlParameter);
		}

		public List<School_Module> GetList(Guid id)
		{
			string sql = "SELECT * FROM School_Module WHERE  SchoolId = @SchoolId and isdeleted=0 ";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@SchoolId", (object)id) };
			return SqlHelper.ExecuteList<School_Module>(sql, sqlParameter);
		}

		public List<School_Module> GetList()
		{
			return SqlHelper.ExecuteList<School_Module>("SELECT * FROM School_Module WHERE IsDeleted = 0", new SqlParameter[0]);
		}

		public List<School_Module> GetPage(int pageIndex, int pageCount)
		{
			string sql = "SELECT * FROM(SELECT *,(ROW_NUMBER() OVER(ORDER BY Id)) rownum FROM School_Module WHERE IsDeleted = 0) t WHERE rownum>=(@pageCount*(@pageIndex-1)+1) AND rownum<=@pageIndex*@pageCount";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@pageIndex", (object)pageIndex), new SqlParameter("@pageCount", (object)pageCount) };
			return SqlHelper.ExecuteList<School_Module>(sql, sqlParameter);
		}

		public School_Module GetSchoolModuleType(Guid schoolId, Guid ModuleId)
		{
			string sql = "select TrialTag from School_Module  where SchoolId = @schoolId And SchoolId = @schoolId and ModuleId=@ModuleId";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@schoolId", (object)schoolId), new SqlParameter("@ModuleId", (object)ModuleId) };
			return SqlHelper.ExecuteEntity<School_Module>(sql, sqlParameter);
		}

		public List<School_Module> GetTag1List(Guid id)
		{
			string sql = "SELECT Module.Tag1 FROM School_Module JOIN Module ON School_Module.ModuleId=Module.Id WHERE School_Module.IsDeleted = 0  AND SchoolId = @SchoolId";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@SchoolId", (object)id) };
			return SqlHelper.ExecuteList<School_Module>(sql, sqlParameter);
		}

		public int SoftDelete(Guid id)
		{
			string sql = "UPDATE School_Module SET IsDeleted = 1 WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}

		public int Update(School_Module school_Module)
		{
			string sql = "UPDATE School_Module SET SchoolId = @SchoolId,ModuleId = @ModuleId,CreateTime = @CreateTime,IsDeleted = @IsDeleted, TrialTag = @TrialTag WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)school_Module.Id), new SqlParameter("@SchoolId", SqlHelper.ToDBValue(school_Module.SchoolId)), new SqlParameter("@ModuleId", SqlHelper.ToDBValue(school_Module.ModuleId)), new SqlParameter("@CreateTime", SqlHelper.ToDBValue(school_Module.CreateTime)), new SqlParameter("@IsDeleted", SqlHelper.ToDBValue(school_Module.IsDeleted)), new SqlParameter("@TrialTag", SqlHelper.ToDBValue(school_Module.TrialTag)) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}
	}
}