using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FindTheWay.Data.DbModels;

namespace FindTheWay.Common.Models
{
    public class NodeDistanceModel
    {
        public Node Node { get; set; }

        public double DistanceFromFinish { get; set; }
    }
}
