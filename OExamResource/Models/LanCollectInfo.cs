using System;
using System.Runtime.CompilerServices;

namespace Models
{
	public class LanCollectInfo
	{
		public Guid CourseId
		{
			get;
			set;
		}

		public string CourseName
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

		public string ImageUrl
		{
			get;
			set;
		}

		public bool? IsShare
		{
			get;
			set;
		}

		public Guid ResourceId
		{
			get;
			set;
		}

		public string ResourceName
		{
			get;
			set;
		}

		public Guid TypeId
		{
			get;
			set;
		}

		public string TypeName
		{
			get;
			set;
		}

		public Guid UserId
		{
			get;
			set;
		}

		public string ViewUrl
		{
			get;
			set;
		}

		public LanCollectInfo()
		{
		}
	}
}