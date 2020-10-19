using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Android.Content;
using MyTherapy.Models;
using MyTherapy.Helpers;
using MyTherapy.Repositories;

namespace MyTherapy
{
	public class AppManager
	{
		private DoctorAppointmentRepository appointmentRepository = DoctorAppointmentRepository.Instance;
		private DailyTherapyRepository therapyRepository = DailyTherapyRepository.Instance;
		private PillsRepository pillsRepository = PillsRepository.Instance;

		private Context appContext;

		public event EventHandler TherapyTaken;

		public AppManager()
		{
			
		}

		public AppManager(Context context)
		{
			appContext = context;
		}

	
		public void AddAppointments(DoctorAppointment appointments) => appointmentRepository.Insert(appointments);

		public List<DoctorAppointment> GetAppointments() => appointmentRepository.Get(orderBy: x => x.Date);
		
		public List<DailyTherapy> GetTherapies() => therapyRepository.Get();

		public void AddTherapies(List<DailyTherapy> therapies) => therapyRepository.InsertAll(therapies);

		public void DeleteTherapy(DailyTherapy item) => therapyRepository.Delete(item);

		public DailyTherapy GetTodayTherapy() => therapyRepository.Get(predicate: x => x.Date == DateTime.Now.Date);

		public void TakeTherapy(DailyTherapy todayTherapy)
		{
			therapyRepository.Update(todayTherapy);
			TherapyTaken?.Invoke(this,null);
			
		}

		public void SetAllData(out string lastInrText, out string nextAppointmentText, out string todayTherapyTextText, out bool takeTherapyButtonEnabled, out string daysLeft)
		{
			var lastInr = appointmentRepository.GetLastAppointment().INR;
			if (lastInr != null)
				lastInrText = lastInr.ToString();
			else
				lastInrText = appContext.Resources.GetString(Resource.String.not_set);

			var nextApp = appointmentRepository.GetNextAppointment().Date;
			if(nextApp.Equals(DateTime.MinValue))
				nextAppointmentText = appContext.Resources.GetString(Resource.String.not_set);
			else
			nextAppointmentText = nextApp.ToShortDateString();

			var todayTherapy = GetTodayTherapy();
			todayTherapyTextText = todayTherapy !=null ? todayTherapy.Dose.ToString(CultureInfo.InvariantCulture): appContext.Resources.GetString(Resource.String.not_set);
			takeTherapyButtonEnabled = todayTherapy != null && todayTherapy.IsTaken;

			var pills = GetLastAddedPills();
			daysLeft = pills.CalculatePillsLeft(GetTherapies()).ToString();
		}

		public Pills GetLastAddedPills()
		{
			
			var tmp = pillsRepository.Get();
			tmp.Reverse();
			return tmp.FirstOrDefault();

		}
	}
}