using Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Providers
{
	public class ResourceUploadProvider
	{
		private readonly static ResourceUploadProvider InstanceObj;

		public static ResourceUploadProvider Instance
		{
			get
			{
				return ResourceUploadProvider.InstanceObj;
			}
		}

		static ResourceUploadProvider()
		{
			ResourceUploadProvider.InstanceObj = new ResourceUploadProvider();
		}

		private ResourceUploadProvider()
		{
		}

		public ResourceUpload Create(ResourceUpload resourceUpload)
		{
			ResourceUpload resourceUpload1;
			if (resourceUpload.Id == Guid.Empty)
			{
				resourceUpload.Id = Guid.NewGuid();
			}
			string sql = "INSERT INTO ResourceUpload (Id, Name, Size, UserId, CategoryId, TypeId, Course, KeyWord, Remark, CreatedTime, IsDeleted)  VALUES (@Id, @Name, @Size, @UserId, @CategoryId, @TypeId, @Course, @KeyWord, @Remark, @CreatedTime, @IsDeleted)";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", SqlHelper.ToDBValue(resourceUpload.Id)), new SqlParameter("@Name", SqlHelper.ToDBValue(resourceUpload.Name)), new SqlParameter("@Size", SqlHelper.ToDBValue(resourceUpload.Size)), new SqlParameter("@UserId", SqlHelper.ToDBValue(resourceUpload.UserId)), new SqlParameter("@CategoryId", SqlHelper.ToDBValue(resourceUpload.CategoryId)), new SqlParameter("@TypeId", SqlHelper.ToDBValue(resourceUpload.TypeId)), new SqlParameter("@Course", SqlHelper.ToDBValue(resourceUpload.Course)), new SqlParameter("@KeyWord", SqlHelper.ToDBValue(resourceUpload.KeyWord)), new SqlParameter("@Remark", SqlHelper.ToDBValue(resourceUpload.Remark)), new SqlParameter("@CreatedTime", SqlHelper.ToDBValue(resourceUpload.CreatedTime)), new SqlParameter("@IsDeleted", SqlHelper.ToDBValue(resourceUpload.IsDeleted)) };
			if (SqlHelper.ExecuteNonQuery(sql, sqlParameter) > 0)
			{
				resourceUpload1 = resourceUpload;
			}
			else
			{
				resourceUpload1 = null;
			}
			return resourceUpload1;
		}

		public int Delete(Guid id)
		{
			string sql = "DELETE ResourceUpload WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}

		public int GetCount()
		{
			return (int)SqlHelper.ExecuteScalar("SELECT COUNT(Id) FROM ResourceUpload WHERE IsDeleted = 0", new SqlParameter[0]);
		}

		public ResourceUpload GetEntity(Guid id)
		{
			string sql = "SELECT * FROM ResourceUpload WHERE Id = @Id AND IsDeleted = 0";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteEntity<ResourceUpload>(sql, sqlParameter);
		}

		public List<ResourceUpload> GetList()
		{
			return SqlHelper.ExecuteList<ResourceUpload>("SELECT * FROM ResourceUpload WHERE IsDeleted = 0", new SqlParameter[0]);
		}

		public List<ResourceUpload> GetPage(int pageIndex, int pageCount)
		{
			string sql = "SELECT * FROM(SELECT *,(ROW_NUMBER() OVER(ORDER BY Id)) rownum FROM ResourceUpload WHERE IsDeleted = 0) t WHERE rownum>=(@pageCount*(@pageIndex-1)+1) AND rownum<=@pageIndex*@pageCount";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@pageIndex", (object)pageIndex), new SqlParameter("@pageCount", (object)pageCount) };
			return SqlHelper.ExecuteList<ResourceUpload>(sql, sqlParameter);
		}

		public int SoftDelete(Guid id)
		{
			string sql = "UPDATE ResourceUpload SET IsDeleted = 1 WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}

		public int Update(ResourceUpload resourceUpload)
		{
			string sql = "UPDATE ResourceUpload SET Name = @Name,Size = @Size,UserId = @UserId,CategoryId = @CategoryId,TypeId = @TypeId,Course = @Course,KeyWord = @KeyWord,Remark = @Remark,CreatedTime = @CreatedTime,IsDeleted = @IsDeleted WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)resourceUpload.Id), new SqlParameter("@Name", SqlHelper.ToDBValue(resourceUpload.Name)), new SqlParameter("@Size", SqlHelper.ToDBValue(resourceUpload.Size)), new SqlParameter("@UserId", SqlHelper.ToDBValue(resourceUpload.UserId)), new SqlParameter("@CategoryId", SqlHelper.ToDBValue(resourceUpload.CategoryId)), new SqlParameter("@TypeId", SqlHelper.ToDBValue(resourceUpload.TypeId)), new SqlParameter("@Course", SqlHelper.ToDBValue(resourceUpload.Course)), new SqlParameter("@KeyWord", SqlHelper.ToDBValue(resourceUpload.KeyWord)), new SqlParameter("@Remark", SqlHelper.ToDBValue(resourceUpload.Remark)), new SqlParameter("@CreatedTime", SqlHelper.ToDBValue(resourceUpload.CreatedTime)), new SqlParameter("@IsDeleted", SqlHelper.ToDBValue(resourceUpload.IsDeleted)) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}
	}
}