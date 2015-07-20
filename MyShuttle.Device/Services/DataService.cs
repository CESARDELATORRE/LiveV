using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using MyShuttle.Device.Enums;

using LiveV.CommonTypes;
using MyShuttle.Device.Model;
using System.IO;

namespace MyShuttle.Device.Services
{
    public static class DataService
    {
        private static readonly int NumDataRegisters = Properties.Settings.Default.NumDataRegisters;
        private static readonly int NumCrashDataRegisters = Properties.Settings.Default.CrashDataRegisters;


        private static string currentVehicleId;
        private static readonly List<GPSCoordinatesEvent> AllVehiclesMovementSimulationData = GenerateVehiclesMovementSimulationData().ToList();
        private static List<GPSCoordinatesEvent> currentVehicleMovementSimulationData;

        //        private static readonly List<AccelerometerEvent> GoodDriverData = GenerateGoodDriverData().ToList();
        //        private static readonly List<AccelerometerEvent> BadDriverData = GenerateBadDriverData().ToList();

        public static string VehicleId { get; set; }
        public static string DeviceId { get; set; }
        public static int DriverId { get; set; }

        public static void  GenerateGPSDataForSpecificVehicle(string vehicleIDtoUse)
        {
            if(currentVehicleMovementSimulationData != null)
                currentVehicleMovementSimulationData.Clear();

            var currentDataForVehicle = from vehicleCoord in AllVehiclesMovementSimulationData
                                        where (vehicleCoord.VehicleId) == vehicleIDtoUse                                                   
                                        select vehicleCoord;

            currentVehicleMovementSimulationData = currentDataForVehicle.ToList<GPSCoordinatesEvent>();
        }

        //(CDLTLL)
        public static GPSCoordinatesEvent GetGPSDrivingData(int index, string vehicleIDtoUse)
        {
            if (currentVehicleId == null)
                currentVehicleId = vehicleIDtoUse;

            if (currentVehicleMovementSimulationData == null)
                GenerateGPSDataForSpecificVehicle(vehicleIDtoUse);

            

            if (vehicleIDtoUse != currentVehicleId)  //Reset current data to use
            {
                currentVehicleId = vehicleIDtoUse;
                GenerateGPSDataForSpecificVehicle(vehicleIDtoUse);
            }
                 
            var gpsCoordEvent = currentVehicleMovementSimulationData[index];            
            return gpsCoordEvent;
        }

        private static IEnumerable<GPSCoordinatesEvent> GenerateVehiclesMovementSimulationData()
        {
            return LoadGPSDataFromCSV(@".\Resources\VehiclesMovementGPSCoordinates.csv");
        }

        private static IEnumerable<GPSCoordinatesEvent> LoadGPSDataFromCSV(string fileName)
        {
            return from line in File.ReadAllLines(fileName).Skip(1)
                   let columns = line.Split(',')                   
                   select new GPSCoordinatesEvent
                   {                       
                       VehicleId = columns[0],
                       Latitude = Double.Parse(columns[1], CultureInfo.InvariantCulture),
                       Longitude = Double.Parse(columns[2], CultureInfo.InvariantCulture),                       
                   };
        }

        //public static CompassEvent GetCompassData()
        //{
        //    var rand = new Random();
        //    var heading = rand.Next(0, 180);

        //    return new CompassEvent
        //    {
        //        DeviceId = DeviceId,
        //        DriverId = DriverId,
        //        EventDateTime = DateTime.UtcNow.ToString(CultureInfo.InvariantCulture),
        //        HeadingDegrees = heading
        //    };
        //}

        //        public static AccelerometerEvent GetDrivingData(int index, SessionStatus status)
        //        {
        //            var acecelEven = (status == SessionStatus.GoodDriver) ? GoodDriverData[index] : BadDriverData[index];
        //            acecelEven.DeviceId = DeviceId;
        //            acecelEven.DriverId = DriverId;
        //            return acecelEven;
        //        }

        //        public static AccelerometerEvent GetCrashData(int index)
        //        {
        //            return BadDriverData[index];
        //        }

        //        public static OBDEvent GetOBDData(string code)
        //        {
        //            return new OBDEvent
        //            {
        //                DriverId = DriverId,
        //                DeviceId = DeviceId,
        //                Code = code
        //            };
        //        }

        //        private static IEnumerable<AccelerometerEvent> GenerateGoodDriverData()
        //        {
        //            return LoadDataFromCSV(@".\Resources\GoodDriver.csv");
        //        }

        //        private static IEnumerable<AccelerometerEvent> GenerateBadDriverData()
        //        {
        //            return LoadDataFromCSV(@".\Resources\BadDriver.csv");
        //        }



    }
}

