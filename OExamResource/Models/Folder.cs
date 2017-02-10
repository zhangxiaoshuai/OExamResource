using System;
using System.Runtime.CompilerServices;

namespace Models
{
	[Serializable]
	public class Folder
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

		public string Remarks
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

		public Folder()
		{
		}
	}
}