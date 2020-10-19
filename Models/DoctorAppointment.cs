using System;
using System.ComponentModel;
using Newtonsoft.Json;
using SQLite;

namespace MyTherapy.Models
{
	[Table(nameof(DoctorAppointment))]
	public class DoctorAppointment
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }

		/// <summary>
		/// Gets or sets value of INR.
		/// </summary>
		public double? INR { get; set; }

		/// <summary>
		/// Gets or sets date of doctors appointment.
		/// </summary>
		public DateTime Date { get; set; }

		
	}
}
