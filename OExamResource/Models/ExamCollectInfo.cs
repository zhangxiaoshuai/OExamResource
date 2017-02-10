using System;
using System.Runtime.CompilerServices;

namespace Models
{
	public class ExamCollectInfo
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

		public Guid PaperId
		{
			get;
			set;
		}

		public string PaperName
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

		public ExamCollectInfo()
		{
		}
	}
}