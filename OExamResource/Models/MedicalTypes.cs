using System;
using System.Runtime.CompilerServices;

namespace Models
{
	public class MedicalTypes
	{
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

		public Guid ParentId
		{
			get;
			set;
		}

		public MedicalTypes()
		{
		}
	}
}