using System;
using System.Runtime.CompilerServices;

namespace Models
{
	[Serializable]
	public class ExamPaper
	{
		public DateTime CreateTime
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

		public int? Means
		{
			get;
			set;
		}

		public int? Month
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}

		public int? SortId
		{
			get;
			set;
		}

		public int? TimeOut
		{
			get;
			set;
		}

		public Guid TypeId
		{
			get;
			set;
		}

		public int? Year
		{
			get;
			set;
		}

		public ExamPaper()
		{
		}
	}
}