using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindTheWay.Data.DbModels
{
    public class Edge
    {
        [Key]
        [Index]
        public long Id { get; set; }

        [Required, ForeignKey("NodeFromId")]
        public Node NodeFrom { get; set; }

        [Required, ForeignKey("NodeToId")]
        public Node NodeTo { get; set; }

        [Index]
        [Required]
        public long NodeFromId { get; set; }

        [Index]
        [Required]
        public long NodeToId { get; set; }


        public double Length { get; set; }
    }
}
