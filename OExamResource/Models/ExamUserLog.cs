using System;
using System.Runtime.CompilerServices;

namespace Models
{
	[Serializable]
	public class ExamUserLog
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

		public Guid PaperId
		{
			get;
			set;
		}

		public string Score
		{
			get;
			set;
		}

		public Guid UserId
		{
			get;
			set;
		}

		public ExamUserLog()
		{
		}
	}
}