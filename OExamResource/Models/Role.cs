using System;
using System.Runtime.CompilerServices;

namespace Models
{
	[Serializable]
	public class Role
	{
		public DateTime CreateTime
		{
			get;
			set;
		}

		public Guid? DepartmentId
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

		public int? Type
		{
			get;
			set;
		}

		public Role()
		{
		}
	}
}