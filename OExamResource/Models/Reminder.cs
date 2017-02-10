using System;
using System.Runtime.CompilerServices;

namespace Models
{
	public class Reminder
	{
		public int DownCount
		{
			get;
			set;
		}

		public DateTime EndTime
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

		public string SellName
		{
			get;
			set;
		}

		public DateTime StartTime
		{
			get;
			set;
		}

		public string StopRemind
		{
			get;
			set;
		}

		public int TrialType
		{
			get;
			set;
		}

		public Reminder()
		{
		}
	}
}