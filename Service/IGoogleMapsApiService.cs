using System.Threading.Tasks;
using MyTherapy.Models;

namespace MyTherapy.Service
{
	public interface IGoogleMapsApiService
	{
		Task<Root> GetPlaceDetails(double lat, double lon);
	}
}