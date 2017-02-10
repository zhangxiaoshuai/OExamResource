using System;
using System.Runtime.CompilerServices;

namespace Models
{
	[Serializable]
	public class User_Role
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

		public bool IsCustom
		{
			get;
			set;
		}

		public bool IsDeleted
		{
			get;
			set;
		}

		public Guid RoleId
		{
			get;
			set;
		}

		public Guid UserId
		{
			get;
			set;
		}

		public User_Role()
		{
		}
	}
}