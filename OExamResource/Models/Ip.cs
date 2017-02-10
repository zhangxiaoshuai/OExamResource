using System;
using System.Runtime.CompilerServices;

namespace Models
{
	[Serializable]
	public class Ip
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

		public byte[] IpEnd
		{
			get;
			set;
		}

		public byte[] IpStart
		{
			get;
			set;
		}

		public bool IsDeleted
		{
			get;
			set;
		}

		public Guid SchoolId
		{
			get;
			set;
		}

		public int Tier
		{
			get;
			set;
		}

		public Ip()
		{
		}
	}
}