using System;
using System.Runtime.CompilerServices;

namespace Models
{
	[Serializable]
	public class Menu
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

		public Guid? ModuleId
		{
			get;
			set;
		}

		public int? new_tag
		{
			get;
			set;
		}

		public Guid? ParentId
		{
			get;
			set;
		}

		public string Tag1
		{
			get;
			set;
		}

		public string Tag2
		{
			get;
			set;
		}

		public int? Tier
		{
			get;
			set;
		}

		public string Title
		{
			get;
			set;
		}

		public int? Type
		{
			get;
			set;
		}

		public string Url
		{
			get;
			set;
		}

		public Menu()
		{
		}
	}
}