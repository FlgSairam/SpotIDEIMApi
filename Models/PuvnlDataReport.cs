namespace DapperAuthApi.Models
{
    public class PuvnlDataReport
    {
        public string BillingDate { get; set; } = string.Empty;
        public long MRMobileNo { get; set; }
        public string Full_Name { get; set; } = string.Empty;
        public string CircleName { get; set; } = string.Empty;
        public string DivisionCode { get; set; } = string.Empty;
        public string DivisionName { get; set; } = string.Empty;
        public int AutoCount { get; set; }
        public int ManualCount { get; set; }
        public int IDFCount { get; set; }
        public int TDMRCount { get; set; }
        public int ProbeCount { get; set; }
        public int TotalCount { get; set; }
    }

    public class PuvnlDataSummaryReport
    {
        public int MRCount { get; set; }
        public string AutoCount { get; set; } = string.Empty;
        public string ManualCount { get; set; } = string.Empty;
        public string IDFCount { get; set; } = string.Empty;
        public string TDMRCount { get; set; } = string.Empty;
        public string ProbeCount { get; set; } = string.Empty;
        public int TotalCount { get; set; }
    }

    public class PuvnlMrExceptionSummaryReport
    {
        public int MRCount { get; set; }
        public string OKWithoutExceptions { get; set; } = string.Empty;
        public string IncorrectReading { get; set; } = string.Empty;
        public string UnclearImage { get; set; } = string.Empty;
        public string IncorrectParameter { get; set; } = string.Empty;
        public string InvalidImage { get; set; } = string.Empty;
        public string Spoof { get; set; } = string.Empty;
        public int TotalCount { get; set; }
    }

    public class PuvnlMrExceptionDetailReport
    {
        public string BillingDate { get; set; } = string.Empty;
          public string MRMobileNo { get; set; } = string.Empty;
        public string Full_Name { get; set; } = string.Empty;
        public string CircleName { get; set; } = string.Empty;
        public string DivisionCode { get; set; } = string.Empty;
        public string DivisionName { get; set; } = string.Empty;
        public string Mr_UniqueId { get; set; } = string.Empty;
        public int OkWithoutExceptions { get; set; }
        public int IncorrectReading { get; set; }
        public int UnclearImage { get; set; }
        public int IncorrectParameter { get; set; }
        public int InvalidImage { get; set; }
        public int Spoof { get; set; }
        public int Total { get; set; }
    }

    public class PuvnlZoneCircleSummaryReport
    {
        public int SrNo { get; set; }
        public string Zone { get; set; } = string.Empty;
        public string Circle { get; set; } = string.Empty;
        public int TotalMRs { get; set; }
        public int MeterReaders { get; set; }
        public double PercentOfTotalMRs { get; set; }
        public int AutoCount { get; set; }
        public int ManualCount { get; set; }
        public int IDFCount { get; set; }
        public int TDMRCount { get; set; }
        public int ProbeCount { get; set; }
        public int TotalCount { get; set; }
        public string LastUpdated { get; set; } = string.Empty;
    }
}
