using System;
using System.Runtime.CompilerServices;

namespace Models
{
	[Serializable]
	public class Room
	{
		public string Address
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

		public string Ip
		{
			get;
			set;
		}

		public bool IsDeleted
		{
			get;
			set;
		}

		public string PortPath
		{
			get;
			set;
		}

		public Guid ResourceTypeId
		{
			get;
			set;
		}

		public Room()
		{
		}
	}
}