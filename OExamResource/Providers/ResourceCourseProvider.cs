using Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Providers
{
	public class ResourceCourseProvider
	{
		private readonly static ResourceCourseProvider InstanceObj;

		public static ResourceCourseProvider Instance
		{
			get
			{
				return ResourceCourseProvider.InstanceObj;
			}
		}

		static ResourceCourseProvider()
		{
			ResourceCourseProvider.InstanceObj = new ResourceCourseProvider();
		}

		private ResourceCourseProvider()
		{
		}

		public ResourceCourse Create(ResourceCourse resourceCourse)
		{
			ResourceCourse resourceCourse1;
			if (resourceCourse.Id == Guid.Empty)
			{
				resourceCourse.Id = Guid.NewGuid();
			}
			string sql = "INSERT INTO ResourceCourse (Id, Title, Size, Author, SchoolId, PackId, Gaozhi, KeyWords, Fescribe, Filepath, NewFilepath, DownloadFilepath, IcoFilepath, PreviewFilepath, FileExt, SortId, IssueTime, DbStatus, Tag2, IsTop, Is211, ModifyTime, CreateTime, IsDeleted, tag1, tag1_uptime)  VALUES (@Id, @Title, @Size, @Author, @SchoolId, @PackId, @Gaozhi, @KeyWords, @Fescribe, @Filepath, @NewFilepath, @DownloadFilepath, @IcoFilepath, @PreviewFilepath, @FileExt, @SortId, @IssueTime, @DbStatus, @Tag2, @IsTop, @Is211, @ModifyTime, @CreateTime, @IsDeleted, @tag1, @tag1_uptime)";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", SqlHelper.ToDBValue(resourceCourse.Id)), new SqlParameter("@Title", SqlHelper.ToDBValue(resourceCourse.Title)), new SqlParameter("@Size", SqlHelper.ToDBValue(resourceCourse.Size)), new SqlParameter("@Author", SqlHelper.ToDBValue(resourceCourse.Author)), new SqlParameter("@SchoolId", SqlHelper.ToDBValue(resourceCourse.SchoolId)), new SqlParameter("@PackId", SqlHelper.ToDBValue(resourceCourse.PackId)), new SqlParameter("@Gaozhi", SqlHelper.ToDBValue(resourceCourse.Gaozhi)), new SqlParameter("@KeyWords", SqlHelper.ToDBValue(resourceCourse.KeyWords)), new SqlParameter("@Fescribe", SqlHelper.ToDBValue(resourceCourse.Fescribe)), new SqlParameter("@Filepath", SqlHelper.ToDBValue(resourceCourse.Filepath)), new SqlParameter("@NewFilepath", SqlHelper.ToDBValue(resourceCourse.NewFilepath)), new SqlParameter("@DownloadFilepath", SqlHelper.ToDBValue(resourceCourse.DownloadFilepath)), new SqlParameter("@IcoFilepath", SqlHelper.ToDBValue(resourceCourse.IcoFilepath)), new SqlParameter("@PreviewFilepath", SqlHelper.ToDBValue(resourceCourse.PreviewFilepath)), new SqlParameter("@FileExt", SqlHelper.ToDBValue(resourceCourse.FileExt)), new SqlParameter("@SortId", SqlHelper.ToDBValue(resourceCourse.SortId)), new SqlParameter("@IssueTime", SqlHelper.ToDBValue(resourceCourse.IssueTime)), new SqlParameter("@DbStatus", SqlHelper.ToDBValue(resourceCourse.DbStatus)), new SqlParameter("@Tag2", SqlHelper.ToDBValue(resourceCourse.Tag2)), new SqlParameter("@IsTop", SqlHelper.ToDBValue(resourceCourse.IsTop)), new SqlParameter("@Is211", SqlHelper.ToDBValue(resourceCourse.Is211)), new SqlParameter("@ModifyTime", SqlHelper.ToDBValue(resourceCourse.ModifyTime)), new SqlParameter("@CreateTime", SqlHelper.ToDBValue(resourceCourse.CreateTime)), new SqlParameter("@IsDeleted", SqlHelper.ToDBValue(resourceCourse.IsDeleted)), new SqlParameter("@tag1", SqlHelper.ToDBValue(resourceCourse.Tag1)), new SqlParameter("@tag1_uptime", SqlHelper.ToDBValue(resourceCourse.Tag1_uptime)) };
			if (SqlHelper.ExecuteNonQuery(sql, sqlParameter) > 0)
			{
				resourceCourse1 = resourceCourse;
			}
			else
			{
				resourceCourse1 = null;
			}
			return resourceCourse1;
		}

		public int Delete(Guid id)
		{
			string sql = "DELETE ResourceCourse WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}

		public int GetCount()
		{
			return (int)SqlHelper.ExecuteScalar("SELECT COUNT(Id) FROM ResourceCourse WHERE IsDeleted = 0", new SqlParameter[0]);
		}

		public ResourceCourse GetEntity(Guid id)
		{
			string sql = "SELECT * FROM ResourceCourse WHERE Id = @Id AND IsDeleted = 0";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteEntity<ResourceCourse>(sql, sqlParameter);
		}

		public List<ResourceCourse> GetList()
		{
			return SqlHelper.ExecuteList<ResourceCourse>("SELECT * FROM ResourceCourse WHERE IsDeleted = 0", new SqlParameter[0]);
		}

		public List<ResourceCourse> GetPage(int pageIndex, int pageCount)
		{
			string sql = "SELECT * FROM(SELECT *,(ROW_NUMBER() OVER(ORDER BY Id)) rownum FROM ResourceCourse WHERE IsDeleted = 0) t WHERE rownum>=(@pageCount*(@pageIndex-1)+1) AND rownum<=@pageIndex*@pageCount";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@pageIndex", (object)pageIndex), new SqlParameter("@pageCount", (object)pageCount) };
			return SqlHelper.ExecuteList<ResourceCourse>(sql, sqlParameter);
		}

		public int SoftDelete(Guid id)
		{
			string sql = "UPDATE ResourceCourse SET IsDeleted = 1 WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}

		public int Update(ResourceCourse resourceCourse)
		{
			string sql = "UPDATE ResourceCourse SET Title = @Title,Size = @Size,Author = @Author,SchoolId = @SchoolId,PackId = @PackId,Gaozhi = @Gaozhi,KeyWords = @KeyWords,Fescribe = @Fescribe,Filepath = @Filepath,NewFilepath = @NewFilepath,DownloadFilepath = @DownloadFilepath,IcoFilepath = @IcoFilepath,PreviewFilepath = @PreviewFilepath,FileExt = @FileExt,SortId = @SortId,IssueTime = @IssueTime,DbStatus = @DbStatus,Tag2 = @Tag2,IsTop = @IsTop,Is211 = @Is211,ModifyTime = @ModifyTime,CreateTime = @CreateTime,IsDeleted = @IsDeleted,tag1 = @tag1,tag1_uptime = @tag1_uptime WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)resourceCourse.Id), new SqlParameter("@Title", SqlHelper.ToDBValue(resourceCourse.Title)), new SqlParameter("@Size", SqlHelper.ToDBValue(resourceCourse.Size)), new SqlParameter("@Author", SqlHelper.ToDBValue(resourceCourse.Author)), new SqlParameter("@SchoolId", SqlHelper.ToDBValue(resourceCourse.SchoolId)), new SqlParameter("@PackId", SqlHelper.ToDBValue(resourceCourse.PackId)), new SqlParameter("@Gaozhi", SqlHelper.ToDBValue(resourceCourse.Gaozhi)), new SqlParameter("@KeyWords", SqlHelper.ToDBValue(resourceCourse.KeyWords)), new SqlParameter("@Fescribe", SqlHelper.ToDBValue(resourceCourse.Fescribe)), new SqlParameter("@Filepath", SqlHelper.ToDBValue(resourceCourse.Filepath)), new SqlParameter("@NewFilepath", SqlHelper.ToDBValue(resourceCourse.NewFilepath)), new SqlParameter("@DownloadFilepath", SqlHelper.ToDBValue(resourceCourse.DownloadFilepath)), new SqlParameter("@IcoFilepath", SqlHelper.ToDBValue(resourceCourse.IcoFilepath)), new SqlParameter("@PreviewFilepath", SqlHelper.ToDBValue(resourceCourse.PreviewFilepath)), new SqlParameter("@FileExt", SqlHelper.ToDBValue(resourceCourse.FileExt)), new SqlParameter("@SortId", SqlHelper.ToDBValue(resourceCourse.SortId)), new SqlParameter("@IssueTime", SqlHelper.ToDBValue(resourceCourse.IssueTime)), new SqlParameter("@DbStatus", SqlHelper.ToDBValue(resourceCourse.DbStatus)), new SqlParameter("@Tag2", SqlHelper.ToDBValue(resourceCourse.Tag2)), new SqlParameter("@IsTop", SqlHelper.ToDBValue(resourceCourse.IsTop)), new SqlParameter("@Is211", SqlHelper.ToDBValue(resourceCourse.Is211)), new SqlParameter("@ModifyTime", SqlHelper.ToDBValue(resourceCourse.ModifyTime)), new SqlParameter("@CreateTime", SqlHelper.ToDBValue(resourceCourse.CreateTime)), new SqlParameter("@IsDeleted", SqlHelper.ToDBValue(resourceCourse.IsDeleted)), new SqlParameter("@tag1", SqlHelper.ToDBValue(resourceCourse.Tag1)), new SqlParameter("@tag1_uptime", SqlHelper.ToDBValue(resourceCourse.Tag1_uptime)) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}
	}
}