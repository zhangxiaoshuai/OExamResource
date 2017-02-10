using Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Providers
{
	public class PreviewProvider
	{
		private readonly static PreviewProvider InstanceObj;

		public static PreviewProvider Instance
		{
			get
			{
				return PreviewProvider.InstanceObj;
			}
		}

		static PreviewProvider()
		{
			PreviewProvider.InstanceObj = new PreviewProvider();
		}

		private PreviewProvider()
		{
		}

		public bool getCollectbool(Guid resourceid, Guid teacherid, int ku)
		{
			string sql = "select count(1) from collect where teacherid=@teacherid and resourceid=@resourceid and tag1=@ku";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@teacherid", (object)teacherid), new SqlParameter("@resourceid", (object)resourceid), new SqlParameter("@ku", (object)ku) };
			return (Convert.ToInt32(SqlHelper.ExecuteScalar(sql, sqlParameter)) > 0 ? true : false);
		}

		public List<Resourcelist> GetResourceEntity(Guid id)
		{
			string sql = "select  Id,Title,Size,Author,\r\nCategoryname=(select Name from category where id=r.CategoryId),\r\nSubjectname=(select Name from Subject where id=r.SubjectId),\r\nSpeciality=(select Name from Speciality where id=r.SpecialityId),\r\nTypeName=(select Name from ResourceType where id=r.TypeId),\r\nCoursename=(select Name from Course where id=r.CourseId),\r\nClickcount=(select count(1) from LogDetail where ResourceId=r.Id and Status=0 and Isdeleted=0),\r\nDowncount=(select count(1) from LogDetail where ResourceId=r.Id and Status=2 and Isdeleted=0),\r\nKeywords,Fescribe,DownloadFilepath,IcoFilepath,PreviewFilepath,Filepath=lower(substring(previewfilepath,charindex('.',previewfilepath)+1,3)),FileExt,IsTop from dbo.Resource as r where r.id=@id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@id", (object)id) };
			return SqlHelper.ExecuteList<Resourcelist>(sql, sqlParameter);
		}

		public string InCoursename(string CourseName, Guid Id)
		{
			string str;
			object[] id = new object[] { "select count(1) from dbo.Course where SourceId='", Id, "' and Name='", CourseName, "'" };
			if (Convert.ToInt32(SqlHelper.ExecuteScalar(string.Concat(id), new SqlParameter[0])) <= 0)
			{
				id = new object[] { "insert into dbo.Course values(newid(),'", CourseName, "','", Id, "',getdate(),0)" };
				string sql = string.Concat(id);
				SqlHelper.ExecuteNonQuery(sql, new SqlParameter[0]);
				str = "感谢您填写课程名称！";
			}
			else
			{
				str = "该课程名称已录入数据库中！";
			}
			return str;
		}

		public List<Resourcelist> Preview_list(Guid Glist, bool istop)
		{
			object obj = "select Id,Title,Size,Author,\r\nCategoryName=(select Name from category where id=r.CategoryId),\r\nSubjectName=(select Name from Subject where id=r.SubjectId),\r\nSpecialityName=(select Name from Speciality where id=r.SpecialityId),\r\nTypeName=(select Name from ResourceType where id=r.TypeId),\r\nCoursename=(select Name from Course where id=r.CourseId),\r\nClickcount=(select count(1) from LogDetail where ResourceId=r.Id and Status=0 and Isdeleted=0),\r\nDowncount=(select count(1) from LogDetail where ResourceId=r.Id and Status=2 and Isdeleted=0),\r\nKeywords,Fescribe,DownloadFilepath,IcoFilepath,PreviewFilepath,FileExt,IsTop from dbo.Resource as r where  DbStatus =0 and IsDeleted=0 and";
			object[] glist = new object[] { obj, "(id='", Glist, "' or Packid='", Glist, "')" };
			string sql = string.Concat(string.Concat(glist), " order by Title asc");
			return SqlHelper.ExecuteList<Resourcelist>(sql, new SqlParameter[0]);
		}

		public int Update_Resource_log(Guid resourceid, int status)
		{
			string sql = "update Resource_log set Count=Count+1 where Resourceid=@resourceid and status=@status";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@resourceid", (object)resourceid), new SqlParameter("@status", (object)status) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}
	}
}