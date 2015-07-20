using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LiveV.CommonTypes;

namespace MyShuttle.Device.Model
{
    public class GPSCoordinatesEvent
    {
        public string VehicleId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        
    }
}
