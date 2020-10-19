using MyTherapy.Models;

namespace MyTherapy
{
	public class PillsRepository : Repository<Pills>
	{
		private static readonly string dbName = "PillsDb.db3";


		public PillsRepository():base(dbName)
		{
			
		}
		private static PillsRepository _instance;
		private static readonly object padlock = new object();
		/// <summary>
		/// Creates instance of PillsDatabase class.
		/// </summary>
		public static PillsRepository Instance
		{
			get
			{
				lock (padlock)
					return _instance ??= new PillsRepository();
			}
		}
	}
}