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

        Task<List<int>> GetVehicleIdListAsync();

        Task<bool> IsVehicleInZipCodeAreaAsync(int vehicleId);

        Task AddVehicleToZipAreaAsync(int vehicleId);

        Task RemoveVehicleFromZipAreaAsync(int vehicleId);
        
    }
}
