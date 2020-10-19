using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyTherapy.Models
{
	/// <summary>
	/// Class responsible for pills.
	/// </summary>
	public class Pills
	{
		/// <summary>
		/// Gets or sets date when pills are buyed.
		/// </summary>
		public DateTime Date { get; set; }

		/// <summary>
		/// Gets or sets how much pills are in box.
		/// </summary>
		public int Quantity { get; set; }

	}
}