using System;
using System.Runtime.CompilerServices;

namespace Models
{
	public class DownloadDetails
	{
		public string CategoryName
		{
			get;
			set;
		}

		public DateTime CreateTime
		{
			get;
			set;
		}

		public int DbStatus
		{
			get;
			set;
		}

		public int DownloadCount
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

		public Guid ResourceId
		{
			get;
			set;
		}

		public string ResourceName
		{
			get;
			set;
		}

		public string SchoolName
		{
			get;
			set;
		}

		public string SpecialityName
		{
			get;
			set;
		}

		public string SubjectName
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

		public Guid TeacherId
		{
			get;
			set;
		}

		public string TeacherLoginName
		{
			get;
			set;
		}

		public string TeacherName
		{
			get;
			set;
		}

		public string TypeName
		{
			get;
			set;
		}

		public DownloadDetails()
		{
		}
	}
}