using Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Providers
{
	public class Resource_SubjectProvider
	{
		private readonly static Resource_SubjectProvider InstanceObj;

		public static Resource_SubjectProvider Instance
		{
			get
			{
				return Resource_SubjectProvider.InstanceObj;
			}
		}

		static Resource_SubjectProvider()
		{
			Resource_SubjectProvider.InstanceObj = new Resource_SubjectProvider();
		}

		private Resource_SubjectProvider()
		{
		}

		public void Clickcount(List<Guids> Glist)
		{
			string sql = "";
			for (int i = 0; i < Glist.Count; i++)
			{
				if (Convert.ToInt32(SqlHelper.ExecuteScalar(string.Concat("select count(1) from Resource_Log where ResourceId='", Glist[i].Id, "'"), new SqlParameter[0])) <= 0)
				{
					sql = string.Concat("insert into Resource_Log values(newid(),'", Glist[i].Id, "',1,getdate(),0)");
					SqlHelper.ExecuteNonQuery(sql, new SqlParameter[0]);
				}
				else
				{
					sql = string.Concat("update Resource_Log set Count=Count+1,CreateTime=getdate() where ResourceId='", Glist[i].Id, "'");
					SqlHelper.ExecuteNonQuery(sql, new SqlParameter[0]);
				}
			}
		}

		public List<CollectSource> Collect(Guid? id, int medicalTag, int pageIndex, int pageCount)
		{
			string sql = "\r\nselect Id,Title,Subjectname,Speciality,Time,Packid,IcoFilepath,PreviewFilepath,DownloadFilepath from(\r\nselect Id,Title,Subjectname,Speciality,Time,Packid,IcoFilepath,PreviewFilepath,DownloadFilepath,(ROW_NUMBER() OVER(ORDER BY SortId)) rownum from(\r\nselect Id,Title,\r\nSubjectname=(select Name from ResourceCategory where Id=(select ParentId from ResourceCategory where id=Resourcematerial.CategoryId)),\r\nSpeciality=(select Name from ResourceCategory where Id=Resourcematerial.CategoryId),\r\nTime=(select CreateTime from Logdetail where logdetail.resourceid=resourcematerial.id and status=1 and isdeleted=0 \r\nand teacherid=@teacherid),'00000000-0000-0000-0000-000000000000' as Packid,\r\nIcoFilepath,PreviewFilepath,DownloadFilepath,SortId\r\nfrom resourcematerial where id in (select resourceid from logdetail where \r\nteacherid=@teacherid and Status=1 and isdeleted=0 {0}) \r\nunion all\r\nselect r.Id,title,\r\nSubjectname=(select Name from ResourceCategory where Id=(select ParentId from ResourceCategory where id=(select CategoryId from resourcepack where id=r.packid))),\r\nSpeciality=(select Name from ResourceCategory where Id=(select CategoryId from resourcepack where id=r.packid)),\r\nTime=(select CreateTime from Logdetail where logdetail.resourceid=r.id and status=1 and isdeleted=0 \r\nand teacherid=@teacherid),Packid,\r\nIcoFilepath,PreviewFilepath,DownloadFilepath,SortId\r\nfrom resourcecourse as r where r.id in (select resourceid from logdetail where \r\nteacherid=@teacherid and Status=1 and isdeleted=0  {0})\r\n) temp)t\r\nWHERE rownum>=(@pageCount*(@pageIndex-1)+1) AND rownum<=@pageCount*@pageIndex";
			List<SqlParameter> par = new List<SqlParameter>()
			{
				new SqlParameter("@teacherid", (object)id),
				new SqlParameter("@pageCount", (object)pageCount),
				new SqlParameter("@pageIndex", (object)pageIndex)
			};
			if (medicalTag == 1)
			{
				sql = string.Format(sql, " and Tag1=1");
			}
			else if (medicalTag == 0)
			{
				sql = string.Format(sql, " and Tag1=0 and Medical_Tag=@medicalTag");
				par.Add(new SqlParameter("@medicalTag", (object)medicalTag));
			}
			return SqlHelper.ExecuteList<CollectSource>(sql, par.ToArray());
		}

		public int CollectCount(Guid teacherid, int ku)
		{
			string sql = "select count(1) from LogDetail where TeacherId=@teacherid and Status=1 and IsDeleted=0 and tag1=@ku";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@teacherid", (object)teacherid), new SqlParameter("@ku", (object)ku) };
			return Convert.ToInt32(SqlHelper.ExecuteScalar(sql, sqlParameter));
		}

		public bool Collectdeleted(Guid teacherid, Guid resourceid)
		{
			bool flag;
			string sql = "update LogDetail set isdeleted=1 where ResourceId=@resourceid and Status=1 and tag1=1 and TeacherId=@teacherid";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@resourceid", (object)resourceid), new SqlParameter("@teacherid", (object)teacherid) };
			flag = (SqlHelper.ExecuteNonQuery(sql, sqlParameter) <= 0 ? false : true);
			return flag;
		}

		public void Downlog(Guid resourceid, Guid teacherid, Guid schoolid, string ip, int ku)
		{
			object[] objArray = new object[] { "insert into  LogDetail values(newid(),'", resourceid, "','", teacherid, "','", schoolid, "','", ip, "',", 2, ",'", ku, "',null,getdate(),0)" };
			string sql = string.Concat(objArray);
			SqlHelper.ExecuteNonQuery(sql, new SqlParameter[0]);
		}

		public List<Category> GetCategoryCount(string searchname)
		{
			string sql = string.Concat("select Id,Name,Ico,SortId,Count=(select count(1) from Resource where", " CategoryId=category.id and (title like @searchname  or keywords like @searchname  )");
			sql = string.Concat(sql, "),CreateTime,IsDeleted from Category where isdeleted=0");
			List<SqlParameter> par = new List<SqlParameter>()
			{
				new SqlParameter("@searchname", string.Concat("%", searchname, "%"))
			};
			return SqlHelper.ExecuteList<Category>(sql, par.ToArray());
		}

		public Category GetCategoryEntity(string searchname, Guid id)
		{
			string sql = string.Concat("select Id,Name,Ico,Count=(select count(1) from Resource where", " CategoryId=category.id and (title like @searchname   or keywords like @searchname  )");
			sql = string.Concat(sql, " and isdeleted=0),CreateTime,IsDeleted from Category where Id=@id");
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@id", (object)id), new SqlParameter("@searchname", string.Concat("%", searchname, "%")) };
			return SqlHelper.ExecuteEntity<Category>(sql, sqlParameter);
		}

		public List<Guids> Getclicksourceid(Guid lists)
		{
			string sql = "select Id from Resource where Id =@id or PackId=@id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@id", (object)lists) };
			return SqlHelper.ExecuteList<Guids>(sql, sqlParameter);
		}

		public Guid GetCourseId(string Coursename, Guid speid)
		{
			Guid Courceid = new Guid();
			if (Convert.ToInt32(SqlHelper.ExecuteScalar(string.Concat("select count(1) from Course where Name='", Coursename, "'"), new SqlParameter[0])) <= 0)
			{
				Courceid = Guid.NewGuid();
				object[] coursename = new object[] { "insert into Course values('", Courceid, "','", Coursename, "','", speid, "',getdate(),0)" };
				if (SqlHelper.ExecuteNonQuery(string.Concat(coursename), new SqlParameter[0]) <= 0)
				{
					Courceid = new Guid();
				}
			}
			else
			{
				string sql = string.Concat("select id from Course where name='", Coursename, "'");
				Courceid = (Guid)SqlHelper.ExecuteScalar(sql, new SqlParameter[0]);
			}
			return Courceid;
		}

		public bool Getjiucuo(Guid id, Guid teachid, int ku)
		{
			bool state = false;
			object[] objArray = new object[] { "select count(1) from Correct where Resource_Id='", id, "' and Teach_Id='", teachid, "' and tag2=", ku };
			if (Convert.ToInt32(SqlHelper.ExecuteScalar(string.Concat(objArray), new SqlParameter[0])) <= 0)
			{
				objArray = new object[] { "insert into Correct values(newid(),'", id, "','", teachid, "',1,", ku, ",getdate(),0)" };
				if (SqlHelper.ExecuteNonQuery(string.Concat(objArray), new SqlParameter[0]) > 0)
				{
					state = true;
				}
			}
			else
			{
				objArray = new object[] { "update Correct set Count=Count+1 where Resource_Id='", id, "'  and Teach_Id='", teachid, "' and tag2=", ku };
				if (SqlHelper.ExecuteNonQuery(string.Concat(objArray), new SqlParameter[0]) > 0)
				{
					state = true;
				}
			}
			return state;
		}

		public List<Resource_type> GetList(string searchname, Guid id, int type)
		{
			string sql = "select Id,Name,SortId,BaseType,Count=(SELECT COUNT(Id) from Resource where TypeId =ResourceType.id and DbStatus=0 and IsDeleted=0 and (title like @searchname or keywords like @searchname)";
			switch (type)
			{
				case 0:
				{
					sql = string.Concat(sql, "and CategoryId=@id");
					break;
				}
				case 1:
				{
					sql = string.Concat(sql, "and SubjectId=@id");
					break;
				}
				case 2:
				{
					sql = string.Concat(sql, "and SpecialityId=@id");
					break;
				}
			}
			sql = string.Concat(sql, ") from dbo.ResourceType where IsDeleted=0 order by SortId ASC");
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@id", (object)id), new SqlParameter("@searchname", string.Concat("%", searchname, "%")) };
			return SqlHelper.ExecuteList<Resource_type>(sql, sqlParameter);
		}

		public List<ResourceType> GetList()
		{
			return SqlHelper.ExecuteList<ResourceType>("SELECT * FROM ResourceType WHERE IsDeleted = 0 ", new SqlParameter[0]);
		}

		public List<Resource> GetResource(string searchname, Guid id, int type, Guid typeid, int pageindex, int pagecount)
		{
			string sql = "select * from ( \r\nselect ROW_NUMBER() OVER(ORDER BY case when title like @searchname then 0\r\nelse 1 end, Tag2 asc,IsTop desc ,createtime desc,tag1_uptime desc,Title asc,Gaozhi asc) as rownum\r\n      ,[Id]\r\n      ,[Title]\r\n      ,[CategoryId]\r\n      ,[SubjectId]\r\n      ,[SpecialityId]\r\n      ,[KnowledgeId] \r\n,[PackId]\r\n      ,[IcoFilepath]  \r\n      ,[IsTop]\r\n,[FileExt]\r\n,[CreateTime]\r\n      ,[IsDeleted]\r\n      ,[tag1],Gaozhi,tag1_uptime,\r\nTag2=cast((select [SourceSortId] from ResourceType where Id=Resource.TypeId) as nvarchar(50))\r\nfrom Resource where IsDeleted=0 and TypeId=@typeid and (Title like  @searchname  or keywords like @searchname) ";
			if (type == 0)
			{
				sql = string.Concat(sql, " and CategoryId=@id ");
			}
			else if (type == 1)
			{
				sql = string.Concat(sql, " and SubjectId=@id ");
			}
			else if (type == 2)
			{
				sql = string.Concat(sql, " and SpecialityId=@id ");
			}
			sql = string.Concat(sql, ") as t WHERE rownum>=(@pageCount*(@pageIndex-1)+1) AND rownum<=@pageIndex*@pageCount ");
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@pageindex", (object)pageindex), new SqlParameter("@pagecount", (object)pagecount), new SqlParameter("@typeid", (object)typeid), new SqlParameter("@id", (object)id), new SqlParameter("@searchname", string.Concat("%", searchname, "%")) };
			return SqlHelper.ExecuteList<Resource>(sql, sqlParameter);
		}

		public int GetResourceCount(string searchname, Guid id, int type)
		{
			string sql = "SELECT COUNT(Id) from Resource WHERE ";
			List<SqlParameter> par = new List<SqlParameter>();
			if (searchname != "")
			{
				sql = string.Concat(sql, " (title like @searchname or keywords like @searchname) and ");
			}
			if (id != Guid.Empty)
			{
				if (type == 0)
				{
					sql = string.Concat(sql, " CategoryId=@id and ");
				}
				else if (type == 1)
				{
					sql = string.Concat(sql, " SubjectId=@id and ");
				}
				else if (type == 2)
				{
					sql = string.Concat(sql, " SpecialityId=@id and ");
				}
				par.Add(new SqlParameter("@id", (object)id));
			}
			if (searchname != "")
			{
				par.Add(new SqlParameter("@searchname", string.Concat("%", searchname, "%")));
			}
			sql = string.Concat(sql, " IsDeleted=0");
			int num = Convert.ToInt32(SqlHelper.ExecuteScalar(sql, par.ToArray()));
			return num;
		}

		public bool GetShangchuan(Resource res)
		{
			bool flag;
			string sql = "INSERT INTO [Resource]\r\n           ([Id]\r\n           ,[Title]\r\n           ,[Size]\r\n           ,[Author]\r\n           ,[CategoryId]\r\n           ,[SubjectId]\r\n           ,[SpecialityId]\r\n           ,[KnowledgeId]\r\n           ,[SchoolId]\r\n           ,[PackId]\r\n           ,[TypeId]\r\n           ,[CourseId]\r\n           ,[BoutiqueGrade]\r\n           ,[VideoResolution]\r\n           ,[Sampling]\r\n           ,[VideoTime]\r\n           ,[KeyWords]\r\n           ,[Fescribe]\r\n           ,[Filepath]\r\n           ,[NewFilepath]\r\n           ,[DownloadFilepath]\r\n           ,[IcoFilepath]\r\n           ,[PreviewFilepath]\r\n           ,[FileExt]\r\n           ,[IssueTime]\r\n           ,[DbStatus]\r\n           ,[Tag2]\r\n           ,[IsTop]\r\n           ,[Is211]\r\n           ,[ModifyTime]\r\n           ,[CreateTime]\r\n           ,[IsDeleted])\r\n     VALUES\r\n           (@Id\r\n           ,@Title\r\n           ,@Size\r\n           ,''\r\n           ,@CategoryId\r\n           ,@SubjectId\r\n           ,@SpecialityId\r\n           ,null\r\n           ,@SchoolId\r\n           ,null\r\n           ,@TypeId\r\n           ,@CourseId\r\n           ,null\r\n           ,null\r\n           ,null\r\n           ,null\r\n           ,@KeyWords\r\n           ,@Fescribe\r\n           ,@Filepath\r\n           ,null\r\n           ,null\r\n           ,null\r\n           ,null\r\n           ,@FileExt\r\n           ,getdate()\r\n           ,0\r\n           ,null\r\n           ,0\r\n           ,0\r\n           ,getdate()\r\n           ,getdate()\r\n           ,1)";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)Guid.NewGuid()), new SqlParameter("@Title", res.Title), new SqlParameter("@Size", (object)res.Size), new SqlParameter("@CategoryId", (object)res.CategoryId), new SqlParameter("@SubjectId", (object)res.SubjectId), new SqlParameter("@SpecialityId", (object)res.SpecialityId), new SqlParameter("@SchoolId", (object)res.SchoolId), new SqlParameter("@TypeId", (object)res.TypeId), new SqlParameter("@CourseId", (object)res.CourseId), new SqlParameter("@KeyWords", res.KeyWords), new SqlParameter("@Fescribe", res.Fescribe), new SqlParameter("@Filepath", res.Filepath), new SqlParameter("@FileExt", res.FileExt) };
			flag = (SqlHelper.ExecuteNonQuery(sql, sqlParameter) <= 0 ? false : true);
			return flag;
		}

		public string Getshoucang(Guid id, Guid teachid, Guid schoolid, string ip, int ku)
		{
			string message = "";
			object[] objArray = new object[] { "select count(1) from  LogDetail where TeacherId='", teachid, "' and ResourceId='", id, "' and Status=1 and isdeleted=0 and tag1=", ku };
			if (Convert.ToInt32(SqlHelper.ExecuteScalar(string.Concat(objArray), new SqlParameter[0])) <= 0)
			{
				objArray = new object[] { "insert into  LogDetail values(newid(),'", id, "','", teachid, "','", schoolid, "','", ip, "',", 1, ",", ku, ",null,getdate(),0)" };
				message = (SqlHelper.ExecuteNonQuery(string.Concat(objArray), new SqlParameter[0]) <= 0 ? "添加失败！" : "添加成功！");
			}
			else
			{
				message = "已添加收藏！";
			}
			return message;
		}

		public List<Subject> Getspe(Guid id)
		{
			string sql = "select * from Speciality where SubjectId=@id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@id", (object)id) };
			return SqlHelper.ExecuteList<Subject>(sql, sqlParameter);
		}

		public List<Speciality> GetSpeciality(string searchname, Guid subid)
		{
			string sql = "select Id,Name,SubjectId,Count=(select count(1) from resource where SpecialityId=Speciality.id and (name like @searchname or keywords like @searchname)) from Speciality where SubjectId=@subid and Count<>0 order by count desc";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@subid", (object)subid), new SqlParameter("@searchname", string.Concat("%", searchname, "%")) };
			return SqlHelper.ExecuteList<Speciality>(sql, sqlParameter);
		}

		public Speciality GetSpecialityEntity(string searchname, Guid id)
		{
			string sql = "select Id,Name,SubjectId,Count=(select count(1) from Resource where SpecialityId=Speciality.Id and (title like @searchname   or Keywords like @searchname) and isdeleted=0),\r\nCreateTime,Isdeleted from Speciality where id=@id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@id", (object)id), new SqlParameter("@searchname", string.Concat("%", searchname, "%")) };
			return SqlHelper.ExecuteEntity<Speciality>(sql, sqlParameter);
		}

		public List<Subject> Getsub(Guid id)
		{
			string sql = "select * from Subject where CategoryId=@id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@id", (object)id) };
			return SqlHelper.ExecuteList<Subject>(sql, sqlParameter);
		}

		public List<Subject> GetSubject(string searchname, Guid cateid)
		{
			List<SqlParameter> par = new List<SqlParameter>();
			string sql = "select Id,Name,CategoryId,CreateTime,IsDeleted,Count=(select count(Id) from Resource where SubjectId=Subject.Id";
			if (searchname != "")
			{
				sql = string.Concat(sql, " and (title like @searchname  or Keywords like @searchname)");
				par.Add(new SqlParameter("@searchname", string.Concat("%", searchname, "%")));
			}
			sql = string.Concat(sql, " and isdeleted=0) from Subject where CateGoryId=@cateid order by sortid");
			par.Add(new SqlParameter("@cateid", (object)cateid));
			return SqlHelper.ExecuteList<Subject>(sql, par.ToArray());
		}

		public Subject GetSubjectEntity(string searchname, Guid id)
		{
			string sql = "select Id,Name,CateGoryId,Count=(select count(1) from Resource where SubjectId=Subject.Id and (title like @searchname   or Keywords like @searchname) and isdeleted=0),\r\nCreateTime,IsDeleted from Subject where id=@id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@id", (object)id), new SqlParameter("@searchname", string.Concat("%", searchname, "%")) };
			return SqlHelper.ExecuteEntity<Subject>(sql, sqlParameter);
		}

		public List<Resource> GettypeResource(Guid id, int type, int pageindex, int pagecount, int typeId)
		{
			string sql = "select * from (select ROW_NUMBER() OVER(ORDER BY IsTop desc ,createtime desc,Title asc) as rownum,[Id]\r\n      ,[Title]  \r\n      ,[CategoryId]\r\n      ,[SubjectId]\r\n      ,[SpecialityId]\r\n      ,[KnowledgeId]  \r\n,[PackId]\r\n      ,[IcoFilepath]  \r\n      ,[IsTop] \r\n ,[FileExt]\r\n,[CreateTime]\r\n      ,[IsDeleted]\r\n      ,[tag1] from resource where IsDeleted=0 ";
			if (type == 0)
			{
				sql = string.Concat(sql, "  CategoryId=@id");
			}
			else if (type == 1)
			{
				sql = string.Concat(sql, "  SubjectId=@id");
			}
			else if (type == 2)
			{
				sql = string.Concat(sql, "  SpecialityId=@id");
			}
			sql = string.Concat(sql, " and TypeId=@TypeId");
			sql = string.Concat(sql, " ) as a  WHERE rownum>=(@pageCount*(@pageIndex-1)+1) AND rownum<=@pageIndex*@pageCount");
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@pageindex", (object)pageindex), new SqlParameter("@pagecount", (object)pagecount), new SqlParameter("@id", (object)id), new SqlParameter("@TypeId", (object)typeId) };
			return SqlHelper.ExecuteList<Resource>(sql, sqlParameter);
		}

		public void preiplog(Guid schoolid, List<Guids> Glist, string ip)
		{
			string sql = "";
			for (int i = 0; i < Glist.Count; i++)
			{
				object[] id = new object[] { "insert into  LogDetail values(newid(),'", Glist[i].Id, "','", Guid.Empty, "','", schoolid, "','", ip, "',", 0, ",", 0, ",null,getdate(),0)" };
				sql = string.Concat(id);
				SqlHelper.ExecuteNonQuery(sql, new SqlParameter[0]);
			}
		}

		public void preloginlog(Guid teacherid, List<Guids> Glist, Guid schoolid, string ip)
		{
			string sql = "";
			for (int i = 0; i < Glist.Count; i++)
			{
				object[] id = new object[] { "insert into  LogDetail values(newid(),'", Glist[i].Id, "','", teacherid, "','", schoolid, "','", ip, "',", 0, ",", 0, ",null,getdate(),0)" };
				sql = string.Concat(id);
				SqlHelper.ExecuteNonQuery(sql, new SqlParameter[0]);
			}
		}
	}
}