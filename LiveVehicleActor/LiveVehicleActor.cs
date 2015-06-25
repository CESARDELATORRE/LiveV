using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LiveVehicleActor.Interfaces;
using Microsoft.ServiceFabric;
using Microsoft.ServiceFabric.Actors;

namespace LiveVehicleActor
{
    public class LiveVehicleActor : Actor<LiveVehicleActorState>, ILiveVehicleActor
    {
        public override Task OnActivateAsync()
        {
            if (this.State == null)
            {
                this.State = new LiveVehicleActorState() {  VehicleId = 0, Latitude = 0, Longitude = 0 };
            }

            ActorEventSource.Current.ActorMessage(this, "State initialized to {0}", this.State);
            return Task.FromResult(true);
        }

        public Task<int> GetVehicleIdAsync()
        {
            ActorEventSource.Current.ActorMessage(this, "Getting current VehicleId as {0}", this.State.VehicleId);
            return Task.FromResult(this.State.VehicleId);
        }

        public Task SetVehicleIdAsync(int vehicleId)
        {
            ActorEventSource.Current.ActorMessage(this, "Setting current VehicleId to {0}", vehicleId);
            this.State.VehicleId = vehicleId;

            return Task.FromResult(true);
        }
    }
}
