using PetaPoco;
using Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;

namespace Providers
{
	public class ResourceTypeProvider
	{
		private readonly static ResourceTypeProvider InstanceObj;

		public static ResourceTypeProvider Instance
		{
			get
			{
				return ResourceTypeProvider.InstanceObj;
			}
		}

		static ResourceTypeProvider()
		{
			ResourceTypeProvider.InstanceObj = new ResourceTypeProvider();
		}

		private ResourceTypeProvider()
		{
		}

		public ResourceType Create(ResourceType resourceType)
		{
			ResourceType resourceType1;
			if (resourceType.Id == Guid.Empty)
			{
				resourceType.Id = Guid.NewGuid();
			}
			string sql = "INSERT INTO ResourceType (Id, Name, SortId, SourceSortId, Count, CreateTime, IsDeleted, BaseType)  VALUES (@Id, @Name, @SortId, @SourceSortId, @Count, @CreateTime, @IsDeleted, @BaseType)";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", SqlHelper.ToDBValue(resourceType.Id)), new SqlParameter("@Name", SqlHelper.ToDBValue(resourceType.Name)), new SqlParameter("@SortId", SqlHelper.ToDBValue(resourceType.SortId)), new SqlParameter("@SourceSortId", SqlHelper.ToDBValue(resourceType.SourceSortId)), new SqlParameter("@Count", SqlHelper.ToDBValue(resourceType.Count)), new SqlParameter("@CreateTime", SqlHelper.ToDBValue(resourceType.CreateTime)), new SqlParameter("@IsDeleted", SqlHelper.ToDBValue(resourceType.IsDeleted)), new SqlParameter("@BaseType", SqlHelper.ToDBValue(resourceType.BaseType)) };
			if (SqlHelper.ExecuteNonQuery(sql, sqlParameter) > 0)
			{
				resourceType1 = resourceType;
			}
			else
			{
				resourceType1 = null;
			}
			return resourceType1;
		}

