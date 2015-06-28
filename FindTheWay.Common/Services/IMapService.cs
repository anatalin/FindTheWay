using FindTheWay.Common.Models;
using FindTheWay.Data.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindTheWay.Common.Services
{
    public interface IMapService
    {
        IList<Node> GetPath(List<long> nodeIds);

        CoordinatesModel ApproximatePoint(CoordinatesModel coords);
    }
}
