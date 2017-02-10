using System;
using System.Runtime.CompilerServices;

namespace Models
{
	[Serializable]
	public class Test
	{
		public int? ClickCount
		{
			get;
			set;
		}

		public DateTime CreateTime
		{
			get;
			set;
		}

		public string Flag
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

		public bool IsTop
		{
			get;
			set;
		}

		public int? ItemCount
		{
			get;
			set;
		}

		public string Logo
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}

		public Guid? ParentId
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

		public int? SubCount
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

		public int? Tier
		{
			get;
			set;
		}

		public Test()
		{
		}
	}
}