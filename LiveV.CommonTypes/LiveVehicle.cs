

namespace LiveV.CommonTypes
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public class LiveVehicle
    {
        public LiveVehicle()
        {
            this.GPSCoordinates = new GPSCoordinates();
        }

        [DataMember]
        public int VehicleId { get; set; }
        [DataMember]
        public GPSCoordinates GPSCoordinates { get; set; }
        [DataMember]
        public string CurrentZipCode { get; set; }
        [DataMember]
        public string VehicleMake { get; set; }
        [DataMember]
        public string VehicleModel { get; set; }
        [DataMember]
        public string DriverId { get; set; }
        [DataMember]
        public string DriverName { get; set; }
        [DataMember]
        public string CarrierName { get; set; }
    }

    [DataContract]
    public class GPSCoordinates
    {
        [DataMember]
        public double Latitude { get; set; }

        [DataMember]
        public double Longitude { get; set; }                
    }
}


