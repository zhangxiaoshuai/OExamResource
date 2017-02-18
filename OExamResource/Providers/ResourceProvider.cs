using Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Providers
{
	public class ResourceProvider
	{
		private readonly static ResourceProvider InstanceObj;

		public static ResourceProvider Instance
		{
			get
			{
				return ResourceProvider.InstanceObj;
			}
		}

		static ResourceProvider()
		{
			ResourceProvider.InstanceObj = new ResourceProvider();
		}

		private ResourceProvider()
		{
		}

		public Resource Create(Resource resource)
		{
			Resource resource1;
			if (resource.Id == Guid.Empty)
			{
				resource.Id = Guid.NewGuid();
			}
			string sql = "INSERT INTO Resource (Id, Title, Size, Author, CategoryId, SubjectId, SpecialityId, KnowledgeId, SchoolId, PackId, TypeId, CourseId, BoutiqueGrade, VideoResolution, Sampling, VideoTime, KeyWords, Fescribe, Filepath, NewFilepath, DownloadFilepath, IcoFilepath, PreviewFilepath, FileExt, IssueTime, DbStatus, Tag2, IsTop, Is211, ModifyTime, CreateTime, IsDeleted)  VALUES (@Id, @Title, @Size, @Author, @CategoryId, @SubjectId, @SpecialityId, @KnowledgeId, @SchoolId, @PackId, @TypeId, @CourseId, @BoutiqueGrade, @VideoResolution, @Sampling, @Gaozhi, @KeyWords, @Fescribe, @Filepath, @NewFilepath, @DownloadFilepath, @IcoFilepath, @PreviewFilepath, @FileExt, @IssueTime, @DbStatus, @Tag2, @IsTop, @Is211, @ModifyTime, @CreateTime, @IsDeleted)";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", SqlHelper.ToDBValue(resource.Id)), new SqlParameter("@Title", SqlHelper.ToDBValue(resource.Title)), new SqlParameter("@Size", SqlHelper.ToDBValue(resource.Size)), new SqlParameter("@Author", SqlHelper.ToDBValue(resource.Author)), new SqlParameter("@CategoryId", SqlHelper.ToDBValue(resource.CategoryId)), new SqlParameter("@SubjectId", SqlHelper.ToDBValue(resource.SubjectId)), new SqlParameter("@SpecialityId", SqlHelper.ToDBValue(resource.SpecialityId)), new SqlParameter("@KnowledgeId", SqlHelper.ToDBValue(resource.KnowledgeId)), new SqlParameter("@SchoolId", SqlHelper.ToDBValue(resource.SchoolId)), new SqlParameter("@PackId", SqlHelper.ToDBValue(resource.PackId)), new SqlParameter("@TypeId", SqlHelper.ToDBValue(resource.TypeId)), new SqlParameter("@CourseId", SqlHelper.ToDBValue(resource.CourseId)), new SqlParameter("@BoutiqueGrade", SqlHelper.ToDBValue(resource.BoutiqueGrade)), new SqlParameter("@VideoResolution", SqlHelper.ToDBValue(resource.VideoResolution)), new SqlParameter("@Sampling", SqlHelper.ToDBValue(resource.Sampling)), new SqlParameter("@Gaozhi", SqlHelper.ToDBValue(resource.Gaozhi)), new SqlParameter("@KeyWords", SqlHelper.ToDBValue(resource.KeyWords)), new SqlParameter("@Fescribe", SqlHelper.ToDBValue(resource.Fescribe)), new SqlParameter("@Filepath", SqlHelper.ToDBValue(resource.Filepath)), new SqlParameter("@NewFilepath", SqlHelper.ToDBValue(resource.NewFilepath)), new SqlParameter("@DownloadFilepath", SqlHelper.ToDBValue(resource.DownloadFilepath)), new SqlParameter("@IcoFilepath", SqlHelper.ToDBValue(resource.IcoFilepath)), new SqlParameter("@PreviewFilepath", SqlHelper.ToDBValue(resource.PreviewFilepath)), new SqlParameter("@FileExt", SqlHelper.ToDBValue(resource.FileExt)), new SqlParameter("@IssueTime", SqlHelper.ToDBValue(resource.IssueTime)), new SqlParameter("@DbStatus", SqlHelper.ToDBValue(resource.DbStatus)), new SqlParameter("@Tag2", SqlHelper.ToDBValue(resource.Tag2)), new SqlParameter("@IsTop", SqlHelper.ToDBValue(resource.IsTop)), new SqlParameter("@Is211", SqlHelper.ToDBValue(resource.Is211)), new SqlParameter("@ModifyTime", SqlHelper.ToDBValue(resource.ModifyTime)), new SqlParameter("@CreateTime", SqlHelper.ToDBValue(resource.CreateTime)), new SqlParameter("@IsDeleted", SqlHelper.ToDBValue(resource.IsDeleted)) };
			if (SqlHelper.ExecuteNonQuery(sql, sqlParameter) > 0)
			{
				resource1 = resource;
			}
			else
			{
				resource1 = null;
			}
			return resource1;
		}

		public int Delete(Guid id)
		{
			string sql = "DELETE Resource WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}

		public List<Resourcelist> GetCateCountList()
		{
			return SqlHelper.ExecuteList<Resourcelist>("select a.id as Id,a.Name as CategoryName,\r\n(select count(1) from dbo.ResourceMaterial where ResourceMaterial.CategoryId in\r\n(select b.id from ResourceCategory as b where b.parentid in \r\n(select c.id from ResourceCategory as c where c.parentid=a.id))\r\n) as Size,\r\n(select cast(sum(cast([Size] as float)) as nvarchar(50)) from dbo.ResourceMaterial where ResourceMaterial.CategoryId in \r\n(select b.id from ResourceCategory as b where b.parentid in \r\n(select c.id from ResourceCategory as c where c.parentid=a.id))) as Tag1\r\n from  ResourceCategory as a where  a.tier=0", new SqlParameter[0]);
		}

		public int GetCount(Guid schoolid, Guid cateid, Guid typeid, string name, int isdeleted)
		{
			string sql = string.Concat("SELECT COUNT(Id) FROM ResourceMaterial WHERE IsDeleted = ", isdeleted);
			if (schoolid != Guid.Empty)
			{
				sql = string.Concat(sql, " AND ResourceMaterial.SchoolId = @schoolid ");
			}
			if (cateid != Guid.Empty)
			{
				sql = string.Concat(sql, " AND ResourceMaterial.CategoryId = @cateid ");
			}
			if (typeid != Guid.Empty)
			{
				sql = string.Concat(sql, " AND ResourceMaterial.TypeId = @typeid ");
			}
			if (!string.IsNullOrEmpty(name))
			{
				sql = string.Concat(sql, " AND (ResourceMaterial.Title LIKE @name OR ResourceMaterial.KeyWords LIKE @name) ");
			}
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@schoolid", (object)schoolid), new SqlParameter("@cateid", (object)cateid), new SqlParameter("@typeid", (object)typeid), new SqlParameter("@name", string.Concat("%", name, "%")) };
			return (int)SqlHelper.ExecuteScalar(sql, sqlParameter);
		}

		public int GetCount()
		{
			return (int)SqlHelper.ExecuteScalar("SELECT COUNT(Id) FROM Resource WHERE IsDeleted = 0", new SqlParameter[0]);
		}

		public int GetCountByCategory(Guid id)
		{
			string sql = "SELECT COUNT(Id) FROM Resource WHERE CategoryId = @CategoryId ";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@CategoryId", (object)id) };
			return (int)SqlHelper.ExecuteScalar(sql, sqlParameter);
		}

		public int GetCountByKnowledge(Guid id)
		{
			string sql = "SELECT COUNT(Id) FROM Resource WHERE KnowledgeId = @KnowledgeId ";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@KnowledgeId", (object)id) };
			return (int)SqlHelper.ExecuteScalar(sql, sqlParameter);
		}

		public int GetCountBySpeciality(Guid id)
		{
			string sql = "SELECT COUNT(Id) FROM Resource WHERE SpecialityId = @SpecialityId ";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@SpecialityId", (object)id) };
			return (int)SqlHelper.ExecuteScalar(sql, sqlParameter);
		}

		public int GetCountBySubject(Guid id)
		{
			string sql = "SELECT COUNT(Id) FROM Resource WHERE SubjectId = @SubjectId ";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@SubjectId", (object)id) };
			return (int)SqlHelper.ExecuteScalar(sql, sqlParameter);
		}

		public int GetCourceCount(Guid schoolid, Guid cateid, Guid typeid, string name, int isdeleted)
		{
			string sql = string.Concat("SELECT COUNT(Id) FROM ResourceCourse WHERE IsDeleted = ", isdeleted);
			if (schoolid != Guid.Empty)
			{
				sql = string.Concat(sql, " AND ResourceCourse.SchoolId = @schoolid ");
			}
			if (cateid != Guid.Empty)
			{
				sql = string.Concat(sql, " AND ResourceCourse.packid in (select id from resourcepack where CategoryId=@cateid ) ");
			}
			if (typeid != Guid.Empty)
			{
				sql = string.Concat(sql, " AND ResourceCourse.packid in (select id from resourcepack where typeid=@typeid)");
			}
			if (!string.IsNullOrEmpty(name))
			{
				sql = string.Concat(sql, " AND (ResourceCourse.Title LIKE @name OR ResourceCourse.KeyWords LIKE @name) ");
			}
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@schoolid", (object)schoolid), new SqlParameter("@cateid", (object)cateid), new SqlParameter("@typeid", (object)typeid), new SqlParameter("@name", string.Concat("%", name, "%")) };
			return (int)SqlHelper.ExecuteScalar(sql, sqlParameter);
		}

		public List<Resourcelist> GetCourseCateCountList()
		{
			return SqlHelper.ExecuteList<Resourcelist>("select a.id as Id,a.Name as CategoryName,\r\n(select count(1) from dbo.ResourceCourse where ResourceCourse.packid in\r\n(select id from Resourcepack where CategoryId in (\r\nselect b.id from ResourceCategory as b where b.parentid in \r\n(select c.id from ResourceCategory as c where c.parentid=a.id)\r\n))) as Size,\r\n(select cast(sum(cast([Size] as float)) as nvarchar(50)) from dbo.ResourceCourse where ResourceCourse.packid in \r\n(select id from Resourcepack where CategoryId in (\r\nselect b.id from ResourceCategory as b where b.parentid in \r\n(select c.id from ResourceCategory as c where c.parentid=a.id)\r\n))) as Tag1\r\n from  ResourceCategory as a where  a.tier=0", new SqlParameter[0]);
		}

		public Resource GetEntity(Guid id)
		{
			string sql = "SELECT * FROM Resource WHERE Id = @Id AND IsDeleted = 0";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteEntity<Resource>(sql, sqlParameter);
		}

		public List<Resourcelist> GetList(Guid schoolid, Guid csid, Guid specialityid, Guid typeid, string name, int pageIndex, int pageCount, int isdeleted)
		{
			string sql = string.Concat("select * from (SELECT a.Id\r\n      ,a.Title\r\n      ,a.Size,\r\n(select b.name from dbo.ResourceCategory as b where b.id =\r\n(select  c.ParentId from ResourceCategory as c where c.id=(\r\nselect d.parentid from dbo.ResourceCategory as d where d.id=a.CategoryId\r\n)))as CategoryName\r\n      ,(select b.name from dbo.ResourceCategory as b where b.id =\r\n(select  c.ParentId from ResourceCategory as c where c.id=a.CategoryId))as SubjectName,\r\n(select b.name from dbo.ResourceCategory as b where b.id =a.CategoryId)as SpecialityName\r\n      ,a.SchoolId\r\n      ,(select name from dbo.ResourceType where id=a.typeid) as TypeName \r\n      ,a.SortId \r\n      ,a.CreateTime\r\n      ,a.IsDeleted,\r\nROW_NUMBER() OVER(ORDER BY a.CreateTime DESC) rownum \r\n  FROM  ResourceMaterial as a                                      \r\n\t\t                    WHERE a.Isdeleted =", isdeleted);
			if (schoolid != Guid.Empty)
			{
				sql = string.Concat(sql, " AND (a.SchoolId = @schoolid OR a.SchoolId IS NULL )");
			}
			if (specialityid != Guid.Empty)
			{
				sql = string.Concat(sql, " AND a.CategoryId = @specialityid ");
			}
			if (typeid != Guid.Empty)
			{
				sql = string.Concat(sql, " AND a.TypeId = @typeid ");
			}
			if (!string.IsNullOrEmpty(name))
			{
				sql = string.Concat(sql, " AND (a.Title LIKE @name OR a.KeyWords LIKE @name) ");
			}
			sql = string.Concat(sql, " ) as t\r\n                      WHERE rownum>=(@pageCount*(@pageIndex-1)+1) AND rownum<=@pageIndex*@pageCount");
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@pageIndex", (object)pageIndex), new SqlParameter("@pageCount", (object)pageCount), new SqlParameter("@schoolid", (object)schoolid), new SqlParameter("@specialityid", (object)specialityid), new SqlParameter("@typeid", (object)typeid), new SqlParameter("@name", string.Concat("%", name, "%")) };
			return SqlHelper.ExecuteList<Resourcelist>(sql, sqlParameter);
		}

		public List<Resource> GetList()
		{
			return SqlHelper.ExecuteList<Resource>("SELECT * FROM Resource WHERE IsDeleted = 0", new SqlParameter[0]);
		}

		public List<Resource> GetPage(int pageIndex, int pageCount)
		{
			string sql = "SELECT * FROM(SELECT *,(ROW_NUMBER() OVER(ORDER BY Id)) rownum FROM Resource WHERE IsDeleted = 0) t WHERE rownum>=(@pageCount*(@pageIndex-1)+1) AND rownum<=@pageIndex*@pageCount";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@pageIndex", (object)pageIndex), new SqlParameter("@pageCount", (object)pageCount) };
			return SqlHelper.ExecuteList<Resource>(sql, sqlParameter);
		}

		public Resource GetResource(Guid id)
		{
			string sql = "SELECT * FROM Resource WHERE Id = @Id ";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteEntity<Resource>(sql, sqlParameter);
		}

		public ResourceCourse GetResourceCourse(Guid id)
		{
			string sql = "SELECT * FROM ResourceCourse WHERE Id = @Id ";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteEntity<ResourceCourse>(sql, sqlParameter);
		}

		public List<Resourcelist> GetResourceCourseList(Guid schoolid, Guid csid, Guid specialityid, Guid typeid, string name, int pageIndex, int pageCount, int isdeleted)
		{
			string sql = string.Concat("select * from (SELECT a.Id\r\n      ,a.Title\r\n      ,a.Size,(select name from resourcepack where id=a.packid) as Packname,Packid,\r\n(select b.name from dbo.ResourceCategory as b where b.id =\r\n(select  c.ParentId from ResourceCategory as c where c.id=(\r\nselect d.parentid from dbo.ResourceCategory as d where d.id=(select CategoryId from resourcepack where id=a.packid)\r\n)))as CategoryName\r\n      ,(select b.name from dbo.ResourceCategory as b where b.id =\r\n(select  c.ParentId from ResourceCategory as c where c.id=(select CategoryId from resourcepack where id=a.packid)))as SubjectName,\r\n(select b.name from dbo.ResourceCategory as b where b.id =(select CategoryId from resourcepack where id=a.packid))as SpecialityName\r\n      ,a.SchoolId\r\n      ,(select name from dbo.ResourceType where id=\r\n(select typeid from dbo.ResourcePack where id=a.packid)) as TypeName \r\n      ,a.SortId \r\n      ,a.CreateTime\r\n      ,a.IsDeleted,\r\nROW_NUMBER() OVER(ORDER BY a.CreateTime DESC,packid) rownum \r\n  FROM  ResourceCourse as a   WHERE a.Isdeleted =", isdeleted);
			if (schoolid != Guid.Empty)
			{
				sql = string.Concat(sql, " AND (a.SchoolId = @schoolid OR a.SchoolId IS NULL )");
			}
			if (specialityid != Guid.Empty)
			{
				sql = string.Concat(sql, " AND a.packid in (select id from resourcepack where CategoryId = @CategoryId )");
			}
			if (typeid != Guid.Empty)
			{
				sql = string.Concat(sql, " AND a.packid in (select id from resourcepack where typeid= @typeid )");
			}
			if (!string.IsNullOrEmpty(name))
			{
				sql = string.Concat(sql, " AND (a.Title LIKE @name OR a.KeyWords LIKE @name) ");
			}
			sql = string.Concat(sql, " ) as t\r\n                      WHERE rownum>=(@pageCount*(@pageIndex-1)+1) AND rownum<=@pageIndex*@pageCount order by rownum");
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@pageIndex", (object)pageIndex), new SqlParameter("@pageCount", (object)pageCount), new SqlParameter("@schoolid", (object)schoolid), new SqlParameter("@CategoryId", (object)specialityid), new SqlParameter("@typeid", (object)typeid), new SqlParameter("@name", string.Concat("%", name, "%")) };
			return SqlHelper.ExecuteList<Resourcelist>(sql, sqlParameter);
		}

		public ResourceMaterial GetResourceMaterial(Guid id)
		{
			string sql = "SELECT * FROM ResourceMaterial WHERE Id = @Id ";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteEntity<ResourceMaterial>(sql, sqlParameter);
		}

		public List<Resourcelist> GetResourceMaterialList(Guid schoolid, Guid csid, Guid specialityid, Guid typeid, string name, int pageIndex, int pageCount, int isdeleted)
		{
			string sql = string.Concat("select * from (SELECT a.Id\r\n      ,a.Title\r\n      ,a.Size,\r\n(select b.name from dbo.ResourceCategory as b where b.id =\r\n(select  c.ParentId from ResourceCategory as c where c.id=(\r\nselect d.parentid from dbo.ResourceCategory as d where d.id=a.CategoryId\r\n)))as CategoryName\r\n      ,(select b.name from dbo.ResourceCategory as b where b.id =\r\n(select  c.ParentId from ResourceCategory as c where c.id=a.CategoryId))as SubjectName,\r\n(select b.name from dbo.ResourceCategory as b where b.id =a.CategoryId)as SpecialityName\r\n      ,a.SchoolId\r\n      ,(select name from dbo.ResourceType where id=a.typeid) as TypeName \r\n      ,a.SortId \r\n      ,a.CreateTime\r\n      ,a.IsDeleted,\r\nROW_NUMBER() OVER(ORDER BY a.CreateTime DESC) rownum \r\n  FROM  ResourceMaterial as a                               \r\n\t\t                    WHERE a.Isdeleted =", isdeleted);
			if (schoolid != Guid.Empty)
			{
				sql = string.Concat(sql, " AND (a.SchoolId = @schoolid OR a.SchoolId IS NULL )");
			}
			if (specialityid != Guid.Empty)
			{
				sql = string.Concat(sql, " AND a.CategoryId = @CategoryId ");
			}
			if (typeid != Guid.Empty)
			{
				sql = string.Concat(sql, " AND a.TypeId = @typeid ");
			}
			if (!string.IsNullOrEmpty(name))
			{
				sql = string.Concat(sql, " AND (a.Title LIKE @name OR a.KeyWords LIKE @name) ");
			}
			sql = string.Concat(sql, " ) as t\r\n                      WHERE rownum>=(@pageCount*(@pageIndex-1)+1) AND rownum<=@pageIndex*@pageCount order by rownum ");
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@pageIndex", (object)pageIndex), new SqlParameter("@pageCount", (object)pageCount), new SqlParameter("@schoolid", (object)schoolid), new SqlParameter("@CategoryId", (object)specialityid), new SqlParameter("@typeid", (object)typeid), new SqlParameter("@name", string.Concat("%", name, "%")) };
			return SqlHelper.ExecuteList<Resourcelist>(sql, sqlParameter);
		}

		public List<Resourcelist> GetSubCountList()
		{
			return SqlHelper.ExecuteList<Resourcelist>("select a.id as Id,(select name from ResourceCategory where id=a.parentid)as CategoryName,a.Name as SubjectName,\r\n(select count(1) from dbo.ResourceCourse where ResourceCourse.packid in\r\n(select id from resourcepack where CategoryId in (select c.id from ResourceCategory as c where c.parentid=a.id\r\n)))as Size,\r\n(select cast(sum(cast([Size] as float)) as nvarchar(50)) from dbo.ResourceCourse where ResourceCourse.packid in\r\n(select id from resourcepack where CategoryId in (select c.id from ResourceCategory as c where c.parentid=a.id\r\n))) as Tag1\r\n from  ResourceCategory as a where  a.tier=1", new SqlParameter[0]);
		}

		public int SoftDelete(Guid id)
		{
			string sql = "UPDATE Resource SET IsDeleted = 1 WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}

		public int Update(Resource resource)
		{
			string sql = "UPDATE Resource SET Title = @Title,Size = @Size,Author = @Author,CategoryId = @CategoryId,SubjectId = @SubjectId,SpecialityId = @SpecialityId,KnowledgeId = @KnowledgeId,SchoolId = @SchoolId,PackId = @PackId,TypeId = @TypeId,CourseId = @CourseId,BoutiqueGrade = @BoutiqueGrade,VideoResolution = @VideoResolution,Sampling = @Sampling,Gaozhi = @Gaozhi,KeyWords = @KeyWords,Fescribe = @Fescribe,Filepath = @Filepath,NewFilepath = @NewFilepath,DownloadFilepath = @DownloadFilepath,IcoFilepath = @IcoFilepath,PreviewFilepath = @PreviewFilepath,FileExt = @FileExt,IssueTime = @IssueTime,DbStatus = @DbStatus,Tag2 = @Tag2,IsTop = @IsTop,Is211 = @Is211,ModifyTime = @ModifyTime,CreateTime = @CreateTime,IsDeleted = @IsDeleted WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)resource.Id), new SqlParameter("@Title", SqlHelper.ToDBValue(resource.Title)), new SqlParameter("@Size", SqlHelper.ToDBValue(resource.Size)), new SqlParameter("@Author", SqlHelper.ToDBValue(resource.Author)), new SqlParameter("@CategoryId", SqlHelper.ToDBValue(resource.CategoryId)), new SqlParameter("@SubjectId", SqlHelper.ToDBValue(resource.SubjectId)), new SqlParameter("@SpecialityId", SqlHelper.ToDBValue(resource.SpecialityId)), new SqlParameter("@KnowledgeId", SqlHelper.ToDBValue(resource.KnowledgeId)), new SqlParameter("@SchoolId", SqlHelper.ToDBValue(resource.SchoolId)), new SqlParameter("@PackId", SqlHelper.ToDBValue(resource.PackId)), new SqlParameter("@TypeId", SqlHelper.ToDBValue(resource.TypeId)), new SqlParameter("@CourseId", SqlHelper.ToDBValue(resource.CourseId)), new SqlParameter("@BoutiqueGrade", SqlHelper.ToDBValue(resource.BoutiqueGrade)), new SqlParameter("@VideoResolution", SqlHelper.ToDBValue(resource.VideoResolution)), new SqlParameter("@Sampling", SqlHelper.ToDBValue(resource.Sampling)), new SqlParameter("@Gaozhi", SqlHelper.ToDBValue(resource.Gaozhi)), new SqlParameter("@KeyWords", SqlHelper.ToDBValue(resource.KeyWords)), new SqlParameter("@Fescribe", SqlHelper.ToDBValue(resource.Fescribe)), new SqlParameter("@Filepath", SqlHelper.ToDBValue(resource.Filepath)), new SqlParameter("@NewFilepath", SqlHelper.ToDBValue(resource.NewFilepath)), new SqlParameter("@DownloadFilepath", SqlHelper.ToDBValue(resource.DownloadFilepath)), new SqlParameter("@IcoFilepath", SqlHelper.ToDBValue(resource.IcoFilepath)), new SqlParameter("@PreviewFilepath", SqlHelper.ToDBValue(resource.PreviewFilepath)), new SqlParameter("@FileExt", SqlHelper.ToDBValue(resource.FileExt)), new SqlParameter("@IssueTime", SqlHelper.ToDBValue(resource.IssueTime)), new SqlParameter("@DbStatus", SqlHelper.ToDBValue(resource.DbStatus)), new SqlParameter("@Tag2", SqlHelper.ToDBValue(resource.Tag2)), new SqlParameter("@IsTop", SqlHelper.ToDBValue(resource.IsTop)), new SqlParameter("@Is211", SqlHelper.ToDBValue(resource.Is211)), new SqlParameter("@ModifyTime", SqlHelper.ToDBValue(resource.ModifyTime)), new SqlParameter("@CreateTime", SqlHelper.ToDBValue(resource.CreateTime)), new SqlParameter("@IsDeleted", SqlHelper.ToDBValue(resource.IsDeleted)) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}
	}
}