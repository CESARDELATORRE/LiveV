using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using VehiclesLocatorActor.Interfaces;
using Microsoft.ServiceFabric;
using Microsoft.ServiceFabric.Actors;

namespace VehiclesLocatorActor
{
    [DataContract]
    public class VehiclesLocatorActorState
    {
        public VehiclesLocatorActorState()
        {
            this.VehicleIdsList = new List<int>();
        }

        [DataMember]
        public List<int> VehicleIdsList { get; set; }

        [DataMember]
        public string ZipCode;
        
    }
}