namespace DapperAuthApi.Models
{
    public class Supervisor
    {
    }

    public class SvAttendance
    {
        public int qrydate { get; set; }
        public long supervisor_fid { get; set; }
    }

    public class Emp
    {
        // New or inherited fields (ensure they're in base class or declare here)
        public string? employee_id { get; set; }
        public string? full_name { get; set; }
        public string? mobile_number { get; set; }
    }

    public class SvAttendanceResponse : Emp
    {
       

        // Attendance-related
        public string? attendance_status { get; set; }
        public string? selfieephoto_url { get; set; }
        public string? leave_type { get; set; }
        public string? absent_remark { get; set; }
        public string? sick_doccopy { get; set; }
        public int qrydate { get; set; }
        public string? work_latitude { get; set; }
        public string? work_longitude { get; set; }
        public string? attendance_time { get; set; }

        // Supervisor info
        public string? supervisor_fid { get; set; }
        public string? supervisor_name { get; set; }

        // Position info
        public string? position { get; set; }
    }


    public class SvPerformance: SvAttendance
    {
        
    }   

    public class SvPerformanceResponse:Emp
    {
        public string? attendance_status { get; set; }
        public int? noof_billissued { get; set; }
        public int? noof_exeptionbillissued { get; set; }
        public int? noof_transaction { get; set; }
        public decimal? amount_collected { get; set; }
        public string? remark { get; set; }
        public string? dashboard_copy { get; set; }
        public string? final_status { get; set; }
    }

    public class EmployeeAttendanceCounts
    {
        public int Total_PresentCount { get; set; }
        public int Total_LeaveCount { get; set; }
        public int Total_AbsentCount { get; set; }
        public int MR_PresentCount { get; set; }
        public int MR_LeaveCount { get; set; }
        public int MR_AbsentCount { get; set; }
        public int SV_PresentCount { get; set; }
        public int SV_LeaveCount { get; set; }
        public int SV_AbsentCount { get; set; }
        public int CI_PresentCount { get; set; }
        public int CI_LeaveCount { get; set; }
        public int CI_AbsentCount { get; set; }
    }
}
