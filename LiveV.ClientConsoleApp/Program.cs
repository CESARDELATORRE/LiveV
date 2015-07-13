using System;
using System.Text;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

//
using System.IO;
using System.Net;
using Newtonsoft.Json;
using LiveV.CommonTypes;

namespace LiveV.ClientConsoleApp
{
    class Program
    {
        private const string SeatMapGatewayServiceUrl = "http://localhost:8080/api/";
        private static IDictionary<string, MenuItem> _menuItems = new Dictionary<string, MenuItem>();

        static void Main(string[] args)
        {
            AddMenuItem("Ping ASP.NET 5 service in Service Fabric Cluster", Ping);
            AddMenuItem("Add new vehicles", AddVehicles);
            AddMenuItem("Update vehicles' coordinates Forward", UpdateCoordinatesForward);
            AddMenuItem("Update vehicles' coordinates Backward", UpdateCoordinatesBackward);
            AddMenuItem("Quit", "q");

            var selection = String.Empty;

            Header("MyShuttle Live IoT Vehicles Test Console v1.0");
            DisplayMenu();
            selection = GetSelection();

            do
            {
                switch (selection)
                {
                    case "q":
                        Console.WriteLine("Good bye");
                        return;
                    case "m":
                        DisplayMenu();
                        selection = GetSelection();
                        break;
                    default:
                        if (_menuItems.ContainsKey(selection))
                            _menuItems[selection].Run();
                        else
                            Console.WriteLine("Invalid selection");

                        selection = GetSelection();
                        break;
                }

            } while (true);        

        }

        static void Ping()
        {
            var vehicleId = PromptVehicleId();
            Console.WriteLine("Pinging MyShuttle Live IoT Front-End...");
            Console.WriteLine("\nResponse: {0}", PingVehicleActorThruGateway(vehicleId));
        }

        static string PingVehicleActorThruGateway(string vehicleId)
        {
            //TBD query services

            //return VEHICLE DATA;
            return "TBD";
        }

        static void AddVehicles()
        {
            Console.WriteLine("\nCreating and adding vehicles to the Live IoT system");

            string defaultCurrentZipCode = "98101";

            //ADD VEHICLE 1 /////////////////////////////////////////////
            int vehicleId1 = 1;
            LiveVehicle tempVehicleData1 = new LiveVehicle();
            tempVehicleData1.VehicleId = vehicleId1;
            //Seattle Pike's Place coordinates
            tempVehicleData1.GPSCoordinates.Latitude = 47.608875;
            tempVehicleData1.GPSCoordinates.Longitude = -122.340098;

            //Here we would find out the ZipCode related to the GPS coordinates.
            //For now, I set the ZipCode directly            
            tempVehicleData1.CurrentZipCode = defaultCurrentZipCode;

            //Call Http Service to add vehicle
            AddLiveVehicle(tempVehicleData1);
            /////////////////////////////////////////////////////////////////////////

            //ADD VEHICLE 2 /////////////////////////////////////////////
            int vehicleId2 = 2;
            LiveVehicle tempVehicleData2 = new LiveVehicle();
            tempVehicleData2.VehicleId = vehicleId2;
            //Seattle STARBUCKS ORIGINAL coordinates 47.610021, -122.342649
            tempVehicleData2.GPSCoordinates.Latitude = 47.610021;
            tempVehicleData2.GPSCoordinates.Longitude = -122.342649;

            //Here we would find out the ZipCode related to the GPS coordinates.
            //For now, I set the ZipCode directly            
            tempVehicleData2.CurrentZipCode = defaultCurrentZipCode;

            //Call Http Service to add vehicle
            AddLiveVehicle(tempVehicleData2);
            /////////////////////////////////////////////////////////////////////////

            //ADD VEHICLE 3 /////////////////////////////////////////////
            int vehicleId3 = 3;
            LiveVehicle tempVehicleData3 = new LiveVehicle();
            tempVehicleData3.VehicleId = vehicleId3;
            //Seattle CONVENTION CENTER coordinates 47.612283, -122.331918
            tempVehicleData3.GPSCoordinates.Latitude = 47.612283;
            tempVehicleData3.GPSCoordinates.Longitude = -122.331918;

            //Here we would find out the ZipCode related to the GPS coordinates.
            //For now, I set the ZipCode directly            
            tempVehicleData3.CurrentZipCode = defaultCurrentZipCode;

            //Call Http Service to add vehicle
            AddLiveVehicle(tempVehicleData3);
            /////////////////////////////////////////////////////////////////////////

            Console.WriteLine("##### END OF ADDING VEHICLES PROCESS #####");
            Console.WriteLine("##########################################");

        }

