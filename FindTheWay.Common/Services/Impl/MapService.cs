using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FindTheWay.Data.DbModels;
using FindTheWay.Data;
using FindTheWay.Common.Models;

namespace FindTheWay.Common.Services.Impl
{
    public class MapService : IMapService
    {
        public MapService(FindTheWayDbContext db)
        {
            this.db = db;
        }

        public IList<Node> GetPath(List<long> nodeIds)
        {
            var nodes = new List<Node>();
            if (nodeIds.Count < 2)
            {
                return nodes;
            }
            long nodeStartId = nodeIds[0],
                nodeFinishId = nodeIds[1];
            Node start = this.db.Nodes.First(n => n.Id == nodeStartId),
                target = this.db.Nodes.First(n => n.Id == nodeFinishId);
            var closed = new List<Node>();
            var open = new List<NodeDistanceModel>();
            var g = new Dictionary<long, double>();
            var f = new Dictionary<long, double>();
            var parent = new Dictionary<long, long>();
            g.Add(start.Id, 0);
            open.Add(new NodeDistanceModel {
                Node = start,
                DistanceFromFinish = EucledianDistance(
                                    start.Longitude, start.Latitude,
                                    target.Longitude, target.Latitude)
            });

            f.Add(start.Id, 
                g[start.Id] + EucledianDistance(start.Longitude, start.Latitude, target.Longitude, target.Latitude)                );

            while (open.Any())
            {
                var minF = int.MaxValue;
                var minIndex = -1;
                for (var i = 0; i < open.Count; i++)
                {
                    if (!g.ContainsKey(open[i].Node.Id))
                    {
                        g.Add(open[i].Node.Id, Distance(start.Id, open[i].Node.Id));
                    }

                    var nextF = Convert.ToInt32((g[open[i].Node.Id] + open[i].DistanceFromFinish));

                    if (nextF < minF)
                    {
                        minF = nextF;
                        minIndex = i;
                    }

                }

                Node x = open[minIndex].Node;
                open.RemoveAt(minIndex);
                if (x.Id == target.Id)
                {
                    // found the target
                    break;
                }

                closed.Add(x);
                foreach (var e in this.db.Edges
                    .Where(e => e.NodeFromId == x.Id || e.NodeToId == x.Id)
                    .Include(e => e.NodeFrom)
                    .Include(e => e.NodeTo)
                    .ToList())
                {
                    var y = e.NodeToId == x.Id ? e.NodeFrom : e.NodeTo;
                    if (closed.IndexOf(y) != -1)
                    {
                        continue;
                    }

                    double tmp = g[x.Id] + e.Length;
                    var isBetter = false;
                    if (open.All(o => o.Node.Id != y.Id))
                    {
                        open.Add(new NodeDistanceModel
                        {
                            Node = y,
                            DistanceFromFinish = EucledianDistance(
                                    y.Longitude, y.Latitude, 
                                    target.Longitude, target.Latitude)
                        });

                        isBetter = true;
                    }
                    else
                    {
                        isBetter = tmp < g[y.Id];
                    }

                    if (isBetter)
                    {
                        parent[y.Id] = x.Id;
                        g[y.Id] = tmp;
                        tmp += EucledianDistance(
                            y.Longitude, y.Latitude,
                            target.Longitude, target.Latitude);
                        f[y.Id] = tmp;
                    }
                }
            }

            for (var ind = target.Id; parent.ContainsKey(ind); ind = parent[ind])
            {
                nodes.Add(this.db.Nodes.First(n => n.Id == ind));
            }

            return nodes;
        }

        public CoordinatesModel ApproximatePoint(CoordinatesModel coords)
        {
            var newCoords = new CoordinatesModel
            {
                Latitude = coords.Latitude,
                Longitude = coords.Longitude
            };

            var minDistance = double.MaxValue;
            var distance = 0.0;
            foreach (var n in this.db.Nodes)
            {
                distance = EucledianDistance(n.Latitude, n.Longitude, coords.Latitude, coords.Longitude);

                if (distance < minDistance)
                {
                    newCoords.NodeId = n.Id;
                    newCoords.Latitude = n.Latitude;
                    newCoords.Longitude = n.Longitude;
                    minDistance = distance;
                }
            }

            return newCoords;
        }

        private double EucledianDistance(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt(Math.Pow(y2 - y1, 2) + Math.Pow(x2 - x1, 2));
        }

        private double Distance(long nodeIdFrom, long nodeIdTo)
        {
            var edge = this.db.Edges
                .FirstOrDefault(e =>
                    (e.NodeFromId == nodeIdFrom && e.NodeToId == nodeIdTo) ||
                    (e.NodeFromId == nodeIdTo && e.NodeToId == nodeIdFrom));

            return edge != null ? edge.Length : double.MaxValue;
        }



        private readonly FindTheWayDbContext db;
    }
}
