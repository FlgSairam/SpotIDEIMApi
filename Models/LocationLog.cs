namespace DapperAuthApi.Models
{
    public class LocationLog
    {
        public long EmployeeFid { get; set; }
        public string? MobileNumber { get; set; }
        public long WorkLocationFid { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string? CapturedTime { get; set; }
        public string? DeviceId { get; set; }
        public int BatteryLevel { get; set; } 
        public int CreatedBy { get; set; }
    }
}
