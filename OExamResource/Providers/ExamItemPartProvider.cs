using Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Providers
{
	public class ExamItemPartProvider
	{
		private readonly static ExamItemPartProvider InstanceObj;

		public static ExamItemPartProvider Instance
		{
			get
			{
				return ExamItemPartProvider.InstanceObj;
			}
		}

		static ExamItemPartProvider()
		{
			ExamItemPartProvider.InstanceObj = new ExamItemPartProvider();
		}

		private ExamItemPartProvider()
		{
		}

		public ExamItemPart Create(ExamItemPart examItemPart)
		{
			ExamItemPart examItemPart1;
			if (examItemPart.Id == Guid.Empty)
			{
				examItemPart.Id = Guid.NewGuid();
			}
			string sql = "INSERT INTO ExamItemPart (Id, Name, PaperId, ItemScore, ItemCount, SortId, CreateTime, IsDeleted)  VALUES (@Id, @Name, @PaperId, @ItemScore, @ItemCount, @SortId, @CreateTime, @IsDeleted)";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", SqlHelper.ToDBValue(examItemPart.Id)), new SqlParameter("@Name", SqlHelper.ToDBValue(examItemPart.Name)), new SqlParameter("@PaperId", SqlHelper.ToDBValue(examItemPart.PaperId)), new SqlParameter("@ItemScore", SqlHelper.ToDBValue(examItemPart.ItemScore)), new SqlParameter("@ItemCount", SqlHelper.ToDBValue(examItemPart.ItemCount)), new SqlParameter("@SortId", SqlHelper.ToDBValue(examItemPart.SortId)), new SqlParameter("@CreateTime", SqlHelper.ToDBValue(examItemPart.CreateTime)), new SqlParameter("@IsDeleted", SqlHelper.ToDBValue(examItemPart.IsDeleted)) };
			if (SqlHelper.ExecuteNonQuery(sql, sqlParameter) > 0)
			{
				examItemPart1 = examItemPart;
			}
			else
			{
				examItemPart1 = null;
			}
			return examItemPart1;
		}

		public int Delete(Guid id)
		{
			string sql = "DELETE ExamItemPart WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}

		public int GetCount()
		{
			return (int)SqlHelper.ExecuteScalar("SELECT COUNT(Id) FROM ExamItemPart WHERE IsDeleted = 0", new SqlParameter[0]);
		}

		public ExamItemPart GetEntity(Guid id)
		{
			string sql = "SELECT * FROM ExamItemPart WHERE Id = @Id AND IsDeleted = 0";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteEntity<ExamItemPart>(sql, sqlParameter);
		}

		public List<ExamItemPart> GetList()
		{
			return SqlHelper.ExecuteList<ExamItemPart>("SELECT * FROM ExamItemPart WHERE IsDeleted = 0", new SqlParameter[0]);
		}

		public List<ExamItemPart> GetListByPaper(Guid paperId)
		{
			string sql = "SELECT * FROM ExamItemPart WHERE PaperId = @PaperId AND IsDeleted = 0 ORDER BY SortId";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@PaperId", (object)paperId) };
			return SqlHelper.ExecuteList<ExamItemPart>(sql, sqlParameter);
		}

		public List<ExamItemPart> GetPage(int pageIndex, int pageCount)
		{
			string sql = "SELECT * FROM(SELECT *,(ROW_NUMBER() OVER(ORDER BY Id)) rownum FROM ExamItemPart WHERE IsDeleted = 0) t WHERE rownum>=(@pageCount*(@pageIndex-1)+1) AND rownum<=@pageIndex*@pageCount";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@pageIndex", (object)pageIndex), new SqlParameter("@pageCount", (object)pageCount) };
			return SqlHelper.ExecuteList<ExamItemPart>(sql, sqlParameter);
		}

		public int SoftDelete(Guid id)
		{
			string sql = "UPDATE ExamItemPart SET IsDeleted = 1 WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}

		public int Update(ExamItemPart examItemPart)
		{
			string sql = "UPDATE ExamItemPart SET Name = @Name,PaperId = @PaperId,ItemScore = @ItemScore,ItemCount = @ItemCount,SortId = @SortId,CreateTime = @CreateTime,IsDeleted = @IsDeleted WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)examItemPart.Id), new SqlParameter("@Name", SqlHelper.ToDBValue(examItemPart.Name)), new SqlParameter("@PaperId", SqlHelper.ToDBValue(examItemPart.PaperId)), new SqlParameter("@ItemScore", SqlHelper.ToDBValue(examItemPart.ItemScore)), new SqlParameter("@ItemCount", SqlHelper.ToDBValue(examItemPart.ItemCount)), new SqlParameter("@SortId", SqlHelper.ToDBValue(examItemPart.SortId)), new SqlParameter("@CreateTime", SqlHelper.ToDBValue(examItemPart.CreateTime)), new SqlParameter("@IsDeleted", SqlHelper.ToDBValue(examItemPart.IsDeleted)) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}
	}
}