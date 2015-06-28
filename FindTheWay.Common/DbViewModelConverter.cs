using FindTheWay.Common.Models;
using FindTheWay.Data.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindTheWay.Common
{
    public static class DbViewModelConverter
    {
        public static CoordinatesModel Convert(Node from)
        {
            return new CoordinatesModel
            {
                Latitude = from.Latitude,
                Longitude = from.Longitude
            };
        }
    }
}
