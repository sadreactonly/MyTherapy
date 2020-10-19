using System;
using System.Collections.Generic;
using System.Linq;
using MyTherapy.Models;

namespace MyTherapy.Repositories
{


	public class DoctorAppointmentRepository : Repository<DoctorAppointment>
	{
		private static readonly string dbName = "Appointments.db3";


		public DoctorAppointmentRepository() : base(dbName)
		{

		}
		private static DoctorAppointmentRepository _instance;
		private static readonly object padlock = new object();

		/// <summary>
		/// Creates instance of DoctorAppointmentRepository class.
		/// </summary>
		public static DoctorAppointmentRepository Instance
		{
			get
			{
				lock (padlock)
					return _instance ??= new DoctorAppointmentRepository();
			}
		}

		/// <summary>
		/// Gets today's therapy.
		/// </summary>
		/// <returns></returns>
		public DoctorAppointment GetTodayAppointment() => Get().FirstOrDefault(x => x.Date == DateTime.Now.Date);

		public DoctorAppointment GetLastAppointment()
		{
			DoctorAppointment doctorAppointment = new DoctorAppointment();
			var list = Get();
			list.Reverse();
			foreach (var tmp in list)
			{
				if (tmp.INR != 0)
				{
					doctorAppointment = tmp;
					break;
				}

			}

			return doctorAppointment;
		}

		public DoctorAppointment GetNextAppointment()
		{
			DoctorAppointment doctorAppointment = new DoctorAppointment();
			var list = Get();
			list.Reverse();
			foreach (var tmp in list)
			{
				if (tmp.INR == 0)
				{
					doctorAppointment = tmp;
					break;
				}

			}

			return doctorAppointment;
		}


	}
}