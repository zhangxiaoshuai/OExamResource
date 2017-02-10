using Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Providers
{
	public class ExamItemProvider
	{
		private readonly static ExamItemProvider InstanceObj;

		public static ExamItemProvider Instance
		{
			get
			{
				return ExamItemProvider.InstanceObj;
			}
		}

		static ExamItemProvider()
		{
			ExamItemProvider.InstanceObj = new ExamItemProvider();
		}

		private ExamItemProvider()
		{
		}

		public ExamItem Create(ExamItem examItem)
		{
			ExamItem examItem1;
			if (examItem.Id == Guid.Empty)
			{
				examItem.Id = Guid.NewGuid();
			}
			string sql = "INSERT INTO ExamItem (Id, Question, Answer, Explain, OptionCount, PaperId, PartId, TypeId, SortId, CreateTime, IsDeleted)  VALUES (@Id, @Question, @Answer, @Explain, @OptionCount, @PaperId, @PartId, @TypeId, @SortId, @CreateTime, @IsDeleted)";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", SqlHelper.ToDBValue(examItem.Id)), new SqlParameter("@Question", SqlHelper.ToDBValue(examItem.Question)), new SqlParameter("@Answer", SqlHelper.ToDBValue(examItem.Answer)), new SqlParameter("@Explain", SqlHelper.ToDBValue(examItem.Explain)), new SqlParameter("@OptionCount", SqlHelper.ToDBValue(examItem.OptionCount)), new SqlParameter("@PaperId", SqlHelper.ToDBValue(examItem.PaperId)), new SqlParameter("@PartId", SqlHelper.ToDBValue(examItem.PartId)), new SqlParameter("@TypeId", SqlHelper.ToDBValue(examItem.TypeId)), new SqlParameter("@SortId", SqlHelper.ToDBValue(examItem.SortId)), new SqlParameter("@CreateTime", SqlHelper.ToDBValue(examItem.CreateTime)), new SqlParameter("@IsDeleted", SqlHelper.ToDBValue(examItem.IsDeleted)) };
			if (SqlHelper.ExecuteNonQuery(sql, sqlParameter) > 0)
			{
				examItem1 = examItem;
			}
			else
			{
				examItem1 = null;
			}
			return examItem1;
		}

		public int Delete(Guid id)
		{
			string sql = "DELETE ExamItem WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}

		public int GetCount()
		{
			return (int)SqlHelper.ExecuteScalar("SELECT COUNT(Id) FROM ExamItem WHERE IsDeleted = 0", new SqlParameter[0]);
		}

		public ExamItem GetEntity(Guid id)
		{
			string sql = "SELECT * FROM ExamItem WHERE Id = @Id AND IsDeleted = 0";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteEntity<ExamItem>(sql, sqlParameter);
		}

		public List<ExamItem> GetList()
		{
			return SqlHelper.ExecuteList<ExamItem>("SELECT * FROM ExamItem WHERE IsDeleted = 0", new SqlParameter[0]);
		}

		public List<ExamItem> GetListByPaper(Guid paperId)
		{
			string sql = "SELECT * FROM ExamItem WHERE PaperId = @PaperId AND IsDeleted = 0 ORDER BY SortId";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@PaperId", (object)paperId) };
			return SqlHelper.ExecuteList<ExamItem>(sql, sqlParameter);
		}

		public List<ExamItem> GetPage(int pageIndex, int pageCount)
		{
			string sql = "SELECT * FROM(SELECT *,(ROW_NUMBER() OVER(ORDER BY Id)) rownum FROM ExamItem WHERE IsDeleted = 0) t WHERE rownum>=(@pageCount*(@pageIndex-1)+1) AND rownum<=@pageIndex*@pageCount";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@pageIndex", (object)pageIndex), new SqlParameter("@pageCount", (object)pageCount) };
			return SqlHelper.ExecuteList<ExamItem>(sql, sqlParameter);
		}

		public int SoftDelete(Guid id)
		{
			string sql = "UPDATE ExamItem SET IsDeleted = 1 WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)id) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}

		public int Update(ExamItem examItem)
		{
			string sql = "UPDATE ExamItem SET Question = @Question,Answer = @Answer,Explain = @Explain,OptionCount = @OptionCount,PaperId = @PaperId,PartId = @PartId,TypeId = @TypeId,SortId = @SortId,CreateTime = @CreateTime,IsDeleted = @IsDeleted WHERE Id = @Id";
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@Id", (object)examItem.Id), new SqlParameter("@Question", SqlHelper.ToDBValue(examItem.Question)), new SqlParameter("@Answer", SqlHelper.ToDBValue(examItem.Answer)), new SqlParameter("@Explain", SqlHelper.ToDBValue(examItem.Explain)), new SqlParameter("@OptionCount", SqlHelper.ToDBValue(examItem.OptionCount)), new SqlParameter("@PaperId", SqlHelper.ToDBValue(examItem.PaperId)), new SqlParameter("@PartId", SqlHelper.ToDBValue(examItem.PartId)), new SqlParameter("@TypeId", SqlHelper.ToDBValue(examItem.TypeId)), new SqlParameter("@SortId", SqlHelper.ToDBValue(examItem.SortId)), new SqlParameter("@CreateTime", SqlHelper.ToDBValue(examItem.CreateTime)), new SqlParameter("@IsDeleted", SqlHelper.ToDBValue(examItem.IsDeleted)) };
			return SqlHelper.ExecuteNonQuery(sql, sqlParameter);
		}
	}
}