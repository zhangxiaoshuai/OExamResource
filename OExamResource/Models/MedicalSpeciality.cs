using System;
using System.Runtime.CompilerServices;

namespace Models
{
	[Serializable]
	public class MedicalSpeciality
	{
		public int? Count
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

		public int? SortId
		{
			get;
			set;
		}

		public MedicalSpeciality()
		{
		}
	}
}