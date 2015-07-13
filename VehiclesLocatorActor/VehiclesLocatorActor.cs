using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VehiclesLocatorActor.Interfaces;
using Microsoft.ServiceFabric;
using Microsoft.ServiceFabric.Actors;

namespace VehiclesLocatorActor
{
    public class VehiclesLocatorActor : Actor<VehiclesLocatorActorState>, IVehiclesLocatorActor
    {
        public override Task OnActivateAsync()
        {
            if (this.State == null)
            {
                this.State = new VehiclesLocatorActorState() { ZipCode = "0" };
            }

            ActorEventSource.Current.ActorMessage(this, "State initialized to {0}", this.State);
            return Task.FromResult(true);
        }

        public Task<List<int>> GetVehicleIdListAsync()
        {
            return Task.FromResult(this.State.VehicleIdsList);

            // (CDLTLL) Do something else async after returning the response
            //return this.DoSomethingelseAsync().ContinueWith(t => this.State.VehicleIdsList);
        }

        public Task AddVehicleToZipAreaAsync(int vehicleId)
        {
            this.State.VehicleIdsList.Add(vehicleId);
            return Task.FromResult(true);
        }

        public Task<bool> IsVehicleInZipCodeAreaAsync(int vehicleId)
        {
            bool existsInArea = false;
            for (int i = 0; i < this.State.VehicleIdsList.Count; i++)
            {
                if (this.State.VehicleIdsList[i].Equals(vehicleId))
                {
                    existsInArea = true;
                    break;
                }
            }
            return Task.FromResult(existsInArea);
        }

        public Task RemoveVehicleFromZipAreaAsync(int vehicleId)
        {
            for (int i = 0; i < this.State.VehicleIdsList.Count; i++)
            {
                if (this.State.VehicleIdsList[i].Equals(vehicleId))
                {
                    this.State.VehicleIdsList.RemoveAt(i);
                    break;
                }
            }
            return Task.FromResult(true);
        }

    }
}
