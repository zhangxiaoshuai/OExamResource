using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Models
{
	public class ChartData
	{
		public string Count
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}

		public ChartData()
		{
		}

		public static List<ChartData> CreateCompanyList(List<ResourceCategory> list)
		{
			List<ChartData> company1 = new List<ChartData>();
			foreach (ResourceCategory item in list)
			{
				ChartData chartDatum = new ChartData()
				{
					Name = item.Name,
					Count = item.Count.ToString()
				};
				company1.Add(chartDatum);
			}
			return company1;
		}

		public static List<ChartData> CreateCompanyList2()
		{
			List<ChartData> company2 = new List<ChartData>();
			ChartData chartDatum = new ChartData()
			{
				Name = "January",
				Count = ""
			};
			company2.Add(chartDatum);
			ChartData chartDatum1 = new ChartData()
			{
				Name = "February",
				Count = ""
			};
			company2.Add(chartDatum1);
			ChartData chartDatum2 = new ChartData()
			{
				Name = "March",
				Count = ""
			};
			company2.Add(chartDatum2);
			return company2;
		}
	}
}