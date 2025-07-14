namespace DapperAuthApi.Models;

public class EmployeeAttendance
{
    public long Pid { get; set; }
    public long? Employee_Fid { get; set; }
    public long? Employee_Id { get; set; }
    public string? SelfieePhoto_Url { get; set; }
    public DateTime Attendance_Date { get; set; }
    public string? Discom { get; set; }
    public int? Work_Location_Fid { get; set; }
    public string? Work_Latitude { get; set; }
    public string? Work_Longitude { get; set; }
    public string? Attendance_Status { get; set; } // Present, Absent
    public string? Leave_Type { get; set; } // None, Holiday, Sick, Casual
    public string? Sick_DocCopy { get; set; }
    public string? Absent_Remark { get; set; } 
    public int QryDate { get; set; }
    public int QryMonth { get; set; }
    public int Created_By { get; set; }
    public int? Updated_By { get; set; } 
}
public class EmployeeAttendanceResponse
{
    // Transaction metadata
    public string? Trx_Code { get; set; }
    public string? Trx_Status { get; set; }
}