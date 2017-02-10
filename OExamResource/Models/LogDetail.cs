using System;
using System.Runtime.CompilerServices;

namespace Models
{
	[Serializable]
	public class LogDetail
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

		public string Ip
		{
			get;
			set;
		}

		public bool IsDeleted
		{
			get;
			set;
		}

		public Guid ResourceId
		{
			get;
			set;
		}

		public Guid? SchoolId
		{
			get;
			set;
		}

		public int Status
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

		public Guid? TeacherId
		{
			get;
			set;
		}

		public LogDetail()
		{
		}
	}
}