        private static void AddLiveVehicle(LiveVehicle vehicle)
        {
            Uri serviceAddress = new Uri(SeatMapGatewayServiceUrl + "LiveVehicle");
            HttpWebRequest req = WebRequest.CreateHttp(serviceAddress);

            string data = JsonConvert.SerializeObject(vehicle);
            //This gives you the byte array.

            var dataToSend = Encoding.UTF8.GetBytes(data);

            req.ContentType = "application/json";
            req.ContentLength = dataToSend.Length;
            req.Method = "POST";

            req.GetRequestStream().Write(dataToSend, 0, dataToSend.Length);

            var response = req.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.
            string responseFromServer = reader.ReadToEnd();
            //Console.WriteLine("********************************");            
            Console.WriteLine(responseFromServer);
            //Console.WriteLine(" ");
        }
        
        static void UpdateCoordinatesForward()
        {
            Console.WriteLine("\nUpdating vehicles' coordinates forward");

            //UPDATE COORDINATES VEHICLE 1 //////////////////////////////////////////
            int vehicleId1 = 1;
            GPSCoordinates coordinates1 = new GPSCoordinates();
            //Seattle Pike's Place --> move to North to 2nd Av. --> 47.609321, -122.339041
            coordinates1.Latitude = 47.609321;
            coordinates1.Longitude = -122.339041;
            //Call Http Service to update GPS coordinates
            UpdateGPSCoordinates(vehicleId1, coordinates1);
            /////////////////////////////////////////////////////////////////////////

            //UPDATE COORDINATES VEHICLE 2 //////////////////////////////////////////
            int vehicleId2 = 2;
            GPSCoordinates coordinates2 = new GPSCoordinates();
            //Starbucks 1st --> move to Virginia St. --> 47.610515, -122.343569
            coordinates2.Latitude = 47.610515;
            coordinates2.Longitude = -122.343569;
            //Call Http Service to update GPS coordinates
            UpdateGPSCoordinates(vehicleId2, coordinates2);
            /////////////////////////////////////////////////////////////////////////

            //UPDATE COORDINATES VEHICLE 3 //////////////////////////////////////////
            int vehicleId3 = 3;
            GPSCoordinates coordinates3 = new GPSCoordinates();
            //Seattle Convention Center --> move to 7th Av. --> 47.611636, -122.333409
            coordinates3.Latitude = 47.611636;
            coordinates3.Longitude = -122.333409;
            //Call Http Service to update GPS coordinates
            UpdateGPSCoordinates(vehicleId3, coordinates3);
            /////////////////////////////////////////////////////////////////////////

            Console.WriteLine("##### END OF COORDINATES FORWARD UPDATE #####");
            Console.WriteLine("#############################################");

        }

