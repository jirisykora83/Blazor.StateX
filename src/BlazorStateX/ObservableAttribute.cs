using System;
namespace Blazor.StateX
{
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
	public class ObservableAttribute : Attribute
	{
		public string[] ObservedProperties { get; set; }

		public ObservableAttribute()
		{
		}

		public ObservableAttribute(params string[] values)
		{
			ObservedProperties = values;
		}
	}
}
