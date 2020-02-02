using System.Collections.Generic;
using System.Threading.Tasks;
using FindTheWay.Common.Models;
using FindTheWay.Common.Services;
using Microsoft.AspNetCore.Mvc;

namespace FindTheWay.Web.Controllers
{
    public class MapController : Controller
    {
        private readonly IMapService mapService;

        public MapController(IMapService mapService)
        {
            this.mapService = mapService;
        }

        [HttpPost]
        public Task<IEnumerable<CoordinatesModel>> GetPathAsync([FromBody]List<long> nodeIdsReceived)
        {
            return this.mapService.GetPathAsync(nodeIdsReceived);
        }

        [HttpPost]
        public Task<CoordinatesModel> ApproximatePointAsync([FromBody]CoordinatesModel coordsRcvd)
        {
            return this.mapService.ApproximatePointAsync(coordsRcvd);
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
    }
}
