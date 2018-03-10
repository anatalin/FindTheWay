using FindTheWay.Common.Models;
using FindTheWay.Data.DbModels;

namespace FindTheWay.Common
{
	public static class DbViewModelConverter
    {
        public static CoordinatesModel Convert(Node from)
        {
            return new CoordinatesModel
            {
                Latitude = from.Latitude,
                Longitude = from.Longitude
            };
        }
    }
}
