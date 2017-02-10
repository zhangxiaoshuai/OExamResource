using System;
using System.Runtime.CompilerServices;

namespace Models
{
	[Serializable]
	public class ResourceUpload
	{
		public Guid? CategoryId
		{
			get;
			set;
		}

		public string Course
		{
			get;
			set;
		}

		public DateTime CreatedTime
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

		public int? Size
		{
			get;
			set;
		}

		public Guid? TypeId
		{
			get;
			set;
		}

		public Guid? UserId
		{
			get;
			set;
		}

		public ResourceUpload()
		{
		}
	}
}