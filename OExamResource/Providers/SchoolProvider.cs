using Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Providers
{
	public class SchoolProvider
	{
		private readonly static SchoolProvider InstanceObj;

		public static SchoolProvider Instance
		{
			get
			{
				return SchoolProvider.InstanceObj;
			}
		}

		static SchoolProvider()
		{
			SchoolProvider.InstanceObj = new SchoolProvider();
		}

		private SchoolProvider()
		{
		}

		public School Create(School school)
		{
			School school1;
			if (school.Id == Guid.Empty)
			{
				school.Id = Guid.NewGuid();
			}
			string sql = "INSERT INTO School (Id, Name, ParentId, Tier, StartTime, EndTime, StopRemind, SellId, Remark, LogoImg, NameImg, BannerImgs, LanCourseCount, LanResourceCount, TrialType, CreateTime, ModifyTime, IsDeleted, Tag1, Tag2,ProvinceId)  VALUES (@Id, @Name, @ParentId, @Tier, @StartTime, @EndTime, @StopRemind, @SellId, @Remark, @LogoImg, @NameImg, @BannerImgs, @LanCourseCount, @LanResourceCount, @TrialType, @CreateTime, @ModifyTime, @IsDeleted, @Tag1, @Tag2,@ProvinceId)";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", SqlHelper.ToDBValue(school.Id)), new SqlParameter("@Name", SqlHelper.ToDBValue(school.Name)), new SqlParameter("@ParentId", SqlHelper.ToDBValue(school.ParentId)), new SqlParameter("@Tier", SqlHelper.ToDBValue(school.Tier)), new SqlParameter("@StartTime", SqlHelper.ToDBValue(school.StartTime)), new SqlParameter("@EndTime", SqlHelper.ToDBValue(school.EndTime)), new SqlParameter("@StopRemind", SqlHelper.ToDBValue(school.StopRemind)), new SqlParameter("@SellId", SqlHelper.ToDBValue(school.SellId)), new SqlParameter("@Remark", SqlHelper.ToDBValue(school.Remark)), new SqlParameter("@LogoImg", SqlHelper.ToDBValue(school.LogoImg)), new SqlParameter("@NameImg", SqlHelper.ToDBValue(school.NameImg)), new SqlParameter("@BannerImgs", SqlHelper.ToDBValue(school.BannerImgs)), new SqlParameter("@LanCourseCount", SqlHelper.ToDBValue(school.LanCourseCount)), new SqlParameter("@LanResourceCount", SqlHelper.ToDBValue(school.LanResourceCount)), new SqlParameter("@TrialType", SqlHelper.ToDBValue(school.TrialType)), new SqlParameter("@CreateTime", SqlHelper.ToDBValue(school.CreateTime)), new SqlParameter("@ModifyTime", SqlHelper.ToDBValue(school.ModifyTime)), new SqlParameter("@IsDeleted", SqlHelper.ToDBValue(school.IsDeleted)), new SqlParameter("@Tag1", SqlHelper.ToDBValue(school.Tag1)), new SqlParameter("@Tag2", SqlHelper.ToDBValue(school.Tag2)), new SqlParameter("@ProvinceId", SqlHelper.ToDBValue(school.ProvinceId)) };
			if (SqlHelper.ExecuteNonQuery(sql, sqlParameter) > 0)
			{
				school1 = school;
			}
			else
			{
				school1 = null;
			}
			return school1;
		}

		public int Delete(Guid id)
		{
			string sql = "DELETE School WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}

		public int DeleteFacDepByFacId(Guid id)
		{
			string sql = "delete School where Id = @id OR ParentId = @id ";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@id", (object)id) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}

		public int DeleteSchool(Guid id)
		{
			string sql = "delete School where Id = @id OR ParentId = @id OR ParentId IN (select Id from School where ParentId = @id)";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@id", (object)id) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}

		public int GetCount(Guid id, Guid privinceID, string name, int tier, string TrialTag, string Startime, string Endtime, Guid schoolid)
		{
			List<SqlParameter> par = new List<SqlParameter>();
			string sql = "SELECT COUNT(Id) FROM School WHERE IsDeleted = 0 AND  (School.Tier =@tier OR School.ParentId IS NULL) ";
			if (id != Guid.Empty)
			{
				sql = string.Concat(sql, " AND School.SellId=@id ");
				par.Add(new SqlParameter("@id", (object)id));
			}
			if (!string.IsNullOrEmpty(name))
			{
				sql = string.Concat(sql, " AND School.[Name] LIKE @name ");
				par.Add(new SqlParameter("@name", string.Concat("%", name, "%")));
			}
			if (!string.IsNullOrEmpty(Startime))
			{
				sql = string.Concat(sql, " and  EndTime >= @Startime");
				par.Add(new SqlParameter("@Startime", Startime));
			}
			if (!string.IsNullOrEmpty(Endtime))
			{
				sql = string.Concat(sql, " and  EndTime <= @Endtime");
				par.Add(new SqlParameter("@Endtime", Endtime));
			}
			if (!string.IsNullOrEmpty(TrialTag))
			{
				sql = string.Concat(sql, " and  TrialType = @TrialType");
				par.Add(new SqlParameter("@TrialType", TrialTag));
			}
			if (privinceID != Guid.Empty)
			{
				sql = string.Concat(sql, " and  ProvinceId = @ProvinceId");
				par.Add(new SqlParameter("@ProvinceId", (object)privinceID));
			}
			if (schoolid != Guid.Empty)
			{
				sql = string.Concat(sql, " and  School.Id = @ID");
				par.Add(new SqlParameter("@ID", (object)schoolid));
			}
			par.Add(new SqlParameter("@tier", (object)tier));
			return (int)SqlHelper.ExecuteScalar(sql, par.ToArray());
		}

		public int GetCount()
		{
			return (int)SqlHelper.ExecuteScalar("SELECT COUNT(Id) FROM School WHERE IsDeleted = 0", new SqlParameter[0]);
		}

		public List<School> GetDepList(Guid schoolid)
		{
			string sql = "select * from School where ParentId in (select Id from School where ParentId = @Id and IsDeleted = 0) and IsDeleted = 0 ORDER BY Name";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)schoolid) };
			return SqlHelper.ExecuteList<School>(sql, sqlParameter);
		}

		public List<SchoolLogDetail> GetDownSchoolDetail(string name, string Startime, string teacherName, string Endtime, string ip, int pageIndex, int pageCount)
		{
			List<SqlParameter> par = new List<SqlParameter>();
			string sql = " SELECT * FROM(\r\n                                select COUNT(id) as DownLoadCount ,Ip,Name,ISNULL(teachername,'ip下载') as tag1 ,isnull(sellname,'') as tag2\r\n                                ,ROW_NUMBER() OVER(ORDER BY ip DESC) rownum ,SchoolId as id \r\n                                 from (\r\n                                select dbo.LogDetail.Id, School.Name,dbo.LogDetail.ip,\r\n                                teachername = isnull((select name from dbo.UserInfo where id=dbo.LogDetail.TeacherId),'ip下载')\r\n                                ,sellname = (select name from dbo.UserInfo where id=School.sellid),LogDetail.SchoolId\r\n                                from dbo.LogDetail,School  where dbo.LogDetail.SchoolId = school.Id and \r\n                                Status = 2     \r\n                                ";
			if (!string.IsNullOrEmpty(name))
			{
				sql = string.Concat(sql, " AND School.[Name] LIKE @name ");
				par.Add(new SqlParameter("@name", string.Concat("%", name, "%")));
			}
			if (string.IsNullOrEmpty(Startime))
			{
				sql = string.Concat(sql, "  and  DateDiff(hh,dbo.LogDetail.CreateTime,getDate())<=24 ");
			}
			if (!string.IsNullOrEmpty(Startime))
			{
				sql = string.Concat(sql, " and  dbo.LogDetail.CreateTime >= @Startime");
				par.Add(new SqlParameter("@Startime", Startime));
			}
			if (!string.IsNullOrEmpty(Endtime))
			{
				sql = string.Concat(sql, " and  dbo.LogDetail.CreateTime <= @Endtime");
				par.Add(new SqlParameter("@Endtime", Endtime));
			}
			if (!string.IsNullOrEmpty(ip))
			{
				sql = string.Concat(sql, " and  dbo.LogDetail.Ip = @Ip");
				par.Add(new SqlParameter("@Ip", ip));
			}
			sql = string.Concat(sql, "   ) as  a ");
			if (!string.IsNullOrEmpty(teacherName))
			{
				sql = string.Concat(sql, "   where teachername  like @teachername");
				par.Add(new SqlParameter("@teachername", string.Concat("%", teacherName, "%")));
			}
			sql = string.Concat(sql, " group by Ip,Name,teachername,sellname,SchoolId  ) as t\r\n                      WHERE rownum>=(@pageCount*(@pageIndex-1)+1) AND rownum<=@pageIndex*@pageCount");
			par.Add(new SqlParameter("@pageIndex", (object)pageIndex));
			par.Add(new SqlParameter("@pageCount", (object)pageCount));
			return SqlHelper.ExecuteList<SchoolLogDetail>(sql, par.ToArray());
		}

		public int GetDownSchoolDetailCount(string name, string Startime, string Endtime, string ip, string teacherName)
		{
			List<SqlParameter> par = new List<SqlParameter>();
			string sql = " SELECT count(*) FROM(\r\n                                select COUNT(id) as num,Ip,Name,ISNULL(teachername,'ip下载') as teachername ,sellname\r\n                                ,ROW_NUMBER() OVER(ORDER BY ip DESC) rownum  \r\n                                 from (\r\n                                select dbo.LogDetail.Id, School.Name,dbo.LogDetail.ip, teachername = (select name from dbo.UserInfo where id=dbo.LogDetail.TeacherId)\r\n                                ,sellname = (select name from dbo.UserInfo where id=School.sellid)\r\n                                from dbo.LogDetail,School  where dbo.LogDetail.SchoolId = school.Id and \r\n                                Status = 2\r\n                                ";
			if (!string.IsNullOrEmpty(name))
			{
				sql = string.Concat(sql, " AND School.[Name] LIKE @name ");
				par.Add(new SqlParameter("@name", string.Concat("%", name, "%")));
			}
			if (string.IsNullOrEmpty(Startime))
			{
				sql = string.Concat(sql, "  and  DateDiff(hh,dbo.LogDetail.CreateTime,getDate())<=24 ");
			}
			if (!string.IsNullOrEmpty(Startime))
			{
				sql = string.Concat(sql, " and  dbo.LogDetail.CreateTime >= @Startime");
				par.Add(new SqlParameter("@Startime", Startime));
			}
			if (!string.IsNullOrEmpty(Endtime))
			{
				sql = string.Concat(sql, " and  dbo.LogDetail.CreateTime <= @Endtime");
				par.Add(new SqlParameter("@Endtime", Endtime));
			}
			if (!string.IsNullOrEmpty(ip))
			{
				sql = string.Concat(sql, " and  dbo.LogDetail.Ip = @Ip");
				par.Add(new SqlParameter("@Ip", ip));
			}
			sql = string.Concat(sql, "   ) as  a  ");
			sql = string.Concat(sql, " group by Ip,Name,teachername,sellname  ) as t");
			if (!string.IsNullOrEmpty(teacherName))
			{
				sql = string.Concat(sql, "   where teachername  like @teachername");
				par.Add(new SqlParameter("@teachername", string.Concat("%", teacherName, "%")));
			}
			return (int)SqlHelper.ExecuteScalar(sql, par.ToArray());
		}

		public School GetEntity(string name)
		{
			string sql = "SELECT * FROM School WHERE Name = @Name AND IsDeleted = 0";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Name", name) };
			return SqlHelper.ExecuteEntity<School>(sql, sqlParameter);
		}

		public School GetEntity(Guid id)
		{
			string sql = "SELECT * FROM School WHERE Id = @Id AND IsDeleted = 0";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteEntity<School>(sql, sqlParameter);
		}

		public School GetEntityById(Guid id)
		{
			string sql = "SELECT * FROM School WHERE Id = @Id ";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteEntity<School>(sql, sqlParameter);
		}

		public School GetEntityDep(string name, Guid schoolid)
		{
			string sql = "SELECT * FROM School WHERE Name = @Name AND IsDeleted = 0 and Tier=2 and ParentId=@schoolid";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Name", name), new SqlParameter("@schoolid", (object)schoolid) };
			return SqlHelper.ExecuteEntity<School>(sql, sqlParameter);
		}

		public School GetEntityFac(string name, Guid schoolid)
		{
			string sql = "SELECT * FROM School WHERE Name = @Name AND IsDeleted = 0 and Tier=1 and ParentId=@schoolid";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Name", name), new SqlParameter("@schoolid", (object)schoolid) };
			return SqlHelper.ExecuteEntity<School>(sql, sqlParameter);
		}

		public int GetFacDepCount(Guid id, string name, int tier)
		{
			string sql = "SELECT COUNT(Id) FROM School WHERE IsDeleted = 0 AND School.Tier =@tier ";
			if (id != Guid.Empty)
			{
				sql = string.Concat(sql, " AND School.ParentId=@id ");
			}
			if (!string.IsNullOrEmpty(name.Trim()))
			{
				sql = string.Concat(sql, " AND School.[Name] LIKE @name ");
			}
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@id", (object)id), new SqlParameter("@name", string.Concat("%", name, "%")), new SqlParameter("@tier", (object)tier) };
			return (int)SqlHelper.ExecuteScalar(sql, sqlParameter);
		}

		public List<School> GetFacDepList(Guid id, string name, int tier, int pageIndex, int pageCount)
		{
			string sql = " SELECT * FROM(\r\n                                SELECT School.Id ,\r\n                                School.[Name],\r\n                                School.EndTime,\r\n                                School.StartTime,\r\n                                School.StopRemind,\r\n                                UserInfo.[Name] Tag1,\r\n                                STUFF((SELECT\r\n                                     ',' + [Module].[Name]\r\n                                   FROM\r\n                                     [Module] join School_Module\r\n                                     on [Module].Id = School_Module.ModuleId\r\n                                     where School_Module.SchoolId = School.Id\r\n                                   FOR XML PATH('')\r\n                                   ),\r\n                                   1, 1, '') as Tag2 ,\r\n                                ROW_NUMBER() OVER(ORDER BY School.CreateTime DESC) rownum\r\n                                FROM dbo.School \r\n                                JOIN UserInfo \r\n                                ON UserInfo.Id = School.SellID\r\n                                WHERE School.IsDeleted = 0 AND School.Tier =@tier";
			if (id != Guid.Empty)
			{
				sql = string.Concat(sql, " AND School.ParentId=@id ");
			}
			if (!string.IsNullOrEmpty(name))
			{
				sql = string.Concat(sql, " AND School.[Name] LIKE @name ");
			}
			sql = string.Concat(sql, " ) as t\r\n                      WHERE rownum>=(@pageCount*(@pageIndex-1)+1) AND rownum<=@pageIndex*@pageCount");
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@pageIndex", (object)pageIndex), new SqlParameter("@pageCount", (object)pageCount), new SqlParameter("@id", (object)id), new SqlParameter("@name", string.Concat("%", name, "%")), new SqlParameter("@tier", (object)tier) };
			return SqlHelper.ExecuteList<School>(sql, sqlParameter);
		}

		public List<School> GetList(Guid id, string name, string TrialTag, string Startime, string Endtime, int tier, int pageIndex, int pageCount)
		{
			List<SqlParameter> par = new List<SqlParameter>();
			string sql = " SELECT * FROM(\r\n                                select School.Id ,\r\n                                School.[Name],\r\n                                School.EndTime,\r\n                                School.StartTime,\r\n                                School.StopRemind,\r\n                                School.TrialType,\r\n                                UserInfo.[Name] Tag1,\r\n                                STUFF((SELECT\r\n                                     ',' + [Module].[Name]\r\n                                   FROM\r\n                                     [Module] join School_Module\r\n                                     on [Module].Id = School_Module.ModuleId\r\n                                     where School_Module.SchoolId = School.Id\r\n                                     order by [Module].CreateTime\r\n                                   FOR XML PATH('')\r\n                                   ),\r\n                                   1, 1, '') as Tag2 ,\r\n                                isnull(logt.PressCount,0) as PressCount,isnull(logt.DownLoadCount,0) as DownLoadCount,\r\n                                ROW_NUMBER() OVER(ORDER BY School.CreateTime DESC) rownum\r\n                                FROM dbo.School \r\n                                LEFT JOIN UserInfo \r\n                                ON UserInfo.Id = School.SellID\r\n                                left join (\r\n                                select sum(case Status when 0 then 1 else 0 end) as PressCount, \r\n                                sum(case Status when 2 then 1 else 0 end) as DownLoadCount,\r\n                                SchoolId from LogDetail group by SchoolId\r\n                                ) as logt on School.Id=logt.SchoolId\r\n                                WHERE School.IsDeleted = 0 AND (School.Tier =@tier OR School.ParentId IS NULL) ";
			par.Add(new SqlParameter("@tier", (object)tier));
			if (id != Guid.Empty)
			{
				sql = string.Concat(sql, " AND School.SellId=@id ");
				par.Add(new SqlParameter("@id", (object)id));
			}
			if (!string.IsNullOrEmpty(name))
			{
				sql = string.Concat(sql, " AND School.[Name] LIKE @name ");
				par.Add(new SqlParameter("@name", string.Concat("%", name, "%")));
			}
			if (!string.IsNullOrEmpty(Startime))
			{
				sql = string.Concat(sql, " and  EndTime >= @Startime");
				par.Add(new SqlParameter("@Startime", Startime));
			}
			if (!string.IsNullOrEmpty(Endtime))
			{
				sql = string.Concat(sql, " and  EndTime <= @Endtime");
				par.Add(new SqlParameter("@Endtime", Endtime));
			}
			if (!string.IsNullOrEmpty(TrialTag))
			{
				sql = string.Concat(sql, " and  TrialType = @TrialType");
				par.Add(new SqlParameter("@TrialType", TrialTag));
			}
			sql = string.Concat(sql, " ) as t\r\n                      WHERE rownum>=(@pageCount*(@pageIndex-1)+1) AND rownum<=@pageIndex*@pageCount");
			par.Add(new SqlParameter("@pageIndex", (object)pageIndex));
			par.Add(new SqlParameter("@pageCount", (object)pageCount));
			return SqlHelper.ExecuteList<School>(sql, par.ToArray());
		}

		public List<School> GetList(int tier)
		{
			string sql = "select * from School where Tier =@tier";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@tier", (object)tier) };
			return SqlHelper.ExecuteList<School>(sql, sqlParameter);
		}

		public List<School> GetList()
		{
			return SqlHelper.ExecuteList<School>("SELECT * FROM School WHERE IsDeleted = 0", new SqlParameter[0]);
		}

		public List<School> GetListBySchoolId(Guid id)
		{
			string sql = "select * from School where (Id = @id OR ParentId = @id OR ParentId IN (select Id from School where ParentId = @id)) AND IsDeleted = 0 ORDER BY Name";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteList<School>(sql, sqlParameter);
		}

		public List<SchoolLogDetail> GetListLogDetail(Guid id, Guid privinceID, string name, string TrialTag, string Startime, string Endtime, int tier, Guid schoolid, int pageIndex, int pageCount)
		{
			List<SqlParameter> par = new List<SqlParameter>();
			string sql = " SELECT * FROM(\r\n                                select School.Id ,\r\n                                School.[Name],\r\n                                School.EndTime,\r\n                                School.StartTime,\r\n                                School.StopRemind,\r\n                                School.TrialType,\r\n                                UserInfo.[Name] Tag1,\r\n                                STUFF((SELECT\r\n                                     ',' + [Module].[Name]\r\n                                   FROM\r\n                                     [Module] join School_Module\r\n                                     on [Module].Id = School_Module.ModuleId\r\n                                     where School_Module.SchoolId = School.Id\r\n                                     order by [Module].CreateTime\r\n                                   FOR XML PATH('')\r\n                                   ),\r\n                                   1, 1, '') as Tag2 ,isnull((SELECT [Name] FROM dbo.Province where ID= School.ProvinceId),'') as ProvinceName,\r\n                                isnull(logt.PressCount,0) as PressCount,isnull(logt.DownLoadCount,0) as DownLoadCount,\r\n                                ROW_NUMBER() OVER(ORDER BY School.CreateTime DESC) rownum\r\n                                FROM dbo.School \r\n                                LEFT JOIN UserInfo \r\n                                ON UserInfo.Id = School.SellID\r\n                                left join (\r\n                                select sum(case Status when 0 then 1 else 0 end) as PressCount, \r\n                                sum(case Status when 2 then 1 else 0 end) as DownLoadCount,\r\n                                SchoolId from LogDetail group by SchoolId\r\n                                ) as logt on School.Id=logt.SchoolId\r\n                                WHERE School.IsDeleted = 0 AND (School.Tier =@tier OR School.ParentId IS NULL) ";
			par.Add(new SqlParameter("@tier", (object)tier));
			if (id != Guid.Empty)
			{
				sql = string.Concat(sql, " AND School.SellId=@id ");
				par.Add(new SqlParameter("@id", (object)id));
			}
			if (!string.IsNullOrEmpty(name))
			{
				sql = string.Concat(sql, " AND School.[Name] LIKE @name ");
				par.Add(new SqlParameter("@name", string.Concat("%", name, "%")));
			}
			if (!string.IsNullOrEmpty(Startime))
			{
				sql = string.Concat(sql, " and  EndTime >= @Startime");
				par.Add(new SqlParameter("@Startime", Startime));
			}
			if (!string.IsNullOrEmpty(Endtime))
			{
				sql = string.Concat(sql, " and  EndTime <= @Endtime");
				par.Add(new SqlParameter("@Endtime", Endtime));
			}
			if (!string.IsNullOrEmpty(TrialTag))
			{
				sql = string.Concat(sql, " and  TrialType = @TrialType");
				par.Add(new SqlParameter("@TrialType", TrialTag));
			}
			if (privinceID != Guid.Empty)
			{
				sql = string.Concat(sql, " and  ProvinceId = @ProvinceId");
				par.Add(new SqlParameter("@ProvinceId", (object)privinceID));
			}
			if (schoolid != Guid.Empty)
			{
				sql = string.Concat(sql, " and  School.Id = @ID");
				par.Add(new SqlParameter("@ID", (object)schoolid));
			}
			sql = string.Concat(sql, " ) as t\r\n                      WHERE rownum>=(@pageCount*(@pageIndex-1)+1) AND rownum<=@pageIndex*@pageCount");
			par.Add(new SqlParameter("@pageIndex", (object)pageIndex));
			par.Add(new SqlParameter("@pageCount", (object)pageCount));
			return SqlHelper.ExecuteList<SchoolLogDetail>(sql, par.ToArray());
		}

		public List<School> GetListSub(Guid id)
		{
			string sql = "select * from School where ParentId = @Id and IsDeleted = 0 ORDER BY Name";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteList<School>(sql, sqlParameter);
		}

		public List<School> GetListSup(Guid parentId)
		{
			string sql = "WITH CTE AS\r\n                            (\r\n                            SELECT * FROM School WHERE Id = @Id\r\n                            UNION ALL\r\n                            SELECT School.* FROM School join CTE ON School.Id = CTE.ParentId\r\n                            )\r\n                            SELECT * FROM CTE ORDER BY Tier";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)parentId) };
			return SqlHelper.ExecuteList<School>(sql, sqlParameter);
		}

		public List<School> GetPage(int pageIndex, int pageCount)
		{
			string sql = "SELECT * FROM(SELECT *,(ROW_NUMBER() OVER(ORDER BY Id)) rownum FROM School WHERE IsDeleted = 0) t WHERE rownum>=(@pageCount*(@pageIndex-1)+1) AND rownum<=@pageIndex*@pageCount";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@pageIndex", (object)pageIndex), new SqlParameter("@pageCount", (object)pageCount) };
			return SqlHelper.ExecuteList<School>(sql, sqlParameter);
		}

		public int GetReminderCountDateAll(string Tier, string TrialTag, string Startime, string Endtime, int? loginType, Guid? loginId)
		{
			List<SqlParameter> par = new List<SqlParameter>();
			string sql = " SELECT COUNT(Id) FROM School where 1=1 and  tier=0 ";
			if (string.IsNullOrEmpty(Tier))
			{
				if (!string.IsNullOrEmpty(Startime))
				{
					sql = string.Concat(sql, " and  EndTime >= @Startime");
					par.Add(new SqlParameter("@Startime", Startime));
				}
				if (!string.IsNullOrEmpty(Endtime))
				{
					sql = string.Concat(sql, " and  EndTime <= @Endtime");
					par.Add(new SqlParameter("@Endtime", Endtime));
				}
			}
			else
			{
				if (Tier.Equals("0"))
				{
					if (!string.IsNullOrEmpty(Startime))
					{
						sql = string.Concat(sql, " and  EndTime >= @Startime");
						par.Add(new SqlParameter("@Startime", Startime));
					}
					sql = string.Concat(sql, " and  EndTime < getdate()");
				}
				if (Tier.Equals("1"))
				{
					sql = string.Concat(sql, " and  EndTime > getdate()");
					if (!string.IsNullOrEmpty(Endtime))
					{
						sql = string.Concat(sql, " and  EndTime <= @Endtime");
						par.Add(new SqlParameter("@Endtime", Endtime));
					}
				}
			}
			if (!string.IsNullOrEmpty(TrialTag))
			{
				sql = string.Concat(sql, " and  TrialType = @TrialType");
				par.Add(new SqlParameter("@TrialType", TrialTag));
			}
			
			if (loginType!= null)
			{
				sql = string.Concat(sql, " and  SellId = @SellId");
				par.Add(new SqlParameter("@SellId", (object)loginId));
			}
			return (int)SqlHelper.ExecuteScalar(sql, par.ToArray());
		}

		public List<Reminder> GetReminderDateAll(string Tier, string TrialTag, string Startime, string Endtime, int? loginType, Guid? loginId, int pageIndex, int pageCount)
		{
			List<SqlParameter> par = new List<SqlParameter>();
			string sql = "select * from (\r\nSELECT Id,Name,StartTime,EndTime,StopRemind,IsDeleted,\r\nSellName=(select name from dbo.UserInfo where id=School.sellid),TrialType ,\r\nROW_NUMBER() OVER ( ORDER BY   EndTime desc) rownum \r\nFROM  School where   tier=0 ";
			if (string.IsNullOrEmpty(Tier))
			{
				if (!string.IsNullOrEmpty(Startime))
				{
					sql = string.Concat(sql, " and  EndTime >= @Startime");
					par.Add(new SqlParameter("@Startime", Startime));
				}
				if (!string.IsNullOrEmpty(Endtime))
				{
					sql = string.Concat(sql, " and  EndTime <= @Endtime");
					par.Add(new SqlParameter("@Endtime", Endtime));
				}
			}
			else
			{
				if (Tier.Equals("0"))
				{
					if (!string.IsNullOrEmpty(Startime))
					{
						sql = string.Concat(sql, " and  EndTime >= @Startime");
						par.Add(new SqlParameter("@Startime", Startime));
					}
					sql = string.Concat(sql, " and  EndTime < getdate()");
				}
				if (Tier.Equals("1"))
				{
					sql = string.Concat(sql, " and  EndTime > getdate()");
					if (!string.IsNullOrEmpty(Endtime))
					{
						sql = string.Concat(sql, " and  EndTime <= @Endtime");
						par.Add(new SqlParameter("@Endtime", Endtime));
					}
				}
			}
			if (!string.IsNullOrEmpty(TrialTag))
			{
				sql = string.Concat(sql, " and  TrialType = @TrialType");
				par.Add(new SqlParameter("@TrialType", TrialTag));
			}
			
			if (loginType!=null)
			{
				sql = string.Concat(sql, " and  SellId = @SellId");
				par.Add(new SqlParameter("@SellId", (object)loginId));
			}
			sql = string.Concat(sql, " ) as t WHERE rownum>=(@pageCount*(@pageIndex-1)+1) AND rownum<=@pageIndex*@pageCount");
			par.Add(new SqlParameter("@pageIndex", (object)pageIndex));
			par.Add(new SqlParameter("@pageCount", (object)pageCount));
			return SqlHelper.ExecuteList<Reminder>(sql, par.ToArray());
		}

		public List<Exceptions> GetSchoolDownExceptionAll()
		{
			return SqlHelper.ExecuteList<Exceptions>(" select Id,Name,Count=1,Daycountnum=1,Datatime='',StopRemind,SellName=(select name from dbo.UserInfo where id=School.sellid),TrialType from \r\ndbo.School where tier=0 and id in (select SchoolId from  dbo.LogDetail where Status=2 and Tag1=0) order by EndTime desc", new SqlParameter[0]);
		}

		public List<School> GetSchoolList()
		{
			return SqlHelper.ExecuteList<School>("select * from School where ParentId IS NULL and isdeleted=0 ORDER BY Name", new SqlParameter[0]);
		}

		public List<Reminder> GetSchoolStopDate(int? pageIndex, int pageCount)
		{
			string sql = string.Concat("select * from (\r\n SELECT Id,Name,StartTime,EndTime,StopRemind,\r\nSellName=(select name from dbo.UserInfo where id=School.sellid),TrialType ,\r\nROW_NUMBER() OVER ( ORDER BY   EndTime desc) rownum \r\nFROM  School where endtime<=getdate() and tier=0  ", " ) as t WHERE rownum>=(@pageCount*(@pageIndex-1)+1) AND rownum<=@pageIndex*@pageCount");
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@pageIndex", (object)pageIndex), new SqlParameter("@pageCount", (object)pageCount) };
			return SqlHelper.ExecuteList<Reminder>(sql, sqlParameter);
		}

		public List<Reminder> GetSchoolStopDateAll(int? pageIndex, int pageCount)
		{
			string sql = string.Concat("select * from (\r\nSELECT Id,Name,StartTime,EndTime,StopRemind,\r\nSellName=(select name from dbo.UserInfo where id=School.sellid),TrialType ,\r\nROW_NUMBER() OVER ( ORDER BY   EndTime desc) rownum \r\nFROM  School where   tier=0 ", " ) as t WHERE rownum>=(@pageCount*(@pageIndex-1)+1) AND rownum<=@pageIndex*@pageCount");
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@pageIndex", (object)pageIndex), new SqlParameter("@pageCount", (object)pageCount) };
			return SqlHelper.ExecuteList<Reminder>(sql, sqlParameter);
		}

		public List<Reminder> GetSchoolStopDates(int? pageIndex, int pageCount)
		{
			string sql = string.Concat(" select * from (\r\n  SELECT Id,Name,StartTime,EndTime,StopRemind,\r\nSellName=(select name from dbo.UserInfo where id=School.sellid),TrialType, \r\nROW_NUMBER() OVER ( ORDER BY   EndTime desc) rownum \r\nFROM  School where endtime>getdate() and tier=0 ", " ) as t WHERE rownum>=(@pageCount*(@pageIndex-1)+1) AND rownum<=@pageIndex*@pageCount");
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@pageIndex", (object)pageIndex), new SqlParameter("@pageCount", (object)pageCount) };
			return SqlHelper.ExecuteList<Reminder>(sql, sqlParameter);
		}

		public int SoftDelete(Guid id)
		{
			string sql = "UPDATE School SET IsDeleted = 1 WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}

		public int Update(School school)
		{
			string sql = "UPDATE School SET Name = @Name,ParentId = @ParentId,Tier = @Tier,StartTime = @StartTime,EndTime = @EndTime,StopRemind = @StopRemind,SellId = @SellId,Remark = @Remark,LogoImg = @LogoImg,NameImg = @NameImg,BannerImgs = @BannerImgs,LanCourseCount = @LanCourseCount,LanResourceCount = @LanResourceCount,TrialType = @TrialType,CreateTime = @CreateTime,ModifyTime = @ModifyTime,IsDeleted = @IsDeleted,Tag1 = @Tag1,Tag2 = @Tag2,ProvinceId=@ProvinceId WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)school.Id), new SqlParameter("@Name", SqlHelper.ToDBValue(school.Name)), new SqlParameter("@ParentId", SqlHelper.ToDBValue(school.ParentId)), new SqlParameter("@Tier", SqlHelper.ToDBValue(school.Tier)), new SqlParameter("@StartTime", SqlHelper.ToDBValue(school.StartTime)), new SqlParameter("@EndTime", SqlHelper.ToDBValue(school.EndTime)), new SqlParameter("@StopRemind", SqlHelper.ToDBValue(school.StopRemind)), new SqlParameter("@SellId", SqlHelper.ToDBValue(school.SellId)), new SqlParameter("@Remark", SqlHelper.ToDBValue(school.Remark)), new SqlParameter("@LogoImg", SqlHelper.ToDBValue(school.LogoImg)), new SqlParameter("@NameImg", SqlHelper.ToDBValue(school.NameImg)), new SqlParameter("@BannerImgs", SqlHelper.ToDBValue(school.BannerImgs)), new SqlParameter("@LanCourseCount", SqlHelper.ToDBValue(school.LanCourseCount)), new SqlParameter("@LanResourceCount", SqlHelper.ToDBValue(school.LanResourceCount)), new SqlParameter("@TrialType", SqlHelper.ToDBValue(school.TrialType)), new SqlParameter("@CreateTime", SqlHelper.ToDBValue(school.CreateTime)), new SqlParameter("@ModifyTime", SqlHelper.ToDBValue(school.ModifyTime)), new SqlParameter("@IsDeleted", SqlHelper.ToDBValue(school.IsDeleted)), new SqlParameter("@Tag1", SqlHelper.ToDBValue(school.Tag1)), new SqlParameter("@Tag2", SqlHelper.ToDBValue(school.Tag2)), new SqlParameter("@ProvinceId", SqlHelper.ToDBValue(school.ProvinceId)) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}
	}
}