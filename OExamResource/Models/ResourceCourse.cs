using System;
using System.Runtime.CompilerServices;

namespace Models
{
	[Serializable]
	public class ResourceCourse
	{
		public string Author
		{
			get;
			set;
		}

		public DateTime CreateTime
		{
			get;
			set;
		}

		public int? DbStatus
		{
			get;
			set;
		}

		public string DownloadFilepath
		{
			get;
			set;
		}

		public string Fescribe
		{
			get;
			set;
		}

		public string FileExt
		{
			get;
			set;
		}

		public string Filepath
		{
			get;
			set;
		}

		public string Gaozhi
		{
			get;
			set;
		}

		public string IcoFilepath
		{
			get;
			set;
		}

		public Guid Id
		{
			get;
			set;
		}

		public bool? Is211
		{
			get;
			set;
		}

		public bool IsDeleted
		{
			get;
			set;
		}

		public DateTime? IssueTime
		{
			get;
			set;
		}

		public bool? IsTop
		{
			get;
			set;
		}

		public string KeyWords
		{
			get;
			set;
		}

		public int Medical_Tag
		{
			get;
			set;
		}

		public DateTime? ModifyTime
		{
			get;
			set;
		}

		public string NewFilepath
		{
			get;
			set;
		}

		public Guid? PackId
		{
			get;
			set;
		}

		public string PreviewFilepath
		{
			get;
			set;
		}

		public Guid? SchoolId
		{
			get;
			set;
		}

		public int? Size
		{
			get;
			set;
		}

		public int? SortId
		{
			get;
			set;
		}

		public string Tag1
		{
			get;
			set;
		}

		public DateTime? Tag1_uptime
		{
			get;
			set;
		}

		public string Tag2
		{
			get;
			set;
		}

		public string Title
		{
			get;
			set;
		}

		public ResourceCourse()
		{
		}
	}
}