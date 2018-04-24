using BookingForm.Stores;
using Microsoft.AspNetCore.Blazor.Browser.Rendering;
using Microsoft.AspNetCore.Blazor.Browser.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BookingForm
{
    public class Program
    {
		public static void Main(string[] args)
        {
            var serviceProvider = new BrowserServiceProvider(services =>
            {
				services.AddSingleton<PassengerStore>();
			});

            new BrowserRenderer(serviceProvider).AddComponent<App>("app");
        }
    }
}
