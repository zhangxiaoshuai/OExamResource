using PetaPoco;
using System;
using System.Runtime.CompilerServices;

namespace Providers
{
	public class DB : Database
	{
		[ThreadStatic]
		private static DB _instance;

		public static DB.IFactory Factory
		{
			get;
			set;
		}

		public DB() : base("ConnectionString")
		{
		}

		public DB(string connectionStringName) : base(connectionStringName)
		{
		}

		public static DB GetInstance()
		{
			if (DB._instance == null)
			{
				DB._instance = new DB();
			}
			return DB._instance;
		}

		public override void OnBeginTransaction()
		{
			if (DB._instance == null)
			{
				DB._instance = this;
			}
		}

		public override void OnEndTransaction()
		{
			if (DB._instance == this)
			{
				DB._instance = null;
			}
		}

		public interface IFactory
		{
			DB GetInstance();
		}
	}
}