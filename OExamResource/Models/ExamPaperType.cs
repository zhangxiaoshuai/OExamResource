using System;
using System.Runtime.CompilerServices;

namespace Models
{
	[Serializable]
	public class ExamPaperType
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

		public string Logo
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}

		public int? PaperCount
		{
			get;
			set;
		}

		public Guid ParentId
		{
			get;
			set;
		}

		public int? SortId
		{
			get;
			set;
		}

		public int Tier
		{
			get;
			set;
		}

		public ExamPaperType()
		{
		}
	}
}