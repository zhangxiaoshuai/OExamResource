using System;
using System.Runtime.CompilerServices;

namespace Models
{
	[Serializable]
	public class User_Permission
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

		public bool IsAdd
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

		public Guid UserId
		{
			get;
			set;
		}

		public User_Permission()
		{
		}
	}
}