using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace FindTheWay.DbInitializer
{
    class Program
    {
        static double EuclidDistance(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
        }

        static string CreateNodeSqlRow(string Id, string Latitude, string Longitude)
        {
            return string.Format(
                "INSERT [dbo].[Nodes] ([Id], [Latitude], [Longitude]) VALUES({0}, {1}, {2})",
                Id, Latitude, Longitude);
        }

        static string CreateEdgeSqlRow(string nodeFromId, string nodeToId, double length)
        {
            return string.Format(
                "INSERT [dbo].[Edges] ([NodeFromId], [NodeToId], [Length]) VALUES({0}, {1}, {2})",
                nodeFromId, nodeToId, length.ToString(CultureInfo.InvariantCulture));
        }

        static void Main(string[] args)
        {
           //for node ids to be unique
           HashSet <long> nodeIds = new HashSet<long>();
            var sw = new StreamWriter("map.sql");
            sw.WriteLine("USE [FindTheWayDb]");
            sw.WriteLine("GO");
            sw.WriteLine("SET IDENTITY_INSERT [dbo].[Nodes] ON ");

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("map.osm");

            // selecting all <way> with <tag k="highway"> in it
            // which means that <way> is part of a street
            var ways = xmlDoc.SelectNodes("/osm/way[tag[@k='highway']]");
            XmlNode node1, node2;

            foreach (XmlNode way in ways)
            {
                var nodes = way.SelectNodes("nd");
                node1 = xmlDoc.SelectSingleNode("/osm/node[@id='" + nodes[0].Attributes["ref"].Value + "']");
                if (nodeIds.Add(long.Parse(node1.Attributes["id"].Value)))
                {
                    sw.WriteLine(CreateNodeSqlRow(
                        node1.Attributes["id"].Value, 
                        node1.Attributes["lat"].Value, 
                        node1.Attributes["lon"].Value));
                }

                for (var i = 1; i < nodes.Count; i++)
                {
                    node2 = xmlDoc.SelectSingleNode("/osm/node[@id='" + nodes[i].Attributes["ref"].Value + "']");
                    var node1Id = node1.Attributes["id"].Value;
                    var node2Id = node2.Attributes["id"].Value;

                    if (nodeIds.Add(long.Parse(node2.Attributes["id"].Value)))
                    {
                        sw.WriteLine(CreateNodeSqlRow(
                            node2Id,
                            node2.Attributes["lat"].Value,
                            node2.Attributes["lon"].Value));
                    }

                    sw.WriteLine(CreateEdgeSqlRow(
                        node1Id, 
                        node2Id,
                        EuclidDistance(
                            double.Parse(node1.Attributes["lon"].Value, CultureInfo.InvariantCulture),
                            double.Parse(node1.Attributes["lat"].Value, CultureInfo.InvariantCulture),
                            double.Parse(node2.Attributes["lon"].Value, CultureInfo.InvariantCulture),
                            double.Parse(node2.Attributes["lat"].Value, CultureInfo.InvariantCulture)
                        )));

                    node1 = xmlDoc.SelectSingleNode("/osm/node[@id='" + nodes[i].Attributes["ref"].Value + "']");
                }
            }

            sw.WriteLine("SET IDENTITY_INSERT [dbo].[Nodes] OFF ");
            sw.Close();

            stopwatch.Stop();
            Console.WriteLine("The time was " + TimeSpan.FromMilliseconds(stopwatch.ElapsedMilliseconds).TotalSeconds);
            Console.ReadLine();
        }


    }
}
