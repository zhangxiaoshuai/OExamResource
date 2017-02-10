using System;
using System.Runtime.CompilerServices;

namespace Models
{
	[Serializable]
	public class ResourceError
	{
		public int Code
		{
			get;
			set;
		}

		public DateTime CreatedTime
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

		public string Remark
		{
			get;
			set;
		}

		public Guid ResourceId
		{
			get;
			set;
		}

		public ResourceError()
		{
		}
	}
}