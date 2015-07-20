
namespace MyShuttle.Device.IoTSender
{
    //using Microsoft.ServiceBus;
    //using Microsoft.ServiceBus.Messaging;
    //using MyShuttle.Model;
    
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Diagnostics;
    using System.Linq;

    //
    using System.IO;
    using System.Net;
    using Newtonsoft.Json;
    using LiveV.CommonTypes;
    using MyShuttle.Device.Model;

    public static class Sender
    {
        private const string SeatMapGatewayServiceUrl = "http://localhost:8080/api/";

        //TBD to send to Azure Event Hubs
        //static readonly string EventHubName = Properties.Settings.Default.EventHubName;

        public static void SendEventDirectToServiceFabricMicroservice(GPSCoordinatesEvent gpsCoordVehicleEvent)
        {            
            GPSCoordinates coordinates = new GPSCoordinates();
            coordinates.Latitude = gpsCoordVehicleEvent.Latitude;
            coordinates.Longitude = gpsCoordVehicleEvent.Longitude;

            //Call Http Service to update GPS coordinates
            UpdateGPSCoordinates(gpsCoordVehicleEvent.VehicleId, coordinates);

        }

        // PUT api/LiveVehicle/3/GPSCoordinates        
        // HttpPut "{vehicleId}/GPSCoordinates"
        private static void UpdateGPSCoordinates(string vehicleID, GPSCoordinates coordinates)
        {
            Uri serviceAddress = new Uri(SeatMapGatewayServiceUrl + "LiveVehicle/" + vehicleID.ToString() + "/GPSCoordinates");
            HttpWebRequest req = WebRequest.CreateHttp(serviceAddress);

            string data = JsonConvert.SerializeObject(coordinates);
            //This gives you the byte array.

            var dataToSend = Encoding.UTF8.GetBytes(data);

            req.ContentType = "application/json";
            req.ContentLength = dataToSend.Length;
            req.Method = "PUT";

            //Send data
            req.GetRequestStream().Write(dataToSend, 0, dataToSend.Length);

            var response = req.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.
            string responseFromServer = reader.ReadToEnd();                                 
        }

        //Send to Azure Event Hubs --> TBD
        //public static async void SendEventToEventHubs(MetricEvent info)
        //{
        //    Create EventHubClient
        //    EventHubClient client = EventHubClient.CreateFromConnectionString(GetServiceBusConnectionString(), EventHubName);

        //    try
        //    {
        //        Trace.WriteLine(String.Format("Sending message to Event Hub {0}", client.Path));
        //        var serializedString = JsonConvert.SerializeObject(info);
        //        EventData data = new EventData(Encoding.UTF8.GetBytes(serializedString));

        //        Send the metric to Event Hub
        //        await client.SendAsync(data);

        //    }
        //    catch (Exception exp)
        //    {
        //        Trace.WriteLine("Error on send: " + exp.Message);
        //    }

        //    client.CloseAsync().Wait();
        //}

        //static void OutputMessageInfo(string action, EventData data, MetricEvent info)
        //{
        //    if (data == null)
        //    {
        //        return;
        //    }

        //    if (info != null)
        //    {
        //        Console.WriteLine("{0}{1} - Device {2}.", action, data, info.DeviceId);
        //    }
        //}

        //static string GetServiceBusConnectionString()
        //{
        //    string connectionString = ConfigurationManager.AppSettings["Microsoft.ServiceBus.ConnectionString"];
        //    if (string.IsNullOrEmpty(connectionString))
        //    {
        //        Console.WriteLine("Did not find Service Bus connections string in appsettings (app.config)");
        //        return string.Empty;
        //    }
        //    ServiceBusConnectionStringBuilder builder = new ServiceBusConnectionStringBuilder(connectionString);
        //    builder.TransportType = TransportType.Amqp;
        //    return builder.ToString();
        //}

    }
}
