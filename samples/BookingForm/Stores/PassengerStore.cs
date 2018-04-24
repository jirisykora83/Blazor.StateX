using BookingForm.Models;
using System.Collections.Generic;
using System.ComponentModel;
namespace BookingForm.Stores
{
    public class PassengerStore : INotifyPropertyChanged
	{
		//------------------------------
		//  Variables
		//------------------------------

		public event PropertyChangedEventHandler PropertyChanged;

		private List<Passenger> passengers = new List<Passenger>();

		//------------------------------
		//  Properties
		//------------------------------

		public IReadOnlyList<Passenger> Passengers { get; private set; } = new List<Passenger>(0).AsReadOnly();

		//------------------------------
		//  Actions
		//------------------------------

		public void AddNewPassenger()
		{
			// All actions which add new entities to store should be done via "Action" in store.
			// Or unfortunately you must implement INotifyPropertyChanged for collection
			// with ObservableCollection.

			// Also it depend on your need if all items in store collection go through action
			// you can also subscribe notify event here. Because this example fire PropertyCanged
			// only if you add new passenger but not when you latter change some property on it.

			passengers.Add(new Passenger());
			Passengers = passengers.AsReadOnly();
		}

		public bool RemovePassenger(Passenger passenger)
		{
			if (passengers.Remove(passenger))
			{
				Passengers = passengers.AsReadOnly();
				return true;
			}
			return false;
		}
	}
}
