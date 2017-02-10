using System;
using System.Runtime.CompilerServices;

namespace Models
{
	public class ExamImportPaper
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

		public int Month
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}

		public int SortId
		{
			get;
			set;
		}

		public int TimeOut
		{
			get;
			set;
		}

		public string TypeString
		{
			get;
			set;
		}

		public int Year
		{
			get;
			set;
		}

		public ExamImportPaper()
		{
		}
	}
}