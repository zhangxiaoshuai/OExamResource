using System;
using System.Runtime.CompilerServices;

namespace Models
{
	[Serializable]
	public class QualitySubject1
	{
		public int Count
		{
			get;
			set;
		}

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

		public string Name
		{
			get;
			set;
		}

		public Guid SubjectId
		{
			get;
			set;
		}

		public QualitySubject1()
		{
		}
	}
}