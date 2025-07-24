using System.Numerics;
using System.Security.Cryptography;

namespace DapperAuthApi.Models;

public class MeterreaderInfo
{
    public string? Name { get; set; }
    public string? Photograph { get; set; }
    public string? Contact_Details { get; set; }
    public string? DISCOM { get; set; }
    public string? Division { get; set; }
    public string? UPPCL_Reporting_Officer_Contact_Details { get; set; }
    public string? Service_Status { get; set; }
    public string? Agency_Name { get; set; }

    public string? Trx_Code { get; set; }

    public string? Trx_Status { get; set; }
}

public class EmployeeLoginInfo
{
    public long employee_Id { get; set; }
    public string? offer_letter_no { get; set; }
    public string? reader_name { get; set; }
    public string? father_name { get; set; }
    public DateTime date_of_birth { get; set; }
    public DateTime date_of_joining { get; set; } 
    public string? photograph_url { get; set; }
    public string? blood_group { get; set; }
    public int experience_years { get; set; }
    public string? primary_mobile_number { get; set; }
    public string? alternative_mobile_number { get; set; }
    public string? email_id { get; set; }
    public string? permanent_address { get; set; }
    public int division_fid { get; set; }
    public string? division_allocated { get; set; }
    public long supervisor_fid { get; set; }
    public string? supervisor_name { get; set; }
    public int work_location_fid { get; set; }
    public string? work_location { get; set; }
    public string? position { get; set; }
    public string? discom { get; set; }
    public int discom_fid { get; set; }        
    public string? Service_Status { get; set; } 
    public string? record_status { get; set; }
    public string? OTP_Code { get; set; } 
    public string? Trx_Code { get; set; }
    public string? Trx_Status { get; set; }
}