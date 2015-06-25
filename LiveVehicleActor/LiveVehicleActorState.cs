using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using LiveVehicleActor.Interfaces;
using Microsoft.ServiceFabric;
using Microsoft.ServiceFabric.Actors;

namespace LiveVehicleActor
{
    [DataContract]
    public class LiveVehicleActorState
    {        
        [DataMember]
        public int VehicleId { get; set; }
        [DataMember]
        public double Latitude { get; set; }
        [DataMember]
        public double Longitude { get; set; }

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, 
                                 "LiveVehicleActorState[VehicleId = {0}, Latitude = {1}, Longitude = {2}]", 
                                                                 VehicleId, Latitude, Longitude);
        }
    }
}