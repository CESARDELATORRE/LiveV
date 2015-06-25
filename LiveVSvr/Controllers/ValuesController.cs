using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;

using LiveVStateful.Interfaces;
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

            var vehicleProxy = ActorProxy.Create<ILiveVStateful>(ActorId.NewId(), "fabric:/LiveVStatefulApp");

            int count = 25;
            //Output TBD --> ("Setting Count to in Actor {0}: {1}", vehicleProxy.GetActorId(), count);
            vehicleProxy.SetCountAsync(count).Wait();

            string resultDone = "Count from Actor " + vehicleProxy.GetActorId().ToString() + " - " + vehicleProxy.GetCountAsync().Result.ToString();

            return new string[] { "Vehicles Coordinates", "Super Cool!!!!!!!!", resultDone };

        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
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
