using System;
using System.Runtime.CompilerServices;

namespace Models
{
	public class ExamImportItem
	{
		public string Answer
		{
			get;
			set;
		}

		public DateTime CreateTime
		{
			get;
			set;
		}

		public string Explain
		{
			get;
			set;
		}

		public Guid Id
		{
			get;
			set;
		}

		public bool IsDeleted
		{
			get;
			set;
		}

		public int OptionCount
		{
			get;
			set;
		}

		public string Question
		{
			get;
			set;
		}

		public float Score
		{
			get;
			set;
		}

		public int? SortId
		{
			get;
			set;
		}

		public int TypeId
		{
			get;
			set;
		}

		public ExamImportItem()
		{
		}
	}
}