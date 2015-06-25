using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;

namespace LiveVStateful.Interfaces
{
    public interface ILiveVStateful : IActor
    {
        Task<int> GetCountAsync();

        Task SetCountAsync(int count);
    }
}
