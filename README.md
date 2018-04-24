# Blazor StateX
Example how is possible to manage state in Blazor application with INotifyPropertyChanged inspired with MobX.

This example use [PropertyChanged.Fody NuGet package](https://nuget.org/packages/PropertyChanged.Fody/) to generate most of boilerplate code associate with INotifyPropertyChanged pattern.

It is first time when i use [PropertyChanged.Fody](https://github.com/Fody/PropertyChanged/) and seems it work well.

Main goal is minimizing boilerplate.

### Getting started
##### 0. Install fody
Install fody (on all project when you have Stores or Models)
```PM> Install-Package PropertyChanged.Fody```
```PM> Install-Package Fody```

##### 1. Add  ```Models```
Add folder ```Models``` (or use .Shared project in template with more project in solution)

##### 2. Add model entity

```
public class Passenger : INotifyPropertyChanged // <--- Add this to each model entity which you are mutable
{
	public event PropertyChangedEventHandler PropertyChanged; // <---- And this (Fody generate rest of the boilerplate)

	public string FirstName { get; set; }

	public string LastName { get; set; }

	public string DocumentNumber { get; set; }

	public string SomeUnuseProperty { get; set; }

	public string FullName => (FirstName ?? string.Empty) + " " + (LastName ?? string.Empty);
}
```

##### 3. Add folder ```Stores```
Add folder "Stores" or whatever you want to use for manage state

##### 4. Add store
```
public class PassengerStore : INotifyPropertyChanged // <--- Add this to each store
{
	//------------------------------
	//  Variables
	//------------------------------

	public event PropertyChangedEventHandler PropertyChanged; // <---- And this (Fody generate rest of the boilerplate)

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
```
##### 5. Add 1. component
```
@inherits StateXComponent // <---- Nessesry for apply observable attribute

<div>
	<div>Passengers</div>
	<hr />
	@foreach (var passenger in passengerStore.Passengers)
	{
		<PassengerForm Passenger="passenger" />
	}
</div>

@functions
{
	[Inject, Observable] // <---- You muset inject store here no on top of the component (because only here is possible to set [Observable])
	private PassengerStore passengerStore { get; set; }
}
```

Component now refresh each time when you modife store (add new passengers / remove)

Warn: this component not observed each passenger!

##### 6. Add 2. component

This component observed Passanger when you somewhere change Passanger properties component will be refresh.

```
@inherits StateXComponent

<div>
	<div class="form-group">
		<label>First name</label>
		<!-- https://github.com/aspnet/Blazor/issues/659 -->
		<input bind="@Passenger.FirstName" placeholder="First name" />
	</div>
	<div class="form-group">
		<label>Last name</label>
		<input bind="@(Passenger.LastName)" placeholder="Last name" />
	</div>
	<div class="form-group">
		<label>Passport or ID number</label>
		<input bind="@Passenger.DocumentNumber" placeholder="Documment ID" />
	</div>
	<hr />
</div>

@functions
{
	[Observable]
	public Passenger Passenger { get; set; }
}
```

Also you can use:
```
[Observable(nameof(Models.Passenger.FirstName))]
```

If you want observed only FirstName properties.

### Instalation

This is just example i dont have any nuget package you need add ```ObservableAttribute.cs``` and ```StateXComponent.cs``` to your project.