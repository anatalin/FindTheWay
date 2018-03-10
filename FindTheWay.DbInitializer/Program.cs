using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace FindTheWay.DbInitializer
{
    class Program
    {
		private static HashSet<long> nodeIds = new HashSet<long>();
		private static object lockObj = new object();
		private static object lockObj2 = new object();

		static double EuclidDistance(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
        }

        static string CreateNodeSqlRow(string Id, string Latitude, string Longitude)
        {
            return $"INSERT [dbo].[Nodes] ([Id], [Latitude], [Longitude]) VALUES({Id}, {Latitude}, {Longitude})";
        }

        static string CreateEdgeSqlRow(string nodeFromId, string nodeToId, double length)
        {
			return $"INSERT [dbo].[Edges] ([NodeFromId], [NodeToId], [Length]) VALUES({nodeFromId}, {nodeToId}, {length.ToString(CultureInfo.InvariantCulture)})";
        }

        static void Main(string[] args)
        {
           //for node ids to be unique
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
			var waysCount = ways.Count;
			var waysDone = 0;
			var sb = new StringBuilder();

			Parallel.For(0, waysCount, i =>
			{
				var nodesSql = CreateNodesSql(ways[i], xmlDoc);
				lock (lockObj2) 
				{
					sw.WriteLine(nodesSql);
				}

				Interlocked.Increment(ref waysDone);
				Console.WriteLine($"[{stopwatch.ElapsedMilliseconds} ms.] Adding nodes, ways done {waysDone} out of {waysCount}");
			});

			waysDone = 0;
			Parallel.For(0, waysCount, i =>
			{
				var nodesSql = CreateWaySql(ways[i], xmlDoc);
				lock (lockObj2)
				{
					sw.WriteLine(nodesSql);
				}

				Interlocked.Increment(ref waysDone);
				Console.WriteLine($"[{stopwatch.ElapsedMilliseconds} ms.] Adding ways, done {waysDone} out of {waysCount}");
			});

			sw.WriteLine("SET IDENTITY_INSERT [dbo].[Nodes] OFF ");
            sw.Close();

            stopwatch.Stop();
            Console.WriteLine("The time was " + TimeSpan.FromMilliseconds(stopwatch.ElapsedMilliseconds).TotalSeconds);
            Console.ReadLine();
        }

		// creates SQL to add nodes from way
		private static string CreateNodesSql(XmlNode way, XmlDocument xmlDoc)
		{
			var nodes = way.SelectNodes("nd");
			var node1 = xmlDoc.SelectSingleNode("/osm/node[@id='" + nodes[0].Attributes["ref"].Value + "']");
			var sb = new StringBuilder();

			lock (lockObj)
			{
				if (nodeIds.Add(long.Parse(node1.Attributes["id"].Value)))
				{
					sb.AppendLine(CreateNodeSqlRow(
						node1.Attributes["id"].Value,
						node1.Attributes["lat"].Value,
						node1.Attributes["lon"].Value));
				}
			}

			for (var i = 1; i < nodes.Count; i++)
			{
				var node2 = xmlDoc.SelectSingleNode("/osm/node[@id='" + nodes[i].Attributes["ref"].Value + "']");
				var node1Id = node1.Attributes["id"].Value;
				var node2Id = node2.Attributes["id"].Value;

				lock (lockObj)
				{
					if (nodeIds.Add(long.Parse(node2.Attributes["id"].Value)))
					{
						sb.AppendLine(CreateNodeSqlRow(
							node2Id,
							node2.Attributes["lat"].Value,
							node2.Attributes["lon"].Value));
					}
				}

				node1 = xmlDoc.SelectSingleNode("/osm/node[@id='" + nodes[i].Attributes["ref"].Value + "']");
			}

			return sb.ToString();
		}

		private static string CreateWaySql(XmlNode way, XmlDocument xmlDoc)
		{
			var nodes = way.SelectNodes("nd");
			var node1 = xmlDoc.SelectSingleNode("/osm/node[@id='" + nodes[0].Attributes["ref"].Value + "']");
			var sb = new StringBuilder();

			for (var i = 1; i < nodes.Count; i++)
			{
				var node2 = xmlDoc.SelectSingleNode("/osm/node[@id='" + nodes[i].Attributes["ref"].Value + "']");
				var node1Id = node1.Attributes["id"].Value;
				var node2Id = node2.Attributes["id"].Value;

				sb.AppendLine(CreateEdgeSqlRow(
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

			return sb.ToString();
		}
    }
}
