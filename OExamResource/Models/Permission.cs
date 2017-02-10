using System;
using System.Runtime.CompilerServices;

namespace Models
{
	[Serializable]
	public class Permission
	{
		public DateTime CreatTime
		{
			get;
			set;
		}

		public string Flag
		{
			get;
			set;
		}

		public Guid Id
		{
			get;
			set;
		}

		public bool IsDelete
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}

		public Guid? ParentId
		{
			get;
			set;
		}

		public int? Tier
		{
			get;
			set;
		}

		public Permission()
		{
		}
	}
}