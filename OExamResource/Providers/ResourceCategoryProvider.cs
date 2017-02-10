using Microsoft.CSharp.RuntimeBinder;
using PetaPoco;
using Common;
using Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace Providers
{
	public class ResourceCategoryProvider
	{
		private readonly static ResourceCategoryProvider InstanceObj;

		public static ResourceCategoryProvider Instance
		{
			get
			{
				return ResourceCategoryProvider.InstanceObj;
			}
		}

		static ResourceCategoryProvider()
		{
			ResourceCategoryProvider.InstanceObj = new ResourceCategoryProvider();
		}

		private ResourceCategoryProvider()
		{
		}

		public ResourceCategory Create(ResourceCategory resourceCategory)
		{
			ResourceCategory resourceCategory1;
			if (resourceCategory.Id == Guid.Empty)
			{
				resourceCategory.Id = Guid.NewGuid();
			}
			string sql = "INSERT INTO ResourceCategory (Id, ParentId, Tier, Name, Ico, SortId, Count, CreateTime, IsDeleted)  VALUES (@Id, @ParentId, @Tier, @Name, @Ico, @SortId, @Count, @CreateTime, @IsDeleted)";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", SqlHelper.ToDBValue(resourceCategory.Id)), new SqlParameter("@ParentId", SqlHelper.ToDBValue(resourceCategory.ParentId)), new SqlParameter("@Tier", SqlHelper.ToDBValue(resourceCategory.Tier)), new SqlParameter("@Name", SqlHelper.ToDBValue(resourceCategory.Name)), new SqlParameter("@Ico", SqlHelper.ToDBValue(resourceCategory.Ico)), new SqlParameter("@SortId", SqlHelper.ToDBValue(resourceCategory.SortId)), new SqlParameter("@Count", SqlHelper.ToDBValue(resourceCategory.Count)), new SqlParameter("@CreateTime", SqlHelper.ToDBValue(resourceCategory.CreateTime)), new SqlParameter("@IsDeleted", SqlHelper.ToDBValue(resourceCategory.IsDeleted)) };
			if (SqlHelper.ExecuteNonQuery(sql, sqlParameter) > 0)
			{
				resourceCategory1 = resourceCategory;
			}
			else
			{
				resourceCategory1 = null;
			}
			return resourceCategory1;
		}

		public int Delete(Guid id)
		{
			string sql = "DELETE ResourceCategory WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}

		public List<ResourceCategory> GetCategoryTier0()
		{
			return SqlHelper.ExecuteList<ResourceCategory>("SELECT * FROM ResourceCategory WHERE IsDeleted = 0 and tier=0 order by sortId ", new SqlParameter[0]);
		}

		public List<ResourceCategory> GetCategoryTier1(Guid subid)
		{
			string sql = "SELECT * FROM ResourceCategory WHERE IsDeleted = 0 and ParentId=@subid order by sortId ";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@subid", (object)subid) };
			return SqlHelper.ExecuteList<ResourceCategory>(sql, sqlParameter);
		}

		public List<ResourceCategory> GetCategoryTier2(Guid speid)
		{
			string sql = "SELECT * FROM ResourceCategory WHERE IsDeleted = 0 and ParentId=@speid order by sortId ";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@speid", (object)speid) };
			return SqlHelper.ExecuteList<ResourceCategory>(sql, sqlParameter);
		}

		public List<ResourceCategory> GetCategoryTier3(Guid Know)
		{
			string sql = "SELECT * FROM ResourceCategory WHERE IsDeleted = 0 and ParentId=@Know order by sortId ";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Know", (object)Know) };
			return SqlHelper.ExecuteList<ResourceCategory>(sql, sqlParameter);
		}

		public int GetCount()
		{
			return (int)SqlHelper.ExecuteScalar("SELECT COUNT(Id) FROM ResourceCategory WHERE IsDeleted = 0", new SqlParameter[0]);
		}

		public ResourceCategory GetEntity(Guid id)
		{
			string sql = "SELECT * FROM ResourceCategory WHERE Id = @Id AND IsDeleted = 0";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteEntity<ResourceCategory>(sql, sqlParameter);
		}

		public bool GetIdBool(Guid? id)
		{
			bool flag;
			string sql = "SELECT * FROM ResourceCategory WHERE  IsDeleted = 0 AND Name = @name";
			List<SqlParameter> para = new List<SqlParameter>();
			Guid? nullable = id;
			Guid empty = Guid.Empty;
			if (nullable != null)
			{
				sql = string.Concat(sql, " and Id=@ID");
				para.Add(new SqlParameter("@Id", (object)id));
			}
			flag = (SqlHelper.ExecuteList<ResourceCategory>(sql, para.ToArray()).Count <= 0 ? false : true);
			return flag;
		}

		public List<ResourceCategory> GetList(Enums.ResourceType tier)
		{
			string sql = "SELECT * FROM ResourceCategory WHERE Medical_Tag=0 AND IsDeleted = 0 AND Tier = @0 ORDER BY SortId";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@0", (object)((int)tier)) };
			return SqlHelper.ExecuteList<ResourceCategory>(sql, sqlParameter);
		}

		public List<ResourceCategory> GetList(Guid parentId)
		{
			string sql = "WITH CTE AS\r\n                            (\r\n                            SELECT * FROM ResourceCategory WHERE Id = @Id AND IsDeleted = 0\r\n                            UNION ALL\r\n                            SELECT ResourceCategory.* FROM ResourceCategory join CTE ON ResourceCategory.ParentId = CTE.Id WHERE ResourceCategory.IsDeleted = 0\r\n                            )\r\n                            SELECT * FROM CTE ORDER BY SortId ASC,Tier DESC";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)parentId) };
			return SqlHelper.ExecuteList<ResourceCategory>(sql, sqlParameter);
		}

		public List<ResourceCategory> GetList()
		{
			return SqlHelper.ExecuteList<ResourceCategory>("SELECT * FROM ResourceCategory WHERE IsDeleted = 0", new SqlParameter[0]);
		}

		public bool GetNameBool(Guid? id, string name)
		{
			bool flag;
			string sql = "SELECT * FROM ResourceCategory WHERE  IsDeleted = 0 AND Name = @name";
			List<SqlParameter> para = new List<SqlParameter>()
			{
				new SqlParameter("@name", name)
			};
			Guid? nullable = id;
			Guid empty = Guid.Empty;
			if (nullable != null)
			{
				sql = string.Concat(sql, " and Id=@ID");
				para.Add(new SqlParameter("@Id", (object)id));
			}
			flag = (SqlHelper.ExecuteList<ResourceCategory>(sql, para.ToArray()).Count <= 0 ? false : true);
			return flag;
		}

		public List<ResourceCategory> GetPage(int pageIndex, int pageCount)
		{
			string sql = "SELECT * FROM(SELECT *,(ROW_NUMBER() OVER(ORDER BY Id)) rownum FROM ResourceCategory WHERE IsDeleted = 0) t WHERE rownum>=(@pageCount*(@pageIndex-1)+1) AND rownum<=@pageIndex*@pageCount";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@pageIndex", (object)pageIndex), new SqlParameter("@pageCount", (object)pageCount) };
			return SqlHelper.ExecuteList<ResourceCategory>(sql, sqlParameter);
		}

		public List<ResourceCategory> GetSubList(Guid? parentId, string key, bool isJX, bool IsResource)
		{
			List<ResourceCategory> list;
			string sqlCate;
			object[] variable;
			DB db = DB.GetInstance();
			string isResourceSql = "";
			string isPackSql = "";
			string isMaterialSql = "";
			if (IsResource)
			{
				isResourceSql = " and ResourceCategory.Medical_Tag =0 ";
				isPackSql = " AND ResourcePack.Medical_Tag=0";
				isMaterialSql = " AND ResourceMaterial.Medical_Tag=0";
			}
			string sql = "";
			sql = (IsResource ? "SELECT * FROM ResourceCategory WHERE {0} AND IsDeleted = 0 {1} ORDER BY SortId" : "SELECT Id,ParentId ,Tier ,Name,Ico ,SortId,Count=MedicalCount ,Count_JX=MedicalCount,CreateTime ,IsDeleted FROM ResourceCategory \r\n                       WHERE {0} AND IsDeleted = 0 {1} ORDER BY SortId");
			if (parentId.HasValue)
			{
				sql = string.Format(sql, "ParentId = @ParentId", isResourceSql);
				variable = new object[] { new { ParentId = parentId } };
				list = db.Fetch<ResourceCategory>(sql, variable);
			}
			else
			{
				sql = string.Format(sql, "Tier=0 AND ParentId is null", isResourceSql);
				list = db.Fetch<ResourceCategory>(sql, new object[0]);
			}
			if (!string.IsNullOrWhiteSpace(key))
			{
				string sqlCount = ";WITH CTE AS (SELECT Id FROM ResourceCategory where IsDeleted = 0 {0}";
				sqlCount = (parentId.HasValue ? string.Concat(sqlCount, "AND ParentId = @ParentId ") : string.Concat(sqlCount, "AND ParentId is null"));
				sqlCount = string.Concat(sqlCount, " UNION ALL SELECT ResourceCategory.Id FROM ResourceCategory join CTE ON ResourceCategory.ParentId = CTE.Id WHERE ResourceCategory.IsDeleted = 0 {0})");
				sqlCount = string.Concat(sqlCount, "select CategoryId,SUM(c) as C from\r\n(select  ResourcePack.CategoryId,Sum([Count]) as c FROM ResourcePack JOIN CTE ON CategoryId = CTE.Id WHERE contains((Name,KeyWord),@key) and IsDeleted=0 {1} group by ResourcePack.CategoryId\r\nunion all\r\nselect ResourceMaterial.CategoryId,c=1 FROM ResourceMaterial JOIN CTE ON CategoryId = CTE.Id WHERE contains((Title,KeyWords),@key) and IsDeleted=0  {2}\r\n) t group by t.CategoryId");
				sqlCount = string.Format(sqlCount, isResourceSql, isPackSql, isMaterialSql);
				sqlCate = (parentId.HasValue ? ";WITH CTE AS (SELECT Id,ParentId FROM ResourceCategory where  IsDeleted = 0 AND ParentId = @ParentId \r\nUNION ALL SELECT ResourceCategory.Id,ResourceCategory.ParentId FROM ResourceCategory join CTE ON ResourceCategory.ParentId = CTE.Id WHERE ResourceCategory.IsDeleted = 0 ) SELECT Id,ParentId FROM CTE" : "SELECT Id,ParentId FROM ResourceCategory where ParentId is not null and IsDeleted = 0 {0}");
				sqlCate = string.Format(sqlCate, isResourceSql);
				variable = new object[] { new { ParentId = parentId, key = string.Concat("\"", key, "\"") } };
				List<object> listCount = db.Fetch<object>(sqlCount, variable);
				variable = new object[] { new { ParentId = parentId } };
				List<object> listCate = db.Fetch<object>(sqlCate, variable);
				foreach (ResourceCategory item in list)
				{
					if (!isJX)
					{
						item.Count = new int?(this.Recursion(item.Id, listCount, listCate));
					}
					else
					{
						item.Count_JX = new int?(this.Recursion(item.Id, listCount, listCate));
					}
				}
				list = (!isJX ? (
					from p in list
					orderby p.Count descending
					select p).ToList<ResourceCategory>() : (
					from p in list
					orderby p.Count_JX descending
					select p).ToList<ResourceCategory>());
			}
			if (!isJX)
			{
				list.RemoveAll((ResourceCategory p) => {
					int? count = p.Count;
					return (count.GetValueOrDefault() > 0 ? false : count.HasValue);
				});
			}
			else
			{
				list.RemoveAll((ResourceCategory p) => {
					int? countJX = p.Count_JX;
					return (countJX.GetValueOrDefault() > 0 ? false : countJX.HasValue);
				});
			}
			return list;
		}

		public List<ResourceCategory> GetSuperList(Guid? id, int i = 0)
		{
			List<ResourceCategory> resourceCategories;
			if (id.HasValue)
			{
				string sql = ";WITH CTE AS\r\n                            (\r\n                            SELECT * FROM ResourceCategory WHERE Id = @Id  {0}\r\n                            UNION ALL\r\n                            SELECT ResourceCategory.* FROM ResourceCategory join CTE ON ResourceCategory.Id = CTE.ParentId where 1=1 {0}\r\n                            )\r\n                            SELECT * FROM CTE  ORDER BY Tier,SortId";
				string sqlc = "";
				if (i != 0)
				{
					sqlc = " and ResourceCategory.tier != 0 ";
				}
				sql = string.Format(sql, sqlc);
				DB instance = DB.GetInstance();
				object[] variable = new object[] { new { Id = id } };
				resourceCategories = instance.Fetch<ResourceCategory>(sql, variable);
			}
			else
			{
				resourceCategories = new List<ResourceCategory>();
			}
			return resourceCategories;
		}

		private int Recursion(Guid cateId, List<dynamic> listCount, List<dynamic> listCate)
		{
            //int c = listCount.Where<object>((object p) => {
            //	CSharpArgumentInfo[] cSharpArgumentInfoArray;

            //             if (ResourceCategoryProvider.<Recursion>o__SiteContainer8.<>p__Site9 == null)
            //	{
            //		ResourceCategoryProvider.<Recursion>o__SiteContainer8.<>p__Site9 = CallSite<Func<CallSite, object, bool>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(bool), typeof(ResourceCategoryProvider)));
            //	}
            //	!0 target = ResourceCategoryProvider.<Recursion>o__SiteContainer8.<>p__Site9.Target;
            //	CallSite<Func<CallSite, object, bool>> u003cu003ep_Site9 = ResourceCategoryProvider.<Recursion>o__SiteContainer8.<>p__Site9;
            //	if (ResourceCategoryProvider.<Recursion>o__SiteContainer8.<>p__Sitea == null)
            //	{
            //		Type type = typeof(ResourceCategoryProvider);
            //		cSharpArgumentInfoArray = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
            //		ResourceCategoryProvider.<Recursion>o__SiteContainer8.<>p__Sitea = CallSite<Func<CallSite, object, Guid, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, type, cSharpArgumentInfoArray));
            //	}
            //	!0 _u00210 = ResourceCategoryProvider.<Recursion>o__SiteContainer8.<>p__Sitea.Target;
            //	CallSite<Func<CallSite, object, Guid, object>> u003cu003ep_Sitea = ResourceCategoryProvider.<Recursion>o__SiteContainer8.<>p__Sitea;
            //	if (ResourceCategoryProvider.<Recursion>o__SiteContainer8.<>p__Siteb == null)
            //	{
            //		Type type1 = typeof(ResourceCategoryProvider);
            //		cSharpArgumentInfoArray = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
            //		ResourceCategoryProvider.<Recursion>o__SiteContainer8.<>p__Siteb = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "CategoryId", type1, cSharpArgumentInfoArray));
            //	}
            //	return target(u003cu003ep_Site9, _u00210(u003cu003ep_Sitea, ResourceCategoryProvider.<Recursion>o__SiteContainer8.<>p__Siteb.Target(ResourceCategoryProvider.<Recursion>o__SiteContainer8.<>p__Siteb, p), cateId));
            //}).Sum<object>((object p) => (int)p.C);
            //IEnumerable<object> list = listCate.Where<object>((object p) => {
            //	CSharpArgumentInfo[] cSharpArgumentInfoArray;
            //	if (ResourceCategoryProvider.<Recursion>o__SiteContainer8.<>p__Sitee == null)
            //	{
            //		ResourceCategoryProvider.<Recursion>o__SiteContainer8.<>p__Sitee = CallSite<Func<CallSite, object, bool>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(bool), typeof(ResourceCategoryProvider)));
            //	}
            //	!0 target = ResourceCategoryProvider.<Recursion>o__SiteContainer8.<>p__Sitee.Target;
            //	CallSite<Func<CallSite, object, bool>> u003cu003ep_Sitee = ResourceCategoryProvider.<Recursion>o__SiteContainer8.<>p__Sitee;
            //	if (ResourceCategoryProvider.<Recursion>o__SiteContainer8.<>p__Sitef == null)
            //	{
            //		Type type = typeof(ResourceCategoryProvider);
            //		cSharpArgumentInfoArray = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
            //		ResourceCategoryProvider.<Recursion>o__SiteContainer8.<>p__Sitef = CallSite<Func<CallSite, object, Guid, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, type, cSharpArgumentInfoArray));
            //	}
            //	!0 _u00210 = ResourceCategoryProvider.<Recursion>o__SiteContainer8.<>p__Sitef.Target;
            //	CallSite<Func<CallSite, object, Guid, object>> u003cu003ep_Sitef = ResourceCategoryProvider.<Recursion>o__SiteContainer8.<>p__Sitef;
            //	if (ResourceCategoryProvider.<Recursion>o__SiteContainer8.<>p__Site10 == null)
            //	{
            //		Type type1 = typeof(ResourceCategoryProvider);
            //		cSharpArgumentInfoArray = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };
            //		ResourceCategoryProvider.<Recursion>o__SiteContainer8.<>p__Site10 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ParentId", type1, cSharpArgumentInfoArray));
            //	}
            //	return target(u003cu003ep_Sitee, _u00210(u003cu003ep_Sitef, ResourceCategoryProvider.<Recursion>o__SiteContainer8.<>p__Site10.Target(ResourceCategoryProvider.<Recursion>o__SiteContainer8.<>p__Site10, p), cateId));
            //});
            //foreach (dynamic item in list)
            //{
            //	c = (int)(c + this.Recursion(item.Id, listCount, listCate));
            //}
            //return c;
            return 0;
		}

		public int SoftDelete(Guid id)
		{
			string sql = "UPDATE ResourceCategory SET IsDeleted = 1 WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}

		public int Update(ResourceCategory resourceCategory)
		{
			string sql = "UPDATE ResourceCategory SET ParentId = @ParentId,Tier = @Tier,Name = @Name,Ico = @Ico,SortId = @SortId,Count = @Count,CreateTime = @CreateTime,IsDeleted = @IsDeleted WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)resourceCategory.Id), new SqlParameter("@ParentId", SqlHelper.ToDBValue(resourceCategory.ParentId)), new SqlParameter("@Tier", SqlHelper.ToDBValue(resourceCategory.Tier)), new SqlParameter("@Name", SqlHelper.ToDBValue(resourceCategory.Name)), new SqlParameter("@Ico", SqlHelper.ToDBValue(resourceCategory.Ico)), new SqlParameter("@SortId", SqlHelper.ToDBValue(resourceCategory.SortId)), new SqlParameter("@Count", SqlHelper.ToDBValue(resourceCategory.Count)), new SqlParameter("@CreateTime", SqlHelper.ToDBValue(resourceCategory.CreateTime)), new SqlParameter("@IsDeleted", SqlHelper.ToDBValue(resourceCategory.IsDeleted)) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}
	}
}