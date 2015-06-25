using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;

using LiveVehicleActor.Interfaces;
using Microsoft.ServiceFabric.Actors;


namespace LiveVSvr.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            ActorId myActorId = ActorId.NewId();

            var vehicleProxy = ActorProxy.Create<ILiveVehicleActor>(ActorId.NewId(), "fabric:/LiveVStatefulApp");

            int vehicleId = 25;
            //Output TBD --> ("Setting vehicleId {0} to Actor {1}", vehicleId, vehicleProxy.GetActorId() );
            vehicleProxy.SetVehicleIdAsync(vehicleId).Wait();

            string resultDone = "Actor " + vehicleProxy.GetActorId().ToString() + " is External-VehicleID: " + vehicleProxy.GetVehicleIdAsync().Result.ToString();

            return new string[] { "TEST: ", resultDone };

        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            ActorId actorId = new ActorId(id);

            var vehicleProxy = ActorProxy.Create<ILiveVehicleActor>(actorId, "fabric:/LiveVStatefulApp");

            string resultDone = "Actor " + vehicleProxy.GetActorId().ToString() + " is External-VehicleID: " + vehicleProxy.GetVehicleIdAsync().Result.ToString();
            return "Vehicle's Data: " + resultDone;
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
