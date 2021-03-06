﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LiveVehicleActor.Interfaces;
using Microsoft.ServiceFabric;
using Microsoft.ServiceFabric.Actors;

using LiveV.CommonTypes;

namespace LiveVehicleActor
{
    public class LiveVehicleActor : Actor<LiveVehicleActorState>, ILiveVehicleActor
    {
        public override Task OnActivateAsync()
        {
            if (this.State == null)
            {
                this.State = new LiveVehicleActorState();
            }

            ActorEventSource.Current.ActorMessage(this, "State initialized to {0}", this.State);
            return Task.FromResult(true);
        }
        
        public Task<LiveVehicle> GetCurrentVehicleLiveDataAsync()
        {
            //ActorEventSource.Current.ActorMessage(this, "Returning live info for VehicleId as {0}", this.State.);
            return Task.FromResult(this.State.CurrentVehicleLiveData);
        }

        public Task SetVehicleLiveDataAsync(LiveVehicle vehicleData)
        {
            ActorEventSource.Current.ActorMessage(this, "Setting current live data to VehicleId to {0}", vehicleData.VehicleId);
            this.State.CurrentVehicleLiveData = vehicleData;

            return Task.FromResult(true);
        }
        
        public Task UpdateCoordinatesAsync(GPSCoordinates coordinates)
        {
            ActorEventSource.Current.ActorMessage(this, "Updating GPS coordiantes to VehicleId to {0}", this.State.CurrentVehicleLiveData.VehicleId);
            this.State.CurrentVehicleLiveData.GPSCoordinates = coordinates;

            return Task.FromResult(true);
        }

        public Task<string> PingAsync()
        {
            return Task.FromResult(this.State.CurrentVehicleLiveData.VehicleId == 0 ? String.Format("Vehicle '{0}' looks like has not been initialized .", this.State.CurrentVehicleLiveData.VehicleId)
                : String.Format("Hello MyShutte! ({0}) - Connected to VehicleActor '{1}' on Cluster Node {2}..", DateTime.Now, this.State.CurrentVehicleLiveData.VehicleId, System.Fabric.FabricRuntime.GetNodeContext().NodeName));
        }

        //public Task<int> GetVehicleIdAsync()
        //{
        //    ActorEventSource.Current.ActorMessage(this, "Getting current VehicleId as {0}", this.State.VehicleId);
        //    return Task.FromResult(this.State.VehicleId);
        //}

        //public Task SetVehicleIdAsync(int vehicleId)
        //{
        //    ActorEventSource.Current.ActorMessage(this, "Setting current VehicleId to {0}", vehicleId);
        //    this.State.VehicleId = vehicleId;

        //    return Task.FromResult(true);
        //}
    }
}
