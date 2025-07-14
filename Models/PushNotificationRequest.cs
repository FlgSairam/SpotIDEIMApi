namespace DapperAuthApi.Models
{
    public class PushNotificationRequest
    {
        public int Empid { get; set; }
        public string? ExpoPushToken { get; set; } // Required: Expo Token (Mobile ID)
        public string? Title { get; set; } = "Notification"; // Optional: Notification Title
        public string? Message { get; set; } // Required: Notification Body
        public int? BadgeCount { get; set; } = 1; // Optional: Badge Count (default 1)
    }

    public class DeviceInfo
    {
        public long EmployeeFid { get; set; }
        public long EmployeeId { get; set; }
        public string? DeviceId { get; set; }
    }

    public class DeviceToken
    {
        public int employee_id { get; set; }
        public string? full_name { get; set; }  // Optional, used for greeting
        public string? device_Id { get; set; } // Optional, if fetched with token
    }
}
