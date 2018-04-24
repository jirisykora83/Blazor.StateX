using System.ComponentModel;

namespace BookingForm.Models
{
	public class Passenger : INotifyPropertyChanged // <--- Add this to each model entity which you are mutable
	{
		public event PropertyChangedEventHandler PropertyChanged; // <---- And this (Fody generate rest of the boilerplate)

		public string FirstName { get; set; }

		public string LastName { get; set; }

		public PassengerTitle Title { get; set; }

		public string DocumentNumber { get; set; }

		public string FullName => (FirstName ?? string.Empty) + " " + (LastName ?? string.Empty);
	}

	public enum PassengerTitle
	{
		Mr, Ms, Mrs
	}
}
