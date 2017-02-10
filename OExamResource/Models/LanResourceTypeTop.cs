using System;
using System.Runtime.CompilerServices;

namespace Models
{
	[Serializable]
	public class LanResourceTypeTop
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

		public int? SortId
		{
			get;
			set;
		}

		public Guid TypeId
		{
			get;
			set;
		}

		public Guid UserId
		{
			get;
			set;
		}

		public LanResourceTypeTop()
		{
		}
	}
}