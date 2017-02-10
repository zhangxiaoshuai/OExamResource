using System;
using System.Runtime.CompilerServices;

namespace Models
{
	[Serializable]
	public class LanCourse
	{
		public int? ClickCount
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

		public string ImageUrl
		{
			get;
			set;
		}

		public bool IsDeleted
		{
			get;
			set;
		}

		public bool? IsLab
		{
			get;
			set;
		}

		public Guid? K4Id
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}

		public string Remark
		{
			get;
			set;
		}

		public int? ResourceCount
		{
			get;
			set;
		}

		public Guid? SchoolId
		{
			get;
			set;
		}

		public string Tag1
		{
			get;
			set;
		}

		public string Tag2
		{
			get;
			set;
		}

		public Guid? UserId
		{
			get;
			set;
		}

		public LanCourse()
		{
		}
	}
}