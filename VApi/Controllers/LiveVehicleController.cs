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
        public LiveVehicle Get(int id)
        {
            ActorId vehicleActorId = new ActorId(id);

            var vehicleProxy = ActorProxy.Create<ILiveVehicleActor>(vehicleActorId, "fabric:/LiveVStatefulApp");

            LiveVehicle retVehicle = vehicleProxy.GetCurrentVehicleLiveDataAsync().Result;            
            return retVehicle;           
        }

        // POST api/LiveVehicle
        [HttpPost]
        public IActionResult Post([FromBody]LiveVehicle vehicle)
        {
            //Add vehicle as new vehicle Actor and add it to the VehicleLocator collection
            //Check first that it doesn't exist!

            //Here we would find out the ZipCode related to the GPS coordinates provided by the vehicle data.
            //For now, I set the ZipCode directly
            vehicle.CurrentZipCode = "98101";

            //Add (if not exists to List of vehicles per ZipCode in VehiclesLocatorActor Service)

            //Create a VehicleLocator proxy to check if the vehicle already exists in our system
            //I use ZipCode as ID for the VehiclesList locator actor ID service            
            ActorId vehiclesLocatorActorId = new ActorId(vehicle.CurrentZipCode);
            var vehicleLocatorProxy = ActorProxy.Create<IVehiclesLocatorActor>(vehiclesLocatorActorId, "fabric:/LiveVStatefulApp");

            bool vehicleExist = false;
            vehicleExist = vehicleLocatorProxy.IsVehicleInZipCodeAreaAsync(vehicle.VehicleId).Result;

            string detailedResponse;

            if (!vehicleExist)
            {
                //Add vehicle to ZipCode Area Actor Manager
                vehicleLocatorProxy.AddVehicleToZipAreaAsync(vehicle.VehicleId);

                //Create and update the vehicle actor just in case it didn't exist
                ActorId vehicleActorId = new ActorId(vehicle.VehicleId);
                var vehicleProxy = ActorProxy.Create<ILiveVehicleActor>(vehicleActorId, "fabric:/LiveVStatefulApp");

                //Add/Update data to Vehicle Actor
                vehicleProxy.SetVehicleLiveDataAsync(vehicle).Wait();

                detailedResponse = "Vehicle ID " + vehicle.VehicleId.ToString() + "added to ZipCode area " + vehicle.CurrentZipCode;
            }
            else
                detailedResponse = "Vehicle already in ZipCode area. Nothing is added";

            // Get the response.  
            //(The response could be improved by adding the Partition where the Actor has been created, etc.) 
            // --> (CDLTLL) --> System.Fabric.FabricRuntime.GetNodeContext().NodeName

            //string message = "Vehicle added to MyShuttle Live IoT system";
            //String.Format("{0} added to partition {1} at {2}", seatMap.Name, client.ResolvedServicePartition.Info.Id, serviceAddress),
            //Encoding.UTF8,
            //"text/html");

            return new HttpStatusCodeResult(201); //201 Created              
        }


        // PUT api/LiveVehicle/3/GPSCoordinates                
        [HttpPut("{vehicleId}/GPSCoordinates", Name = "UpdateVehicleGPSCoordinates")]
        //[Route("{vehicleId}/GPSCoordinates", Name = "UpdateVehicleGPSCoordinates")]
        public IActionResult UpdateVehicleGPSCoordinates(int vehicleId, [FromBody]GPSCoordinates coordinates)
        {       
            //Create and update the vehicle actor just in case it didn't exist
            ActorId vehicleActorId = new ActorId(vehicleId);
            var vehicleProxy = ActorProxy.Create<ILiveVehicleActor>(vehicleActorId, "fabric:/LiveVStatefulApp");

            //Add/Update data to Vehicle Actor
            vehicleProxy.UpdateCoordinatesAsync(coordinates).Wait();

            return new HttpStatusCodeResult(200); //200 OK              
            
        }

        //[HttpPost]
        //public async Task<int> Post([FromBody]Vehicle vehicle)
        //{
        //    vehicle.CarrierId = await _securityContext.GetCarrierId(User.Identity);
        //    return await _vehicleRepository.AddAsync(vehicle);
        //}

        //[HttpPut]
        //public async Task Put([FromBody]Vehicle vehicle)
        //{
        //    vehicle.CarrierId = await _securityContext.GetCarrierId(User.Identity);
        //    await _vehicleRepository.UpdateAsync(vehicle);
        //}

        //// PUT api/LiveVehicle/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody]string value)
        //{
        //}

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

        // GET: api/LiveVehicle/1/GPSCoordinates
        [HttpGet]
        [Route("{id}/GPSCoordinates", Name = "GetVehicleGPSCoordinates")]
        public GPSCoordinates GetVehicleGPSCoordinates(int id)
        {
            LiveVehicle vehicle = this.Get(id);

            //ActorId vehicleActorId = new ActorId(id);
            //var vehicleProxy = ActorProxy.Create<ILiveVehicleActor>(vehicleActorId, "fabric:/LiveVStatefulApp");
            //LiveVehicle vehicle = vehicleProxy.GetCurrentVehicleLiveDataAsync().Result;

            return vehicle.GPSCoordinates;
        }

        //// POST api/values
        //[HttpPost]
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT api/LiveVehicle/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/LiveVehicle/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
