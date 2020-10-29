using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using Android.Util;
using System.Linq;
using MyTherapy.Adapters;
using Result = MyTherapy.Models.Result;
using Android.Gms.Maps;
using MyTherapy.Service;
using System.Threading.Tasks;
using Android.Gms.Maps.Model;
using Xamarin.Essentials;

namespace MyTherapy.Activities
{
	[Activity(Label = "MapActivity")]
    public class MapActivity : Activity, IOnMapReadyCallback
    {

        private const string ApiKey = "AIzaSyAxIEoh0pEXqOh8J1HDGre76kdnNkWdpVA";
        private ListView placeList;
        private PlaceAdapter adapter;
        private List<Result> results = new List<Result>();
        Location location;
        private GoogleMap googleMap;
        List<Marker> markers = new List<Marker>();
        public string TAG
        {
            get;
            private set;
        }

        protected async override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.mapLayout);
            GoogleMapsApiService.Initialize(ApiKey);

            var mapFragment = FragmentManager.FindFragmentById<MapFragment>(Resource.Id.fragment1);
            mapFragment.GetMapAsync(this);

          
            placeList = FindViewById<ListView>(Resource.Id.listView1);
			placeList.ItemClick += PlaceList_ItemClick;
        }

		private void PlaceList_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
		{
            markers.ForEach(x => x.Remove());
            var currentPlace = results[e.Position];

            LatLng clickedPosition = new LatLng(currentPlace.geometry.location.lat, currentPlace.geometry.location.lng);

            markers.Add(AddMarker(googleMap, clickedPosition, currentPlace.name));

            googleMap.MoveCamera(GetCameraOptions(clickedPosition));
        }

		public async void OnMapReady(GoogleMap map)
        {
            var request = new GeolocationRequest(GeolocationAccuracy.High);
            location = await Geolocation.GetLocationAsync(request);
            googleMap = map;
            // Map settings
            map.MapType = GoogleMap.MapTypeNormal;
            map.UiSettings.ZoomControlsEnabled = true;

            // Camera settings
            LatLng currentPosition = new LatLng(location.Latitude, location.Longitude);
            map.MoveCamera(GetCameraOptions(currentPosition));

            // Get places
            var gmapsService = new GoogleMapsApiService();
            var response = await gmapsService.GetPlaceDetails(location.Latitude, location.Longitude);

            if (response.status == "OK")
            {
                results = response.results;
                adapter = new PlaceAdapter(this, results);
                placeList.Adapter = adapter;
            }

            AddMarker(map, currentPosition, "Current position");
        }

        private Marker AddMarker(GoogleMap map, LatLng position, string title)
		{
            // Set Markers
            MarkerOptions markerOpt1 = new MarkerOptions();
            markerOpt1.SetPosition(position);
            markerOpt1.SetTitle(title);

            return map.AddMarker(markerOpt1);
        }
        private CameraUpdate GetCameraOptions(LatLng position)
		{

            CameraPosition.Builder builder = CameraPosition.InvokeBuilder();
            builder.Target(position);
            builder.Zoom(15);

            CameraPosition cameraPosition = builder.Build();

            CameraUpdate cameraUpdate = CameraUpdateFactory.NewCameraPosition(cameraPosition);
            return cameraUpdate;
        }
	}
}