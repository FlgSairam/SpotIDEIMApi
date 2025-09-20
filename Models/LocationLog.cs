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


    public class LocationLogViewModel
    {
        public long Pid { get; set; }
        public long EmployeeFid { get; set; }
        public string? FullName { get; set; }
        public string? Position { get; set; }
        public string? WorkLocation { get; set; }
        public string? MobileNumber { get; set; }
        public long WorkLocationFid { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public DateTime CapturedTime { get; set; }
        public string? DeviceId { get; set; }
        public int BatteryLevel { get; set; }
        public int QryDate { get; set; }
    }
}
