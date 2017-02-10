using Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Providers
{
	public class ResourceErrorProvider
	{
		private readonly static ResourceErrorProvider InstanceObj;

		public static ResourceErrorProvider Instance
		{
			get
			{
				return ResourceErrorProvider.InstanceObj;
			}
		}

		static ResourceErrorProvider()
		{
			ResourceErrorProvider.InstanceObj = new ResourceErrorProvider();
		}

		private ResourceErrorProvider()
		{
		}

		public ResourceError Create(ResourceError resourceError)
		{
			ResourceError resourceError1;
			if (resourceError.Id == Guid.Empty)
			{
				resourceError.Id = Guid.NewGuid();
			}
			string sql = "INSERT INTO ResourceError (Id, ResourceId, Code, Remark, CreatedTime, IsDeleted)  VALUES (@Id, @ResourceId, @Code, @Remark, @CreatedTime, @IsDeleted)";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", SqlHelper.ToDBValue(resourceError.Id)), new SqlParameter("@ResourceId", SqlHelper.ToDBValue(resourceError.ResourceId)), new SqlParameter("@Code", SqlHelper.ToDBValue(resourceError.Code)), new SqlParameter("@Remark", SqlHelper.ToDBValue(resourceError.Remark)), new SqlParameter("@CreatedTime", SqlHelper.ToDBValue(resourceError.CreatedTime)), new SqlParameter("@IsDeleted", SqlHelper.ToDBValue(resourceError.IsDeleted)) };
			if (SqlHelper.ExecuteNonQuery(sql, sqlParameter) > 0)
			{
				resourceError1 = resourceError;
			}
			else
			{
				resourceError1 = null;
			}
			return resourceError1;
		}

		public int Delete(Guid id)
		{
			string sql = "DELETE ResourceError WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}

		public int GetCount()
		{
			return (int)SqlHelper.ExecuteScalar("SELECT COUNT(Id) FROM ResourceError WHERE IsDeleted = 0", new SqlParameter[0]);
		}

		public ResourceError GetEntity(Guid id)
		{
			string sql = "SELECT * FROM ResourceError WHERE Id = @Id AND IsDeleted = 0";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteEntity<ResourceError>(sql, sqlParameter);
		}

		public List<ResourceError> GetList()
		{
			return SqlHelper.ExecuteList<ResourceError>("SELECT * FROM ResourceError WHERE IsDeleted = 0", new SqlParameter[0]);
		}

		public List<ResourceError> GetPage(int pageIndex, int pageCount)
		{
			string sql = "SELECT * FROM(SELECT *,(ROW_NUMBER() OVER(ORDER BY Id)) rownum FROM ResourceError WHERE IsDeleted = 0) t WHERE rownum>=(@pageCount*(@pageIndex-1)+1) AND rownum<=@pageIndex*@pageCount";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@pageIndex", (object)pageIndex), new SqlParameter("@pageCount", (object)pageCount) };
			return SqlHelper.ExecuteList<ResourceError>(sql, sqlParameter);
		}

		public int SoftDelete(Guid id)
		{
			string sql = "UPDATE ResourceError SET IsDeleted = 1 WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}

		public int Update(ResourceError resourceError)
		{
			string sql = "UPDATE ResourceError SET ResourceId = @ResourceId,Code = @Code,Remark = @Remark,CreatedTime = @CreatedTime,IsDeleted = @IsDeleted WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)resourceError.Id), new SqlParameter("@ResourceId", SqlHelper.ToDBValue(resourceError.ResourceId)), new SqlParameter("@Code", SqlHelper.ToDBValue(resourceError.Code)), new SqlParameter("@Remark", SqlHelper.ToDBValue(resourceError.Remark)), new SqlParameter("@CreatedTime", SqlHelper.ToDBValue(resourceError.CreatedTime)), new SqlParameter("@IsDeleted", SqlHelper.ToDBValue(resourceError.IsDeleted)) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}
	}
}