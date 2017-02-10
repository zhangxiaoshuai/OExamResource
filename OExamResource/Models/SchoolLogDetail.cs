using System;
using System.Runtime.CompilerServices;

namespace Models
{
	[Serializable]
	public class SchoolLogDetail
	{
		public string BannerImgs
		{
			get;
			set;
		}

		public DateTime CreateTime
		{
			get;
			set;
		}

		public int DownLoadCount
		{
			get;
			set;
		}

		public DateTime? EndTime
		{
			get;
			set;
		}

		public Guid Id
		{
			get;
			set;
		}

		public string ip
		{
			get;
			set;
		}

		public bool IsDeleted
		{
			get;
			set;
		}

		public int? LanCourseCount
		{
			get;
			set;
		}

		public int? LanResourceCount
		{
			get;
			set;
		}

		public string LogoImg
		{
			get;
			set;
		}

		public DateTime? ModifyTime
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}

		public string NameImg
		{
			get;
			set;
		}

		public Guid? ParentId
		{
			get;
			set;
		}

		public int PressCount
		{
			get;
			set;
		}

		public Guid ProvinceId
		{
			get;
			set;
		}

		public string ProvinceName
		{
			get;
			set;
		}

		public string Remark
		{
			get;
			set;
		}

		public Guid SellId
		{
			get;
			set;
		}

		public DateTime? StartTime
		{
			get;
			set;
		}

		public string StopRemind
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

		public int Tier
		{
			get;
			set;
		}

		public int? TrialType
		{
			get;
			set;
		}

		public SchoolLogDetail()
		{
		}
	}
}