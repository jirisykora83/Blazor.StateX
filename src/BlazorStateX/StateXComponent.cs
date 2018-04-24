using Microsoft.AspNetCore.Blazor.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Blazor.StateX
{
	public class StateXComponent : BlazorComponent, IDisposable
	{
		private Dictionary<Type, string[]> observedProperties = new Dictionary<Type, string[]>();

		public void Dispose()
		{
			// TODO: possibly some type of caching..
			// TODO: consolidate with SetPropertyChangeNotify (duplicate code..).

			var properties = GetType().GetProperties(BindingFlags.Public
				| BindingFlags.GetProperty
				| BindingFlags.Instance
				| BindingFlags.NonPublic);

			for (int i = 0; i < properties.Length; i++)
			{
				if (properties[i].PropertyType != null &&
					properties[i].GetCustomAttribute<ObservableAttribute>() != null)
				{
					var notifyProperty = (INotifyPropertyChanged)properties[i].GetValue(this);
					if (notifyProperty != null)
					{
						notifyProperty.PropertyChanged -= HandleUpdateNotify;
					}
				}
			}
		}

		protected override void OnParametersSet()
		{
			SetPropertyChangeNotify();
			base.OnParametersSet();
		}

		protected void HandleUpdateNotify(object sender, PropertyChangedEventArgs e)
		{
			// TODO: possibly some caching..
			var senderType = sender.GetType();
			if (observedProperties.TryGetValue(senderType, out var properties))
			{
				if (properties != null && properties.Contains(e.PropertyName))
				{
					StateHasChanged();
				}
				// skip
				return;
			}
			else
			{
				StateHasChanged();
			}
		}

		private void SetPropertyChangeNotify()
		{
			// TODO: possibly some type of caching..

			var properties = GetType().GetProperties(BindingFlags.Public
				| BindingFlags.GetProperty
				| BindingFlags.Instance
				| BindingFlags.NonPublic);

			for (int i = 0; i < properties.Length; i++)
			{
				if (properties[i].PropertyType != null)
				{
					var observableAttribute = properties[i].GetCustomAttribute<ObservableAttribute>();
					if (observableAttribute == null)
					{
						return;
					}
					if (!properties[i].PropertyType.GetInterfaces().Contains(typeof(INotifyPropertyChanged)))
					{
						throw new Exception($"{properties[i].Name} is mark as [Observable] but do not implement INotifyPropertyChanged.");
					}
					var notifyProperty = (INotifyPropertyChanged)properties[i].GetValue(this);
					if (notifyProperty != null)
					{
						var propertyType = properties[i].PropertyType;
						if (observableAttribute.ObservedProperties != null && !observedProperties.ContainsKey(propertyType))
						{
							observedProperties.Add(propertyType, observableAttribute.ObservedProperties);
						}
						notifyProperty.PropertyChanged -= HandleUpdateNotify;
						notifyProperty.PropertyChanged += HandleUpdateNotify;
					}
				}
			}
		}
	}
}
