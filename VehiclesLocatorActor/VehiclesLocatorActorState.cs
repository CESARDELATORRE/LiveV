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
        [DataMember]
        public int Count;

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "VehiclesLocatorActorState[Count = {0}]", Count);
        }
    }
}