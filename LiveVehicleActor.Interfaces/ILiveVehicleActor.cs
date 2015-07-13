using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;

using LiveV.CommonTypes;

namespace LiveVehicleActor.Interfaces
{
    public interface ILiveVehicleActor : IActor
    {
        //[Readonly]
        //Task<int> GetVehicleIdAsync();

        //Task SetVehicleIdAsync(int count);

        [Readonly]
        Task<LiveVehicle> GetCurrentVehicleLiveDataAsync();

        Task SetVehicleLiveDataAsync(LiveVehicle vehicleData);

        Task UpdateCoordinatesAsync(GPSCoordinates coordinates);

        Task<string> PingAsync();

    }
}
