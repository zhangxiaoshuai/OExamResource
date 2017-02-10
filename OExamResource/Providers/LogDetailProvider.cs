using Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Providers
{
	public class LogDetailProvider
	{
		private readonly static LogDetailProvider InstanceObj;

		public static LogDetailProvider Instance
		{
			get
			{
				return LogDetailProvider.InstanceObj;
			}
		}

		static LogDetailProvider()
		{
			LogDetailProvider.InstanceObj = new LogDetailProvider();
		}

		private LogDetailProvider()
		{
		}

		public LogDetail Create(LogDetail logDetail)
		{
			LogDetail logDetail1;
			if (logDetail.Id == Guid.Empty)
			{
				logDetail.Id = Guid.NewGuid();
			}
			string sql = "INSERT INTO LogDetail (Id, ResourceId, TeacherId, SchoolId, Ip, Status, Tag1, Tag2, CreateTime, IsDeleted)  VALUES (@Id, @ResourceId, @TeacherId, @SchoolId, @Ip, @Status, @Tag1, @Tag2, @CreateTime, @IsDeleted)";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", SqlHelper.ToDBValue(logDetail.Id)), new SqlParameter("@ResourceId", SqlHelper.ToDBValue(logDetail.ResourceId)), new SqlParameter("@TeacherId", SqlHelper.ToDBValue(logDetail.TeacherId)), new SqlParameter("@SchoolId", SqlHelper.ToDBValue(logDetail.SchoolId)), new SqlParameter("@Ip", SqlHelper.ToDBValue(logDetail.Ip)), new SqlParameter("@Status", SqlHelper.ToDBValue(logDetail.Status)), new SqlParameter("@Tag1", SqlHelper.ToDBValue(logDetail.Tag1)), new SqlParameter("@Tag2", SqlHelper.ToDBValue(logDetail.Tag2)), new SqlParameter("@CreateTime", SqlHelper.ToDBValue(logDetail.CreateTime)), new SqlParameter("@IsDeleted", SqlHelper.ToDBValue(logDetail.IsDeleted)) };
			if (SqlHelper.ExecuteNonQuery(sql, sqlParameter) > 0)
			{
				logDetail1 = logDetail;
			}
			else
			{
				logDetail1 = null;
			}
			return logDetail1;
		}

		public int Delete(Guid id)
		{
			string sql = "DELETE LogDetail WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}

		public List<DownloadDetails> GetCateDetailsSE(Guid schoolid, int status, string tag)
		{
			string sql = "SELECT   EPC.Id AS Id,\r\n                                    isnull(sum(ExamPaper.ItemCount),0) AS DownloadCount,\r\n                                    EPC.Name AS Tag2\r\n                           FROM  LogDetail \r\n                           INNER JOIN ExamPaper \r\n                           ON LogDetail.ResourceId = ExamPaper.Id \r\n                           INNER JOIN ExamPaperType EPA\r\n                           ON EPA.Id = ExamPaper.TypeId \r\n                           INNER JOIN ExamPaperType EPB\r\n                           ON EPB.Id = EPA.Parentid\r\n                           INNER JOIN ExamPaperType EPC\r\n                           ON EPC.Id = EPB.Parentid \r\n                           WHERE LogDetail.SchoolId = @SchoolId \r\n                                 AND LogDetail.Status = @Status AND LogDetail.Tag1 = @Tag1\r\n                           GROUP BY EPC.Id,EPC.name\r\n                           ORDER BY DownloadCount DESC";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@SchoolId", (object)schoolid), new SqlParameter("@Status", (object)status), new SqlParameter("@Tag1", tag) };
			return SqlHelper.ExecuteList<DownloadDetails>(sql, sqlParameter);
		}

		public List<DownloadDetails> GetCateDetailsSR(Guid schoolid, int status, string tag)
		{
			string sql = "SELECT  Category.Id,\r\n                                   COUNT(1) AS DownloadCount,\r\n                                   Category.[Name]  AS Tag2\r\n                           FROM  LogDetail \r\n                           INNER JOIN Resource \r\n                           ON LogDetail.ResourceId = Resource.Id \r\n                           INNER JOIN Category \r\n                           ON Category.Id = Resource.CategoryId \r\n                           WHERE LogDetail.SchoolId = @SchoolId \r\n                                 AND LogDetail.Status = @Status AND LogDetail.Tag1 = @Tag1\r\n                           GROUP BY Category.[Name],Category.Id\r\n                           ORDER BY DownloadCount DESC ";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@SchoolId", (object)schoolid), new SqlParameter("@Status", (object)status), new SqlParameter("@Tag1", tag) };
			return SqlHelper.ExecuteList<DownloadDetails>(sql, sqlParameter);
		}

		public List<DownloadDetails> GetCateDetailsSRnew(Guid schoolid, int status, string tag)
		{
			string sql = "with CTE AS\r\n(\r\n SELECT Id,name,id as genID FROM ResourceCategory WHERE IsDeleted=0  AND Tier=0 AND ParentId is null\r\nUNION ALL\r\nSELECT ResourceCategory.Id,CTE.name,CTE.genID\r\nFROM ResourceCategory join CTE ON \r\nResourceCategory.ParentId = CTE.Id AND ResourceCategory.IsDeleted=0\r\n)\r\nselect name as Tag2 ,genID as id ,sum([Count]) as DownloadCount  from (\r\n select *  from CTE as mulu\t \r\n inner join \r\n(\r\nselect ResourcePack.CategoryId ,ResourcePack.[Count] from LogDetail\r\ninner join ResourcePack on LogDetail.ResourceId=ResourcePack.id \r\nWHERE LogDetail.SchoolId = @SchoolId \r\n                                 AND LogDetail.Status = @Status AND LogDetail.Tag1 = @Tag1\r\nunion all \r\nselect ResourceMaterial.CategoryId ,1 as [count] from LogDetail\r\ninner join ResourceMaterial on LogDetail.ResourceId=ResourceMaterial.id \r\nWHERE LogDetail.SchoolId = @SchoolId \r\n                                 AND LogDetail.Status = @Status AND LogDetail.Tag1 = @Tag1\r\n)\r\nas res on mulu.id= res.CategoryId\r\n) as resmulu group by name ,genID order by DownloadCount desc";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@SchoolId", (object)schoolid), new SqlParameter("@Status", (object)status), new SqlParameter("@Tag1", tag) };
			return SqlHelper.ExecuteList<DownloadDetails>(sql, sqlParameter);
		}

		public List<DownloadDetails> GetCateDetailsTE(Guid teacherid, int status, string tag)
		{
			string sql = "SELECT   EPC.Id AS Id,\r\n                                    COUNT(1) AS DownloadCount,\r\n                                    EPC.Name AS Tag2\r\n                           FROM  LogDetail \r\n                           INNER JOIN ExamPaper \r\n                           ON LogDetail.ResourceId = ExamPaper.Id \r\n                           INNER JOIN ExamPaperType EPA\r\n                           ON EPA.Id = ExamPaper.TypeId \r\n                           INNER JOIN ExamPaperType EPB\r\n                           ON EPB.Id = EPA.Parentid\r\n                           INNER JOIN ExamPaperType EPC\r\n                           ON EPC.Id = EPB.Parentid \r\n                           WHERE LogDetail.TeacherId = @TeacherId \r\n                                 AND LogDetail.Status = @Status AND LogDetail.Tag1 = @Tag1\r\n                           GROUP BY EPC.Id,EPC.name\r\n                           ORDER BY DownloadCount DESC";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@TeacherId", (object)teacherid), new SqlParameter("@Status", (object)status), new SqlParameter("@Tag1", tag) };
			return SqlHelper.ExecuteList<DownloadDetails>(sql, sqlParameter);
		}

		public List<DownloadDetails> GetCateDetailsTR(Guid teacherid, int status, string tag)
		{
			string sql = "SELECT   Category.Id,\r\n                                    COUNT(1) AS DownloadCount,\r\n                                    Category.[Name]  AS Tag2\r\n                           FROM  LogDetail \r\n                           INNER JOIN Resource \r\n                           ON LogDetail.ResourceId = Resource.Id \r\n                           INNER JOIN Category \r\n                           ON Category.Id = Resource.CategoryId \r\n                           WHERE LogDetail.TeacherId = @TeacherId\r\n                                 AND LogDetail.Status = @Status AND LogDetail.Tag1 = @Tag1\r\n                           GROUP BY  Category.[Name], Category.Id\r\n                           ORDER BY DownloadCount DESC ";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@TeacherId", (object)teacherid), new SqlParameter("@Status", (object)status), new SqlParameter("@Tag1", tag) };
			return SqlHelper.ExecuteList<DownloadDetails>(sql, sqlParameter);
		}

		public int GetCount(Guid schoolid, int status, string tag)
		{
			string sql = "SELECT COUNT(1) as Count FROM LogDetail \r\n                            WHERE SchoolId = @SchoolId \r\n                            AND Status = @Status AND Tag1 = @Tag1";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@SchoolId", (object)schoolid), new SqlParameter("@Status", (object)status), new SqlParameter("@Tag1", tag) };
			return (int)SqlHelper.ExecuteScalar(sql, sqlParameter);
		}

		public int GetCount()
		{
			return (int)SqlHelper.ExecuteScalar("SELECT COUNT(Id) FROM LogDetail WHERE IsDeleted = 0", new SqlParameter[0]);
		}

		public int GetCountExam(Guid schoolid, int status, string tag, int numExam)
		{
			string sql = "SELECT  isnull(sum(ExamPaper.ItemCount)/@numExam,0)  as Count FROM LogDetail inner join ExamPaper on LogDetail.ResourceId=ExamPaper.id\r\n                            WHERE LogDetail.SchoolId = @SchoolId \r\n                            AND LogDetail.Status = @Status AND LogDetail.Tag1 = @Tag1";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@numExam", (object)numExam), new SqlParameter("@SchoolId", (object)schoolid), new SqlParameter("@Status", (object)status), new SqlParameter("@Tag1", tag) };
			return (int)SqlHelper.ExecuteScalar(sql, sqlParameter);
		}

		public int GetCountResource(Guid schoolid, int status, string tag)
		{
			string sql = " select sum([Count]) as [count] from (\r\n                            SELECT isnull(SUM([Count]),0) as Count FROM LogDetail inner join ResourcePack\r\n                            on ResourceId =ResourcePack.id\r\n                            WHERE LogDetail.SchoolId = @SchoolId \r\n                            AND LogDetail.Status = @Status AND LogDetail.Tag1 = @Tag1\r\n                            union all \r\n                            SELECT count(ResourceMaterial.id) as Count FROM LogDetail inner join ResourceMaterial\r\n                            on ResourceId =ResourceMaterial.id\r\n                            WHERE LogDetail.SchoolId = @SchoolId \r\n                            AND LogDetail.Status = @Status AND LogDetail.Tag1 = @Tag1\r\n                            ) as a\r\n                          ";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@SchoolId", (object)schoolid), new SqlParameter("@Status", (object)status), new SqlParameter("@Tag1", tag) };
			return (int)SqlHelper.ExecuteScalar(sql, sqlParameter);
		}

		public string GetCounts(Guid schoolid, int status, string tag)
		{
			string sql = "select count(1) FROM LogDetail  where SchoolId = @SchoolId AND Status = @Status AND Tag1 = @Tag1";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@SchoolId", (object)schoolid), new SqlParameter("@Status", (object)status), new SqlParameter("@Tag1", tag) };
			return SqlHelper.ExecuteScalar(sql, sqlParameter).ToString();
		}

		public int GetCountT(Guid teacherid, int status, string tag)
		{
			string sql = "SELECT COUNT(1) as Count FROM LogDetail WHERE TeacherId = @TeacherId AND Status = @Status AND Tag1 = @Tag1";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@TeacherId", (object)teacherid), new SqlParameter("@Status", (object)status), new SqlParameter("@Tag1", tag) };
			return (int)SqlHelper.ExecuteScalar(sql, sqlParameter);
		}

		public int GetCountTest(Guid schoolid, int status, string tag, int num)
		{
			string sql = "SELECT isnull(sum(ItemCount)/@num,0) as Count FROM LogDetail \r\n                           inner join \r\n                           dbo.Test on  LogDetail.ResourceId=Test.id\r\n                           WHERE LogDetail.SchoolId = @SchoolId \r\n                           AND LogDetail.Status = @Status AND LogDetail.Tag1 = @Tag1";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@num", (object)num), new SqlParameter("@SchoolId", (object)schoolid), new SqlParameter("@Status", (object)status), new SqlParameter("@Tag1", tag) };
			return (int)SqlHelper.ExecuteScalar(sql, sqlParameter);
		}

		public string GetDaycountData(Guid schoolid, int status, string tag)
		{
			string sql = "SELECT TOP 1 CONVERT(VARCHAR(12),CreateTime,111) as Datatime\r\n                            FROM LogDetail \r\n                            WHERE SchoolId = @SchoolId AND Status = @Status AND Tag1 = @Tag1\r\n                            GROUP BY CONVERT(varchar(12),CreateTime,111) \r\n                            ORDER BY COUNT(1) DESC";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@SchoolId", (object)schoolid), new SqlParameter("@Status", (object)status), new SqlParameter("@Tag1", tag) };
			return SqlHelper.ExecuteScalar(sql, sqlParameter).ToString();
		}

		public int GetDaycountnum(Guid schoolid, int status, string tag)
		{
			string sql = "SELECT TOP 1 COUNT(1) as Daycountnum\r\n                            FROM LogDetail \r\n                            WHERE SchoolId = @SchoolId AND Status = @Status AND Tag1 = @Tag1\r\n                            GROUP BY CONVERT(varchar(12),CreateTime,111) \r\n                            ORDER BY COUNT(1) DESC";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@SchoolId", (object)schoolid), new SqlParameter("@Status", (object)status), new SqlParameter("@Tag1", tag) };
			SqlParameter[] para = sqlParameter;
			int num = int.Parse(SqlHelper.ExecuteScalar(sql, para).ToString());
			return num;
		}

		public LogDetail GetEntity(Guid id)
		{
			string sql = "SELECT * FROM LogDetail WHERE Id = @Id AND IsDeleted = 0";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteEntity<LogDetail>(sql, sqlParameter);
		}

		public List<DownloadDetails> GetIpDetails(Guid schoolid, int status, int tag)
		{
			string sql = "SELECT top(51) COUNT(1) AS DownloadCount,\r\n                                  Ip AS Tag2\r\n                           FROM LogDetail \r\n                           WHERE Ip IS NOT NULL \r\n                           AND SchoolId = @SchoolId \r\n                                AND Status = @Status AND Tag1 = @Tag1\r\n                           GROUP BY Ip \r\n                           ORDER BY DownloadCount DESC";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@SchoolId", (object)schoolid), new SqlParameter("@Status", (object)status), new SqlParameter("@Tag1", (object)tag) };
			return SqlHelper.ExecuteList<DownloadDetails>(sql, sqlParameter);
		}

		public List<DownloadDetails> GetIpDetailsExamBrowse(Guid schoolid, int status, int tag, int numExam)
		{
			string sql = "SELECT top(51) isnull(sum(ExamPaper.ItemCount)/@numExam,0) AS DownloadCount,\r\n                                  Ip AS Tag2\r\n                           FROM LogDetail inner join ExamPaper on LogDetail.ResourceId=ExamPaper.id\r\n                           WHERE Ip IS NOT NULL \r\n                           AND LogDetail.SchoolId = @SchoolId \r\n                                AND LogDetail.Status = @Status AND LogDetail.Tag1 = @Tag1\r\n                           GROUP BY Ip \r\n                           ORDER BY DownloadCount DESC";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@numExam", (object)numExam), new SqlParameter("@SchoolId", (object)schoolid), new SqlParameter("@Status", (object)status), new SqlParameter("@Tag1", (object)tag) };
			return SqlHelper.ExecuteList<DownloadDetails>(sql, sqlParameter);
		}

		public List<DownloadDetails> GetIpDetailsResource(Guid schoolid, int status, int tag)
		{
			string sql = "SELECT top(51) isnull(sum(ResourcePack.[count]),0)+isnull(count(ResourceMaterial.id),0) AS DownloadCount,\r\n                                  Ip AS Tag2\r\n                           FROM LogDetail \r\n                           left join ResourcePack on ResourceId =ResourcePack.id\r\n                           left join  dbo.ResourceMaterial on LogDetail.ResourceId=ResourceMaterial.id\r\n                           WHERE Ip IS NOT NULL \r\n                           AND LogDetail.SchoolId = @SchoolId \r\n                                AND LogDetail.Status = @Status AND LogDetail.Tag1 = @Tag1  \r\n                           GROUP BY Ip \r\n                           ORDER BY DownloadCount DESC";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@SchoolId", (object)schoolid), new SqlParameter("@Status", (object)status), new SqlParameter("@Tag1", (object)tag) };
			return SqlHelper.ExecuteList<DownloadDetails>(sql, sqlParameter);
		}

		public List<DownloadDetails> GetIpDetailsT(Guid teacherid, int status, string tag)
		{
			string sql = "SELECT COUNT(1) AS DownloadCount,\r\n                                  Ip AS Tag2\r\n                           FROM LogDetail \r\n                           WHERE Ip IS NOT NULL \r\n                           AND LogDetail.TeacherId = @TeacherId\r\n                                AND LogDetail.Status = @Status AND LogDetail.Tag1 = @Tag1\r\n                           GROUP BY Ip \r\n                           ORDER BY DownloadCount DESC";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@TeacherId", (object)teacherid), new SqlParameter("@Status", (object)status), new SqlParameter("@Tag1", tag) };
			return SqlHelper.ExecuteList<DownloadDetails>(sql, sqlParameter);
		}

		public List<DownloadDetails> GetIpDetailsTestBrowse(Guid schoolid, int status, int tag, int num)
		{
			string sql = "SELECT top(51) isnull(sum(ItemCount)/@num,0) AS DownloadCount,\r\n                                  Ip AS Tag2\r\n                           FROM LogDetail inner join \r\n                           dbo.Test on  LogDetail.ResourceId=Test.id\r\n                           WHERE Ip IS NOT NULL \r\n                           AND LogDetail.SchoolId = @SchoolId \r\n                                AND LogDetail.Status = @Status AND LogDetail.Tag1 = @Tag1\r\n                           GROUP BY Ip \r\n                           ORDER BY DownloadCount DESC";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@num", (object)num), new SqlParameter("@SchoolId", (object)schoolid), new SqlParameter("@Status", (object)status), new SqlParameter("@Tag1", (object)tag) };
			return SqlHelper.ExecuteList<DownloadDetails>(sql, sqlParameter);
		}

		public List<LogDetail> GetList()
		{
			return SqlHelper.ExecuteList<LogDetail>("SELECT * FROM LogDetail WHERE IsDeleted = 0", new SqlParameter[0]);
		}

		public List<DownloadDetails> GetListSE(Guid schoolid, int status, int pageIndex, int pageCount, string tag)
		{
			string sql = " SELECT * FROM( \r\n                                SELECT  LogDetail.ResourceId ,\r\n                                        ExamPaper.[Name] ResourceName ,\r\n                                        UserInfo.LoginName   TeacherLoginName ,\r\n                                        UserInfo.[Name]  TeacherName ,\r\n                                        LogDetail.TeacherId ,\r\n                                        School.[Name] SchoolName ,\r\n                                        LogDetail.Ip ,\r\n                                        LogDetail.CreateTime ,\r\n                                        ROW_NUMBER() OVER(ORDER BY LogDetail.CreateTime DESC ) rownum \r\n                                FROM LogDetail\r\n                                    LEFT JOIN UserInfo\r\n                                    ON LogDetail.TeacherId = UserInfo.Id\r\n                                    LEFT JOIN School\r\n                                    ON school.Id = LogDetail.SchoolId\r\n                                    LEFT JOIN ExamPaper\r\n                                    ON ExamPaper.Id = LogDetail.ResourceId\r\n                                WHERE LogDetail.SchoolId = @SchoolId \r\n                                         AND LogDetail.Status = @Status AND LogDetail.Tag1 = @Tag1\r\n                                ) as t WHERE rownum>=(@pageCount*(@pageIndex-1)+1) AND rownum<=@pageIndex*@pageCount ";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@pageIndex", (object)pageIndex), new SqlParameter("@pageCount", (object)pageCount), new SqlParameter("@SchoolId", (object)schoolid), new SqlParameter("@Status", (object)status), new SqlParameter("@Tag1", tag) };
			return SqlHelper.ExecuteList<DownloadDetails>(sql, sqlParameter);
		}

		public List<DownloadDetails> GetListSM(Guid schoolid, int status, int pageIndex, int pageCount, string tag)
		{
			string sql = " SELECT * FROM( \r\n                                SELECT  LogDetail.ResourceId ,\r\n                                        Medical.[Title] ResourceName ,\r\n                                        UserInfo.LoginName   TeacherLoginName ,\r\n                                        UserInfo.[Name]  TeacherName ,\r\n                                        LogDetail.TeacherId ,\r\n                                        School.[Name] SchoolName ,\r\n                                        LogDetail.Ip ,\r\n                                        LogDetail.CreateTime ,\r\n                                        ROW_NUMBER() OVER(ORDER BY LogDetail.CreateTime DESC ) rownum \r\n                                FROM LogDetail\r\n                                    LEFT JOIN UserInfo\r\n                                    ON LogDetail.TeacherId = UserInfo.Id\r\n                                    LEFT JOIN School\r\n                                    ON school.Id = LogDetail.SchoolId\r\n                                    LEFT JOIN Medical\r\n                                    ON Medical.Id = LogDetail.ResourceId\r\n                                WHERE LogDetail.SchoolId = @SchoolId \r\n                                         AND LogDetail.Status = @Status AND LogDetail.Tag1 = @Tag1\r\n                                ) as t WHERE rownum>=(@pageCount*(@pageIndex-1)+1) AND rownum<=@pageIndex*@pageCount ";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@pageIndex", (object)pageIndex), new SqlParameter("@pageCount", (object)pageCount), new SqlParameter("@SchoolId", (object)schoolid), new SqlParameter("@Status", (object)status), new SqlParameter("@Tag1", tag) };
			return SqlHelper.ExecuteList<DownloadDetails>(sql, sqlParameter);
		}

		public List<DownloadDetails> GetListSQ(Guid schoolid, int status, int pageIndex, int pageCount, string tag)
		{
			string sql = " SELECT * FROM( \r\n                                SELECT  LogDetail.ResourceId ,\r\n                                        QualityCourse.[Name] ResourceName ,\r\n                                        UserInfo.LoginName   TeacherLoginName ,\r\n                                        UserInfo.[Name]  TeacherName ,\r\n                                        LogDetail.TeacherId ,\r\n                                        School.[Name] SchoolName ,\r\n                                        LogDetail.Ip ,\r\n                                        LogDetail.CreateTime ,\r\n                                        ROW_NUMBER() OVER(ORDER BY LogDetail.CreateTime DESC ) rownum \r\n                                FROM LogDetail\r\n                                    LEFT JOIN UserInfo\r\n                                    ON LogDetail.TeacherId = UserInfo.Id\r\n                                    LEFT JOIN School\r\n                                    ON school.Id = LogDetail.SchoolId\r\n                                    LEFT JOIN QualityCourse\r\n                                    ON QualityCourse.Id = LogDetail.ResourceId\r\n                                WHERE LogDetail.SchoolId = @SchoolId \r\n                                         AND LogDetail.Status = @Status AND LogDetail.Tag1 = @Tag1\r\n                                ) as t WHERE rownum>=(@pageCount*(@pageIndex-1)+1) AND rownum<=@pageIndex*@pageCount ";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@pageIndex", (object)pageIndex), new SqlParameter("@pageCount", (object)pageCount), new SqlParameter("@SchoolId", (object)schoolid), new SqlParameter("@Status", (object)status), new SqlParameter("@Tag1", tag) };
			return SqlHelper.ExecuteList<DownloadDetails>(sql, sqlParameter);
		}

		public List<DownloadDetails> GetListSR(Guid schoolid, int status, int pageIndex, int pageCount, string tag)
		{
			string sql = "select * from (\r\nselect  ROW_NUMBER() OVER(ORDER BY  CreateTime DESC ) rownum ,sd.* from (\r\nSELECT  LogDetail.ResourceId ,Resourcematerial.[Title] ResourceName ,\r\n                                        UserInfo.LoginName   TeacherLoginName ,\r\n                                        UserInfo.[Name]  TeacherName ,\r\n                                        LogDetail.TeacherId ,\r\n                                        School.[Name] SchoolName ,\r\n                                        LogDetail.Ip ,\r\n                                        LogDetail.CreateTime \r\n                                FROM LogDetail\r\n                                    LEFT JOIN UserInfo\r\n                                    ON LogDetail.TeacherId = UserInfo.Id\r\n                                    LEFT JOIN School\r\n                                    ON school.Id = LogDetail.SchoolId\r\n                                    inner JOIN Resourcematerial\r\n                                    ON Resourcematerial.Id = LogDetail.ResourceId\r\n                                WHERE LogDetail.SchoolId = @SchoolId \r\n                                         AND LogDetail.Status = @Status AND LogDetail.Tag1 = @Tag1\r\n\r\nunion \r\nSELECT  LogDetail.ResourceId ,\r\n                                        resourcepack.[Name] ResourceName ,\r\n                                        UserInfo.LoginName   TeacherLoginName ,\r\n                                        UserInfo.[Name]  TeacherName ,\r\n                                        LogDetail.TeacherId ,\r\n                                        School.[Name] SchoolName ,\r\n                                        LogDetail.Ip ,\r\n                                        LogDetail.CreateTime \r\n                                FROM LogDetail\r\n                                    LEFT JOIN UserInfo\r\n                                    ON LogDetail.TeacherId = UserInfo.Id\r\n                                    LEFT JOIN School\r\n                                    ON school.Id = LogDetail.SchoolId\r\n                                    inner JOIN resourcepack\r\n                                    ON resourcepack.Id = LogDetail.ResourceId\r\n                              WHERE LogDetail.SchoolId = @SchoolId \r\n                                         AND LogDetail.Status = @Status AND LogDetail.Tag1 = @Tag1)as sd\r\n)as t WHERE rownum>=(@pageCount*(@pageIndex-1)+1) AND rownum<=@pageIndex*@pageCount order by CreateTime desc";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@pageIndex", (object)pageIndex), new SqlParameter("@pageCount", (object)pageCount), new SqlParameter("@SchoolId", (object)schoolid), new SqlParameter("@Status", (object)status), new SqlParameter("@Tag1", tag) };
			return SqlHelper.ExecuteList<DownloadDetails>(sql, sqlParameter);
		}

		public List<DownloadDetails> GetListST(Guid schoolid, int status, int pageIndex, int pageCount, string tag)
		{
			string sql = " SELECT * FROM( \r\n                                SELECT  LogDetail.ResourceId ,\r\n                                        Test.[Name] ResourceName ,\r\n                                        UserInfo.LoginName   TeacherLoginName ,\r\n                                        UserInfo.[Name]  TeacherName ,\r\n                                        LogDetail.TeacherId ,\r\n                                        School.[Name] SchoolName ,\r\n                                        LogDetail.Ip ,\r\n                                        LogDetail.CreateTime ,\r\n                                        ROW_NUMBER() OVER(ORDER BY LogDetail.CreateTime DESC ) rownum \r\n                                FROM LogDetail\r\n                                    LEFT JOIN UserInfo\r\n                                    ON LogDetail.TeacherId = UserInfo.Id\r\n                                    LEFT JOIN School\r\n                                    ON school.Id = LogDetail.SchoolId\r\n                                    LEFT JOIN Test\r\n                                    ON Test.Id = LogDetail.ResourceId\r\n                                WHERE LogDetail.SchoolId = @SchoolId \r\n                                         AND LogDetail.Status = @Status AND LogDetail.Tag1 = @Tag1\r\n                                ) as t WHERE rownum>=(@pageCount*(@pageIndex-1)+1) AND rownum<=@pageIndex*@pageCount ";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@pageIndex", (object)pageIndex), new SqlParameter("@pageCount", (object)pageCount), new SqlParameter("@SchoolId", (object)schoolid), new SqlParameter("@Status", (object)status), new SqlParameter("@Tag1", tag) };
			return SqlHelper.ExecuteList<DownloadDetails>(sql, sqlParameter);
		}

		public List<DownloadDetails> GetListTE(Guid teacherid, int status, int pageIndex, int pageCount, string tag)
		{
			string sql = " SELECT * FROM( \r\n                                SELECT  LogDetail.ResourceId ,\r\n                                        ExamPaper.[Name] ResourceName ,\r\n                                        UserInfo.LoginName   TeacherLoginName ,\r\n                                        UserInfo.[Name]  TeacherName ,\r\n                                        LogDetail.TeacherId ,\r\n                                        School.[Name] SchoolName ,\r\n                                        LogDetail.Ip ,\r\n                                        LogDetail.CreateTime ,\r\n                                        ROW_NUMBER() OVER(ORDER BY LogDetail.CreateTime DESC ) rownum \r\n                                FROM LogDetail\r\n                                    JOIN UserInfo\r\n                                    ON LogDetail.TeacherId = UserInfo.Id\r\n                                    JOIN School\r\n                                    ON school.Id = LogDetail.SchoolId\r\n                                    JOIN ExamPaper\r\n                                    ON ExamPaper.Id = LogDetail.ResourceId\r\n                                WHERE LogDetail.Status = @Status AND LogDetail.Tag1 = @Tag1 AND LogDetail.TeacherId = @TeacherId\r\n                                ) as t WHERE rownum>=(@pageCount*(@pageIndex-1)+1) AND rownum<=@pageIndex*@pageCount ";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@pageIndex", (object)pageIndex), new SqlParameter("@pageCount", (object)pageCount), new SqlParameter("@TeacherId", (object)teacherid), new SqlParameter("@Status", (object)status), new SqlParameter("@Tag1", tag) };
			return SqlHelper.ExecuteList<DownloadDetails>(sql, sqlParameter);
		}

		public List<DownloadDetails> GetListTM(Guid teacherid, int status, int pageIndex, int pageCount, string tag)
		{
			string sql = " SELECT * FROM( \r\n                                SELECT  LogDetail.ResourceId ,\r\n                                        Medical.[Title] ResourceName ,\r\n                                        UserInfo.LoginName   TeacherLoginName ,\r\n                                        UserInfo.[Name]  TeacherName ,\r\n                                        LogDetail.TeacherId ,\r\n                                        School.[Name] SchoolName ,\r\n                                        LogDetail.Ip ,\r\n                                        LogDetail.CreateTime ,\r\n                                        ROW_NUMBER() OVER(ORDER BY LogDetail.CreateTime DESC ) rownum \r\n                                FROM LogDetail\r\n                                    LEFT JOIN UserInfo\r\n                                    ON LogDetail.TeacherId = UserInfo.Id\r\n                                    LEFT JOIN School\r\n                                    ON school.Id = LogDetail.SchoolId\r\n                                    LEFT JOIN Medical\r\n                                    ON Medical.Id = LogDetail.ResourceId\r\n                                WHERE LogDetail.Status = @Status AND LogDetail.Tag1 = @Tag1 AND LogDetail.TeacherId = @TeacherId\r\n                                ) as t WHERE rownum>=(@pageCount*(@pageIndex-1)+1) AND rownum<=@pageIndex*@pageCount ";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@pageIndex", (object)pageIndex), new SqlParameter("@pageCount", (object)pageCount), new SqlParameter("@TeacherId", (object)teacherid), new SqlParameter("@Tag1", tag), new SqlParameter("@Status", (object)status) };
			return SqlHelper.ExecuteList<DownloadDetails>(sql, sqlParameter);
		}

		public List<DownloadDetails> GetListTQ(Guid teacherid, int status, int pageIndex, int pageCount, string tag)
		{
			string sql = " SELECT * FROM( \r\n                                SELECT  LogDetail.ResourceId ,\r\n                                        QualityCourse.[Name] ResourceName ,\r\n                                        UserInfo.LoginName   TeacherLoginName ,\r\n                                        UserInfo.[Name]  TeacherName ,\r\n                                        LogDetail.TeacherId ,\r\n                                        School.[Name] SchoolName ,\r\n                                        LogDetail.Ip ,\r\n                                        LogDetail.CreateTime ,\r\n                                        ROW_NUMBER() OVER(ORDER BY LogDetail.CreateTime DESC ) rownum \r\n                                FROM LogDetail\r\n                                    LEFT JOIN UserInfo\r\n                                    ON LogDetail.TeacherId = UserInfo.Id\r\n                                    LEFT JOIN School\r\n                                    ON school.Id = LogDetail.SchoolId\r\n                                    LEFT JOIN QualityCourse\r\n                                    ON QualityCourse.Id = LogDetail.ResourceId\r\n                                WHERE LogDetail.TeacherId = @TeacherId \r\n                                         AND LogDetail.Status = @Status AND LogDetail.Tag1 = @Tag1\r\n                                ) as t WHERE rownum>=(@pageCount*(@pageIndex-1)+1) AND rownum<=@pageIndex*@pageCount ";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@pageIndex", (object)pageIndex), new SqlParameter("@pageCount", (object)pageCount), new SqlParameter("@TeacherId", (object)teacherid), new SqlParameter("@Status", (object)status), new SqlParameter("@Tag1", tag) };
			return SqlHelper.ExecuteList<DownloadDetails>(sql, sqlParameter);
		}

		public List<DownloadDetails> GetListTR(Guid teacherid, int status, int pageIndex, int pageCount, string tag)
		{
			string sql = " SELECT * FROM( \r\n                                SELECT  LogDetail.ResourceId ,\r\n                                        Resource.[Title] ResourceName ,\r\n                                        UserInfo.LoginName   TeacherLoginName ,\r\n                                        UserInfo.[Name]  TeacherName ,\r\n                                        LogDetail.TeacherId ,\r\n                                        School.[Name] SchoolName ,\r\n                                        LogDetail.Ip ,\r\n                                        LogDetail.CreateTime ,\r\n                                        ROW_NUMBER() OVER(ORDER BY LogDetail.CreateTime DESC ) rownum \r\n                                FROM LogDetail\r\n                                    JOIN UserInfo\r\n                                    ON LogDetail.TeacherId = UserInfo.Id\r\n                                    JOIN School\r\n                                    ON school.Id = LogDetail.SchoolId\r\n                                    JOIN Resource\r\n                                    ON Resource.Id = LogDetail.ResourceId\r\n                                WHERE LogDetail.Status = @Status AND LogDetail.Tag1 = @Tag1 AND LogDetail.TeacherId = @TeacherId\r\n                                ) as t WHERE rownum>=(@pageCount*(@pageIndex-1)+1) AND rownum<=@pageIndex*@pageCount ";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@pageIndex", (object)pageIndex), new SqlParameter("@pageCount", (object)pageCount), new SqlParameter("@TeacherId", (object)teacherid), new SqlParameter("@Status", (object)status), new SqlParameter("@Tag1", tag) };
			return SqlHelper.ExecuteList<DownloadDetails>(sql, sqlParameter);
		}

		public List<DownloadDetails> GetListTrialreport(Guid schoolid, int status, string tag)
		{
			string sql = "select top(300)* from ( \r\nSELECT  LogDetail.ResourceId ,Resourcematerial.[Title] ResourceName ,\r\n                                        UserInfo.LoginName   TeacherLoginName ,\r\n                                        UserInfo.[Name]  TeacherName ,\r\n                                        LogDetail.TeacherId ,\r\n                                        School.[Name] SchoolName ,\r\n                                        LogDetail.Ip ,\r\n                                        LogDetail.CreateTime \r\n                                FROM LogDetail\r\n                                    LEFT JOIN UserInfo\r\n                                    ON LogDetail.TeacherId = UserInfo.Id\r\n                                    LEFT JOIN School\r\n                                    ON school.Id = LogDetail.SchoolId\r\n                                    inner JOIN Resourcematerial\r\n                                    ON Resourcematerial.Id = LogDetail.ResourceId\r\n                                WHERE LogDetail.SchoolId = @SchoolId \r\n                                         AND LogDetail.Status = @Status AND LogDetail.Tag1 = @Tag1\r\n\r\nunion \r\nSELECT  LogDetail.ResourceId ,\r\n                                        resourcepack.[Name] ResourceName ,\r\n                                        UserInfo.LoginName   TeacherLoginName ,\r\n                                        UserInfo.[Name]  TeacherName ,\r\n                                        LogDetail.TeacherId ,\r\n                                        School.[Name] SchoolName ,\r\n                                        LogDetail.Ip ,\r\n                                        LogDetail.CreateTime \r\n                                FROM LogDetail\r\n                                    LEFT JOIN UserInfo\r\n                                    ON LogDetail.TeacherId = UserInfo.Id\r\n                                    LEFT JOIN School\r\n                                    ON school.Id = LogDetail.SchoolId\r\n                                    inner JOIN resourcepack\r\n                                    ON resourcepack.Id = LogDetail.ResourceId\r\n                              WHERE LogDetail.SchoolId = @SchoolId \r\n                                         AND LogDetail.Status = @Status AND LogDetail.Tag1 = @Tag1 \r\n)as t   order by CreateTime desc";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@SchoolId", (object)schoolid), new SqlParameter("@Status", (object)status), new SqlParameter("@Tag1", tag) };
			return SqlHelper.ExecuteList<DownloadDetails>(sql, sqlParameter);
		}

		public List<DownloadDetails> GetListTrialreportExam(Guid schoolid, int status, string tag)
		{
			string sql = "select top(300)* from ( \r\n                            SELECT  LogDetail.ResourceId ,ExamPaper.Name ResourceName ,\r\n                                        UserInfo.LoginName   TeacherLoginName ,\r\n                                        UserInfo.[Name]  TeacherName ,\r\n                                        LogDetail.TeacherId ,\r\n                                        School.[Name] SchoolName ,\r\n                                        LogDetail.Ip ,\r\n                                        LogDetail.CreateTime \r\n                                FROM LogDetail\r\n                                    LEFT JOIN UserInfo\r\n                                    ON LogDetail.TeacherId = UserInfo.Id\r\n                                    LEFT JOIN School\r\n                                    ON school.Id = LogDetail.SchoolId\r\n                                    inner JOIN ExamPaper\r\n                                    ON ExamPaper.Id = LogDetail.ResourceId\r\n                                WHERE LogDetail.SchoolId = @SchoolId \r\n                                         AND LogDetail.Status = @Status AND LogDetail.Tag1 = @Tag1\r\n)as t   order by CreateTime desc";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@SchoolId", (object)schoolid), new SqlParameter("@Status", (object)status), new SqlParameter("@Tag1", tag) };
			return SqlHelper.ExecuteList<DownloadDetails>(sql, sqlParameter);
		}

		public List<DownloadDetails> GetListTrialreportSQ(Guid schoolid, int status, string tag)
		{
			string sql = "select top(300)* from ( \r\n                            SELECT  LogDetail.ResourceId ,QualityCourse.Name ResourceName ,\r\n                                        UserInfo.LoginName   TeacherLoginName ,\r\n                                        UserInfo.[Name]  TeacherName ,\r\n                                        LogDetail.TeacherId ,\r\n                                        School.[Name] SchoolName ,\r\n                                        LogDetail.Ip ,\r\n                                        LogDetail.CreateTime \r\n                                FROM LogDetail\r\n                                    LEFT JOIN UserInfo\r\n                                    ON LogDetail.TeacherId = UserInfo.Id\r\n                                    LEFT JOIN School\r\n                                    ON school.Id = LogDetail.SchoolId\r\n                                    inner JOIN QualityCourse\r\n                                    ON QualityCourse.Id = LogDetail.ResourceId\r\n                                WHERE LogDetail.SchoolId = @SchoolId \r\n                                         AND LogDetail.Status = @Status AND LogDetail.Tag1 = @Tag1\r\n)as t   order by CreateTime desc";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@SchoolId", (object)schoolid), new SqlParameter("@Status", (object)status), new SqlParameter("@Tag1", tag) };
			return SqlHelper.ExecuteList<DownloadDetails>(sql, sqlParameter);
		}

		public List<DownloadDetails> GetListTrialreportTest(Guid schoolid, int status, string tag)
		{
			string sql = "select top(300)* from ( \r\n                            SELECT  LogDetail.ResourceId ,Test.Name ResourceName ,\r\n                                        UserInfo.LoginName   TeacherLoginName ,\r\n                                        UserInfo.[Name]  TeacherName ,\r\n                                        LogDetail.TeacherId ,\r\n                                        School.[Name] SchoolName ,\r\n                                        LogDetail.Ip ,\r\n                                        LogDetail.CreateTime \r\n                                FROM LogDetail\r\n                                    LEFT JOIN UserInfo\r\n                                    ON LogDetail.TeacherId = UserInfo.Id\r\n                                    LEFT JOIN School\r\n                                    ON school.Id = LogDetail.SchoolId\r\n                                    inner JOIN Test\r\n                                    ON Test.Id = LogDetail.ResourceId\r\n                                WHERE LogDetail.SchoolId = @SchoolId \r\n                                         AND LogDetail.Status = @Status AND LogDetail.Tag1 = @Tag1\r\n)as t   order by CreateTime desc";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@SchoolId", (object)schoolid), new SqlParameter("@Status", (object)status), new SqlParameter("@Tag1", tag) };
			return SqlHelper.ExecuteList<DownloadDetails>(sql, sqlParameter);
		}

		public List<DownloadDetails> GetListTT(Guid teacherid, int status, int pageIndex, int pageCount, string tag)
		{
			string sql = " SELECT * FROM( \r\n                                SELECT  LogDetail.ResourceId ,\r\n                                        Test.[Name] ResourceName ,\r\n                                        UserInfo.LoginName   TeacherLoginName ,\r\n                                        UserInfo.[Name]  TeacherName ,\r\n                                        LogDetail.TeacherId ,\r\n                                        School.[Name] SchoolName ,\r\n                                        LogDetail.Ip ,\r\n                                        LogDetail.CreateTime ,\r\n                                        ROW_NUMBER() OVER(ORDER BY LogDetail.CreateTime DESC ) rownum \r\n                                FROM LogDetail\r\n                                    JOIN UserInfo\r\n                                    ON LogDetail.TeacherId = UserInfo.Id\r\n                                    JOIN School\r\n                                    ON school.Id = LogDetail.SchoolId\r\n                                    JOIN Test\r\n                                    ON Test.Id = LogDetail.ResourceId\r\n                                WHERE LogDetail.Status = @Status AND LogDetail.Tag1 = @Tag1 AND LogDetail.TeacherId = @TeacherId\r\n                                ) as t WHERE rownum>=(@pageCount*(@pageIndex-1)+1) AND rownum<=@pageIndex*@pageCount ";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@pageIndex", (object)pageIndex), new SqlParameter("@pageCount", (object)pageCount), new SqlParameter("@TeacherId", (object)teacherid), new SqlParameter("@Status", (object)status), new SqlParameter("@Tag1", tag) };
			return SqlHelper.ExecuteList<DownloadDetails>(sql, sqlParameter);
		}

		public List<DownloadDetails> GetMonthDetails(Guid schoolid, int status, int tag)
		{
			string sql = "SELECT COUNT(1) AS DownloadCount,\r\n                               SUBSTRING(CONVERT(VARCHAR(90),CreateTime,23),0,8) AS Tag2\r\n                           FROM LogDetail \r\n                           WHERE Ip IS NOT NULL \r\n                           AND  SchoolId = @SchoolId \r\n                                AND Status = @Status AND Tag1 = @Tag1\r\n                           GROUP BY substring(convert(varchar(90),createtime,23),0,8) \r\n                           ORDER BY Tag2 ";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@SchoolId", (object)schoolid), new SqlParameter("@Status", (object)status), new SqlParameter("@Tag1", (object)tag) };
			return SqlHelper.ExecuteList<DownloadDetails>(sql, sqlParameter);
		}

		public List<DownloadDetails> GetMonthDetailsExam(Guid schoolid, int status, int tag, int numExam)
		{
			string sql = "SELECT isnull(sum(ExamPaper.ItemCount)/@numExam,0) AS DownloadCount,\r\n                               SUBSTRING(CONVERT(VARCHAR(90),LogDetail.CreateTime,23),0,8) AS Tag2\r\n                           FROM LogDetail inner join ExamPaper on LogDetail.ResourceId=ExamPaper.id\r\n                           WHERE Ip IS NOT NULL \r\n                           AND  LogDetail.SchoolId = @SchoolId \r\n                                AND LogDetail.Status = @Status AND LogDetail.Tag1 = @Tag1\r\n                           GROUP BY substring(convert(varchar(90),LogDetail.createtime,23),0,8) \r\n                           ORDER BY Tag2 ";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@numExam", (object)numExam), new SqlParameter("@SchoolId", (object)schoolid), new SqlParameter("@Status", (object)status), new SqlParameter("@Tag1", (object)tag) };
			return SqlHelper.ExecuteList<DownloadDetails>(sql, sqlParameter);
		}

		public List<DownloadDetails> GetMonthDetailsResource(Guid schoolid, int status, int tag)
		{
			string sql = "select isnull(a.DownloadCount,0)+isnull(b.DownloadCount,0) as DownloadCount,\r\n                         isnull(a.tag2,b.tag2) as tag2 from (\r\n                        SELECT isnull(SUM([Count]),0)  AS DownloadCount,\r\n                        SUBSTRING(CONVERT(VARCHAR(90),LogDetail.CreateTime,23),0,8) AS Tag2\r\n                        FROM LogDetail inner join ResourcePack\r\n                        on ResourceId =ResourcePack.id\r\n                        WHERE Ip IS NOT NULL \r\n                        AND  LogDetail.SchoolId = @SchoolId \r\n                        AND LogDetail.Status = @Status AND LogDetail.Tag1 = @Tag1\r\n                        GROUP BY substring(convert(varchar(90),LogDetail.createtime,23),0,8) \r\n                        ) a full join (\r\n\r\n                        SELECT count(ResourceMaterial.id) as DownloadCount,\r\n                        SUBSTRING(CONVERT(VARCHAR(90),LogDetail.CreateTime,23),0,8) AS Tag2\r\n                        FROM LogDetail inner join ResourceMaterial\r\n                        on ResourceId =ResourceMaterial.id\r\n                        WHERE Ip IS NOT NULL \r\n                        AND  LogDetail.SchoolId = @SchoolId \r\n                        AND LogDetail.Status = @Status AND LogDetail.Tag1 = @Tag1\r\n                        GROUP BY substring(convert(varchar(90),LogDetail.createtime,23),0,8)) \r\n                        as b on a.tag2=b.tag2  order by tag2";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@SchoolId", (object)schoolid), new SqlParameter("@Status", (object)status), new SqlParameter("@Tag1", (object)tag) };
			return SqlHelper.ExecuteList<DownloadDetails>(sql, sqlParameter);
		}

		public List<DownloadDetails> GetMonthDetailsT(Guid teacherid, int status, string tag)
		{
			string sql = "SELECT COUNT(1) AS DownloadCount,\r\n                               SUBSTRING(CONVERT(VARCHAR(90),CreateTime,23),0,8) AS Tag2\r\n                           FROM LogDetail \r\n                           WHERE Ip IS NOT NULL \r\n                           AND LogDetail.TeacherId = @TeacherId\r\n                                AND LogDetail.Status = @Status AND LogDetail.Tag1 = @Tag1 \r\n                           GROUP BY substring(convert(varchar(90),createtime,23),0,8) \r\n                           ORDER BY Tag2 ";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@TeacherId", (object)teacherid), new SqlParameter("@Status", (object)status), new SqlParameter("@Tag1", tag) };
			return SqlHelper.ExecuteList<DownloadDetails>(sql, sqlParameter);
		}

		public List<DownloadDetails> GetMonthDetailsTest(Guid schoolid, int status, int tag, int num)
		{
			string sql = "SELECT isnull(sum(ItemCount)/@num,0)  AS DownloadCount,\r\n                               SUBSTRING(CONVERT(VARCHAR(90),LogDetail.CreateTime,23),0,8) AS Tag2\r\n                           FROM LogDetail inner join Test\r\n                            on ResourceId =Test.id\r\n                           WHERE Ip IS NOT NULL \r\n                           AND  LogDetail.SchoolId = @SchoolId \r\n                                AND LogDetail.Status = @Status AND LogDetail.Tag1 = @Tag1\r\n                           GROUP BY substring(convert(varchar(90),LogDetail.createtime,23),0,8) \r\n                           ORDER BY LogDetail.Tag2 ";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@num", (object)num), new SqlParameter("@SchoolId", (object)schoolid), new SqlParameter("@Status", (object)status), new SqlParameter("@Tag1", (object)tag) };
			return SqlHelper.ExecuteList<DownloadDetails>(sql, sqlParameter);
		}

		public DownloadDetails GetMostDay(Guid schoolid, int status, string tag)
		{
			string sql = "SELECT TOP 1 \t\r\n                                COUNT(1) AS DownloadCount,\t\r\n                                CONVERT(VARCHAR(12),CreateTime,111) AS Tag2 \r\n                            FROM LogDetail \r\n                            WHERE SchoolId = @SchoolId AND Status = @Status AND Tag1 = @Tag1\r\n                            GROUP BY CONVERT(varchar(12),CreateTime,111) \r\n                            ORDER BY DownloadCount DESC";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@SchoolId", (object)schoolid), new SqlParameter("@Status", (object)status), new SqlParameter("@Tag1", tag) };
			return SqlHelper.ExecuteEntity<DownloadDetails>(sql, sqlParameter);
		}

		public DownloadDetails GetMostDayExam(Guid schoolid, int status, string tag, int numExam)
		{
			string sql = "SELECT TOP 1 \t\r\n                                isnull(sum(ExamPaper.ItemCount)/@numExam,0) AS DownloadCount,\t\r\n                                CONVERT(VARCHAR(12),LogDetail.CreateTime,111) AS Tag2 \r\n                            FROM LogDetail inner join ExamPaper on LogDetail.ResourceId=ExamPaper.id\r\n                            WHERE LogDetail.SchoolId = @SchoolId AND LogDetail.Status = @Status AND LogDetail.Tag1 = @Tag1\r\n                            GROUP BY CONVERT(varchar(12),LogDetail.CreateTime,111) \r\n                            ORDER BY DownloadCount DESC";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@numExam", (object)numExam), new SqlParameter("@SchoolId", (object)schoolid), new SqlParameter("@Status", (object)status), new SqlParameter("@Tag1", tag) };
			return SqlHelper.ExecuteEntity<DownloadDetails>(sql, sqlParameter);
		}

		public DownloadDetails GetMostDayResource(Guid schoolid, int status, string tag)
		{
			string sql = "\r\n                    select top 1 * from (\r\n                    select isnull(a.DownloadCount,0)+isnull(b.DownloadCount,0)\r\n                        as DownloadCount,isnull(a.tag2,b.tag2) as tag2 from (\r\n                    SELECT \tisnull(SUM([Count]),0) as DownloadCount,\t\r\n                    CONVERT(VARCHAR(12),LogDetail.CreateTime,111) AS Tag2 \r\n                    FROM LogDetail inner join ResourcePack\r\n                    on ResourceId =ResourcePack.id\r\n                    WHERE LogDetail.SchoolId = @SchoolId AND LogDetail.Status = @Status AND LogDetail.Tag1 = @Tag1\r\n                    GROUP BY CONVERT(varchar(12),LogDetail.CreateTime,111)) as a \r\n                    full join \r\n                    (SELECT \tcount(ResourceMaterial.id) as DownloadCount,\r\n                    CONVERT(VARCHAR(12),LogDetail.CreateTime,111) AS Tag2 \r\n                    FROM LogDetail inner join ResourceMaterial\r\n                    on ResourceId =ResourceMaterial.id\r\n                    WHERE LogDetail.SchoolId = @SchoolId AND LogDetail.Status = @Status AND LogDetail.Tag1 = @Tag1\r\n                    AND LogDetail.Status = 2 AND LogDetail.Tag1 = 0\r\n                    GROUP BY CONVERT(varchar(12),LogDetail.CreateTime,111) )as b\r\n                    on a.tag2=b.tag2 ) as a order by DownloadCount desc ";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@SchoolId", (object)schoolid), new SqlParameter("@Status", (object)status), new SqlParameter("@Tag1", tag) };
			return SqlHelper.ExecuteEntity<DownloadDetails>(sql, sqlParameter);
		}

		public DownloadDetails GetMostDayT(Guid teacherid, int status, string tag)
		{
			string sql = "SELECT TOP 1 \t\r\n                                COUNT(1) AS DownloadCount,\t\r\n                                CONVERT(VARCHAR(12),CreateTime,111) AS Tag2 \r\n                            FROM LogDetail \r\n                            WHERE TeacherId = @TeacherId AND Status = @Status AND Tag1 = @Tag1\r\n                            GROUP BY CONVERT(varchar(12),CreateTime,111) \r\n                            ORDER BY DownloadCount DESC";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@TeacherId", (object)teacherid), new SqlParameter("@Status", (object)status), new SqlParameter("@Tag1", tag) };
			return SqlHelper.ExecuteEntity<DownloadDetails>(sql, sqlParameter);
		}

		public DownloadDetails GetMostResource(Guid schoolid, int status, string tag)
		{
			string sql = "SELECT TOP 1 COUNT(1) AS DownloadCount,\r\n                                        Resource.Title AS Tag2\r\n                           FROM  LogDetail \r\n                           JOIN Resource\r\n                           ON LogDetail.ResourceId = Resource.Id \r\n                           WHERE LogDetail.SchoolId = @SchoolId \r\n                                 AND LogDetail.Status = @Status AND LogDetail.Tag1 = @Tag1\r\n                           GROUP BY Resource.Title  \r\n                           ORDER BY DownloadCount DESC ";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@SchoolId", (object)schoolid), new SqlParameter("@Status", (object)status), new SqlParameter("@Tag1", tag) };
			return SqlHelper.ExecuteEntity<DownloadDetails>(sql, sqlParameter);
		}

		public DownloadDetails GetMostResourceE(Guid schoolid, int status, string tag, int numExam)
		{
			string sql = "SELECT TOP 1 COUNT(1)*ExamPaper.ItemCount/@numExam AS DownloadCount,\r\n                                        ExamPaper.Name AS Tag2\r\n                           FROM  LogDetail \r\n                           JOIN ExamPaper \r\n                           ON LogDetail.ResourceId = ExamPaper.Id \r\n                           WHERE LogDetail.SchoolId = @SchoolId \r\n                                 AND LogDetail.Status = @Status AND LogDetail.Tag1 = @Tag1\r\n                           GROUP BY ExamPaper.Name,ExamPaper.ItemCount\r\n                           ORDER BY DownloadCount DESC ";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@numExam", (object)numExam), new SqlParameter("@SchoolId", (object)schoolid), new SqlParameter("@Status", (object)status), new SqlParameter("@Tag1", tag) };
			return SqlHelper.ExecuteEntity<DownloadDetails>(sql, sqlParameter);
		}

		public DownloadDetails GetMostResourceM(Guid schoolid, int status, string tag)
		{
			string sql = "SELECT TOP 1 COUNT(1) AS DownloadCount,\r\n                                        Medical.Title AS Tag2\r\n                           FROM  LogDetail \r\n                           JOIN Medical \r\n                           ON LogDetail.ResourceId = Medical.Id \r\n                           WHERE LogDetail.SchoolId = @SchoolId \r\n                                 AND LogDetail.Status = @Status AND LogDetail.Tag1 = @Tag1\r\n                           GROUP BY Medical.Title \r\n                           ORDER BY DownloadCount DESC ";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@SchoolId", (object)schoolid), new SqlParameter("@Status", (object)status), new SqlParameter("@Tag1", tag) };
			return SqlHelper.ExecuteEntity<DownloadDetails>(sql, sqlParameter);
		}

		public DownloadDetails GetMostResourceMaterial(Guid schoolid, int status, string tag)
		{
			string sql = "SELECT TOP 1 COUNT(1) AS DownloadCount,\r\n                                        (ResourceType.name+':'+ResourceMaterial.Title) AS Tag2\r\n                           FROM  LogDetail \r\n                           JOIN ResourceMaterial \r\n                           ON LogDetail.ResourceId = ResourceMaterial.Id \r\n                           left join ResourceType on ResourceMaterial.TypeId=ResourceType.id\r\n                           WHERE LogDetail.SchoolId = @SchoolId \r\n                                 AND LogDetail.Status = @Status AND LogDetail.Tag1 = @Tag1\r\n                           GROUP BY ResourceMaterial.Title ,ResourceType.name\r\n                           ORDER BY DownloadCount DESC ";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@SchoolId", (object)schoolid), new SqlParameter("@Status", (object)status), new SqlParameter("@Tag1", tag) };
			return SqlHelper.ExecuteEntity<DownloadDetails>(sql, sqlParameter);
		}

		public DownloadDetails GetMostResourcePack(Guid schoolid, int status, string tag)
		{
			string sql = "SELECT TOP 1 isnull(SUM(ResourcePack.[Count]),0) AS DownloadCount,\r\n                                        (ResourceType.name+':'+ResourcePack.NAME) AS Tag2\r\n                           FROM  LogDetail \r\n                           JOIN ResourcePack \r\n                           ON LogDetail.ResourceId = ResourcePack.Id \r\n                           left join ResourceType on ResourcePack.TypeId=ResourceType.id\r\n                           WHERE LogDetail.SchoolId = @SchoolId \r\n                                 AND LogDetail.Status = @Status AND LogDetail.Tag1 = @Tag1\r\n                           GROUP BY ResourcePack.NAME ,ResourceType.name\r\n                           ORDER BY DownloadCount DESC ";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@SchoolId", (object)schoolid), new SqlParameter("@Status", (object)status), new SqlParameter("@Tag1", tag) };
			return SqlHelper.ExecuteEntity<DownloadDetails>(sql, sqlParameter);
		}

		public DownloadDetails GetMostResourceQ(Guid schoolid, int status, string tag)
		{
			string sql = "SELECT TOP 1 COUNT(1) AS DownloadCount,\r\n                                        QualityCourse.Name AS Tag2\r\n                           FROM  LogDetail \r\n                           JOIN QualityCourse \r\n                           ON LogDetail.ResourceId = QualityCourse.Id \r\n                           WHERE LogDetail.SchoolId = @SchoolId \r\n                                 AND LogDetail.Status = @Status AND LogDetail.Tag1 = @Tag1\r\n                           GROUP BY QualityCourse.Name \r\n                           ORDER BY DownloadCount DESC ";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@SchoolId", (object)schoolid), new SqlParameter("@Status", (object)status), new SqlParameter("@Tag1", tag) };
			return SqlHelper.ExecuteEntity<DownloadDetails>(sql, sqlParameter);
		}

		public DownloadDetails GetMostResourceT(Guid schoolid, int status, string tag, int num)
		{
			string sql = "SELECT TOP 1 isnull(sum(ItemCount)/@num,0) AS DownloadCount,\r\n                                        Test.Name AS Tag2\r\n                           FROM  LogDetail \r\n                           JOIN Test \r\n                           ON LogDetail.ResourceId = Test.Id \r\n                           WHERE LogDetail.SchoolId = @SchoolId \r\n                                 AND LogDetail.Status = @Status AND LogDetail.Tag1 = @Tag1\r\n                           GROUP BY Test.Name \r\n                           ORDER BY DownloadCount DESC ";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@num", (object)num), new SqlParameter("@SchoolId", (object)schoolid), new SqlParameter("@Status", (object)status), new SqlParameter("@Tag1", tag) };
			return SqlHelper.ExecuteEntity<DownloadDetails>(sql, sqlParameter);
		}

		public DownloadDetails GetMostResourceTE(Guid teacherid, int status, string tag)
		{
			string sql = "SELECT TOP 1 COUNT(1) AS DownloadCount,\r\n                                        ExamPaper.Name AS Tag2\r\n                           FROM  LogDetail \r\n                           INNER JOIN ExamPaper \r\n                           ON LogDetail.ResourceId = ExamPaper.Id \r\n                           WHERE LogDetail.TeacherId = @TeacherId\r\n                                 AND LogDetail.Status = @Status \r\n                                 AND LogDetail.Tag1 = @Tag1\r\n                           GROUP BY ExamPaper.Name \r\n                           ORDER BY DownloadCount DESC ";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@TeacherId", (object)teacherid), new SqlParameter("@Status", (object)status), new SqlParameter("@Tag1", tag) };
			return SqlHelper.ExecuteEntity<DownloadDetails>(sql, sqlParameter);
		}

		public DownloadDetails GetMostResourceTM(Guid teacherid, int status, string tag)
		{
			string sql = "SELECT TOP 1 COUNT(1) AS DownloadCount,\r\n                                        Medical.Title AS Tag2\r\n                           FROM  LogDetail \r\n                           JOIN Medical \r\n                           ON LogDetail.ResourceId = Medical.Id \r\n                           WHERE LogDetail.TeacherId = @TeacherId\r\n                                 AND LogDetail.Status = @Status \r\n                                 AND LogDetail.Tag1 = @Tag1\r\n                           GROUP BY Medical.Title \r\n                           ORDER BY DownloadCount DESC ";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@TeacherId", (object)teacherid), new SqlParameter("@Status", (object)status), new SqlParameter("@Tag1", tag) };
			return SqlHelper.ExecuteEntity<DownloadDetails>(sql, sqlParameter);
		}

		public DownloadDetails GetMostResourceTQ(Guid teacherid, int status, string tag)
		{
			string sql = "SELECT TOP 1 COUNT(1) AS DownloadCount,\r\n                                        QualityCourse.Name AS Tag2\r\n                           FROM  LogDetail \r\n                           JOIN QualityCourse \r\n                           ON LogDetail.ResourceId = QualityCourse.Id \r\n                           WHERE LogDetail.TeacherId = @TeacherId \r\n                                 AND LogDetail.Status = @Status AND LogDetail.Tag1 = @Tag1\r\n                           GROUP BY QualityCourse.Name \r\n                           ORDER BY DownloadCount DESC ";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@TeacherId", (object)teacherid), new SqlParameter("@Status", (object)status), new SqlParameter("@Tag1", tag) };
			return SqlHelper.ExecuteEntity<DownloadDetails>(sql, sqlParameter);
		}

		public DownloadDetails GetMostResourceTR(Guid teacherid, int status, string tag)
		{
			string sql = "SELECT TOP 1 COUNT(1) AS DownloadCount,\r\n                                        Resource.Title AS Tag2\r\n                           FROM  LogDetail \r\n                           INNER JOIN Resource \r\n                           ON LogDetail.ResourceId = Resource.Id \r\n                           WHERE LogDetail.TeacherId = @TeacherId\r\n                                 AND LogDetail.Status = @Status \r\n                                 AND LogDetail.Tag1 = @Tag1\r\n                           GROUP BY Resource.Title \r\n                           ORDER BY DownloadCount DESC ";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@TeacherId", (object)teacherid), new SqlParameter("@Status", (object)status), new SqlParameter("@Tag1", tag) };
			return SqlHelper.ExecuteEntity<DownloadDetails>(sql, sqlParameter);
		}

		public DownloadDetails GetMostResourceTT(Guid teacherid, int status, string tag)
		{
			string sql = "SELECT TOP 1 COUNT(1) AS DownloadCount,\r\n                                        Test.Name AS Tag2\r\n                           FROM  LogDetail \r\n                           INNER JOIN Test \r\n                           ON LogDetail.ResourceId = Test.Id \r\n                           WHERE LogDetail.TeacherId = @TeacherId\r\n                                 AND LogDetail.Status = @Status \r\n                                 AND LogDetail.Tag1 = @Tag1\r\n                           GROUP BY Test.Name \r\n                           ORDER BY DownloadCount DESC ";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@TeacherId", (object)teacherid), new SqlParameter("@Status", (object)status), new SqlParameter("@Tag1", tag) };
			return SqlHelper.ExecuteEntity<DownloadDetails>(sql, sqlParameter);
		}

		public List<LogDetail> GetPage(int pageIndex, int pageCount)
		{
			string sql = "SELECT * FROM(SELECT *,(ROW_NUMBER() OVER(ORDER BY Id)) rownum FROM LogDetail WHERE IsDeleted = 0) t WHERE rownum>=(@pageCount*(@pageIndex-1)+1) AND rownum<=@pageIndex*@pageCount";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@pageIndex", (object)pageIndex), new SqlParameter("@pageCount", (object)pageCount) };
			return SqlHelper.ExecuteList<LogDetail>(sql, sqlParameter);
		}

		public List<DownloadDetails> GetSpeDetailsSE(Guid schoolid, int status, string tag)
		{
			string sql = "SELECT  ExamPaperType.ParentId AS Id,\r\n                                   COUNT(1) AS DownloadCount,\r\n                                   ExamPaperType.[Name]  AS Tag2\r\n                           FROM  LogDetail \r\n                           INNER JOIN ExamPaper \r\n                           ON LogDetail.ResourceId = ExamPaper.Id \r\n                           INNER JOIN ExamPaperType \r\n                           ON ExamPaperType.Id = ExamPaper.TypeId \r\n                           WHERE LogDetail.SchoolId = @SchoolId \r\n                                 AND LogDetail.Status = @Status AND LogDetail.Tag1 = @Tag1\r\n                           GROUP BY ExamPaperType.ParentId,ExamPaperType.[Name]\r\n                           ORDER BY DownloadCount DESC ";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@SchoolId", (object)schoolid), new SqlParameter("@Status", (object)status), new SqlParameter("@Tag1", tag) };
			return SqlHelper.ExecuteList<DownloadDetails>(sql, sqlParameter);
		}

		public List<DownloadDetails> GetSpeDetailsTE(Guid teacherid, int status, string tag)
		{
			string sql = "SELECT  ExamPaperType.ParentId AS Id,\r\n                                   COUNT(1) AS DownloadCount,\r\n                                   ExamPaperType.[Name]  AS Tag2\r\n                           FROM  LogDetail \r\n                           INNER JOIN ExamPaper \r\n                           ON LogDetail.ResourceId = ExamPaper.Id \r\n                           INNER JOIN ExamPaperType \r\n                           ON ExamPaperType.Id = ExamPaper.TypeId \r\n                           WHERE LogDetail.TeacherId = @TeacherId \r\n                                 AND LogDetail.Status = @Status AND LogDetail.Tag1 = @Tag1\r\n                           GROUP BY ExamPaperType.ParentId,ExamPaperType.[Name]\r\n                           ORDER BY DownloadCount DESC ";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@TeacherId", (object)teacherid), new SqlParameter("@Status", (object)status), new SqlParameter("@Tag1", tag) };
			return SqlHelper.ExecuteList<DownloadDetails>(sql, sqlParameter);
		}

		public List<DownloadDetails> GetSubDetailsSE(Guid schoolid, int status, string tag)
		{
			string sql = "SELECT   EPB.ParentId AS Id,\r\n                                    isnull(sum(ExamPaper.ItemCount),0) AS DownloadCount,\r\n                                    EPB.Name AS Tag2\r\n                           FROM  LogDetail \r\n                           INNER JOIN ExamPaper \r\n                           ON LogDetail.ResourceId = ExamPaper.Id \r\n                           INNER JOIN ExamPaperType EPA\r\n                           ON EPA.Id = ExamPaper.TypeId \r\n                           INNER JOIN ExamPaperType EPB\r\n                           ON EPB.Id = EPA.Parentid \r\n                           WHERE LogDetail.SchoolId = @SchoolId \r\n                                 AND LogDetail.Status = @Status AND LogDetail.Tag1 = @Tag1\r\n                           GROUP BY  EPB.ParentId,EPB.name\r\n                           ORDER BY DownloadCount DESC";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@SchoolId", (object)schoolid), new SqlParameter("@Status", (object)status), new SqlParameter("@Tag1", tag) };
			return SqlHelper.ExecuteList<DownloadDetails>(sql, sqlParameter);
		}

		public List<DownloadDetails> GetSubDetailsSM(Guid schoolid, int status, string tag)
		{
			string sql = "SELECT  COUNT(1) AS DownloadCount,\r\n                                   MedicalCategory.[Name]  AS Tag2,\r\n                                   MedicalCategory.Id as id\r\n                           FROM  LogDetail \r\n                           INNER JOIN Medical \r\n                           ON LogDetail.ResourceId = Medical.Id \r\n                           INNER JOIN MedicalCategory \r\n                           ON MedicalCategory.Id = Medical.CategoryId \r\n                           WHERE LogDetail.SchoolId = @SchoolId \r\n                                 AND LogDetail.Status = @Status AND LogDetail.Tag1 = @Tag1\r\n                           GROUP BY MedicalCategory.[Name],MedicalCategory.Id\r\n                           ORDER BY DownloadCount DESC ";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@SchoolId", (object)schoolid), new SqlParameter("@Status", (object)status), new SqlParameter("@Tag1", tag) };
			return SqlHelper.ExecuteList<DownloadDetails>(sql, sqlParameter);
		}

		public List<DownloadDetails> GetSubDetailsSMTwo(Guid schoolid, int status, string tag)
		{
			string sql = "SELECT  COUNT(1) AS DownloadCount,\r\n                                   MedicalSpeciality.[Name]  AS Tag2,\r\n                                   Medical.CategoryId as id\r\n                           FROM  LogDetail \r\n                           INNER JOIN Medical \r\n                           ON LogDetail.ResourceId = Medical.Id \r\n                           INNER JOIN MedicalSpeciality \r\n                           ON MedicalSpeciality.Id = Medical.SpecialityId \r\n                           WHERE LogDetail.SchoolId = @SchoolId \r\n                                 AND LogDetail.Status = @Status AND LogDetail.Tag1 = @Tag1\r\n                           GROUP BY MedicalSpeciality.[Name], Medical.CategoryId \r\n                           ORDER BY DownloadCount DESC ";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@SchoolId", (object)schoolid), new SqlParameter("@Status", (object)status), new SqlParameter("@Tag1", tag) };
			return SqlHelper.ExecuteList<DownloadDetails>(sql, sqlParameter);
		}

		public List<DownloadDetails> GetSubDetailsSQ(Guid schoolid, int status, string tag)
		{
			string sql = "SELECT  COUNT(1) AS DownloadCount,\r\n                                   QualitySubject2.[Name]  AS Tag2\r\n                           FROM  LogDetail \r\n                           INNER JOIN QualityCourse \r\n                           ON LogDetail.ResourceId = QualityCourse.Id \r\n                           INNER JOIN QualitySubject2 \r\n                           ON QualitySubject2.Id = QualityCourse.Subject2 \r\n                           WHERE LogDetail.SchoolId = @SchoolId \r\n                                 AND LogDetail.Status = @Status AND LogDetail.Tag1 = @Tag1\r\n                           GROUP BY QualitySubject2.[Name]\r\n                           ORDER BY DownloadCount DESC ";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@SchoolId", (object)schoolid), new SqlParameter("@Status", (object)status), new SqlParameter("@Tag1", tag) };
			return SqlHelper.ExecuteList<DownloadDetails>(sql, sqlParameter);
		}

		public List<DownloadDetails> GetSubDetailsSR(Guid schoolid, int status, string tag)
		{
			string sql = "SELECT  Subject.CategoryId AS Id,\r\n                                   COUNT(1) AS DownloadCount,\r\n                                   Subject.[Name]  AS Tag2\r\n                           FROM  LogDetail \r\n                           INNER JOIN Resource \r\n                           ON LogDetail.ResourceId = Resource.Id \r\n                           INNER JOIN SUBJECT \r\n                           ON Subject.Id = Resource.SubjectId \r\n                           WHERE LogDetail.SchoolId = @SchoolId \r\n                                 AND LogDetail.Status = @Status AND LogDetail.Tag1 = @Tag1\r\n                           GROUP BY Subject.[Name],Subject.CategoryId\r\n                           ORDER BY DownloadCount DESC ";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@SchoolId", (object)schoolid), new SqlParameter("@Status", (object)status), new SqlParameter("@Tag1", tag) };
			return SqlHelper.ExecuteList<DownloadDetails>(sql, sqlParameter);
		}

		public List<DownloadDetails> GetSubDetailsSRnew(Guid schoolid, int status, string tag)
		{
			string sql = "\r\nwith CTE AS\r\n(\r\n SELECT Id,ParentId,name,id as genID FROM ResourceCategory WHERE IsDeleted=0  AND Tier=1 \r\nUNION ALL\r\nSELECT ResourceCategory.Id,CTE.ParentId ,CTE.name,CTE.genID\r\nFROM ResourceCategory join CTE ON \r\nResourceCategory.ParentId = CTE.Id AND ResourceCategory.IsDeleted=0\r\n)\r\n\r\nselect sum([count]) as DownloadCount ,Parentid as id ,name as Tag2 ,genID  from(\r\n select *  from CTE as mulu\t \r\n inner join \r\n(\r\nselect ResourcePack.CategoryId ,ResourcePack.[Count] from LogDetail\r\ninner join ResourcePack on LogDetail.ResourceId=ResourcePack.id \r\nWHERE LogDetail.SchoolId = @SchoolId \r\n                                 AND LogDetail.Status = @Status AND LogDetail.Tag1 = @Tag1\r\nunion all \r\nselect ResourceMaterial.CategoryId ,1 as [count] from LogDetail\r\ninner join ResourceMaterial on LogDetail.ResourceId=ResourceMaterial.id \r\nWHERE LogDetail.SchoolId = @SchoolId \r\n                                 AND LogDetail.Status = @Status AND LogDetail.Tag1 = @Tag1\r\n)\r\nas res on mulu.id= res.CategoryId\r\n)as oneresmulu group by Parentid,name,genID order by DownloadCount desc";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@SchoolId", (object)schoolid), new SqlParameter("@Status", (object)status), new SqlParameter("@Tag1", tag) };
			return SqlHelper.ExecuteList<DownloadDetails>(sql, sqlParameter);
		}

		public List<DownloadDetails> GetSubDetailsTE(Guid teacherid, int status, string tag)
		{
			string sql = "SELECT   EPB.ParentId AS Id,\r\n                                    COUNT(1) AS DownloadCount,\r\n                                    EPB.Name AS Tag2\r\n                           FROM  LogDetail \r\n                           INNER JOIN ExamPaper \r\n                           ON LogDetail.ResourceId = ExamPaper.Id \r\n                           INNER JOIN ExamPaperType EPA\r\n                           ON EPA.Id = ExamPaper.TypeId \r\n                           INNER JOIN ExamPaperType EPB\r\n                           ON EPB.Id = EPA.Parentid \r\n                           WHERE LogDetail.TeacherId = @TeacherId \r\n                                 AND LogDetail.Status = @Status AND LogDetail.Tag1 = @Tag1\r\n                           GROUP BY  EPB.ParentId,EPB.name\r\n                           ORDER BY DownloadCount DESC";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@TeacherId", (object)teacherid), new SqlParameter("@Status", (object)status), new SqlParameter("@Tag1", tag) };
			return SqlHelper.ExecuteList<DownloadDetails>(sql, sqlParameter);
		}

		public List<DownloadDetails> GetSubDetailsTM(Guid teacherid, int status, string tag)
		{
			string sql = "SELECT  COUNT(1) AS DownloadCount,\r\n                                        MedicalCategory.[Name]  AS Tag2\r\n                           FROM  LogDetail \r\n                           INNER JOIN Medical \r\n                           ON LogDetail.ResourceId = Medical.Id \r\n                           INNER JOIN MedicalCategory \r\n                           ON MedicalCategory.Id = Medical.CategoryId \r\n                           WHERE LogDetail.TeacherId = @TeacherId\r\n                                 AND LogDetail.Status = @Status AND LogDetail.Tag1 = @Tag1\r\n                           GROUP BY MedicalCategory.[Name]\r\n                           ORDER BY DownloadCount DESC ";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@TeacherId", (object)teacherid), new SqlParameter("@Status", (object)status), new SqlParameter("@Tag1", tag) };
			return SqlHelper.ExecuteList<DownloadDetails>(sql, sqlParameter);
		}

		public List<DownloadDetails> GetSubDetailsTQ(Guid teacherid, int status, string tag)
		{
			string sql = "SELECT  COUNT(1) AS DownloadCount,\r\n                                   QualitySubject2.[Name]  AS Tag2\r\n                           FROM  LogDetail \r\n                           INNER JOIN QualityCourse \r\n                           ON LogDetail.ResourceId = QualityCourse.Id \r\n                           INNER JOIN QualitySubject2 \r\n                           ON QualitySubject2.Id = QualityCourse.Subject2 \r\n                           WHERE LogDetail.TeacherId = @TeacherId \r\n                                 AND LogDetail.Status = @Status AND LogDetail.Tag1 = @Tag1\r\n                           GROUP BY QualitySubject2.[Name]\r\n                           ORDER BY DownloadCount DESC ";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@TeacherId", (object)teacherid), new SqlParameter("@Status", (object)status), new SqlParameter("@Tag1", tag) };
			return SqlHelper.ExecuteList<DownloadDetails>(sql, sqlParameter);
		}

		public List<DownloadDetails> GetSubDetailsTR(Guid teacherid, int status, string tag)
		{
			string sql = "SELECT       Subject.CategoryId AS Id,\r\n                                        COUNT(1) AS DownloadCount,\r\n                                        Subject.[Name]  AS Tag2\r\n                           FROM  LogDetail \r\n                           INNER JOIN Resource \r\n                           ON LogDetail.ResourceId = Resource.Id \r\n                           INNER JOIN SUBJECT \r\n                           ON Subject.Id = Resource.SubjectId \r\n                           WHERE LogDetail.TeacherId = @TeacherId\r\n                                 AND LogDetail.Status = @Status AND LogDetail.Tag1 = @Tag1\r\n                           GROUP BY Subject.[Name],Subject.CategoryId\r\n                           ORDER BY DownloadCount DESC ";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@TeacherId", (object)teacherid), new SqlParameter("@Status", (object)status), new SqlParameter("@Tag1", tag) };
			return SqlHelper.ExecuteList<DownloadDetails>(sql, sqlParameter);
		}

		public List<DownloadDetails> GetSubOneSQ(Guid schoolid, int status, string tag)
		{
			string sql = "SELECT  COUNT(1) AS DownloadCount,\r\n                                   QualitySubject1.[Name]  AS Tag2,\r\n                                   QualityCourse.[Subject] as ID\r\n                           FROM  LogDetail \r\n                           INNER JOIN QualityCourse \r\n                           ON LogDetail.ResourceId = QualityCourse.Id \r\n                           INNER JOIN QualitySubject1\r\n                           ON QualitySubject1.Id = QualityCourse.Subject1\r\n                           WHERE LogDetail.SchoolId = @SchoolId \r\n                                 AND LogDetail.Status = @Status AND LogDetail.Tag1 = @Tag1\r\n                           GROUP BY QualitySubject1.[Name], QualityCourse.[Subject]\r\n                           ORDER BY DownloadCount DESC ";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@SchoolId", (object)schoolid), new SqlParameter("@Status", (object)status), new SqlParameter("@Tag1", tag) };
			return SqlHelper.ExecuteList<DownloadDetails>(sql, sqlParameter);
		}

		public List<DownloadDetails> GetSubZeroSQ(Guid schoolid, int status, string tag)
		{
			string sql = "SELECT  COUNT(1) AS DownloadCount,\r\n                                   QualitySubject.[Name]  AS Tag2,\r\n                                   QualitySubject.[ID] as ID\r\n                           FROM  LogDetail \r\n                           INNER JOIN QualityCourse \r\n                           ON LogDetail.ResourceId = QualityCourse.Id \r\n                           INNER JOIN QualitySubject\r\n                           ON QualitySubject.Id = QualityCourse.Subject\r\n                           WHERE LogDetail.SchoolId = @SchoolId \r\n                                 AND LogDetail.Status = @Status AND LogDetail.Tag1 = @Tag1\r\n                           GROUP BY QualitySubject.[Name],QualitySubject.[ID]\r\n                           ORDER BY DownloadCount DESC ";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@SchoolId", (object)schoolid), new SqlParameter("@Status", (object)status), new SqlParameter("@Tag1", tag) };
			return SqlHelper.ExecuteList<DownloadDetails>(sql, sqlParameter);
		}

		public List<ResourceCategory> GetSumCount()
		{
			return SqlHelper.ExecuteList<ResourceCategory>("SELECT [Name],[Count] FROM MedicalCategory WHERE IsDeleted = 0 order by SortId asc", new SqlParameter[0]);
		}

		public List<ResourceCategory> GetSumCountExam()
		{
			return SqlHelper.ExecuteList<ResourceCategory>("select Id,Name,PaperCount as Count from ExamPaperType where tier = 0 order by sortid", new SqlParameter[0]);
		}

		public List<ResourceCategory> GetSumCountQC()
		{
			return SqlHelper.ExecuteList<ResourceCategory>("SELECT Name,Count FROM QualitySubject WHERE IsDeleted = 0 and count>0 order by sortid", new SqlParameter[0]);
		}

		public List<ResourceCategory> GetSumCountTest()
		{
			return SqlHelper.ExecuteList<ResourceCategory>("select SubCount as Count,Name from Test where tier = 0 and isdeleted=0  ORDER BY SortId", new SqlParameter[0]);
		}

		public List<ResourceCategory> GetSumCourse()
		{
			return SqlHelper.ExecuteList<ResourceCategory>("with cte as\r\n            ( \r\n            select id,ID as parentID,Name as parentName from resourcecategory where tier = 0\r\n            union all\r\n            select resourcecategory.Id ,CTE.parentID as parentID,cte.parentName as paretName \r\n            from resourcecategory join cte on resourcecategory.parentid=cte.id\r\n            and resourcecategory.isdeleted=0\r\n            )\r\n            select count(ResourceCourse.Id) as Count,cte.parentName as Name from cte \r\n            left join resourcepack on cte.Id = resourcepack.CategoryId \r\n            left join ResourceCourse  on resourcepack.Id=ResourceCourse.PackId\r\n            where resourcepack.id is not null and ResourceCourse.Id is not null\r\n            group by  cte.parentID,cte.parentName", new SqlParameter[0]);
		}

		public List<ResourceCategory> GetSumMaterial()
		{
			return SqlHelper.ExecuteList<ResourceCategory>("with cte as\r\n                        ( \r\n                        select id,ID as parentID,Name as parentName from resourcecategory where tier = 0\r\n                        union all\r\n                        select resourcecategory.Id ,CTE.parentID as parentID,cte.parentName as paretName \r\n                        from resourcecategory join cte on resourcecategory.parentid=cte.id\r\n                        and resourcecategory.isdeleted=0\r\n                        )\r\n                        select COUNT(ResourceMaterial.id) as Count,cte.parentName as Name from cte \r\n                        left join ResourceMaterial on cte.Id = ResourceMaterial.CategoryId \r\n                        group by  cte.parentID,cte.parentName ", new SqlParameter[0]);
		}

		public List<ResourceCategory> GetSumPack()
		{
			return SqlHelper.ExecuteList<ResourceCategory>("with cte as\r\n                    ( \r\n                    select id,ID as parentID,Name as parentName from resourcecategory where tier = 0\r\n                    union all\r\n                    select resourcecategory.Id ,CTE.parentID as parentID,cte.parentName as paretName \r\n                    from resourcecategory join cte on resourcecategory.parentid=cte.id\r\n                    and resourcecategory.isdeleted=0\r\n                    )\r\n                    select COUNT(resourcepack.id) as Count,cte.parentName as Name from cte \r\n                    left join resourcepack on cte.Id = resourcepack.CategoryId \r\n                    group by  cte.parentID,cte.parentName", new SqlParameter[0]);
		}

		public List<DownloadDetails> GetTestSpetDetails(Guid schoolid, int status, string tag, int num)
		{
			string sql = "with CTE AS\r\n(\r\n SELECT Id,name,id as genID FROM Test WHERE IsDeleted=0  AND Tier=1 AND \r\n ParentId is not null\r\nUNION ALL\r\nSELECT Test.Id,CTE.name,CTE.genID\r\nFROM Test join CTE ON \r\nTest.ParentId = CTE.Id AND Test.IsDeleted=0\r\n)\r\nselect name as Tag2 ,genID as id ,isnull(sum([ItemCount])/@num,0) as DownloadCount  from (\r\nselect *  from CTE as mulu\t \r\n inner join \r\n(\r\nselect Test.id as tid,Test.ItemCount from LogDetail\r\ninner join Test on LogDetail.ResourceId=Test.id \r\nWHERE LogDetail.SchoolId =@SchoolId\r\n                                 AND LogDetail.Status = @Status AND LogDetail.Tag1 = @Tag1\r\n)\r\nas res on mulu.id= res.tid\r\n) as resmulu group by name ,genID  order by DownloadCount desc";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@num", (object)num), new SqlParameter("@SchoolId", (object)schoolid), new SqlParameter("@Status", (object)status), new SqlParameter("@Tag1", tag) };
			return SqlHelper.ExecuteList<DownloadDetails>(sql, sqlParameter);
		}

		public List<DownloadDetails> GetTestSubjectDetails(Guid schoolid, int status, string tag, int num)
		{
			string sql = "with CTE AS\r\n(\r\n SELECT Id,name,id as genID FROM Test WHERE IsDeleted=0  AND Tier=0 AND \r\n ParentId='00000000-0000-0000-0000-000000000000'\r\nUNION ALL\r\nSELECT Test.Id,CTE.name,CTE.genID\r\nFROM Test join CTE ON \r\nTest.ParentId = CTE.Id AND Test.IsDeleted=0\r\n)\r\nselect name as Tag2 ,genID as id ,isnull(sum([ItemCount])/@num,0) as DownloadCount  from (\r\nselect *  from CTE as mulu\t \r\n inner join \r\n(\r\nselect Test.id as tid,Test.ItemCount from LogDetail\r\ninner join Test on LogDetail.ResourceId=Test.id \r\nWHERE LogDetail.SchoolId =@SchoolId\r\n                                 AND LogDetail.Status = @Status AND LogDetail.Tag1 = @Tag1\r\n)\r\nas res on mulu.id= res.tid\r\n) as resmulu group by name ,genID order by DownloadCount desc";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@num", (object)num), new SqlParameter("@SchoolId", (object)schoolid), new SqlParameter("@Status", (object)status), new SqlParameter("@Tag1", tag) };
			return SqlHelper.ExecuteList<DownloadDetails>(sql, sqlParameter);
		}

		public int SoftDelete(Guid id)
		{
			string sql = "UPDATE LogDetail SET IsDeleted = 1 WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}

		public int Update(LogDetail logDetail)
		{
			string sql = "UPDATE LogDetail SET ResourceId = @ResourceId,TeacherId = @TeacherId,SchoolId = @SchoolId,Ip = @Ip,Status = @Status,Tag1 = @Tag1,Tag2 = @Tag2,CreateTime = @CreateTime,IsDeleted = @IsDeleted WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)logDetail.Id), new SqlParameter("@ResourceId", SqlHelper.ToDBValue(logDetail.ResourceId)), new SqlParameter("@TeacherId", SqlHelper.ToDBValue(logDetail.TeacherId)), new SqlParameter("@SchoolId", SqlHelper.ToDBValue(logDetail.SchoolId)), new SqlParameter("@Ip", SqlHelper.ToDBValue(logDetail.Ip)), new SqlParameter("@Status", SqlHelper.ToDBValue(logDetail.Status)), new SqlParameter("@Tag1", SqlHelper.ToDBValue(logDetail.Tag1)), new SqlParameter("@Tag2", SqlHelper.ToDBValue(logDetail.Tag2)), new SqlParameter("@CreateTime", SqlHelper.ToDBValue(logDetail.CreateTime)), new SqlParameter("@IsDeleted", SqlHelper.ToDBValue(logDetail.IsDeleted)) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}
	}
}