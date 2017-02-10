using System;
using System.Runtime.CompilerServices;

namespace Models
{
	[Serializable]
	public class SystemInfo
	{
		public DateTime CreatTime
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

		public bool? IsReg
		{
			get;
			set;
		}

		public bool? IsValid
		{
			get;
			set;
		}

		public SystemInfo()
		{
		}
	}
}