using System;
using System.Runtime.CompilerServices;

namespace Models
{
	[Serializable]
	public class MedicalList
	{
		public string Author
		{
			get;
			set;
		}

		public string CategoryName
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

		public string KnowledgeName
		{
			get;
			set;
		}

		public int? Size
		{
			get;
			set;
		}

		public string SpecialityName
		{
			get;
			set;
		}

		public string Title
		{
			get;
			set;
		}

		public string TypeName
		{
			get;
			set;
		}

		public MedicalList()
		{
		}
	}
}