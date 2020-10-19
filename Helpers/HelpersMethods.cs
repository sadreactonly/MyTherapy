using System.Collections.Generic;
using System.Linq;
using MyTherapy.Models;

namespace MyTherapy.Helpers
{
	public static class HelpersMethods
	{

		public static double CalculatePillsLeft(this Pills pills, List<DailyTherapy> therapies)
		{
			if (pills == null)
				return 0;
		
			var temp = therapies.Where(x => x.Date >= pills.Date).ToList();

			double pillsLeft = pills.Quantity - temp.Where(x => x.IsTaken == true).Sum(x => x.Dose);

			return pillsLeft;
		}
	}
}