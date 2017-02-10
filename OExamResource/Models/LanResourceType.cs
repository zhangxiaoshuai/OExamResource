using System;
using System.Runtime.CompilerServices;

namespace Models
{
	[Serializable]
	public class LanResourceType
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

		public string Name
		{
			get;
			set;
		}

		public Guid? SchoolId
		{
			get;
			set;
		}

		public int? SortId
		{
			get;
			set;
		}

		public int? TypeEnum
		{
			get;
			set;
		}

		public Guid? UserId
		{
			get;
			set;
		}

		public LanResourceType()
		{
		}
	}
}