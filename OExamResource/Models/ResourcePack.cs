using System;
using System.Runtime.CompilerServices;

namespace Models
{
	[Serializable]
	public class ResourcePack
	{
		public Guid? CategoryId
		{
			get;
			set;
		}

		public int ClickCount
		{
			get;
			set;
		}

		public int Count
		{
			get;
			set;
		}

		public DateTime CreateTime
		{
			get;
			set;
		}

		public string FilePath
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

		public string KeyWord
		{
			get;
			set;
		}

		public int Medical_Tag
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}

		public string Remark
		{
			get;
			set;
		}

		public int Size
		{
			get;
			set;
		}

		public int? SortId
		{
			get;
			set;
		}

		public Guid? TypeId
		{
			get;
			set;
		}

		public ResourcePack()
		{
		}
	}
}