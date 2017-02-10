using System;
using System.Runtime.CompilerServices;

namespace Models
{
	[Serializable]
	public class LanCollect
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

		public Guid ResourceId
		{
			get;
			set;
		}

		public Guid UserId
		{
			get;
			set;
		}

		public LanCollect()
		{
		}
	}
}