using FindTheWay.Common.Models;
using FindTheWay.Common.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FindTheWay.Controllers
{
    public partial class MapController : Controller
    {
        public MapController(IMapService mapService)
        {
            this.mapService = mapService;
        }

        [HttpPost]
        public virtual ActionResult GetPath()
        {
            var nodeIdsReceived = this.GetEntity<List<long>>();
            var path = this.mapService.GetPath(nodeIdsReceived);
            return Json(path, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public virtual ActionResult ApproximatePoint()
        {
            var coordsRcvd = this.GetEntity<CoordinatesModel>();
            return Json(this.mapService.ApproximatePoint(coordsRcvd));
        }

        // GET: Map
        public virtual ActionResult Index()
        {
            return View();
        }

        private TEntity GetEntity<TEntity>()
        {
            var json = this.GetContent();
            return (TEntity)JsonConvert.DeserializeObject(json, typeof(TEntity));
        }

        private string GetContent()
        {
            var resolveRequest = this.HttpContext.Request;
            resolveRequest.InputStream.Seek(0, SeekOrigin.Begin);
            return new StreamReader(resolveRequest.InputStream).ReadToEnd();
        }

        private readonly IMapService mapService;
    }
}