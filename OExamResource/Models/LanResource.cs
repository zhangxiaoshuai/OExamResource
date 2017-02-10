using System;
using System.Runtime.CompilerServices;

namespace Models
{
	[Serializable]
	public class LanResource
	{
		public int? ClickCount
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

		public string DownloadUrl
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

		public bool? IsShare
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}

		public string NameEx
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

		public Guid? TypeId
		{
			get;
			set;
		}

		public string ViewUrl
		{
			get;
			set;
		}

		public LanResource()
		{
		}
	}
}