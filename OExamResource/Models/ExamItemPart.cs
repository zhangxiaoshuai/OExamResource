using System;
using System.Runtime.CompilerServices;

namespace Models
{
	[Serializable]
	public class ExamItemPart
	{
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

		public int? ItemCount
		{
			get;
			set;
		}

		public double? ItemScore
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}

		public Guid? PaperId
		{
			get;
			set;
		}

		public int? SortId
		{
			get;
			set;
		}

		public ExamItemPart()
		{
		}
	}
}