using PetaPoco;
using Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Providers
{
	public class ResourcePackProvider
	{
		private readonly static ResourcePackProvider InstanceObj;

		public static ResourcePackProvider Instance
		{
			get
			{
				return ResourcePackProvider.InstanceObj;
			}
		}

		static ResourcePackProvider()
		{
			ResourcePackProvider.InstanceObj = new ResourcePackProvider();
		}

		private ResourcePackProvider()
		{
		}

		public ResourcePack Create(ResourcePack resourcePack)
		{
			ResourcePack resourcePack1;
			if (resourcePack.Id == Guid.Empty)
			{
				resourcePack.Id = Guid.NewGuid();
			}
			string sql = "INSERT INTO ResourcePack (Id, Name, CategoryId, TypeId, Count, SortId, Remark, Ico, FilePath, KeyWord, ClickCount, Size, CreateTime, IsDeleted)  VALUES (@Id, @Name, @CategoryId, @TypeId, @Count, @SortId, @Remark, @Ico, @FilePath, @KeyWord, @ClickCount, @Size, @CreateTime, @IsDeleted)";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", SqlHelper.ToDBValue(resourcePack.Id)), new SqlParameter("@Name", SqlHelper.ToDBValue(resourcePack.Name)), new SqlParameter("@CategoryId", SqlHelper.ToDBValue(resourcePack.CategoryId)), new SqlParameter("@TypeId", SqlHelper.ToDBValue(resourcePack.TypeId)), new SqlParameter("@Count", SqlHelper.ToDBValue(resourcePack.Count)), new SqlParameter("@SortId", SqlHelper.ToDBValue(resourcePack.SortId)), new SqlParameter("@Remark", SqlHelper.ToDBValue(resourcePack.Remark)), new SqlParameter("@Ico", SqlHelper.ToDBValue(resourcePack.Ico)), new SqlParameter("@FilePath", SqlHelper.ToDBValue(resourcePack.FilePath)), new SqlParameter("@KeyWord", SqlHelper.ToDBValue(resourcePack.KeyWord)), new SqlParameter("@ClickCount", SqlHelper.ToDBValue(resourcePack.ClickCount)), new SqlParameter("@Size", SqlHelper.ToDBValue(resourcePack.Size)), new SqlParameter("@CreateTime", SqlHelper.ToDBValue(resourcePack.CreateTime)), new SqlParameter("@IsDeleted", SqlHelper.ToDBValue(resourcePack.IsDeleted)) };
			if (SqlHelper.ExecuteNonQuery(sql, sqlParameter) > 0)
			{
				resourcePack1 = resourcePack;
			}
			else
			{
				resourcePack1 = null;
			}
			return resourcePack1;
		}

		public int Delete(Guid id)
		{
			string sql = "DELETE ResourcePack WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}

		public List<dynamic> GetAllPage(bool isJX, string key, Guid? id, bool IsResource, int pageIndex, int pageCount)
		{
			object[] objArray;
			string isResourceSql = "";
			string isPackSql = "";
			string isMaterialSql = "";
			if (IsResource)
			{
				isResourceSql = " and ResourceCategory.Medical_Tag =0 ";
				isPackSql = " AND ResourcePack.Medical_Tag=0";
				isMaterialSql = " AND ResourceMaterial.Medical_Tag=0";
			}
			bool haskey = !string.IsNullOrWhiteSpace(key);
			Sql sql = new Sql();
			sql.Append(";with CTE AS (SELECT Id FROM ResourceCategory WHERE", new object[0]);
			if (id.HasValue)
			{
				objArray = new object[] { id };
				sql.Append("Id = @0", objArray);
			}
			else
			{
				sql.Append("Tier=0 AND ParentId is null", new object[0]);
			}
			string[] strArrays = new string[] { "AND ResourceCategory.IsDeleted=0", isResourceSql, " UNION ALL\r\n                        SELECT ResourceCategory.Id FROM ResourceCategory join CTE ON ResourceCategory.ParentId = CTE.Id AND ResourceCategory.IsDeleted=0 ", isResourceSql, ")\r\n                        SELECT Id,Name,Ico,BaseType,FileExt,[Count] ,Medical_Tag FROM (\r\n                        SELECT Id,Name,Ico,BaseType,FileExt,Medical_Tag,[Count],(ROW_NUMBER() OVER(ORDER BY" };
			sql.Append(string.Concat(strArrays), new object[0]);
			if (haskey)
			{
				sql.Append("RANK DESC,", new object[0]);
			}
			sql.Append("case when  SortId <0 and BaseType=1 then SortId-10 else SortId end,\r\n                     Name)) rownum FROM \r\n                        (\r\n                        SELECT ResourcePack.Id,ResourcePack.Medical_Tag,ResourcePack.Name,ResourcePack.Ico,BaseType=0,FileExt='pack',ResourcePack.Count,ResourcePack.SortId", new object[0]);
			if (haskey)
			{
				sql.Append(",RANK", new object[0]);
			}
			sql.Append("FROM ResourcePack JOIN CTE ON ResourcePack.CategoryId=CTE.Id", new object[0]);
			if (haskey)
			{
				objArray = new object[] { string.Concat("\"", key, "\"") };
				sql.Append("inner join containstable(ResourcePack,(Name,KeyWord), @0) k on ResourcePack.Id=k.[KEY]", objArray);
			}
			sql.Append(string.Concat("WHERE ResourcePack.IsDeleted=0 ", isPackSql), new object[0]);
			sql.Append("UNION ALL\r\n                        SELECT ResourceMaterial.Id,ResourceMaterial.Medical_Tag,ResourceMaterial.Title,ResourceMaterial.IcoFilepath AS Ico,BaseType=1,\r\n                        ResourceMaterial.FileExt,[Count]=1,ResourceMaterial.SortId", new object[0]);
			if (haskey)
			{
				sql.Append(",RANK", new object[0]);
			}
			sql.Append(" FROM ResourceMaterial JOIN CTE ON ResourceMaterial.CategoryId = CTE.Id", new object[0]);
			if (haskey)
			{
				objArray = new object[] { string.Concat("\"", key, "\"") };
				sql.Append("inner join containstable(ResourceMaterial,(Title,KeyWords),@0) k on ResourceMaterial.Id=k.[KEY]", objArray);
			}
			sql.Append(string.Concat("WHERE ResourceMaterial.IsDeleted = 0 ", isMaterialSql), new object[0]);
			sql.Append(") temp", new object[0]);
			objArray = new object[] { pageCount, pageIndex };
			sql.Append(") t  WHERE rownum>=(@0*(@1-1)+1) AND rownum<=@1*@0", objArray);
			return DB.GetInstance().Fetch<object>(sql);
		}

		public int GetCount()
		{
			return (int)SqlHelper.ExecuteScalar("SELECT COUNT(Id) FROM ResourcePack WHERE IsDeleted = 0", new SqlParameter[0]);
		}

		public List<ResourceCourse> GetCourse(Guid packId)
		{
			string sql = "SELECT * FROM ResourceCourse WHERE PackId = @0 AND IsDeleted=0 ORDER BY SORTID,Title,Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@0", (object)packId) };
			return SqlHelper.ExecuteList<ResourceCourse>(sql, sqlParameter);
		}

		public ResourcePack GetEntity(Guid id)
		{
			string sql = "SELECT * FROM ResourcePack WHERE Id = @Id AND IsDeleted = 0";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteEntity<ResourcePack>(sql, sqlParameter);
		}

		public List<ResourcePack> GetList()
		{
			return SqlHelper.ExecuteList<ResourcePack>("SELECT * FROM ResourcePack WHERE IsDeleted = 0", new SqlParameter[0]);
		}

		public List<ResourceMaterial> GetMaterialPage(bool isJX, string key, Guid? id, Guid? typeId, bool IsResource, int pageIndex, int pageCount)
		{
			string isResourceSql = "";
			string isMaterialSql = "";
			if (IsResource)
			{
				isResourceSql = " and ResourceCategory.Medical_Tag =0 ";
				isMaterialSql = " AND ResourceMaterial.Medical_Tag=0";
			}
			string sql = "WITH CTE AS\r\n                        (\r\n                        SELECT Id FROM ResourceCategory WHERE {0} AND ResourceCategory.IsDeleted=0  {1}\r\n                        UNION ALL\r\n                        SELECT ResourceCategory.Id FROM ResourceCategory join CTE ON ResourceCategory.ParentId = CTE.Id AND ResourceCategory.IsDeleted=0 {1}\r\n                        )\r\n                        SELECT Id,Title,IcoFilepath,FileExt,Tag2,Medical_Tag FROM (SELECT ResourceMaterial.Id,ResourceMaterial.Medical_Tag,ResourceMaterial.Title,ResourceMaterial.IcoFilepath,ResourceMaterial.FileExt,ResourceMaterial.Tag2,(ROW_NUMBER() OVER(ORDER BY ";
			if (!string.IsNullOrWhiteSpace(key))
			{
				sql = string.Concat(sql, " case\r\nwhen contains(ResourceMaterial.Title,@key)\r\nthen\r\n0\r\nelse\r\n1\r\nend,");
			}
			sql = string.Concat(sql, "ResourceMaterial.SortId,ResourceMaterial.Title)) rownum FROM ResourceMaterial JOIN CTE \r\n                        ON ResourceMaterial.CategoryId = CTE.Id\r\n                        WHERE ResourceMaterial.IsDeleted = 0  ", isMaterialSql);
			List<SqlParameter> par = new List<SqlParameter>()
			{
				new SqlParameter("@pageCount", (object)pageCount),
				new SqlParameter("@pageIndex", (object)pageIndex)
			};
			if (id.HasValue)
			{
				sql = string.Format(sql, "Id = @Id", isResourceSql);
				par.Add(new SqlParameter("@Id", (object)id));
			}
			else
			{
				sql = string.Format(sql, "Tier=0 AND ParentId is null", isResourceSql);
			}
			if (typeId.HasValue)
			{
				sql = string.Concat(sql, "AND ResourceMaterial.TypeId=@TypeId ");
				par.Add(new SqlParameter("@TypeId", (object)typeId));
			}
			if (!string.IsNullOrWhiteSpace(key))
			{
				sql = string.Concat(sql, " AND contains((ResourceMaterial.Title,ResourceMaterial.KeyWords),@key)");
				par.Add(new SqlParameter("@key", string.Concat("\"", key, "\"")));
			}
			sql = string.Concat(sql, ") t  WHERE rownum>=(@pageCount*(@pageIndex-1)+1) AND rownum<=@pageIndex*@pageCount");
			return SqlHelper.ExecuteList<ResourceMaterial>(sql, par.ToArray());
		}

		public List<ResourcePack> GetPackPage(bool isJX, string key, Guid? id, Guid? typeId, bool IsResource, int pageIndex, int pageCount)
		{
			string isResourceSql = "";
			string isPackSql = "";
			if (IsResource)
			{
				isResourceSql = " and ResourceCategory.Medical_Tag =0 ";
				isPackSql = " AND ResourcePack.Medical_Tag=0";
			}
			string sql = "with CTE AS\r\n                        (\r\n                        SELECT Id FROM ResourceCategory WHERE IsDeleted=0 {0} {1}\r\n                        UNION ALL\r\n                        SELECT ResourceCategory.Id FROM ResourceCategory join CTE ON ResourceCategory.ParentId = CTE.Id AND ResourceCategory.IsDeleted=0 {1}\r\n                        )\r\n                        SELECT Id,Name,Ico,[Count],Medical_Tag FROM (SELECT ResourcePack.Id,ResourcePack.Name,ResourcePack.Ico,ResourcePack.Medical_Tag ,";
			sql = string.Concat(sql, " ResourcePack.Count,", "(ROW_NUMBER() OVER(ORDER BY ");
			if (!string.IsNullOrWhiteSpace(key))
			{
				sql = string.Concat(sql, " case\r\nwhen contains(ResourcePack.Name,@key)\r\nthen\r\n0\r\nelse\r\n1\r\nend,");
			}
			sql = string.Concat(sql, "ResourcePack.SortId)) rownum FROM ResourcePack JOIN CTE ON ResourcePack.CategoryId = CTE.Id\r\n                        WHERE ResourcePack.IsDeleted = 0 ", isPackSql);
			List<SqlParameter> par = new List<SqlParameter>()
			{
				new SqlParameter("@pageCount", (object)pageCount),
				new SqlParameter("@pageIndex", (object)pageIndex)
			};
			if (id.HasValue)
			{
				sql = string.Format(sql, "AND Id = @Id", isResourceSql);
				par.Add(new SqlParameter("@Id", (object)id));
			}
			else
			{
				sql = string.Format(sql, "AND Tier=0 AND ParentId is null", isResourceSql);
			}
			if (typeId.HasValue)
			{
				sql = string.Concat(sql, "AND ResourcePack.TypeId=@TypeId");
				par.Add(new SqlParameter("@TypeId", (object)typeId));
			}
			if (!string.IsNullOrWhiteSpace(key))
			{
				sql = string.Concat(sql, " AND contains((ResourcePack.Name,ResourcePack.Keyword),@key)");
				par.Add(new SqlParameter("@key", string.Concat("\"", key, "\"")));
			}
			sql = string.Concat(sql, ") t  WHERE rownum>=(@pageCount*(@pageIndex-1)+1) AND rownum<=@pageIndex*@pageCount");
			return SqlHelper.ExecuteList<ResourcePack>(sql, par.ToArray());
		}

		public List<ResourcePack> GetPage(int pageIndex, int pageCount)
		{
			string sql = "SELECT * FROM(SELECT *,(ROW_NUMBER() OVER(ORDER BY Id)) rownum FROM ResourcePack WHERE IsDeleted = 0) t WHERE rownum>=(@pageCount*(@pageIndex-1)+1) AND rownum<=@pageIndex*@pageCount";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@pageIndex", (object)pageIndex), new SqlParameter("@pageCount", (object)pageCount) };
			return SqlHelper.ExecuteList<ResourcePack>(sql, sqlParameter);
		}

		public int SoftDelete(Guid id)
		{
			string sql = "UPDATE ResourcePack SET IsDeleted = 1 WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}

		public int Update(ResourcePack resourcePack)
		{
			string sql = "UPDATE ResourcePack SET Name = @Name,CategoryId = @CategoryId,TypeId = @TypeId,Count = @Count,SortId = @SortId,Remark = @Remark,Ico = @Ico,FilePath = @FilePath,KeyWord = @KeyWord,ClickCount = @ClickCount,Size = @Size,CreateTime = @CreateTime,IsDeleted = @IsDeleted WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)resourcePack.Id), new SqlParameter("@Name", SqlHelper.ToDBValue(resourcePack.Name)), new SqlParameter("@CategoryId", SqlHelper.ToDBValue(resourcePack.CategoryId)), new SqlParameter("@TypeId", SqlHelper.ToDBValue(resourcePack.TypeId)), new SqlParameter("@Count", SqlHelper.ToDBValue(resourcePack.Count)), new SqlParameter("@SortId", SqlHelper.ToDBValue(resourcePack.SortId)), new SqlParameter("@Remark", SqlHelper.ToDBValue(resourcePack.Remark)), new SqlParameter("@Ico", SqlHelper.ToDBValue(resourcePack.Ico)), new SqlParameter("@FilePath", SqlHelper.ToDBValue(resourcePack.FilePath)), new SqlParameter("@KeyWord", SqlHelper.ToDBValue(resourcePack.KeyWord)), new SqlParameter("@ClickCount", SqlHelper.ToDBValue(resourcePack.ClickCount)), new SqlParameter("@Size", SqlHelper.ToDBValue(resourcePack.Size)), new SqlParameter("@CreateTime", SqlHelper.ToDBValue(resourcePack.CreateTime)), new SqlParameter("@IsDeleted", SqlHelper.ToDBValue(resourcePack.IsDeleted)) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}
	}
}