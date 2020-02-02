using System.Collections.Generic;
using System.Threading.Tasks;
using FindTheWay.Common.Models;

namespace FindTheWay.Common.Services
{
    public interface IMapService
    {
        Task<IEnumerable<CoordinatesModel>> GetPathAsync(List<long> nodeIds);

        Task<CoordinatesModel> ApproximatePointAsync(CoordinatesModel coords);
    }
}
