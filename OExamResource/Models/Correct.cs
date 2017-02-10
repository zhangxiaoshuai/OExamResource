using System;
using System.Runtime.CompilerServices;

namespace Models
{
	[Serializable]
	public class Correct
	{
		public int? Count
		{
			get;
			set;
		}

		public DateTime? CreateTime
		{
			get;
			set;
		}

		public Guid Id
		{
			get;
			set;
		}

		public bool? IsDeleted
		{
			get;
			set;
		}

		public Guid? Resource_Id
		{
			get;
			set;
		}

		public string Tag2
		{
			get;
			set;
		}

		public Guid? Teach_Id
		{
			get;
			set;
		}

		public Correct()
		{
		}
	}
}