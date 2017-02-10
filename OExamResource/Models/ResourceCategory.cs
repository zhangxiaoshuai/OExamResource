using System;
using System.Runtime.CompilerServices;

namespace Models
{
	[Serializable]
	public class ResourceCategory
	{
		public int? Count
		{
			get;
			set;
		}

		public int? Count_JX
		{
			get;
			set;
		}

		public DateTime CreateTime
		{
			get;
			set;
		}

		public string Ico
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

		public int? MedicalCount
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}

		public Guid? ParentId
		{
			get;
			set;
		}

		public int? SortId
		{
			get;
			set;
		}

		public int Tier
		{
			get;
			set;
		}

		public ResourceCategory()
		{
		}
	}
}