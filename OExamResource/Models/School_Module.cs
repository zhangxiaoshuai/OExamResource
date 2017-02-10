using System;
using System.Runtime.CompilerServices;

namespace Models
{
	[Serializable]
	public class School_Module
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

		public Guid ModuleId
		{
			get;
			set;
		}

		public Guid SchoolId
		{
			get;
			set;
		}

		public int Tag1
		{
			get;
			set;
		}

		public int TrialTag
		{
			get;
			set;
		}

		public School_Module()
		{
		}
	}
}