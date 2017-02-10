using System;
using System.Runtime.CompilerServices;

namespace Models
{
	[Serializable]
	public class Resource_Log
	{
		public int? Count
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

		public Guid ResourceId
		{
			get;
			set;
		}

		public int Status
		{
			get;
			set;
		}

		public Resource_Log()
		{
		}
	}
}