using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;

namespace VehiclesLocatorActor.Interfaces
{
    public interface IVehiclesLocatorActor : IActor
    {
        Task<int> GetCountAsync();

        Task SetCountAsync(int count);
    }
}
