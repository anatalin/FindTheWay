using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FindTheWay.Common.Models;
using FindTheWay.Common.Services;

namespace FindTheWay.Controllers
{
    public partial class DemoController : Controller
    {
        public virtual ActionResult BreadthFirstSearch()
        {
            return View();
        }
        public virtual ActionResult Dijkstra()
        {
            return View();
        }

        public virtual ActionResult AStar()
        {
            return View();
        }

        private readonly IMapService mapService;
    }
}