using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindTheWay.Data.DbModels
{
    public class Node
    {
        [Key]
        [Index]
        public long Id { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }
    }
}
