using System;
using System.Runtime.CompilerServices;

namespace Models
{
	[Serializable]
	public class Resource
	{
		public string Author
		{
			get;
			set;
		}

		public int? BoutiqueGrade
		{
			get;
			set;
		}

		public Guid? CategoryId
		{
			get;
			set;
		}

		public Guid? CourseId
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

		public Guid? KnowledgeId
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

		public string Sampling
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

		public Guid? SpecialityId
		{
			get;
			set;
		}

		public Guid? SubjectId
		{
			get;
			set;
		}

		public string tag1
		{
			get;
			set;
		}

		public DateTime tag1_uptime
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

		public Guid? TypeId
		{
			get;
			set;
		}

		public string VideoResolution
		{
			get;
			set;
		}

		public Resource()
		{
		}
	}
}