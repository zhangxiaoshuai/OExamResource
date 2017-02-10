using Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Providers
{
	public class QualityCourseProvider
	{
		private readonly static QualityCourseProvider InstanceObj;

		public static QualityCourseProvider Instance
		{
			get
			{
				return QualityCourseProvider.InstanceObj;
			}
		}

		static QualityCourseProvider()
		{
			QualityCourseProvider.InstanceObj = new QualityCourseProvider();
		}

		private QualityCourseProvider()
		{
		}

		public QualityCourse Create(QualityCourse qualityCourse)
		{
			QualityCourse qualityCourse1;
			if (qualityCourse.Id == Guid.Empty)
			{
				qualityCourse.Id = Guid.NewGuid();
			}
			string sql = "INSERT INTO QualityCourse (Id, Name, SchoolName, Author, Province, AwardsDate, CourceSort, Hierarchy, Subject, Subject1, Subject2, Connected, Address, Thesenpapier, Introduction, TeacherTeam, Condition, Methods, Content, Effect, Tag1, Tag2, IsDeleted, Createtime)  VALUES (@Id, @Name, @SchoolName, @Author, @Province, @AwardsDate, @CourceSort, @Hierarchy, @Subject, @Subject1, @Subject2, @Connected, @Address, @Thesenpapier, @Introduction, @TeacherTeam, @Condition, @Methods, @Content, @Effect, @Tag1, @Tag2, @IsDeleted, @Createtime)";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", SqlHelper.ToDBValue(qualityCourse.Id)), new SqlParameter("@Name", SqlHelper.ToDBValue(qualityCourse.Name)), new SqlParameter("@SchoolName", SqlHelper.ToDBValue(qualityCourse.SchoolName)), new SqlParameter("@Author", SqlHelper.ToDBValue(qualityCourse.Author)), new SqlParameter("@Province", SqlHelper.ToDBValue(qualityCourse.Province)), new SqlParameter("@AwardsDate", SqlHelper.ToDBValue(qualityCourse.AwardsDate)), new SqlParameter("@CourceSort", SqlHelper.ToDBValue(qualityCourse.CourceSort)), new SqlParameter("@Hierarchy", SqlHelper.ToDBValue(qualityCourse.Hierarchy)), new SqlParameter("@Subject", SqlHelper.ToDBValue(qualityCourse.Subject)), new SqlParameter("@Subject1", SqlHelper.ToDBValue(qualityCourse.Subject1)), new SqlParameter("@Subject2", SqlHelper.ToDBValue(qualityCourse.Subject2)), new SqlParameter("@Connected", SqlHelper.ToDBValue(qualityCourse.Connected)), new SqlParameter("@Address", SqlHelper.ToDBValue(qualityCourse.Address)), new SqlParameter("@Thesenpapier", SqlHelper.ToDBValue(qualityCourse.Thesenpapier)), new SqlParameter("@Introduction", SqlHelper.ToDBValue(qualityCourse.Introduction)), new SqlParameter("@TeacherTeam", SqlHelper.ToDBValue(qualityCourse.TeacherTeam)), new SqlParameter("@Condition", SqlHelper.ToDBValue(qualityCourse.Condition)), new SqlParameter("@Methods", SqlHelper.ToDBValue(qualityCourse.Methods)), new SqlParameter("@Content", SqlHelper.ToDBValue(qualityCourse.Content)), new SqlParameter("@Effect", SqlHelper.ToDBValue(qualityCourse.Effect)), new SqlParameter("@Tag1", SqlHelper.ToDBValue(qualityCourse.Tag1)), new SqlParameter("@Tag2", SqlHelper.ToDBValue(qualityCourse.Tag2)), new SqlParameter("@IsDeleted", SqlHelper.ToDBValue(qualityCourse.IsDeleted)), new SqlParameter("@Createtime", SqlHelper.ToDBValue(qualityCourse.Createtime)) };
			if (SqlHelper.ExecuteNonQuery(sql, sqlParameter) > 0)
			{
				qualityCourse1 = qualityCourse;
			}
			else
			{
				qualityCourse1 = null;
			}
			return qualityCourse1;
		}

		public int Delete(Guid id)
		{
			string sql = "DELETE QualityCourse WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}

		public int GetCount()
		{
			return (int)SqlHelper.ExecuteScalar("SELECT COUNT(Id) FROM QualityCourse WHERE IsDeleted = 0", new SqlParameter[0]);
		}

		public int GetCourseCount(string sort, string year, int conn, string name)
		{
			string sql = "SELECT COUNT(Id) FROM QualityCourse WHERE IsDeleted = 0 ";
			if (sort != "-1")
			{
				sql = string.Concat(sql, " AND CourceSort = @CourceSort ");
			}
			if (year != "-1")
			{
				sql = string.Concat(sql, " AND AwardsDate = @AwardsDate");
			}
			if (conn != -1)
			{
				sql = string.Concat(sql, " AND Connected = @Connected");
			}
			if (!string.IsNullOrEmpty(name))
			{
				sql = string.Concat(sql, " AND Name LIKE @Name ");
			}
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@CourceSort", sort), new SqlParameter("@AwardsDate", year), new SqlParameter("@Connected", (object)conn), new SqlParameter("@Name", string.Concat("%", name, "%")) };
			return (int)SqlHelper.ExecuteScalar(sql, sqlParameter);
		}

		public List<QualityCourse> GetCourseList(string sort, string year, int conn, string name, int pageIndex, int pageCount)
		{
			string sql = "SELECT * FROM( \r\n                                SELECT \r\n\t\t\t\t\t\t            Id,\r\n                                    Name,\r\n                                    SchoolName,\r\n                                    Author,\r\n                                    Province,\r\n                                    AwardsDate,\r\n                                    CourceSort,\r\n                                    Connected,\r\n\t\t\t\t\t\t            ROW_NUMBER() OVER(ORDER BY AwardsDate desc,Connected) rownum \r\n                                FROM QualityCourse \r\n                                WHERE Isdeleted = 0";
			if (sort != "-1")
			{
				sql = string.Concat(sql, " AND CourceSort = @CourceSort ");
			}
			if (year != "-1")
			{
				sql = string.Concat(sql, " AND AwardsDate = @AwardsDate");
			}
			if (conn != -1)
			{
				sql = string.Concat(sql, " AND Connected = @Connected");
			}
			if (!string.IsNullOrEmpty(name))
			{
				sql = string.Concat(sql, " AND Name LIKE @Name ");
			}
			sql = string.Concat(sql, " ) as t\r\n                      WHERE rownum>=(@pageCount*(@pageIndex-1)+1) AND rownum<=@pageIndex*@pageCount");
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@pageIndex", (object)pageIndex), new SqlParameter("@pageCount", (object)pageCount), new SqlParameter("@CourceSort", sort), new SqlParameter("@AwardsDate", year), new SqlParameter("@Connected", (object)conn), new SqlParameter("@Name", string.Concat("%", name, "%")) };
			return SqlHelper.ExecuteList<QualityCourse>(sql, sqlParameter);
		}

		public QualityDetail GetDetail(Guid id)
		{
			string sql = "SELECT  Id \r\n      , Name \r\n      , Schoolname \r\n      , Author \r\n      , Province \r\n      , AwardsDate \r\n      , CourceSort \r\n      , Hierarchy \r\n      , Subject =(select name from QualitySubject as a where a.id=c.Subject)\r\n      , Subject1 =(select name from QualitySubject1 as aa where aa.id=c.Subject1)\r\n      , Subject2 =(select name from QualitySubject2 as aaa where aaa.id=c.Subject2)\r\n      , Connected \r\n      , Address \r\n      , Thesenpapier\r\n      , Introduction \r\n      , TeacherTeam \r\n      ,  Condition\r\n      , Methods \r\n      , Content\r\n      , Effect\r\n      , Tag1 \r\n      , Tag2 \r\n      ,  IsDeleted\r\n      ,  Createtime\r\n  FROM QualityCourse as c where c.Id=@id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@id", (object)id) };
			return SqlHelper.ExecuteEntity<QualityDetail>(sql, sqlParameter);
		}

		public QualityCourse GetEntity(Guid id)
		{
			string sql = "SELECT * FROM QualityCourse WHERE Id = @Id AND IsDeleted = 0";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteEntity<QualityCourse>(sql, sqlParameter);
		}

		public List<QualityCourse> GetHierarchy()
		{
			return SqlHelper.ExecuteList<QualityCourse>("SELECT Hierarchy FROM QualityCourse WHERE IsDeleted = 0 GROUP BY Hierarchy", new SqlParameter[0]);
		}

		public List<QualityCourceSort> Getjibie()
		{
			return SqlHelper.ExecuteList<QualityCourceSort>("select CourceSort from  dbo.QualityCourse group by CourceSort", new SqlParameter[0]);
		}

		public List<QualityCourse> GetList()
		{
			return SqlHelper.ExecuteList<QualityCourse>("SELECT * FROM QualityCourse WHERE IsDeleted = 0", new SqlParameter[0]);
		}

		public QualityNav GetNav(Guid id)
		{
			string sql = " select Id,Name,c.Subject as subid,count=(select Count from QualitySubject as a where a.id=c.Subject),Subject=(select name from QualitySubject as a where a.id=c.Subject),\r\nc.Subject1 as subid1,count1=(select Count from QualitySubject1 as aa where aa.id=c.Subject1),Subject1=(select name from QualitySubject1 as aa where aa.id=c.Subject1),\r\nc.Subject2 as subid2,count2=(select Count from QualitySubject2 as aaa where aaa.id=c.Subject2),Subject2=(select name from QualitySubject2 as aaa where aaa.id=c.Subject2)\r\nfrom QualityCourse as c where c.id=@id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@id", (object)id) };
			return SqlHelper.ExecuteEntity<QualityNav>(sql, sqlParameter);
		}

		public List<QualityAwardsDate> Getniandu()
		{
			return SqlHelper.ExecuteList<QualityAwardsDate>("select  AwardsDate from  dbo.QualityCourse where AwardsDate is not null group by AwardsDate order by AwardsDate desc", new SqlParameter[0]);
		}

		public List<QualityCourse> GetPage(int pageIndex, int pageCount)
		{
			string sql = "SELECT * FROM(SELECT *,(ROW_NUMBER() OVER(ORDER BY Id)) rownum FROM QualityCourse WHERE IsDeleted = 0) t WHERE rownum>=(@pageCount*(@pageIndex-1)+1) AND rownum<=@pageIndex*@pageCount";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@pageIndex", (object)pageIndex), new SqlParameter("@pageCount", (object)pageCount) };
			return SqlHelper.ExecuteList<QualityCourse>(sql, sqlParameter);
		}

		public List<QualityCourse> GetResource(string searchname, Guid id, int type, int pageindex, int pagecount, string jibie, string niandu)
		{
			string sql = "select * from (select ROW_NUMBER() OVER(ORDER BY case when Name like @searchname then 0\r\nelse 1 end,Connected, AwardsDate DESC) as rownum,[Id]\r\n      ,[Name]\r\n      ,[SchoolName]\r\n      ,[Author] \r\n      ,[AwardsDate]\r\n      ,[CourceSort]\r\n      ,[Hierarchy]\r\n      ,[Subject]\r\n      ,[Subject1]\r\n      ,[Subject2]\r\n      ,[Connected]\r\n      ,[Address]  \r\n      ,[Tag2]\r\n      ,[IsDeleted]\r\n      ,[Createtime] from QualityCourse where ";
			if (type == 0)
			{
				sql = string.Concat(sql, "  Subject=@id and");
			}
			else if (type == 1)
			{
				sql = string.Concat(sql, "  Subject1=@id and");
			}
			else if (type == 2)
			{
				sql = string.Concat(sql, "  Subject2=@id and");
			}
			sql = string.Concat(sql, " Name like @searchname   and");
			if ((jibie == "选择级别" ? false : jibie != ""))
			{
				sql = string.Concat(sql, " CourceSort = '", jibie, "' and");
			}
			if ((niandu == "选择年度" ? false : niandu != ""))
			{
				sql = string.Concat(sql, " AwardsDate = '", niandu.Substring(0, niandu.Length - 1), "' and");
			}
			sql = string.Concat(sql, " IsDeleted=0) as a  WHERE rownum>=(@pagecount*(@pageindex-1)+1) AND rownum<=@pageindex*@pagecount");
			List<SqlParameter> par = new List<SqlParameter>()
			{
				new SqlParameter("@pageindex", (object)pageindex),
				new SqlParameter("@pagecount", (object)pagecount),
				new SqlParameter("@id", (object)id),
				new SqlParameter("@searchname", string.Concat("%", searchname, "%"))
			};
			return SqlHelper.ExecuteList<QualityCourse>(sql, par.ToArray());
		}

		public int GetResourceCount(string searchname, Guid id, int type, string jibie, string niandu)
		{
			string sql = "select count(1) from dbo.QualityCourse where ";
			List<SqlParameter> par = new List<SqlParameter>();
			if (type == 0)
			{
				sql = string.Concat(sql, " Subject=@id and");
			}
			else if (type == 1)
			{
				sql = string.Concat(sql, " Subject1=@id and");
			}
			else if (type == 2)
			{
				sql = string.Concat(sql, " Subject2=@id and");
			}
			if (!string.IsNullOrEmpty(searchname))
			{
				sql = string.Concat(sql, " Name like @searchname  and");
			}
			if ((jibie == "选择级别" ? false : jibie != ""))
			{
				sql = string.Concat(sql, " CourceSort = '", jibie, "' and");
			}
			if ((niandu == "选择年度" ? false : niandu != ""))
			{
				sql = string.Concat(sql, " AwardsDate = '", niandu.Substring(0, niandu.Length - 1), "' and");
			}
			sql = string.Concat(sql, " IsDeleted=0");
			par.Add(new SqlParameter("@id", (object)id));
			if (!string.IsNullOrEmpty(searchname))
			{
				par.Add(new SqlParameter("@searchname", string.Concat("%", searchname, "%")));
			}
			int num = Convert.ToInt32(SqlHelper.ExecuteScalar(sql, par.ToArray()));
			return num;
		}

		public QualitySubject1 GetSubject1(string searchname, Guid id, string jibie, string niandu)
		{
			string sql = "select Id,Name,Count=(select count(1) from dbo.QualityCourse where Subject1=QualitySubject1.Id and QualityCourse.Name like @searchname ";
			if ((jibie == "选择级别" ? false : jibie != ""))
			{
				sql = string.Concat(sql, " and CourceSort = '", jibie, "' ");
			}
			if ((niandu == "选择年度" ? false : niandu != ""))
			{
				sql = string.Concat(sql, " and AwardsDate = '", niandu.Substring(0, niandu.Length - 1), "' ");
			}
			sql = string.Concat(sql, "),CreateTime,IsDeleted from dbo.QualitySubject1 where Id=@id");
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@id", (object)id), new SqlParameter("@searchname", string.Concat("%", searchname, "%")) };
			return SqlHelper.ExecuteEntity<QualitySubject1>(sql, sqlParameter);
		}

		public List<QualitySubject1> GetSubject1(string searchname, string jibie, string niandu)
		{
			string sql = "select Id,Name,Count=(select Count(1) from dbo.QualityCourse where Subject1=QualitySubject1.id ";
			if ((jibie == "选择级别" ? false : jibie != ""))
			{
				sql = string.Concat(sql, " and CourceSort = '", jibie, "' ");
			}
			if ((niandu == "选择年度" ? false : niandu != ""))
			{
				sql = string.Concat(sql, " and AwardsDate = '", niandu.Substring(0, niandu.Length - 1), "' ");
			}
			if (!string.IsNullOrEmpty(searchname))
			{
				sql = string.Concat(sql, " and Name like @searchname");
			}
			sql = string.Concat(sql, "),CreateTime,Isdeleted from dbo.QualitySubject1");
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@searchname", string.Concat("%", searchname, "%")) };
			return SqlHelper.ExecuteList<QualitySubject1>(sql, sqlParameter);
		}

		public List<QualitySubject1> GetSubject1list(string searchname, Guid id, string jibie, string niandu)
		{
			string sql = "select Id,Name,Subjectid,Count=(select count(1) from QualityCourse where Subject1=QualitySubject1.Id ";
			if (!string.IsNullOrEmpty(searchname))
			{
				sql = string.Concat(sql, " and  QualityCourse.Name like @searchname");
			}
			if ((jibie == "选择级别" ? false : jibie != ""))
			{
				sql = string.Concat(sql, " and CourceSort = '", jibie, "' ");
			}
			if ((niandu == "选择年度" ? false : niandu != ""))
			{
				sql = string.Concat(sql, " and AwardsDate = '", niandu.Substring(0, niandu.Length - 1), "' ");
			}
			sql = string.Concat(sql, " and isdeleted=0 ),CreateTime,IsDeleted from dbo.QualitySubject1 where Subjectid=@id");
			List<SqlParameter> par = new List<SqlParameter>()
			{
				new SqlParameter("@id", (object)id)
			};
			if (!string.IsNullOrEmpty(searchname))
			{
				par.Add(new SqlParameter("@searchname", string.Concat("%", searchname, "%")));
			}
			return SqlHelper.ExecuteList<QualitySubject1>(sql, par.ToArray());
		}

		public QualitySubject2 GetSubject2(string searchname, Guid id, string jibie, string niandu)
		{
			string sql = "select Id,Name,Subject1id,Count=(select count(1) from QualityCourse where Subject2=QualitySubject2.Id and QualityCourse.Name like @searchname ";
			if ((jibie == "选择级别" ? false : jibie != ""))
			{
				sql = string.Concat(sql, " and CourceSort = '", jibie, "' ");
			}
			if ((niandu == "选择年度" ? false : niandu != ""))
			{
				sql = string.Concat(sql, " and AwardsDate = '", niandu.Substring(0, niandu.Length - 1), "' ");
			}
			sql = string.Concat(sql, "),CreateTime,IsDeleted from dbo.QualitySubject2 where Id=@id");
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@id", (object)id), new SqlParameter("@searchname", string.Concat("%", searchname, "%")) };
			return SqlHelper.ExecuteEntity<QualitySubject2>(sql, sqlParameter);
		}

		public List<QualitySubject2> GetSubject2list(string searchname, Guid id, string jibie, string niandu)
		{
			string sql = "select Id,Name,Subject1id,Count=(select count(1) from QualityCourse where Subject2=QualitySubject2.Id ";
			if (!string.IsNullOrEmpty(searchname))
			{
				sql = string.Concat(sql, " and  QualityCourse.Name like @searchname");
			}
			if ((jibie == "选择级别" ? false : jibie != ""))
			{
				sql = string.Concat(sql, " and CourceSort = '", jibie, "' ");
			}
			if ((niandu == "选择年度" ? false : niandu != ""))
			{
				sql = string.Concat(sql, " and AwardsDate = '", niandu.Substring(0, niandu.Length - 1), "' ");
			}
			sql = string.Concat(sql, " and isdeleted=0 ),CreateTime,IsDeleted from dbo.QualitySubject2 where Subject1id=@id");
			List<SqlParameter> par = new List<SqlParameter>()
			{
				new SqlParameter("@id", (object)id)
			};
			if (!string.IsNullOrEmpty(searchname))
			{
				par.Add(new SqlParameter("@searchname", string.Concat("%", searchname, "%")));
			}
			return SqlHelper.ExecuteList<QualitySubject2>(sql, par.ToArray());
		}

		public int SoftDelete(Guid id)
		{
			string sql = "UPDATE QualityCourse SET IsDeleted = 1 WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}

		public int Update(QualityCourse qualityCourse)
		{
			string sql = "UPDATE QualityCourse SET Name = @Name,SchoolName = @SchoolName,Author = @Author,Province = @Province,AwardsDate = @AwardsDate,CourceSort = @CourceSort,Hierarchy = @Hierarchy,Subject = @Subject,Subject1 = @Subject1,Subject2 = @Subject2,Connected = @Connected,Address = @Address,Thesenpapier = @Thesenpapier,Introduction = @Introduction,TeacherTeam = @TeacherTeam,Condition = @Condition,Methods = @Methods,Content = @Content,Effect = @Effect,Tag1 = @Tag1,Tag2 = @Tag2,IsDeleted = @IsDeleted,Createtime = @Createtime WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)qualityCourse.Id), new SqlParameter("@Name", SqlHelper.ToDBValue(qualityCourse.Name)), new SqlParameter("@SchoolName", SqlHelper.ToDBValue(qualityCourse.SchoolName)), new SqlParameter("@Author", SqlHelper.ToDBValue(qualityCourse.Author)), new SqlParameter("@Province", SqlHelper.ToDBValue(qualityCourse.Province)), new SqlParameter("@AwardsDate", SqlHelper.ToDBValue(qualityCourse.AwardsDate)), new SqlParameter("@CourceSort", SqlHelper.ToDBValue(qualityCourse.CourceSort)), new SqlParameter("@Hierarchy", SqlHelper.ToDBValue(qualityCourse.Hierarchy)), new SqlParameter("@Subject", SqlHelper.ToDBValue(qualityCourse.Subject)), new SqlParameter("@Subject1", SqlHelper.ToDBValue(qualityCourse.Subject1)), new SqlParameter("@Subject2", SqlHelper.ToDBValue(qualityCourse.Subject2)), new SqlParameter("@Connected", SqlHelper.ToDBValue(qualityCourse.Connected)), new SqlParameter("@Address", SqlHelper.ToDBValue(qualityCourse.Address)), new SqlParameter("@Thesenpapier", SqlHelper.ToDBValue(qualityCourse.Thesenpapier)), new SqlParameter("@Introduction", SqlHelper.ToDBValue(qualityCourse.Introduction)), new SqlParameter("@TeacherTeam", SqlHelper.ToDBValue(qualityCourse.TeacherTeam)), new SqlParameter("@Condition", SqlHelper.ToDBValue(qualityCourse.Condition)), new SqlParameter("@Methods", SqlHelper.ToDBValue(qualityCourse.Methods)), new SqlParameter("@Content", SqlHelper.ToDBValue(qualityCourse.Content)), new SqlParameter("@Effect", SqlHelper.ToDBValue(qualityCourse.Effect)), new SqlParameter("@Tag1", SqlHelper.ToDBValue(qualityCourse.Tag1)), new SqlParameter("@Tag2", SqlHelper.ToDBValue(qualityCourse.Tag2)), new SqlParameter("@IsDeleted", SqlHelper.ToDBValue(qualityCourse.IsDeleted)), new SqlParameter("@Createtime", SqlHelper.ToDBValue(qualityCourse.Createtime)) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}
	}
}