namespace DapperAuthApi.Models
{
    public class Supervisor
    {
    }

    public class SvAttendance
    {
        public int qrydate { get; set; }
        public string? reporting_officer_name { get; set; }
    }

    public class Emp
    {
        public long employee_id { get; set; }
        public string? full_name { get; set; }
        public string? mobile_number { get; set; }
    }

    public class SvAttendanceResponse:Emp
    {
            public string? attendance_status { get; set; }
            public string? selfieephoto_url { get; set; }
            public string? leave_type { get; set; }
            public string? absent_remark { get; set; }
            public string? sick_doccopy { get; set; }
            public int qrydate { get; set; }  
            public string? work_latitude { get; set; }
            public string? work_longitude { get; set; }
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
}
