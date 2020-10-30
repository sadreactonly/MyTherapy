using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;


namespace MyTherapy.Helpers
{
	public static class GoogleMapHelper
	{
        private static List<Polyline> polylines = new List<Polyline>();
        private static List<Android.Locations.Location> DecodePolylinePoints(string encodedPoints)
        {
            if (string.IsNullOrEmpty(encodedPoints))
                return null;
            var poly = new List<Android.Locations.Location>();
            char[] polylinechars = encodedPoints.ToCharArray();
            int index = 0;

            int currentLat = 0;
            int currentLng = 0;
            int next5bits;
            int sum;
            int shifter;

            try
            {
                while (index < polylinechars.Length)
                {
                    // calculate next latitude
                    sum = 0;
                    shifter = 0;
                    do
                    {
                        next5bits = (int)polylinechars[index++] - 63;
                        sum |= (next5bits & 31) << shifter;
                        shifter += 5;
                    } while (next5bits >= 32 && index < polylinechars.Length);

                    if (index >= polylinechars.Length)
                        break;

                    currentLat += (sum & 1) == 1 ? ~(sum >> 1) : (sum >> 1);

                    //calculate next longitude
                    sum = 0;
                    shifter = 0;
                    do
                    {
                        next5bits = (int)polylinechars[index++] - 63;
                        sum |= (next5bits & 31) << shifter;
                        shifter += 5;
                    } while (next5bits >= 32 && index < polylinechars.Length);

                    if (index >= polylinechars.Length && next5bits >= 32)
                        break;

                    currentLng += (sum & 1) == 1 ? ~(sum >> 1) : (sum >> 1);
                    Android.Locations.Location p = new Android.Locations.Location("");
                    p.Latitude = Convert.ToDouble(currentLat) / 100000.0;
                    p.Longitude = Convert.ToDouble(currentLng) / 100000.0;
                    poly.Add(p);
                }
            }
            catch
            {


            }
            return poly;
        }
        private static void ClearPolyLines()
		{
            foreach(var tmp in polylines)
			{
                tmp.Remove();
			}
            polylines.Clear();
		}
        public static void DrawRoutes(Activity activity, GoogleMap _map, string encodedPoints)
		{
            ClearPolyLines();
            // Decode the points
            var lstDecodedPoints = DecodePolylinePoints(encodedPoints);
            //convert list of location point to array of latlng type
            var latLngPoints = new LatLng[lstDecodedPoints.Count];
            int index = 0;
            foreach (Android.Locations.Location loc in lstDecodedPoints)
            {
                latLngPoints[index++] = new LatLng(loc.Latitude, loc.Longitude);
            }
            // Create polyline 
            var polylineoption = new PolylineOptions();
            polylineoption.InvokeColor(Android.Graphics.Color.Green);
            polylineoption.Geodesic(true);
            polylineoption.Add(latLngPoints);
            // Don't forget to add it to the main quie, if you was doing the request for a cordinate in background
            // Add polyline to map
            activity.RunOnUiThread(() =>

               polylines.Add(_map.AddPolyline(polylineoption)));
        }
    } 
}