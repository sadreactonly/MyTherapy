using System.Threading.Tasks;
using MyTherapy.Models;
using MyTherapy.Models.GoogleDirections;

namespace MyTherapy.Service
{
	public interface IGoogleMapsApiService
	{
		Task<Root> GetPlaces(double lat, double lon);
		Task<DirectionRoot> GetRoutes(double lat1, double lon1, double lat2, double lon2);
	}
}