        static void UpdateCoordinatesBackward()
        {
            Console.WriteLine("\nUpdating vehicles' coordinates backward");

            //UPDATE COORDINATES VEHICLE 1 //////////////////////////////////////////
            int vehicleId1 = 1;
            GPSCoordinates coordinates1 = new GPSCoordinates();
            //Seattle Pike's Place --> --> 47.608875, -122.340098
            coordinates1.Latitude = 47.608875;
            coordinates1.Longitude = -122.340098;
            //Call Http Service to update GPS coordinates
            UpdateGPSCoordinates(vehicleId1, coordinates1);
            /////////////////////////////////////////////////////////////////////////

            //UPDATE COORDINATES VEHICLE 2 //////////////////////////////////////////
            int vehicleId2 = 2;
            GPSCoordinates coordinates2 = new GPSCoordinates();
            //Seattle STARBUCKS ORIGINAL coordinates 47.610021, -122.342649
            coordinates2.Latitude = 47.610021;
            coordinates2.Longitude = -122.342649;
            //Call Http Service to update GPS coordinates
            UpdateGPSCoordinates(vehicleId2, coordinates2);
            /////////////////////////////////////////////////////////////////////////

            //UPDATE COORDINATES VEHICLE 3 //////////////////////////////////////////
            int vehicleId3 = 3;
            GPSCoordinates coordinates3 = new GPSCoordinates();
            //Seattle CONVENTION CENTER coordinates 47.612283, -122.331918
            coordinates3.Latitude = 47.612283;
            coordinates3.Longitude = -122.331918;
            //Call Http Service to update GPS coordinates
            UpdateGPSCoordinates(vehicleId3, coordinates3);
            /////////////////////////////////////////////////////////////////////////

            Console.WriteLine("##### END OF COORDINATES FORWARD UPDATE #####");
            Console.WriteLine("#############################################");

        }

        // PUT api/LiveVehicle/3/GPSCoordinates        
        // HttpPut "{vehicleId}/GPSCoordinates"
        private static void UpdateGPSCoordinates(int vehicleID, GPSCoordinates coordinates)
        {
            Uri serviceAddress = new Uri(SeatMapGatewayServiceUrl + "LiveVehicle/" + vehicleID.ToString() + "/GPSCoordinates");
            HttpWebRequest req = WebRequest.CreateHttp(serviceAddress);

            string data = JsonConvert.SerializeObject(coordinates);
            //This gives you the byte array.

            var dataToSend = Encoding.UTF8.GetBytes(data);

            req.ContentType = "application/json";
            req.ContentLength = dataToSend.Length;
            req.Method = "PUT";

            req.GetRequestStream().Write(dataToSend, 0, dataToSend.Length);

            var response = req.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.
            string responseFromServer = reader.ReadToEnd();
            //Console.WriteLine("********************************");            
            Console.WriteLine(responseFromServer);
            //Console.WriteLine(" ");
        }

        static string PromptVehicleId()
        {
            return Prompt("Enter vehicle Id number");
        }

        static string Prompt(string message)
        {
            Console.Write(String.Format("\n{0}: ", message));
            return Console.ReadLine();
        }

        static void DisplayMenu()
        {
            Console.WriteLine();
            foreach (var item in _menuItems.Values)
                Console.WriteLine("   {0} - {1}", item.Id, item.Display);
        }

        static void AddMenuItem(string display, Action action)
        {
            var id = (_menuItems.Count + 1).ToString();
            _menuItems.Add(id, new MenuItem(id, display, action));
        }

        static void AddMenuItem(string display, string id)
        {
            _menuItems.Add(id, new MenuItem(id, display));
        }

        static string GetSelection(string selection = null)
        {
            Console.Write("\n\nEnter your selection (m for menu): ");
            if (String.IsNullOrEmpty(selection))
                return Console.ReadLine().Trim().ToLower();

            Console.WriteLine(selection);

            return selection;
        }

        static void Header(string header)
        {
            Console.Clear();
            Console.WriteLine("------------------------------------------------");
            Console.WriteLine(header);
            Console.WriteLine("------------------------------------------------\n");
        }


    }


    class MenuItem
    {
        public string Id { get; private set; }

        public string Display { get; private set; }

        private Action _action;

        public MenuItem(string id, string display, Action action = null)
        {
            Id = id;
            Display = display;
            _action = action;
        }

        public void Run()
        {
            if (_action != null)
                _action.Invoke();
        }
    }

}
