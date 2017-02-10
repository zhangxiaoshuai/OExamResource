using System;
using System.Runtime.CompilerServices;

namespace Models
{
	[Serializable]
	public class Job
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

		public Job()
		{
		}
	}
}