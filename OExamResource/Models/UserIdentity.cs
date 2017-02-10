using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Models
{
	public class UserIdentity
	{
		public Guid Id
		{
			get;
			set;
		}

		public List<string> Modules
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}

		public int? Role
		{
			get;
			set;
		}

		public Guid SchoolId
		{
			get;
			set;
		}

		public int? Type
		{
			get;
			set;
		}

		public UserIdentity()
		{
		}
	}
}