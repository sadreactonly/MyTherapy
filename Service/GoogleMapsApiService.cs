using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Gms.Location.Places;
using Android.OS;
using Android.Runtime;
using Android.Views;
using System.Net.Http;
using System.Net.Http.Headers;
using MyTherapy.Models;

namespace MyTherapy.Service
{
	public class GoogleMapsApiService : IGoogleMapsApiService
	{
        static string _googleMapsKey;

        private const string ApiBaseAddress = "https://maps.googleapis.com/maps/";
        private HttpClient CreateClient()
        {
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(ApiBaseAddress)
            };

            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return httpClient;
        }

        public static void Initialize(string googleMapsKey)
        {
            _googleMapsKey = googleMapsKey;
        }



        public async Task<Root> GetPlaceDetails(double lat, double lon)
		{
            Root result = new Root();
            using (var httpClient = CreateClient())
            {
                var response = await httpClient.GetAsync($"api/place/nearbysearch/json?location={lat},{lon}&radius=2000&type=pharmacy&key={_googleMapsKey}").ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    if (!string.IsNullOrWhiteSpace(json) && json != "ERROR")
                    {
                        result = Newtonsoft.Json.JsonConvert.DeserializeObject<Root>(json); 

                    }
                }
            }
            
            return result;
        }
	}
}