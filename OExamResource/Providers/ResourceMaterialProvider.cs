using Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Providers
{
	public class ResourceMaterialProvider
	{
		private readonly static ResourceMaterialProvider InstanceObj;

		public static ResourceMaterialProvider Instance
		{
			get
			{
				return ResourceMaterialProvider.InstanceObj;
			}
		}

		static ResourceMaterialProvider()
		{
			ResourceMaterialProvider.InstanceObj = new ResourceMaterialProvider();
		}

		private ResourceMaterialProvider()
		{
		}

		public int CourseDelete(Guid id)
		{
			string sql = "DELETE ResourceCourse WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}

		public int CourseSoftDelete(Guid id)
		{
			string sql = "UPDATE ResourceCourse SET IsDeleted = 1 WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}

		public ResourceMaterial Create(ResourceMaterial resourceMaterial)
		{
			ResourceMaterial resourceMaterial1;
			if (resourceMaterial.Id == Guid.Empty)
			{
				resourceMaterial.Id = Guid.NewGuid();
			}
			string sql = "INSERT INTO ResourceMaterial (Id, Title, Size, Author, CategoryId, SchoolId, TypeId, Gaozhi, KeyWords, Fescribe, Filepath, NewFilepath, DownloadFilepath, IcoFilepath, PreviewFilepath, FileExt, SortId, IssueTime, DbStatus, IsTop, Is211, ModifyTime, CreateTime, IsDeleted, tag1, tag1_uptime, Tag2, ClickCount)  VALUES (@Id, @Title, @Size, @Author, @CategoryId, @SchoolId, @TypeId, @Gaozhi, @KeyWords, @Fescribe, @Filepath, @NewFilepath, @DownloadFilepath, @IcoFilepath, @PreviewFilepath, @FileExt, @SortId, @IssueTime, @DbStatus, @IsTop, @Is211, @ModifyTime, @CreateTime, @IsDeleted, @tag1, @tag1_uptime, @Tag2, @ClickCount)";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", SqlHelper.ToDBValue(resourceMaterial.Id)), new SqlParameter("@Title", SqlHelper.ToDBValue(resourceMaterial.Title)), new SqlParameter("@Size", SqlHelper.ToDBValue(resourceMaterial.Size)), new SqlParameter("@Author", SqlHelper.ToDBValue(resourceMaterial.Author)), new SqlParameter("@CategoryId", SqlHelper.ToDBValue(resourceMaterial.CategoryId)), new SqlParameter("@SchoolId", SqlHelper.ToDBValue(resourceMaterial.SchoolId)), new SqlParameter("@TypeId", SqlHelper.ToDBValue(resourceMaterial.TypeId)), new SqlParameter("@Gaozhi", SqlHelper.ToDBValue(resourceMaterial.Gaozhi)), new SqlParameter("@KeyWords", SqlHelper.ToDBValue(resourceMaterial.KeyWords)), new SqlParameter("@Fescribe", SqlHelper.ToDBValue(resourceMaterial.Fescribe)), new SqlParameter("@Filepath", SqlHelper.ToDBValue(resourceMaterial.Filepath)), new SqlParameter("@NewFilepath", SqlHelper.ToDBValue(resourceMaterial.NewFilepath)), new SqlParameter("@DownloadFilepath", SqlHelper.ToDBValue(resourceMaterial.DownloadFilepath)), new SqlParameter("@IcoFilepath", SqlHelper.ToDBValue(resourceMaterial.IcoFilepath)), new SqlParameter("@PreviewFilepath", SqlHelper.ToDBValue(resourceMaterial.PreviewFilepath)), new SqlParameter("@FileExt", SqlHelper.ToDBValue(resourceMaterial.FileExt)), new SqlParameter("@SortId", SqlHelper.ToDBValue(resourceMaterial.SortId)), new SqlParameter("@IssueTime", SqlHelper.ToDBValue(resourceMaterial.IssueTime)), new SqlParameter("@DbStatus", SqlHelper.ToDBValue(resourceMaterial.DbStatus)), new SqlParameter("@IsTop", SqlHelper.ToDBValue(resourceMaterial.IsTop)), new SqlParameter("@Is211", SqlHelper.ToDBValue(resourceMaterial.Is211)), new SqlParameter("@ModifyTime", SqlHelper.ToDBValue(resourceMaterial.ModifyTime)), new SqlParameter("@CreateTime", SqlHelper.ToDBValue(resourceMaterial.CreateTime)), new SqlParameter("@IsDeleted", SqlHelper.ToDBValue(resourceMaterial.IsDeleted)), new SqlParameter("@tag1", SqlHelper.ToDBValue(resourceMaterial.Tag1)), new SqlParameter("@tag1_uptime", SqlHelper.ToDBValue(resourceMaterial.Tag1_uptime)), new SqlParameter("@Tag2", SqlHelper.ToDBValue(resourceMaterial.Tag2)), new SqlParameter("@ClickCount", SqlHelper.ToDBValue(resourceMaterial.ClickCount)) };
			if (SqlHelper.ExecuteNonQuery(sql, sqlParameter) > 0)
			{
				resourceMaterial1 = resourceMaterial;
			}
			else
			{
				resourceMaterial1 = null;
			}
			return resourceMaterial1;
		}

		public int Delete(Guid id)
		{
			string sql = "DELETE ResourceMaterial WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}

		public int GetCount()
		{
			return (int)SqlHelper.ExecuteScalar("SELECT COUNT(Id) FROM ResourceMaterial WHERE IsDeleted = 0", new SqlParameter[0]);
		}

		public ResourceMaterial GetEntity(Guid id)
		{
			string sql = "SELECT * FROM ResourceMaterial WHERE Id = @Id AND IsDeleted = 0";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteEntity<ResourceMaterial>(sql, sqlParameter);
		}

		public List<ResourceMaterial> GetList()
		{
			return SqlHelper.ExecuteList<ResourceMaterial>("SELECT * FROM ResourceMaterial WHERE IsDeleted = 0", new SqlParameter[0]);
		}

		public List<ResourceMaterial> GetPage(int pageIndex, int pageCount)
		{
			string sql = "SELECT * FROM(SELECT *,(ROW_NUMBER() OVER(ORDER BY Id)) rownum FROM ResourceMaterial WHERE IsDeleted = 0) t WHERE rownum>=(@pageCount*(@pageIndex-1)+1) AND rownum<=@pageIndex*@pageCount";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@pageIndex", (object)pageIndex), new SqlParameter("@pageCount", (object)pageCount) };
			return SqlHelper.ExecuteList<ResourceMaterial>(sql, sqlParameter);
		}

		public int SoftDelete(Guid id)
		{
			string sql = "UPDATE ResourceMaterial SET IsDeleted = 1 WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}

		public int Update(ResourceMaterial resourceMaterial)
		{
			string sql = "UPDATE ResourceMaterial SET Title = @Title,Size = @Size,Author = @Author,CategoryId = @CategoryId,SchoolId = @SchoolId,TypeId = @TypeId,Gaozhi = @Gaozhi,KeyWords = @KeyWords,Fescribe = @Fescribe,Filepath = @Filepath,NewFilepath = @NewFilepath,DownloadFilepath = @DownloadFilepath,IcoFilepath = @IcoFilepath,PreviewFilepath = @PreviewFilepath,FileExt = @FileExt,SortId = @SortId,IssueTime = @IssueTime,DbStatus = @DbStatus,IsTop = @IsTop,Is211 = @Is211,ModifyTime = @ModifyTime,CreateTime = @CreateTime,IsDeleted = @IsDeleted,tag1 = @tag1,tag1_uptime = @tag1_uptime,Tag2 = @Tag2,ClickCount = @ClickCount WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)resourceMaterial.Id), new SqlParameter("@Title", SqlHelper.ToDBValue(resourceMaterial.Title)), new SqlParameter("@Size", SqlHelper.ToDBValue(resourceMaterial.Size)), new SqlParameter("@Author", SqlHelper.ToDBValue(resourceMaterial.Author)), new SqlParameter("@CategoryId", SqlHelper.ToDBValue(resourceMaterial.CategoryId)), new SqlParameter("@SchoolId", SqlHelper.ToDBValue(resourceMaterial.SchoolId)), new SqlParameter("@TypeId", SqlHelper.ToDBValue(resourceMaterial.TypeId)), new SqlParameter("@Gaozhi", SqlHelper.ToDBValue(resourceMaterial.Gaozhi)), new SqlParameter("@KeyWords", SqlHelper.ToDBValue(resourceMaterial.KeyWords)), new SqlParameter("@Fescribe", SqlHelper.ToDBValue(resourceMaterial.Fescribe)), new SqlParameter("@Filepath", SqlHelper.ToDBValue(resourceMaterial.Filepath)), new SqlParameter("@NewFilepath", SqlHelper.ToDBValue(resourceMaterial.NewFilepath)), new SqlParameter("@DownloadFilepath", SqlHelper.ToDBValue(resourceMaterial.DownloadFilepath)), new SqlParameter("@IcoFilepath", SqlHelper.ToDBValue(resourceMaterial.IcoFilepath)), new SqlParameter("@PreviewFilepath", SqlHelper.ToDBValue(resourceMaterial.PreviewFilepath)), new SqlParameter("@FileExt", SqlHelper.ToDBValue(resourceMaterial.FileExt)), new SqlParameter("@SortId", SqlHelper.ToDBValue(resourceMaterial.SortId)), new SqlParameter("@IssueTime", SqlHelper.ToDBValue(resourceMaterial.IssueTime)), new SqlParameter("@DbStatus", SqlHelper.ToDBValue(resourceMaterial.DbStatus)), new SqlParameter("@IsTop", SqlHelper.ToDBValue(resourceMaterial.IsTop)), new SqlParameter("@Is211", SqlHelper.ToDBValue(resourceMaterial.Is211)), new SqlParameter("@ModifyTime", SqlHelper.ToDBValue(resourceMaterial.ModifyTime)), new SqlParameter("@CreateTime", SqlHelper.ToDBValue(resourceMaterial.CreateTime)), new SqlParameter("@IsDeleted", SqlHelper.ToDBValue(resourceMaterial.IsDeleted)), new SqlParameter("@tag1", SqlHelper.ToDBValue(resourceMaterial.Tag1)), new SqlParameter("@tag1_uptime", SqlHelper.ToDBValue(resourceMaterial.Tag1_uptime)), new SqlParameter("@Tag2", SqlHelper.ToDBValue(resourceMaterial.Tag2)), new SqlParameter("@ClickCount", SqlHelper.ToDBValue(resourceMaterial.ClickCount)) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}
	}
}