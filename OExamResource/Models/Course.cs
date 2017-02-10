using System;
using System.Runtime.CompilerServices;

namespace Models
{
	[Serializable]
	public class Course
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

		public Guid? SpecialityId
		{
			get;
			set;
		}

		public Course()
		{
		}
	}
}