		public int Delete(Guid id)
		{
			string sql = "DELETE ResourceType WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}

		public int GetCount()
		{
			return (int)SqlHelper.ExecuteScalar("SELECT COUNT(Id) FROM ResourceType WHERE IsDeleted = 0", new SqlParameter[0]);
		}

		public ResourceType GetEntity(Guid id)
		{
			string sql = "SELECT * FROM ResourceType WHERE Id = @Id AND IsDeleted = 0";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteEntity<ResourceType>(sql, sqlParameter);
		}

		public List<ResourceType> GetList()
		{
			return SqlHelper.ExecuteList<ResourceType>("SELECT * FROM ResourceType WHERE IsDeleted = 0", new SqlParameter[0]);
		}

		public List<ResourceType> GetList(Guid? categoryId, string key, bool isJX, bool IsResource)
		{
			string sqlc;
			List<ResourceType> resourceTypes;
			object[] variable;
			string isResourceSql = "";
			string isPackSql = "";
			string isMaterialSql = "";
			if (IsResource)
			{
				isResourceSql = " and ResourceCategory.Medical_Tag =0 ";
				isPackSql = " AND ResourcePack.Medical_Tag=0";
				isMaterialSql = " AND ResourceMaterial.Medical_Tag=0";
			}
			string sql = ";WITH CTE AS\r\n    (\r\n    SELECT Id FROM ResourceCategory WHERE {0} AND IsDeleted = 0\r\n    UNION ALL\r\n    SELECT ResourceCategory.Id FROM ResourceCategory join CTE ON ResourceCategory.ParentId = CTE.Id WHERE ResourceCategory.IsDeleted = 0 {2}\r\n    )\r\n    SELECT [Id],[Name],[SortId],[SourceSortId],[CreateTime],[IsDeleted],[BaseType],{1}\r\n    FROM ResourceType WHERE ResourceType.IsDeleted = 0 ORDER BY SortId";
			if ((categoryId.HasValue ? true : !string.IsNullOrWhiteSpace(key)))
			{
				string sqlCount = "[Count]=\r\n                (\r\n                CASE BaseType\r\n                WHEN 0\r\n                THEN\r\n                (SELECT ISNULL( SUM([Count]),0) FROM ResourcePack WHERE CategoryId IN (SELECT Id FROM CTE) AND TypeId=ResourceType.Id AND ResourcePack.IsDeleted=0 {2}  {0})\r\n                ELSE\r\n                (SELECT COUNT(Id) FROM ResourceMaterial WHERE CategoryId IN (SELECT Id FROM CTE) AND TypeId=ResourceType.Id AND ResourceMaterial.IsDeleted=0 {3} {1})\r\n                END\r\n                ),PackCount=\r\n                (\r\n                CASE BaseType\r\n                WHEN 0\r\n                THEN\r\n                (SELECT COUNT(Id) FROM ResourcePack WHERE CategoryId IN (SELECT Id FROM CTE) AND TypeId=ResourceType.Id AND ResourcePack.IsDeleted=0 {2} {0})\r\n                ELSE\r\n                0\r\n                END\r\n                )";
				string sqlpack = "";
				string sqlmat = "";
				if (isJX)
				{
				}
				if (string.IsNullOrWhiteSpace(key))
				{
					variable = new object[] { sqlpack, sqlmat, isPackSql, isMaterialSql };
					sqlCount = string.Format(sqlCount, variable);
				}
				else
				{
					sqlpack = string.Concat(sqlpack, "AND contains((Name,Keyword),@Key)");
					sqlmat = string.Concat(sqlmat, "AND contains((Title,Keywords),@Key)");
					variable = new object[] { sqlpack, sqlmat, isPackSql, isMaterialSql };
					sqlCount = string.Format(sqlCount, variable);
				}
				sql = string.Format(sql, (!categoryId.HasValue ? "Tier = 0 AND ParentId is null" : "Id = @Id"), sqlCount, isResourceSql);
				DB instance = DB.GetInstance();
				variable = new object[] { new { Id = categoryId, Key = string.Concat("\"", key, "\"") } };
				List<ResourceType> list = instance.Fetch<ResourceType>(sql, variable);
				list.RemoveAll((ResourceType p) => p.Count <= 0);
				resourceTypes = list;
			}
			else
			{
				sqlc = (!isJX ? "[Count],PackCount" : "[Count]=Count_JX,PackCount=PackCount_JX");
				sql = string.Format(sql, "Tier = 0 AND ParentId is null", sqlc, isResourceSql);
				resourceTypes = SqlHelper.ExecuteList<ResourceType>(sql, new SqlParameter[0]);
			}
			return resourceTypes;
		}

		public List<ResourceType> GetPage(int pageIndex, int pageCount)
		{
			string sql = "SELECT * FROM(SELECT *,(ROW_NUMBER() OVER(ORDER BY Id)) rownum FROM ResourceType WHERE IsDeleted = 0) t WHERE rownum>=(@pageCount*(@pageIndex-1)+1) AND rownum<=@pageIndex*@pageCount";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@pageIndex", (object)pageIndex), new SqlParameter("@pageCount", (object)pageCount) };
			return SqlHelper.ExecuteList<ResourceType>(sql, sqlParameter);
		}

		public int SoftDelete(Guid id)
		{
			string sql = "UPDATE ResourceType SET IsDeleted = 1 WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}

		public int Update(ResourceType resourceType)
		{
			string sql = "UPDATE ResourceType SET Name = @Name,SortId = @SortId,SourceSortId = @SourceSortId,Count = @Count,CreateTime = @CreateTime,IsDeleted = @IsDeleted,BaseType = @BaseType WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)resourceType.Id), new SqlParameter("@Name", SqlHelper.ToDBValue(resourceType.Name)), new SqlParameter("@SortId", SqlHelper.ToDBValue(resourceType.SortId)), new SqlParameter("@SourceSortId", SqlHelper.ToDBValue(resourceType.SourceSortId)), new SqlParameter("@Count", SqlHelper.ToDBValue(resourceType.Count)), new SqlParameter("@CreateTime", SqlHelper.ToDBValue(resourceType.CreateTime)), new SqlParameter("@IsDeleted", SqlHelper.ToDBValue(resourceType.IsDeleted)), new SqlParameter("@BaseType", SqlHelper.ToDBValue(resourceType.BaseType)) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}
	}
}