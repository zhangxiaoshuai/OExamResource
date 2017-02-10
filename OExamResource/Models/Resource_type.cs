using System;
using System.Runtime.CompilerServices;

namespace Models
{
	public class Resource_type
	{
		public int BaseType
		{
			get;
			set;
		}

		public int Count
		{
			get;
			set;
		}

		public Guid Id
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}

		public int PackCount
		{
			get;
			set;
		}

		public int SortId
		{
			get;
			set;
		}

		public Resource_type()
		{
		}
	}
}