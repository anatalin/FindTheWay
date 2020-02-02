using System.Collections.Generic;
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
        public IEnumerable<CoordinatesModel> GetPath([FromBody]List<long> nodeIdsReceived)
        {
            var path = this.mapService.GetPath(nodeIdsReceived);

            return path;
        }

        [HttpPost]
        public CoordinatesModel ApproximatePoint([FromBody]CoordinatesModel coordsRcvd)
        {
            return this.mapService.ApproximatePoint(coordsRcvd);
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
    }
}
