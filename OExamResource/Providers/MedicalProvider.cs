using Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Providers
{
	public class MedicalProvider
	{
		private readonly static MedicalProvider InstanceObj;

		public static MedicalProvider Instance
		{
			get
			{
				return MedicalProvider.InstanceObj;
			}
		}

		static MedicalProvider()
		{
			MedicalProvider.InstanceObj = new MedicalProvider();
		}

		private MedicalProvider()
		{
		}

		public void Clickcount(Guid id)
		{
			string sql = "";
			if (Convert.ToInt32(SqlHelper.ExecuteScalar(string.Concat("select count(1) from Medical_Log where MedicalId='", id, "'"), new SqlParameter[0])) <= 0)
			{
				sql = string.Concat("insert into  Medical_Log values(newid(),'", id, "',1,getdate(),0)");
				SqlHelper.ExecuteNonQuery(sql, new SqlParameter[0]);
			}
			else
			{
				sql = string.Concat("update Medical_Log set Count=Count+1,CreateTime=getdate() where MedicalId='", id, "'");
				SqlHelper.ExecuteNonQuery(sql, new SqlParameter[0]);
			}
		}

		public List<CollectSource> Collect(Guid? id, int pageIndex, int pageCount)
		{
			string sql = "select * from (select row_number() over(order by Id)as rownum,\r\nId,\r\nTitle,\r\nSubjectname=(select Name from MedicalCategory where MedicalCategory.Id=medical.CategoryId),\r\nSpeciality=(select Name from MedicalSpeciality where MedicalSpeciality.Id=medical.SpecialityId),\r\nTime=(select CreateTime from Logdetail where logdetail.resourceid=medical.id and status=1 and isdeleted=0  and tag1=1 \r\nand teacherid=@teacherid),\r\nIcoFilepath,\r\nPreviewFilepath,DownloadFilepath from Medical where id in\r\n (select resourceid from logdetail where \r\nteacherid=@teacherid and Status=1 and isdeleted=0 and tag1=2 ))as t where rownum>=(@pageCount*(@pageIndex-1)+1) AND rownum<=@pageIndex*@pageCount";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@teacherid", (object)id), new SqlParameter("@pageCount", (object)pageCount), new SqlParameter("@pageIndex", (object)pageIndex) };
			return SqlHelper.ExecuteList<CollectSource>(sql, sqlParameter);
		}

		public bool Collectdeleted(Guid teacherid, Guid resourceid)
		{
			bool flag;
			string sql = "update LogDetail set isdeleted=1 where ResourceId=@resourceid and Status=1 and tag1=2 and TeacherId=@teacherid";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@resourceid", (object)resourceid), new SqlParameter("@teacherid", (object)teacherid) };
			flag = (SqlHelper.ExecuteNonQuery(sql, sqlParameter) <= 0 ? false : true);
			return flag;
		}

		public Medical Create(Medical medical)
		{
			Medical medical1;
			if (medical.Id == Guid.Empty)
			{
				medical.Id = Guid.NewGuid();
			}
			string sql = "INSERT INTO Medical (Id, Title, Size, Author, CategoryId, SpecialityId, KnowledgeId, SchoolId, PackId, TypeId, CourseId, BoutiqueGrade, VideoResolution, Sampling, VideoTime, KeyWords, Fescribe, Filepath, NewFilepath, DownloadFilepath, IcoFilepath, PreviewFilepath, FileExt, IssueTime, Tag1, Tag2, IsTop, Is211, ModifyTime, CreateTime, IsDeleted)  VALUES (@Id, @Title, @Size, @Author, @CategoryId, @SpecialityId, @KnowledgeId, @SchoolId, @PackId, @TypeId, @CourseId, @BoutiqueGrade, @VideoResolution, @Sampling, @VideoTime, @KeyWords, @Fescribe, @Filepath, @NewFilepath, @DownloadFilepath, @IcoFilepath, @PreviewFilepath, @FileExt, @IssueTime, @Tag1, @Tag2, @IsTop, @Is211, @ModifyTime, @CreateTime, @IsDeleted)";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", SqlHelper.ToDBValue(medical.Id)), new SqlParameter("@Title", SqlHelper.ToDBValue(medical.Title)), new SqlParameter("@Size", SqlHelper.ToDBValue(medical.Size)), new SqlParameter("@Author", SqlHelper.ToDBValue(medical.Author)), new SqlParameter("@CategoryId", SqlHelper.ToDBValue(medical.CategoryId)), new SqlParameter("@SpecialityId", SqlHelper.ToDBValue(medical.SpecialityId)), new SqlParameter("@KnowledgeId", SqlHelper.ToDBValue(medical.KnowledgeId)), new SqlParameter("@SchoolId", SqlHelper.ToDBValue(medical.SchoolId)), new SqlParameter("@PackId", SqlHelper.ToDBValue(medical.PackId)), new SqlParameter("@TypeId", SqlHelper.ToDBValue(medical.TypeId)), new SqlParameter("@CourseId", SqlHelper.ToDBValue(medical.CourseId)), new SqlParameter("@BoutiqueGrade", SqlHelper.ToDBValue(medical.BoutiqueGrade)), new SqlParameter("@VideoResolution", SqlHelper.ToDBValue(medical.VideoResolution)), new SqlParameter("@Sampling", SqlHelper.ToDBValue(medical.Sampling)), new SqlParameter("@VideoTime", SqlHelper.ToDBValue(medical.VideoTime)), new SqlParameter("@KeyWords", SqlHelper.ToDBValue(medical.KeyWords)), new SqlParameter("@Fescribe", SqlHelper.ToDBValue(medical.Fescribe)), new SqlParameter("@Filepath", SqlHelper.ToDBValue(medical.Filepath)), new SqlParameter("@NewFilepath", SqlHelper.ToDBValue(medical.NewFilepath)), new SqlParameter("@DownloadFilepath", SqlHelper.ToDBValue(medical.DownloadFilepath)), new SqlParameter("@IcoFilepath", SqlHelper.ToDBValue(medical.IcoFilepath)), new SqlParameter("@PreviewFilepath", SqlHelper.ToDBValue(medical.PreviewFilepath)), new SqlParameter("@FileExt", SqlHelper.ToDBValue(medical.FileExt)), new SqlParameter("@IssueTime", SqlHelper.ToDBValue(medical.IssueTime)), new SqlParameter("@Tag1", SqlHelper.ToDBValue(medical.Tag1)), new SqlParameter("@Tag2", SqlHelper.ToDBValue(medical.Tag2)), new SqlParameter("@IsTop", SqlHelper.ToDBValue(medical.IsTop)), new SqlParameter("@Is211", SqlHelper.ToDBValue(medical.Is211)), new SqlParameter("@ModifyTime", SqlHelper.ToDBValue(medical.ModifyTime)), new SqlParameter("@CreateTime", SqlHelper.ToDBValue(medical.CreateTime)), new SqlParameter("@IsDeleted", SqlHelper.ToDBValue(medical.IsDeleted)) };
			if (SqlHelper.ExecuteNonQuery(sql, sqlParameter) > 0)
			{
				medical1 = medical;
			}
			else
			{
				medical1 = null;
			}
			return medical1;
		}

		public int Delete(Guid id)
		{
			string sql = "DELETE Medical WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}

		public void Downlog(Guid resourceid, Guid teacherid, Guid schoolid, string ip)
		{
			object[] objArray = new object[] { "insert into  LogDetail values(newid(),'", resourceid, "','", teacherid, "','", schoolid, "','", ip, "',", 1, ",", 2, ",null,getdate(),0)" };
			string sql = string.Concat(objArray);
			SqlHelper.ExecuteNonQuery(sql, new SqlParameter[0]);
		}

		public List<Resourcelist> GetCateCountList()
		{
			return SqlHelper.ExecuteList<Resourcelist>(" SELECT MedicalCategory.Id,\r\n                            MedicalCategory.Name CategoryName,\r\n                            COUNT(1) Size ,\r\n                            cast(sum(cast([Size] as float)) as nvarchar(50)) as Tag1\r\n                            FROM Medical \r\n                            INNER JOIN MedicalCategory \r\n                            ON MedicalCategory.Id=Medical.CategoryId \r\n                            GROUP BY MedicalCategory.Name,MedicalCategory.Id,MedicalCategory.SortId \r\n                            ORDER BY MedicalCategory.SortId", new SqlParameter[0]);
		}

		public List<MedicalTypes> GetCategory(string searchname)
		{
			string sql = "select id,name,Count=(select count(1) from medical where CategoryId=MedicalCategory.id and IsDeleted=0 and SpecialityId is not null\r\nand (title like @searchname or KeyWords like @searchname )  )\r\n from MedicalCategory  where IsDeleted=0 order by Count desc";
			List<SqlParameter> par = new List<SqlParameter>()
			{
				new SqlParameter("@searchname", string.Concat("%", searchname, "%"))
			};
			return SqlHelper.ExecuteList<MedicalTypes>(sql, par.ToArray());
		}

		public int GetCount(Guid categoryid, Guid specialityid, Guid typeid, Guid knowledgeid, string name, int isdeleted)
		{
			string sql = string.Concat("SELECT COUNT(Id) FROM Medical WHERE IsDeleted = ", isdeleted);
			if (categoryid != Guid.Empty)
			{
				sql = string.Concat(sql, " AND CategoryId = @categoryid ");
			}
			if (specialityid != Guid.Empty)
			{
				sql = string.Concat(sql, " AND SpecialityId = @specialityid ");
			}
			if (typeid != Guid.Empty)
			{
				sql = string.Concat(sql, " AND TypeId = @typeid ");
			}
			if (knowledgeid != Guid.Empty)
			{
				sql = string.Concat(sql, " AND KnowledgeId = @knowledgeid ");
			}
			if (!string.IsNullOrEmpty(name))
			{
				sql = string.Concat(sql, " AND ( Title LIKE @name OR KeyWords LIKE @name )");
			}
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@categoryid", (object)categoryid), new SqlParameter("@specialityid", (object)specialityid), new SqlParameter("@knowledgeid", (object)knowledgeid), new SqlParameter("@typeid", (object)typeid), new SqlParameter("@name", string.Concat("%", name, "%")) };
			return (int)SqlHelper.ExecuteScalar(sql, sqlParameter);
		}

		public int GetCount()
		{
			return (int)SqlHelper.ExecuteScalar("SELECT COUNT(Id) FROM Medical WHERE IsDeleted = 0", new SqlParameter[0]);
		}

		public Medical GetEntity(Guid id)
		{
			string sql = "SELECT * FROM Medical WHERE Id = @Id AND IsDeleted = 0";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteEntity<Medical>(sql, sqlParameter);
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

		public List<MedicalTypes> GetKnowledge(Guid id)
		{
			List<SqlParameter> par = new List<SqlParameter>();
			string sql = "select KnowledgeId as id,name=(select name from dbo.MedicalKnowledge where id=s.KnowledgeId and IsDeleted=0),count from \r\n(\r\nselect  KnowledgeId,count(1)as count  from dbo.Medical where\r\n [SpecialityId]=@id and KnowledgeId is not null and IsDeleted=0\r\ngroup by KnowledgeId\r\n) as s";
			par.Add(new SqlParameter("@id", (object)id));
			return SqlHelper.ExecuteList<MedicalTypes>(sql, par.ToArray());
		}

		public List<MedicalList> GetList(Guid categoryid, Guid specialityid, Guid typeid, Guid knowledgeid, string name, int pageIndex, int pageCount, int isdeleted)
		{
			string sql = string.Concat("  SELECT * FROM( \r\n                                SELECT Medical.Id, \r\n                                       Medical.Title,\r\n                                       Medical.[Size], \r\n                                       Medical.Author, \r\n                                       MedicalCategory.Name CategoryName, \r\n                                       MedicalSpeciality.Name SpecialityName,\r\n                                       MedicalKnowledge.Name KnowledgeName,\r\n                                       MedicalType.Name TypeName, \r\n                                       Medical.KeyWords, \r\n                                       Medical.CreateTime, \r\n                                       Medical.IsDeleted,\r\n                                       ROW_NUMBER() OVER(ORDER BY Medical.CreateTime DESC,MedicalCategory.SortId,MedicalSpeciality.SortId ) rownum\r\n                                FROM Medical\r\n                                    LEFT JOIN MedicalCategory\r\n                                    ON Medical.CategoryId=MedicalCategory.Id\r\n                                    LEFT JOIN MedicalKnowledge\r\n                                    ON Medical.KnowledgeId=MedicalKnowledge.Id\r\n                                    LEFT JOIN MedicalSpeciality\r\n                                    ON Medical.SpecialityId=MedicalSpeciality.Id\r\n                                    LEFT JOIN MedicalType\r\n                                    ON Medical.TypeId=MedicalType.Id \r\n\t\t                        WHERE Medical.Isdeleted =", isdeleted);
			if (categoryid != Guid.Empty)
			{
				sql = string.Concat(sql, " AND Medical.CategoryId = @categoryid ");
			}
			if (specialityid != Guid.Empty)
			{
				sql = string.Concat(sql, " AND Medical.SpecialityId = @specialityid ");
			}
			if (typeid != Guid.Empty)
			{
				sql = string.Concat(sql, " AND Medical.TypeId = @typeid ");
			}
			if (knowledgeid != Guid.Empty)
			{
				sql = string.Concat(sql, " AND Medical.KnowledgeId = @knowledgeid ");
			}
			if (!string.IsNullOrEmpty(name))
			{
				sql = string.Concat(sql, " AND ( Medical.Title LIKE @name OR Medical.KeyWords LIKE @name )");
			}
			sql = string.Concat(sql, " ) as t\r\n                      WHERE rownum>=(@pageCount*(@pageIndex-1)+1) AND rownum<=@pageIndex*@pageCount");
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@pageIndex", (object)pageIndex), new SqlParameter("@pageCount", (object)pageCount), new SqlParameter("@categoryid", (object)categoryid), new SqlParameter("@specialityid", (object)specialityid), new SqlParameter("@knowledgeid", (object)knowledgeid), new SqlParameter("@typeid", (object)typeid), new SqlParameter("@name", string.Concat("%", name, "%")) };
			return SqlHelper.ExecuteList<MedicalList>(sql, sqlParameter);
		}

		public List<Medical> GetList()
		{
			return SqlHelper.ExecuteList<Medical>("SELECT * FROM Medical WHERE IsDeleted = 0", new SqlParameter[0]);
		}

		public MedicalCategory GetMedicalCategory(Guid id)
		{
			string sql = "select * from dbo.MedicalCategory where id in (select CategoryId from dbo.Medical where SpecialityId=@id)";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@id", (object)id) };
			return SqlHelper.ExecuteEntity<MedicalCategory>(sql, sqlParameter);
		}

		public List<MedicalListPreview> GetMedicalEntity(Guid id)
		{
			string sql = "select  Id,Title,Size,Author,\r\nCategoryname=(select Name from MedicalCategory where id=m.CategoryId),\r\nSubjectname=(select Name from MedicalSpeciality where id=m.SpecialityId),\r\nSpeciality=(select Name from MedicalKnowledge where id=m.KnowledgeId),\r\nTypeName=(select Name from MedicalType where id=m.TypeId),\r\nCoursename=(select Name from Course where id=m.CourseId),\r\nClickcount=(select count(1) from LogDetail where ResourceId=m.Id and Status=0 and Isdeleted=0),\r\nDowncount=(select count(1) from LogDetail where ResourceId=m.Id and Status=1 and Isdeleted=0),\r\nKeywords,Fescribe,Filepath,DownloadFilepath,IcoFilepath,PreviewFilepath,NewFilepath,FileExt,IsTop from dbo.Medical as m where m.id=@id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@id", (object)id) };
			return SqlHelper.ExecuteList<MedicalListPreview>(sql, sqlParameter);
		}

		public List<MedicalKnowledge> GetMedicalKnowledge(Guid subid)
		{
			string sql = "select * from MedicalKnowledge where id in (select KnowledgeId from Medical where SpecialityId=@id)";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@id", (object)subid) };
			return SqlHelper.ExecuteList<MedicalKnowledge>(sql, sqlParameter);
		}

		public List<MedicalSpeciality> GetMedicalSpeciality(Guid CateId)
		{
			string sql = "select * from MedicalSpeciality where id in (select SpecialityId from Medical where CategoryId=@id)";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@id", (object)CateId) };
			return SqlHelper.ExecuteList<MedicalSpeciality>(sql, sqlParameter);
		}

		public MedicalSpeciality GetMedicalSubject(Guid id)
		{
			string sql = "select * from dbo.MedicalSpeciality where id in (select SpecialityId from dbo.Medical where KnowledgeId=@id)";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@id", (object)id) };
			return SqlHelper.ExecuteEntity<MedicalSpeciality>(sql, sqlParameter);
		}

		public List<Medical> GetPage(int pageIndex, int pageCount)
		{
			string sql = "SELECT * FROM(SELECT *,(ROW_NUMBER() OVER(ORDER BY Id)) rownum FROM Medical WHERE IsDeleted = 0) t WHERE rownum>=(@pageCount*(@pageIndex-1)+1) AND rownum<=@pageIndex*@pageCount";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@pageIndex", (object)pageIndex), new SqlParameter("@pageCount", (object)pageCount) };
			return SqlHelper.ExecuteList<Medical>(sql, sqlParameter);
		}

		public MedicalTypes GetSearchCateEntity(Guid id, string searchname)
		{
			string sql = "SELECT Id,Name,Count=(select count(1) from Medical where CategoryId=MedicalCategory.Id and   SpecialityId is not null and   Isdeleted=0 and  (title like @sear or KeyWords like @sear)) FROM MedicalCategory WHERE Id = @Id AND IsDeleted = 0";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id), new SqlParameter("@sear", string.Concat("%", searchname, "%")) };
			return SqlHelper.ExecuteEntity<MedicalTypes>(sql, sqlParameter);
		}

		public MedicalTypes GetSearchKnoEntity(Guid id, string searchname)
		{
			string sql = "SELECT Id,Name,Count=(select count(1) from Medical where KnowledgeId=MedicalKnowledge.Id and SpecialityId is not null and  Isdeleted=0 and KnowledgeId=@Id and   (title like @sear or KeyWords like @sear)) FROM MedicalKnowledge WHERE   IsDeleted = 0";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@id", (object)id), new SqlParameter("@sear", string.Concat("%", searchname, "%")) };
			return SqlHelper.ExecuteEntity<MedicalTypes>(sql, sqlParameter);
		}

		public MedicalTypes GetSearchSubEntity(Guid id, string searchname)
		{
			string sql = "SELECT Id,Name,ParentId=(select top(1)CategoryId from Medical where SpecialityId=@Id),\r\nCount=(select count(1) from Medical where SpecialityId=MedicalSpeciality.Id and SpecialityId is not null and   \r\nIsdeleted=0 and  SpecialityId = MedicalSpeciality.Id   and  (title like @sear or KeyWords like @sear)) FROM MedicalSpeciality WHERE  id=@Id and IsDeleted = 0";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@id", (object)id), new SqlParameter("@sear", string.Concat("%", searchname, "%")) };
			return SqlHelper.ExecuteEntity<MedicalTypes>(sql, sqlParameter);
		}

		public bool GetShangchuan(Medical res)
		{
			bool flag;
			string sql = "INSERT INTO [Medical]\r\n           ([Id]\r\n           ,[Title]\r\n           ,[Size]\r\n           ,[Author]\r\n           ,[CategoryId]\r\n           ,[SpecialityId]\r\n           ,[KnowledgeId] \r\n           ,[SchoolId]\r\n           ,[PackId]\r\n           ,[TypeId]\r\n           ,[CourseId]\r\n           ,[BoutiqueGrade]\r\n           ,[VideoResolution]\r\n           ,[Sampling]\r\n           ,[VideoTime]\r\n           ,[KeyWords]\r\n           ,[Fescribe]\r\n           ,[Filepath]\r\n           ,[NewFilepath]\r\n           ,[DownloadFilepath]\r\n           ,[IcoFilepath]\r\n           ,[PreviewFilepath]\r\n           ,[FileExt]\r\n           ,[IssueTime]           \r\n           ,[Tag2]\r\n           ,[IsTop]\r\n           ,[Is211]\r\n           ,[ModifyTime]\r\n           ,[CreateTime]\r\n           ,[IsDeleted])\r\n     VALUES\r\n           (@Id\r\n           ,@Title\r\n           ,@Size\r\n           ,''\r\n           ,@CategoryId\r\n           ,@SpecialityId\r\n           ,@KnowledgeId \r\n           ,@SchoolId\r\n           ,null\r\n           ,@TypeId\r\n           ,@CourseId\r\n           ,null\r\n           ,null\r\n           ,null\r\n           ,null\r\n           ,@KeyWords\r\n           ,@Fescribe\r\n           ,@Filepath\r\n           ,null\r\n           ,null\r\n           ,null\r\n           ,null\r\n           ,@FileExt\r\n           ,getdate()  \r\n           ,null\r\n           ,0\r\n           ,0\r\n           ,getdate()\r\n           ,getdate()\r\n           ,1)";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)Guid.NewGuid()), new SqlParameter("@Title", res.Title), new SqlParameter("@Size", (object)res.Size), new SqlParameter("@CategoryId", (object)res.CategoryId), new SqlParameter("@SpecialityId", (object)res.SpecialityId), new SqlParameter("@KnowledgeId", (object)res.KnowledgeId), new SqlParameter("@SchoolId", (object)res.SchoolId), new SqlParameter("@TypeId", (object)res.TypeId), new SqlParameter("@CourseId", (object)res.CourseId), new SqlParameter("@KeyWords", res.KeyWords), new SqlParameter("@Fescribe", res.Fescribe), new SqlParameter("@Filepath", res.Filepath), new SqlParameter("@FileExt", res.FileExt) };
			flag = (SqlHelper.ExecuteNonQuery(sql, sqlParameter) <= 0 ? false : true);
			return flag;
		}

		public string Getshoucang(Guid id, Guid teachid, Guid schoolid, string ip)
		{
			string message = "";
			object[] objArray = new object[] { "select count(1) from  LogDetail where TeacherId='", teachid, "' and ResourceId='", id, "' and Status=1" };
			if (Convert.ToInt32(SqlHelper.ExecuteScalar(string.Concat(objArray), new SqlParameter[0])) <= 0)
			{
				objArray = new object[] { "insert into  LogDetail values(newid(),'", id, "','", teachid, "','", schoolid, "','", ip, "',1,null,null,getdate(),0)" };
				message = (SqlHelper.ExecuteNonQuery(string.Concat(objArray), new SqlParameter[0]) <= 0 ? "添加失败！" : "添加成功！");
			}
			else
			{
				message = "已添加收藏！";
			}
			return message;
		}

		public int GetSourceCount(Guid id, string searchname, int type)
		{
			string sql = "select count(1) from Medical where IsDeleted=0 and SpecialityId is not null ";
			List<SqlParameter> par = new List<SqlParameter>();
			if (type == 0)
			{
				sql = string.Concat(sql, " and  CategoryId=@id");
			}
			else if (type != 1)
			{
				sql = (type != 2 ? sql ?? "" : string.Concat(sql, " and KnowledgeId=@id"));
			}
			else
			{
				sql = string.Concat(sql, " and SpecialityId=@id");
			}
			par.Add(new SqlParameter("@id", (object)id));
			if (searchname != "")
			{
				sql = string.Concat(sql, " and ( Title like @searchname or KeyWords like @searchname) ");
				par.Add(new SqlParameter("@searchname", string.Concat("%", searchname, "%")));
			}
			int num = Convert.ToInt32(SqlHelper.ExecuteScalar(sql, par.ToArray()));
			return num;
		}

		public List<Medical> GetSourceList(string searchname, Guid id, int type, Guid typeid, int pageindex, int pagecount)
		{
			string sql = "select * from (select ROW_NUMBER() OVER(ORDER BY case when title like @searchname then 0\r\nelse 1 end,Len(IcoFilepath) DESC,Tag2 asc, istop desc,CreateTime desc) as rownum,Id,Title,FileExt,IcoFilepath,PackId,IsTop,\r\nTag2=cast((select MedicalSortId from MedicalType where Id=Medical.TypeId) as nvarchar(50)) from Medical where IsDeleted=0";
			List<SqlParameter> par = new List<SqlParameter>();
			if (!typeid.Equals(Guid.Parse("00000000-1234-0000-0000-000000000000")))
			{
				sql = string.Concat(sql, " and TypeId=@typeid ");
			}
			sql = string.Concat(sql, " and (title like @searchname or keywords like @searchname) ");
			if (type == 0)
			{
				sql = string.Concat(sql, " and CategoryId=@id ");
			}
			else if (type == 1)
			{
				sql = string.Concat(sql, " and SpecialityId=@id ");
			}
			else if (type == 2)
			{
				sql = string.Concat(sql, " and KnowledgeId=@id ");
			}
			par.Add(new SqlParameter("@pageindex", (object)pageindex));
			par.Add(new SqlParameter("@pagecount", (object)pagecount));
			par.Add(new SqlParameter("@typeid", (object)typeid));
			par.Add(new SqlParameter("@id", (object)id));
			par.Add(new SqlParameter("@searchname", string.Concat("%", searchname, "%")));
			sql = string.Concat(sql, " ) as t  WHERE rownum>=(@pageCount*(@pageIndex-1)+1) AND rownum<=@pageIndex*@pageCount ");
			return SqlHelper.ExecuteList<Medical>(sql, par.ToArray());
		}

		public List<MedicalSourceType> GetSourceType(Guid id, string searchname, int type)
		{
			string sql = "select Id,Name,Count=(select count(1) from Medical where isdeleted=0 and SpecialityId is not null ";
			List<SqlParameter> par = new List<SqlParameter>();
			if (type == 0)
			{
				sql = string.Concat(sql, " and  CategoryId=@id");
			}
			else if (type != 1)
			{
				sql = (type != 2 ? sql ?? "" : string.Concat(sql, " and KnowledgeId=@id"));
			}
			else
			{
				sql = string.Concat(sql, " and SpecialityId=@id");
			}
			par.Add(new SqlParameter("@id", (object)id));
			if (searchname != "")
			{
				sql = string.Concat(sql, " and ( Title like @searchname or KeyWords like @searchname) ");
				par.Add(new SqlParameter("@searchname", string.Concat("%", searchname, "%")));
			}
			sql = string.Concat(sql, " and TypeId=MedicalType.id) from MedicalType   where IsDeleted=0 order by SortId asc");
			return SqlHelper.ExecuteList<MedicalSourceType>(sql, par.ToArray());
		}

		public List<MedicalTypes> GetSpeciality(string searchname, Guid cateid)
		{
			List<SqlParameter> par = new List<SqlParameter>();
			string sql = "select specialityid as id,name=(select name from dbo.MedicalSpeciality where id=s.SpecialityId and IsDeleted=0),count from \r\n                (select SpecialityId,count(1)as count  from dbo.Medical where IsDeleted=0 and  CategoryId=@cateid and SpecialityId is not null ";
			par.Add(new SqlParameter("@cateid", (object)cateid));
			if (searchname != "")
			{
				sql = string.Concat(sql, " and (title like @searchname  or Keywords like @searchname)");
				par.Add(new SqlParameter("@searchname", string.Concat("%", searchname, "%")));
			}
			sql = string.Concat(sql, " group by SpecialityId) as s order by count desc ");
			return SqlHelper.ExecuteList<MedicalTypes>(sql, par.ToArray());
		}

		public List<Resourcelist> GetSpeCountList()
		{
			return SqlHelper.ExecuteList<Resourcelist>(" SELECT MedicalSpeciality.Id,\r\n                            MedicalCategory.Name CategoryName,\r\n                            MedicalSpeciality.Name SpecialityName,\r\n                            COUNT(1) Size ,\r\n                            cast(sum(cast([Size] as float)) as nvarchar(50)) as Tag1   \r\n                            FROM Medical \r\n                            INNER JOIN MedicalSpeciality \r\n                            ON MedicalSpeciality.Id=Medical.SpecialityId \r\n                            INNER JOIN MedicalCategory \r\n                            ON MedicalCategory.Id=Medical.CategoryId \r\n                            GROUP BY MedicalSpeciality.Name,MedicalCategory.Name,MedicalSpeciality.Id,MedicalSpeciality.SortId,MedicalCategory.SortId \r\n                            ORDER BY MedicalCategory.SortId,MedicalSpeciality.SortId", new SqlParameter[0]);
		}

		public void preiplog(Guid schoolid, List<Guid> Glist, string ip)
		{
			string sql = "";
			for (int i = 0; i < Glist.Count; i++)
			{
				object[] item = new object[] { "insert into  LogDetail values(newid(),'", Glist[i], "','", Guid.Empty, "','", schoolid, "','", ip, "',", 0, ",", 1, ",null,getdate(),0)" };
				sql = string.Concat(item);
				SqlHelper.ExecuteNonQuery(sql, new SqlParameter[0]);
			}
		}

		public void preloginlog(Guid teacherid, List<Guid> Glist, Guid schoolid, string ip)
		{
			string sql = "";
			for (int i = 0; i < Glist.Count; i++)
			{
				object[] item = new object[] { "insert into  LogDetail values(newid(),'", Glist[i], "','", teacherid, "','", schoolid, "','", ip, "',", 0, ",", 1, ",null,getdate(),0)" };
				sql = string.Concat(item);
				SqlHelper.ExecuteNonQuery(sql, new SqlParameter[0]);
			}
		}

		public List<MedicalListPreview> Preview_list(List<Guid> Glist)
		{
			object obj;
			object[] item;
			string sql = "select Id,Title,Size,Author,\r\nCategoryName=(select Name from MedicalCategory where id=m.CategoryId),\r\nSubjectName=(select Name from MedicalSpeciality where id=m.SpecialityId),\r\nSpecialityName=(select Name from MedicalKnowledge where id=m.KnowledgeId),\r\nTypeName=(select Name from MedicalType where id=m.TypeId),\r\nCoursename=(select Name from Course where id=m.CourseId),\r\nClickcount=(select count(1) from LogDetail where ResourceId=m.Id and Status=0 and Isdeleted=0),\r\nDowncount=(select count(1) from LogDetail where ResourceId=m.Id and Status=1 and Isdeleted=0),\r\nKeywords,Fescribe,Filepath,DownloadFilepath,IcoFilepath,PreviewFilepath,NewFilepath,FileExt,IsTop from dbo.Medical as m where IsDeleted=0 and";
			for (int i = 0; i < Glist.Count; i++)
			{
				obj = sql;
				item = new object[] { obj, " id='", Glist[i], "' or" };
				sql = string.Concat(item);
			}
			for (int x = 0; x < Glist.Count; x++)
			{
				obj = sql;
				item = new object[] { obj, " Packid='", Glist[x], "' or" };
				sql = string.Concat(item);
			}
			sql = sql.Substring(0, sql.Length - 2);
			return SqlHelper.ExecuteList<MedicalListPreview>(sql, new SqlParameter[0]);
		}

		public int SoftDelete(Guid id)
		{
			string sql = "UPDATE Medical SET IsDeleted = 1 WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}

		public int Update(Medical medical)
		{
			string sql = "UPDATE Medical SET Title = @Title,Size = @Size,Author = @Author,CategoryId = @CategoryId,SpecialityId = @SpecialityId,KnowledgeId = @KnowledgeId,SchoolId = @SchoolId,PackId = @PackId,TypeId = @TypeId,CourseId = @CourseId,BoutiqueGrade = @BoutiqueGrade,VideoResolution = @VideoResolution,Sampling = @Sampling,VideoTime = @VideoTime,KeyWords = @KeyWords,Fescribe = @Fescribe,Filepath = @Filepath,NewFilepath = @NewFilepath,DownloadFilepath = @DownloadFilepath,IcoFilepath = @IcoFilepath,PreviewFilepath = @PreviewFilepath,FileExt = @FileExt,IssueTime = @IssueTime,Tag1 = @Tag1,Tag2 = @Tag2,IsTop = @IsTop,Is211 = @Is211,ModifyTime = @ModifyTime,CreateTime = @CreateTime,IsDeleted = @IsDeleted WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)medical.Id), new SqlParameter("@Title", SqlHelper.ToDBValue(medical.Title)), new SqlParameter("@Size", SqlHelper.ToDBValue(medical.Size)), new SqlParameter("@Author", SqlHelper.ToDBValue(medical.Author)), new SqlParameter("@CategoryId", SqlHelper.ToDBValue(medical.CategoryId)), new SqlParameter("@SpecialityId", SqlHelper.ToDBValue(medical.SpecialityId)), new SqlParameter("@KnowledgeId", SqlHelper.ToDBValue(medical.KnowledgeId)), new SqlParameter("@SchoolId", SqlHelper.ToDBValue(medical.SchoolId)), new SqlParameter("@PackId", SqlHelper.ToDBValue(medical.PackId)), new SqlParameter("@TypeId", SqlHelper.ToDBValue(medical.TypeId)), new SqlParameter("@CourseId", SqlHelper.ToDBValue(medical.CourseId)), new SqlParameter("@BoutiqueGrade", SqlHelper.ToDBValue(medical.BoutiqueGrade)), new SqlParameter("@VideoResolution", SqlHelper.ToDBValue(medical.VideoResolution)), new SqlParameter("@Sampling", SqlHelper.ToDBValue(medical.Sampling)), new SqlParameter("@VideoTime", SqlHelper.ToDBValue(medical.VideoTime)), new SqlParameter("@KeyWords", SqlHelper.ToDBValue(medical.KeyWords)), new SqlParameter("@Fescribe", SqlHelper.ToDBValue(medical.Fescribe)), new SqlParameter("@Filepath", SqlHelper.ToDBValue(medical.Filepath)), new SqlParameter("@NewFilepath", SqlHelper.ToDBValue(medical.NewFilepath)), new SqlParameter("@DownloadFilepath", SqlHelper.ToDBValue(medical.DownloadFilepath)), new SqlParameter("@IcoFilepath", SqlHelper.ToDBValue(medical.IcoFilepath)), new SqlParameter("@PreviewFilepath", SqlHelper.ToDBValue(medical.PreviewFilepath)), new SqlParameter("@FileExt", SqlHelper.ToDBValue(medical.FileExt)), new SqlParameter("@IssueTime", SqlHelper.ToDBValue(medical.IssueTime)), new SqlParameter("@Tag1", SqlHelper.ToDBValue(medical.Tag1)), new SqlParameter("@Tag2", SqlHelper.ToDBValue(medical.Tag2)), new SqlParameter("@IsTop", SqlHelper.ToDBValue(medical.IsTop)), new SqlParameter("@Is211", SqlHelper.ToDBValue(medical.Is211)), new SqlParameter("@ModifyTime", SqlHelper.ToDBValue(medical.ModifyTime)), new SqlParameter("@CreateTime", SqlHelper.ToDBValue(medical.CreateTime)), new SqlParameter("@IsDeleted", SqlHelper.ToDBValue(medical.IsDeleted)) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}
	}
}