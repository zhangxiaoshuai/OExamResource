using Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Providers
{
	public class TestProvider
	{
		private readonly static TestProvider InstanceObj;

		public static TestProvider Instance
		{
			get
			{
				return TestProvider.InstanceObj;
			}
		}

		static TestProvider()
		{
			TestProvider.InstanceObj = new TestProvider();
		}

		private TestProvider()
		{
		}

		public Test Create(Test test)
		{
			Test test1;
			if (test.Id == Guid.Empty)
			{
				test.Id = Guid.NewGuid();
			}
			string sql = "INSERT INTO Test (Id, Flag, Name, ParentId,Size, Tier, IsTop, SortId, Logo, SubCount, ItemCount, ClickCount, CreateTime, IsDeleted)  VALUES (@Id, @Flag, @Name, @ParentId,@Size, @Tier, @IsTop, @SortId, @Logo, @SubCount, @ItemCount, @ClickCount, @CreateTime, @IsDeleted)";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", SqlHelper.ToDBValue(test.Id)), new SqlParameter("@Flag", SqlHelper.ToDBValue(test.Flag)), new SqlParameter("@Name", SqlHelper.ToDBValue(test.Name)), new SqlParameter("@ParentId", SqlHelper.ToDBValue(test.ParentId)), new SqlParameter("@Size", SqlHelper.ToDBValue(test.Size)), new SqlParameter("@Tier", SqlHelper.ToDBValue(test.Tier)), new SqlParameter("@IsTop", SqlHelper.ToDBValue(test.IsTop)), new SqlParameter("@SortId", SqlHelper.ToDBValue(test.SortId)), new SqlParameter("@Logo", SqlHelper.ToDBValue(test.Logo)), new SqlParameter("@SubCount", SqlHelper.ToDBValue(test.SubCount)), new SqlParameter("@ItemCount", SqlHelper.ToDBValue(test.ItemCount)), new SqlParameter("@ClickCount", SqlHelper.ToDBValue(test.ClickCount)), new SqlParameter("@CreateTime", SqlHelper.ToDBValue(test.CreateTime)), new SqlParameter("@IsDeleted", SqlHelper.ToDBValue(test.IsDeleted)) };
			if (SqlHelper.ExecuteNonQuery(sql, sqlParameter) > 0)
			{
				test1 = test;
			}
			else
			{
				test1 = null;
			}
			return test1;
		}

		public int Delete(Guid id)
		{
			string sql = "DELETE Test WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}

		public int DelTest(Guid id, int isdeleted)
		{
			string sql = "with cte as(\r\n                                select * from test where id = @id\r\n                                union all select test.* from cte, test where cte.id = test.parentid\r\n                            ) \r\n                            update  test set isdeleted=@isdeleted where exists (select id from cte where cte.id = test.id) ";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@id", (object)id), new SqlParameter("@isdeleted", (object)isdeleted) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}

		public int GetCount()
		{
			return (int)SqlHelper.ExecuteScalar("SELECT COUNT(Id) FROM Test WHERE IsDeleted = 0", new SqlParameter[0]);
		}

		public int GetCount(Guid id)
		{
			string sql = "with CTE AS\r\n                        (\r\n                        SELECT Tier,Id FROM Test WHERE Id = @Id AND IsDeleted = 0\r\n                        UNION ALL\r\n                        SELECT Test.Tier,Test.Id FROM Test join CTE ON Test.ParentId = CTE.Id WHERE Test.IsDeleted = 0\r\n                        )\r\n                        select count(1) from CTE Where Tier = 2";
			SqlParameter par = new SqlParameter("@Id", (object)id);
			SqlParameter[] sqlParameterArray = new SqlParameter[] { par };
			return (int)SqlHelper.ExecuteScalar(sql, sqlParameterArray);
		}

		public int GetCount(string key)
		{
			string sql = "SELECT COUNT(1) FROM Test WHERE Tier = 2 AND IsDeleted = 0 AND Name LIKE @Key";
			SqlParameter par = new SqlParameter("@Key", string.Concat("%", key, "%"));
			SqlParameter[] sqlParameterArray = new SqlParameter[] { par };
			return (int)SqlHelper.ExecuteScalar(sql, sqlParameterArray);
		}

		public int GetCount(Guid parentId, string key)
		{
			string sql = "with cte as\r\n                        (\r\n\t                        select * from test where id = @Id and IsDeleted = 0\r\n\t                        union all\r\n\t                        select test.* from test join cte on test.parentid = cte.id where test.IsDeleted = 0\r\n                        ) \r\n                        select count(1) from cte where tier = 2 and Name like @Key";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)parentId), new SqlParameter("@Key", string.Concat("%", key, "%")) };
			return (int)SqlHelper.ExecuteScalar(sql, sqlParameter);
		}

		public int GetCountAdmin()
		{
			return (int)SqlHelper.ExecuteScalar("SELECT COUNT(Id) FROM Test WHERE IsDeleted = 0 and tier=2", new SqlParameter[0]);
		}

		public int GetDelCount(Guid id)
		{
			string sql = "with cte as( select * from test where id =@id\r\nunion all select test.* from cte, test where cte.id = test.parentid)\r\nselect count(1) from cte where isdeleted=1";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@id", (object)id) };
			SqlParameter[] par = sqlParameter;
			int num = int.Parse(SqlHelper.ExecuteScalar(sql, par).ToString());
			return num;
		}

		public Test GetEntity(Guid id)
		{
			string sql = "SELECT * FROM Test WHERE Id = @Id AND IsDeleted = 0";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteEntity<Test>(sql, sqlParameter);
		}

		public Test GetEntityAdmin(Guid id)
		{
			string sql = "SELECT * FROM Test WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteEntity<Test>(sql, sqlParameter);
		}

		public List<Test> GetList()
		{
			return SqlHelper.ExecuteList<Test>("SELECT * FROM Test WHERE IsDeleted = 0", new SqlParameter[0]);
		}

		public List<Test> GetList(int tier)
		{
			string sql = "select * from Test where tier = @tier and isdeleted=0  ORDER BY SortId ";
			SqlParameter par = new SqlParameter("@tier", (object)tier);
			return SqlHelper.ExecuteList<Test>(sql, new SqlParameter[] { par });
		}

		public List<Test> GetList(Guid parentId)
		{
			string sql = "SELECT * FROM Test WHERE ParentId = @ParentId AND Tier = 1 AND IsDeleted = 0 ORDER BY SortId";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@ParentId", (object)parentId) };
			return SqlHelper.ExecuteList<Test>(sql, sqlParameter);
		}

		public List<Test> GetList(Guid parentId, int pageIndex, int pageCount)
		{
			string sql = "with CTE AS\r\n                        (\r\n                        SELECT * FROM Test WHERE Id = @Id AND IsDeleted = 0\r\n                        UNION ALL\r\n                        SELECT Test.* FROM Test join CTE ON Test.ParentId = CTE.Id WHERE Test.IsDeleted = 0\r\n                        )\r\n                        SELECT * FROM (SELECT *,(ROW_NUMBER() OVER(ORDER BY CTE.IsTop desc)) rownum FROM CTE\r\n                        WHERE CTE.Tier = 2) t  WHERE rownum>=(@pageCount*(@pageIndex-1)+1) AND rownum<=@pageIndex*@pageCount";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)parentId), new SqlParameter("@pageCount", (object)pageCount), new SqlParameter("@pageIndex", (object)pageIndex) };
			return SqlHelper.ExecuteList<Test>(sql, sqlParameter);
		}

		public List<Test> GetListAdmin(int tier)
		{
			string sql;
			SqlParameter par;
			List<Test> tests;
			SqlParameter[] sqlParameterArray;
			if (tier != 0)
			{
				sql = "with CTE AS\r\n(\r\nselect id from Test where tier=@tier-1\r\n)\r\nselect Id,Flag,Name,ParentId=(select ParentId from Test as t where t.id=Test.ParentId),Size,Tier,IsTop,SortId,Logo,SubCount,ItemCount,ClickCount,CreateTime,IsDeleted,Tag1,Tag2 from Test where ParentId  in (select id from CTE) and tier=@tier\r\nunion all\r\nselect * from Test where ParentId not in (select id from CTE) and tier=@tier";
				par = new SqlParameter("@tier", (object)tier);
				sqlParameterArray = new SqlParameter[] { par };
				tests = SqlHelper.ExecuteList<Test>(sql, sqlParameterArray);
			}
			else
			{
				sql = "select * from Test as t where tier = @tier  ORDER BY SortId ";
				par = new SqlParameter("@tier", (object)tier);
				sqlParameterArray = new SqlParameter[] { par };
				tests = SqlHelper.ExecuteList<Test>(sql, sqlParameterArray);
			}
			return tests;
		}

		public List<Test> GetListByParentId(Guid parentId)
		{
			string sql = "SELECT * FROM Test WHERE ParentId = @ParentId and isdeleted=0";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@ParentId", (object)parentId) };
			return SqlHelper.ExecuteList<Test>(sql, sqlParameter);
		}

		public List<Test> GetListByParentIdAdmin(Guid parentId)
		{
			string sql = "SELECT * FROM Test WHERE ParentId = @ParentId ";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@ParentId", (object)parentId) };
			return SqlHelper.ExecuteList<Test>(sql, sqlParameter);
		}

		public List<Test> GetPage(int pageIndex, int pageCount)
		{
			string sql = "SELECT * FROM(SELECT *,(ROW_NUMBER() OVER(ORDER BY Id)) rownum FROM Test WHERE IsDeleted = 0) t WHERE rownum>=(@pageCount*(@pageIndex-1)+1) AND rownum<=@pageIndex*@pageCount";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@pageIndex", (object)pageIndex), new SqlParameter("@pageCount", (object)pageCount) };
			return SqlHelper.ExecuteList<Test>(sql, sqlParameter);
		}

		public string GetSchool_StopRemind(Guid Id)
		{
			string sql = "SELECT StopRemind FROM School WHERE Id = @Id AND IsDeleted = 0";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)Id) };
			return SqlHelper.ExecuteScalar(sql, sqlParameter).ToString();
		}

		public List<Test> GetSup(Guid id)
		{
			string sql = "WITH CTE AS\r\n                            (\r\n                            SELECT * FROM Test WHERE Id = @Id AND IsDeleted = 0\r\n                            UNION ALL\r\n                            SELECT Test.* FROM Test join CTE ON Test.Id = CTE.ParentId WHERE Test.IsDeleted = 0\r\n                            )\r\n                            SELECT * FROM CTE ORDER BY Tier,SortId";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteList<Test>(sql, sqlParameter);
		}

		public List<Test> SearchPage(string key, int pageIndex, int pageCount)
		{
			string sql = "SELECT * FROM(\r\n                        select *,(ROW_NUMBER() OVER(ORDER BY IsTop desc)) rownum from test where Tier = 2 and IsDeleted = 0 and Name like @Key\r\n                        ) t WHERE rownum>=(@pageCount*(@pageIndex-1)+1) AND rownum<=@pageIndex*@pageCount";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Key", string.Concat("%", key, "%")), new SqlParameter("@pageCount", (object)pageCount), new SqlParameter("@pageIndex", (object)pageIndex) };
			return SqlHelper.ExecuteList<Test>(sql, sqlParameter);
		}

		public List<Test> SearchPage(Guid id, string key, int pageIndex, int pageCount)
		{
			string sql = "with cte as\r\n                    (\r\n\t                    select * from test where id = @Id and IsDeleted = 0 and tier < 2\r\n\t                    union all\r\n\t                    select test.* from test join cte on test.parentid = cte.id where test.IsDeleted = 0 and test.tier < 2\r\n                    )\r\n                    select * from (\r\n                    select *,(ROW_NUMBER() OVER(ORDER BY IsTop desc)) rownum from test where tier = 2 and IsDeleted = 0 and Name like @Key and ParentId in (select id from cte)\r\n                    ) t WHERE rownum>=(@pageCount*(@pageIndex-1)+1) AND rownum<=@pageIndex*@pageCount";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id), new SqlParameter("@Key", string.Concat("%", key, "%")), new SqlParameter("@pageCount", (object)pageCount), new SqlParameter("@pageIndex", (object)pageIndex) };
			return SqlHelper.ExecuteList<Test>(sql, sqlParameter);
		}

		public List<Test> SearchSub(string key)
		{
			string sql = "with cte as\r\n                        (\r\n                        select * from test where tier = 2 and IsDeleted = 0 and Name like @Key\r\n                        union all\r\n                        select  test.* from test join cte on test.id = cte.parentid where test.tier < 2 and test.IsDeleted = 0\r\n                        )\r\n                        select distinct * from cte where tier < 2 order by SortId";
			SqlParameter par = new SqlParameter("@Key", string.Concat("%", key, "%"));
			return SqlHelper.ExecuteList<Test>(sql, new SqlParameter[] { par });
		}

		public int SoftDelete(Guid id)
		{
			string sql = "UPDATE Test SET IsDeleted = 1 WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}

		public int Update(Test test)
		{
			string sql = "UPDATE Test SET Flag = @Flag,Name = @Name,ParentId = @ParentId,Tier = @Tier,IsTop = @IsTop,SortId = @SortId,Logo = @Logo,SubCount = @SubCount,ItemCount = @ItemCount,ClickCount = @ClickCount,CreateTime = @CreateTime,IsDeleted = @IsDeleted WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)test.Id), new SqlParameter("@Flag", SqlHelper.ToDBValue(test.Flag)), new SqlParameter("@Name", SqlHelper.ToDBValue(test.Name)), new SqlParameter("@ParentId", SqlHelper.ToDBValue(test.ParentId)), new SqlParameter("@Tier", SqlHelper.ToDBValue(test.Tier)), new SqlParameter("@IsTop", SqlHelper.ToDBValue(test.IsTop)), new SqlParameter("@SortId", SqlHelper.ToDBValue(test.SortId)), new SqlParameter("@Logo", SqlHelper.ToDBValue(test.Logo)), new SqlParameter("@SubCount", SqlHelper.ToDBValue(test.SubCount)), new SqlParameter("@ItemCount", SqlHelper.ToDBValue(test.ItemCount)), new SqlParameter("@ClickCount", SqlHelper.ToDBValue(test.ClickCount)), new SqlParameter("@CreateTime", SqlHelper.ToDBValue(test.CreateTime)), new SqlParameter("@IsDeleted", SqlHelper.ToDBValue(test.IsDeleted)) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}

		public int UpTest(Guid id, int isdeleted)
		{
			string sql = "with cte as(\r\n                                select * from test where id = @id\r\n                                union all select test.* from cte, test where cte.parentid = test.id\r\n                            ) \r\n                            update  test set isdeleted=@isdeleted where exists (select id from cte where cte.id = test.id) ";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@id", (object)id), new SqlParameter("@isdeleted", (object)isdeleted) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}
	}
}