using System;
using System.Runtime.CompilerServices;

namespace Models
{
	[Serializable]
	public class Role_Permission
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

		public Guid PermissionId
		{
			get;
			set;
		}

		public Guid RoleId
		{
			get;
			set;
		}

		public Role_Permission()
		{
		}
	}
}