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

using LiveV.CommonTypes;

namespace LiveVehicleActor
{
    [DataContract]
    public class LiveVehicleActorState
    {
        public LiveVehicleActorState()
        {
            this.CurrentVehicleLiveData = new LiveVehicle();
        }

        [DataMember]
        public LiveVehicle CurrentVehicleLiveData { get; set; }        

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, 
                                 "LiveVehicleActorState[VehicleId = {0}, Latitude = {1}, Longitude = {2}]",
                                                                 CurrentVehicleLiveData.VehicleId,
                                                                 CurrentVehicleLiveData.GPSCoordinates.Latitude,
                                                                 CurrentVehicleLiveData.GPSCoordinates.Longitude);
        }
    }
}