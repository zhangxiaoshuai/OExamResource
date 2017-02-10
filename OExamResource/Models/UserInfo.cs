using System;
using System.Runtime.CompilerServices;

namespace Models
{
	[Serializable]
	public class UserInfo
	{
		public DateTime? Birthday
		{
			get;
			set;
		}

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

		public string Email
		{
			get;
			set;
		}

		public Guid? FacultyId
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

		public Guid? JobId
		{
			get;
			set;
		}

		public string LoginName
		{
			get;
			set;
		}

		public string LoginPwd
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}

		public string Phone
		{
			get;
			set;
		}

		public Guid? PostId
		{
			get;
			set;
		}

		public Guid? ProfessionId
		{
			get;
			set;
		}

		public Guid SchoolId
		{
			get;
			set;
		}

		public string Sex
		{
			get;
			set;
		}

		public int? Type
		{
			get;
			set;
		}

		public UserInfo()
		{
		}
	}
}