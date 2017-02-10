using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Models
{
	public class ExamImport
	{
		public static Guid PaparTypeParentId;

		public List<ExamImportItem> Items
		{
			get;
			set;
		}

		public ExamImportPaper Paper
		{
			get;
			set;
		}

		static ExamImport()
		{
			ExamImport.PaparTypeParentId = new Guid("77907121-6d47-4676-9487-71a852473315");
		}

		public ExamImport()
		{
		}
	}
}