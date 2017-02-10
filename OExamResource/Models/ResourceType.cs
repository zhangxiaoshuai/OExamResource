using System;
using System.Runtime.CompilerServices;

namespace Models
{
	[Serializable]
	public class ResourceType
	{
		public int BaseType
		{
			get;
			set;
		}

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

		public int PackCount
		{
			get;
			set;
		}

		public int? SortId
		{
			get;
			set;
		}

		public int? SourceSortId
		{
			get;
			set;
		}

		public ResourceType()
		{
		}
	}
}