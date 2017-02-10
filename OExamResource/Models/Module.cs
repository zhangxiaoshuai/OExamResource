using System;
using System.Runtime.CompilerServices;

namespace Models
{
	[Serializable]
	public class Module
	{
		public string Code
		{
			get;
			set;
		}

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

		public int Tag1
		{
			get;
			set;
		}

		public Module()
		{
		}
	}
}