using System;
using System.Runtime.CompilerServices;

namespace Models
{
	public class CollectSource
	{
		public string DownloadFilepath
		{
			get;
			set;
		}

		public string Icofilepath
		{
			get;
			set;
		}

		public Guid Id
		{
			get;
			set;
		}

		public Guid Packid
		{
			get;
			set;
		}

		public string Previewfilepath
		{
			get;
			set;
		}

		public string Speciality
		{
			get;
			set;
		}

		public string Subjectname
		{
			get;
			set;
		}

		public DateTime Time
		{
			get;
			set;
		}

		public string Title
		{
			get;
			set;
		}

		public CollectSource()
		{
		}
	}
}