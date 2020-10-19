using MyTherapy.Models;

namespace MyTherapy.Repositories
{
	public class DailyTherapyRepository : Repository<DailyTherapy>
	{
		private static readonly string dbName = "TherapiesDb.db3";


		public DailyTherapyRepository() : base(dbName)
		{

		}
		private static DailyTherapyRepository _instance;
		private static readonly object padlock = new object();
	
		/// <summary>
		/// Creates instance of PillsDatabase class.
		/// </summary>
		public static DailyTherapyRepository Instance
		{
			get
			{
				lock (padlock)
					return _instance ??= new DailyTherapyRepository();
			}
		}
	}
}