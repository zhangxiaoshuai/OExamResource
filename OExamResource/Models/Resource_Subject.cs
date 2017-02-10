using System;
using System.Runtime.CompilerServices;

namespace Models
{
	public class Resource_Subject
	{
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

		public Guid SubId
		{
			get;
			set;
		}

		public string SubName
		{
			get;
			set;
		}

		public Resource_Subject()
		{
		}
	}
}