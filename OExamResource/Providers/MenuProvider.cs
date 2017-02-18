using Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Providers
{
	public class MenuProvider
	{
		private readonly static MenuProvider InstanceObj;

		public static MenuProvider Instance
		{
			get
			{
				return MenuProvider.InstanceObj;
			}
		}

		static MenuProvider()
		{
			MenuProvider.InstanceObj = new MenuProvider();
		}

		private MenuProvider()
		{
		}

		public Menu Create(Menu menu)
		{
			Menu menu1;
			if (menu.Id == Guid.Empty)
			{
				menu.Id = Guid.NewGuid();
			}
			string sql = "INSERT INTO Menu (Id, Title, ParentId, Url, ModuleId, Type, Tag1, Tag2, Tier, CreateTime, IsDeleted)  VALUES (@Id, @Title, @ParentId, @Url, @ModuleId, @Type, @Tag1, @Tag2, @Tier, @CreateTime, @IsDeleted)";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", SqlHelper.ToDBValue(menu.Id)), new SqlParameter("@Title", SqlHelper.ToDBValue(menu.Title)), new SqlParameter("@ParentId", SqlHelper.ToDBValue(menu.ParentId)), new SqlParameter("@Url", SqlHelper.ToDBValue(menu.Url)), new SqlParameter("@ModuleId", SqlHelper.ToDBValue(menu.ModuleId)), new SqlParameter("@Type", SqlHelper.ToDBValue(menu.Type)), new SqlParameter("@Tag1", SqlHelper.ToDBValue(menu.Tag1)), new SqlParameter("@Tag2", SqlHelper.ToDBValue(menu.Tag2)), new SqlParameter("@Tier", SqlHelper.ToDBValue(menu.Tier)), new SqlParameter("@CreateTime", SqlHelper.ToDBValue(menu.CreateTime)), new SqlParameter("@IsDeleted", SqlHelper.ToDBValue(menu.IsDeleted)) };
			if (SqlHelper.ExecuteNonQuery(sql, sqlParameter) > 0)
			{
				menu1 = menu;
			}
			else
			{
				menu1 = null;
			}
			return menu1;
		}

		public int Delete(Guid id)
		{
			string sql = "DELETE Menu WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}

		public int DelMenu(Guid id)
		{
			string sql = "DELETE Menu WHERE Id = @Id OR ParentId = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}

		public int GetCount()
		{
			return (int)SqlHelper.ExecuteScalar("SELECT COUNT(Id) FROM Menu WHERE IsDeleted = 0", new SqlParameter[0]);
		}

		public Menu GetEntity(Guid id)
		{
			string sql = "SELECT * FROM Menu WHERE Id = @Id AND IsDeleted = 0";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteEntity<Menu>(sql, sqlParameter);
		}

		public Menu GetInfo(Guid id)
		{
			string sql = "SELECT * FROM Menu WHERE Id = @Id ";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteEntity<Menu>(sql, sqlParameter);
		}

		public List<Menu> GetList()
		{
			return SqlHelper.ExecuteList<Menu>("SELECT * FROM Menu WHERE IsDeleted = 0", new SqlParameter[0]);
		}

		public List<Menu> GetMenuList()
		{
			return SqlHelper.ExecuteList<Menu>("SELECT * FROM Menu where isdeleted=0 ORDER BY CreateTime", new SqlParameter[0]);
		}

		public List<Menu> GetModulesBySchoolId(Guid schoolid)
		{
			string sql = "SELECT * FROM Menu\r\n                           WHERE ModuleId IN (\r\n                            SELECT ModuleId from School_Module\r\n                            WHERE SchoolId = @schoolid and isdeleted=0\r\n                            )AND IsDeleted = 0 ORDER BY CreateTime";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@schoolid", (object)schoolid) };
			return SqlHelper.ExecuteList<Menu>(sql, sqlParameter);
		}

		public List<Menu> GetPage(int pageIndex, int pageCount)
		{
			string sql = "SELECT * FROM(SELECT *,(ROW_NUMBER() OVER(ORDER BY Id)) rownum FROM Menu WHERE IsDeleted = 0) t WHERE rownum>=(@pageCount*(@pageIndex-1)+1) AND rownum<=@pageIndex*@pageCount";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@pageIndex", (object)pageIndex), new SqlParameter("@pageCount", (object)pageCount) };
			return SqlHelper.ExecuteList<Menu>(sql, sqlParameter);
		}

		public List<Menu> GetRoleMenu()
		{
			return SqlHelper.ExecuteList<Menu>("SELECT [Id]\r\n      ,[Title]\r\n      ,[ParentId]\r\n      ,[Url]\r\n      ,[ModuleId]\r\n      ,[Type]\r\n      ,[Tag1]\r\n      ,[Tag2]\r\n      ,[Tier]\r\n      ,[CreateTime]\r\n      ,[IsDeleted]\r\n  FROM [Menu] where tier=0", new SqlParameter[0]);
		}

		public List<Menu> GetRoleMenu(Guid id)
		{
			string sql = "SELECT [Id]\r\n      ,[Title]\r\n      ,[ParentId]\r\n      ,[Url]\r\n      ,[ModuleId]\r\n      ,[Type]\r\n      ,[Tag1]\r\n      ,[Tag2]\r\n      ,[Tier]\r\n      ,[CreateTime]\r\n      ,[IsDeleted]\r\n  FROM [Menu] where ParentId=@id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteList<Menu>(sql, sqlParameter);
		}

		public int SoftDelete(Guid id)
		{
			string sql = "UPDATE Menu SET IsDeleted = 1 WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}

		public int SoftDelMenu(Guid id, int IsDeleted)
		{
			string sql = "UPDATE Menu SET IsDeleted = @IsDeleted WHERE Id = @Id OR ParentId = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id), new SqlParameter("@IsDeleted", (object)IsDeleted) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}

		public int Update(Guid id, int type)
		{
			string sql = "UPDATE Menu SET [Type]=@Type WHERE Id = @Id OR ParentId = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id), new SqlParameter("@Type", (object)type) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}

		public int Update(Menu menu)
		{
			string sql = "UPDATE Menu SET Title = @Title,ParentId = @ParentId,Url = @Url,ModuleId = @ModuleId,Type = @Type,Tag1 = @Tag1,Tag2 = @Tag2,Tier = @Tier,CreateTime = @CreateTime,IsDeleted = @IsDeleted WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)menu.Id), new SqlParameter("@Title", SqlHelper.ToDBValue(menu.Title)), new SqlParameter("@ParentId", SqlHelper.ToDBValue(menu.ParentId)), new SqlParameter("@Url", SqlHelper.ToDBValue(menu.Url)), new SqlParameter("@ModuleId", SqlHelper.ToDBValue(menu.ModuleId)), new SqlParameter("@Type", SqlHelper.ToDBValue(menu.Type)), new SqlParameter("@Tag1", SqlHelper.ToDBValue(menu.Tag1)), new SqlParameter("@Tag2", SqlHelper.ToDBValue(menu.Tag2)), new SqlParameter("@Tier", SqlHelper.ToDBValue(menu.Tier)), new SqlParameter("@CreateTime", SqlHelper.ToDBValue(menu.CreateTime)), new SqlParameter("@IsDeleted", SqlHelper.ToDBValue(menu.IsDeleted)) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}
	}
}