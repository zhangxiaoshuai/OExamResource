using System;
using System.Runtime.CompilerServices;

namespace Models
{
	[Serializable]
	public class Collect
	{
		public DateTime? CreateTime
		{
			get;
			set;
		}

		public Guid FolderId
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

		public bool IsPack
		{
			get;
			set;
		}

		public Guid ResourceId
		{
			get;
			set;
		}

		public int? tag1
		{
			get;
			set;
		}

		public string tag2
		{
			get;
			set;
		}

		public Guid TeacherId
		{
			get;
			set;
		}

		public Collect()
		{
		}
	}
}