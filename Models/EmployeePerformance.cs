namespace DapperAuthApi.Models;

public class EmployeePerformance
{
    public long Pid { get; set; }
    public long? Employee_Fid { get; set; }
    public long? Employee_Id { get; set; }
    public DateTime Performance_Date { get; set; } = DateTime.Now;
    public string? Discom_Fid { get; set; }
    public string? Discom { get; set; }
    public string? Division_Fid { get; set; }
    public string? Division { get; set; }
    public int? Work_Location_Fid { get; set; }
    public string? Work_Location { get; set; }
    public string? Department { get; set; }
    public int NoOf_BillIssued { get; set; } = 0;
    public int NoOf_ExceptionBillIssued { get; set; } = 0;
    public int NoOf_Transaction { get; set; } = 0;
    public decimal Amount_Collected { get; set; } = 0.00m;
    public string? Remark { get; set; }
    public int QryDate { get; set; }
    public int QryMonth { get; set; }
    public int Created_By { get; set; }
    public int? Updated_By { get; set; }
}

public class EmployeePerformanceResponse
{
    // Transaction metadata
    public string? Trx_Code { get; set; }
    public string? Trx_Status { get; set; }
}