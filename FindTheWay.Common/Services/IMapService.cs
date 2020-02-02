using System.Collections.Generic;
using FindTheWay.Common.Models;

namespace FindTheWay.Common.Services
{
    public interface IMapService
    {
		IEnumerable<CoordinatesModel> GetPath(List<long> nodeIds);

		CoordinatesModel ApproximatePoint(CoordinatesModel coords);
    }
}
