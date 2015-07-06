using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;

using LiveVehicleActor.Interfaces;
using VehiclesLocatorActor.Interfaces;
using LiveV.CommonTypes;
using Microsoft.ServiceFabric.Actors;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace VApi.Controllers
{
    [Route("api/[controller]")]
    public class LiveVehicleController : Controller
    {
        // GET: api/LiveVehicle
        [HttpGet]
        public IEnumerable<string> Get()
        {
            //ActorId myActorId = ActorId.NewId();
            int vehicleId = 1;
            ActorId actorId = new ActorId(vehicleId);
            var vehicleProxy = ActorProxy.Create<ILiveVehicleActor>(actorId, "fabric:/LiveVStatefulApp");

            LiveVehicle tempVehicleData1 = new LiveVehicle();
            tempVehicleData1.VehicleId = vehicleId;
            //Seattle Pike's Place coordinates
            tempVehicleData1.GPSCoordinates.Latitude = 47.608875;
            tempVehicleData1.GPSCoordinates.Longitude = -122.340098;

            //Here we would find out the ZipCode related to the GPS coordinates.
            //For now, I set the ZipCode directly
            string currentZipCode = "98101";
            tempVehicleData1.CurrentZipCode = currentZipCode;

            //Output TBD --> ("Setting vehicleId {0} to Actor {1}", vehicleId, vehicleProxy.GetActorId() );
            vehicleProxy.SetVehicleLiveDataAsync(tempVehicleData1).Wait();

            string resultDone = "Actor " + vehicleProxy.GetActorId().ToString() + " is External-VehicleID: " + vehicleProxy.GetCurrentVehicleLiveDataAsync().Result.VehicleId.ToString();

            return new string[] { "TEST: ", resultDone };

            //return new string[] { "TEST: ", "TEST 2" };
        }

        //// GET /api/LiveVehicle/zipcode/98101/Vehicles
        [Route("zipcode/{zipCode}/Vehicles", Name = "GetVehiclesListInZipCodeArea")]
        public IEnumerable<int> GetVehiclesListInZipCodeArea(string zipCode)
        {            
            ActorId vehiclesLocatorQueryActorId = new ActorId(zipCode);
            var vehiclesLocatorQueryActor = ActorProxy.Create<IVehiclesLocatorActor>(vehiclesLocatorQueryActorId, "fabric:/LiveVStatefulApp");
            
            var vehiclesListIds = vehiclesLocatorQueryActor.GetVehicleIdListAsync().Result;

            return vehiclesListIds.AsEnumerable();

            //return new string[] { "Element 1", "Element 2" };
        }


        // GET api/LiveVehicle/1
        [HttpGet("{id}")]
        public string Get(int id)
        {
            ActorId actorId = new ActorId(id);

            var vehicleProxy = ActorProxy.Create<ILiveVehicleActor>(actorId, "fabric:/LiveVStatefulApp");

            string resultDone = "Actor " + vehicleProxy.GetActorId().ToString() + " is External-VehicleID: " + vehicleProxy.GetCurrentVehicleLiveDataAsync().Result.VehicleId.ToString();
            return "Vehicle's Data: " + resultDone;

            //return "TEST for ID";

        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/LiveVehicle/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/LiveVehicle/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
