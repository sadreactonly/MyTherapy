using Newtonsoft.Json;
using SQLite;
using System;

namespace MyTherapy.Models
{
	[Table("DailyTherapy")]
	[Serializable]
	public class DailyTherapy 
	{
	
		[JsonIgnore]
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }

		public Guid Guid { get; set; }

		/// <summary>
		/// Gets or sets daily dosage of therapy.
		/// </summary>
		public double Dose { get; set; }

		/// <summary>
		/// Gets or sets date for daily therapy.
		/// </summary>
		public DateTime Date { get; set; }

		/// <summary>
		/// Gets or sets info is therapy taken.
		/// </summary>
		public bool IsTaken { get; set; }

	}
}
