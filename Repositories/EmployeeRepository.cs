using Dapper;
using DapperAuthApi.Models;
using MySql.Data.MySqlClient;
using System;
using System.Data;

namespace DapperAuthApi.Repositories;

public class EmployeeRepository
{
    private readonly IConfiguration _configuration;
    private readonly string? _connectionString;

    public EmployeeRepository(IConfiguration configuration)
    {
        _configuration = configuration;
        _connectionString = _configuration.GetConnectionString("DefaultConnection");
    }

    private IDbConnection Connection => new MySqlConnection(_connectionString);
    private static readonly Random random = new Random();

    public async Task<EmployeeLoginInfo?> GetEmployeeLoginInfoAsync(string mobileNumber)
    {
        using var db = Connection;
        string sql = @"SELECT
                  pid AS employee_Id,
                  offer_letter_no,
                  reader_name,
                  father_name,
                  date_of_birth,
                  '2025/07/23' AS date_of_joining,
                  photograph_url,
                  blood_group, 
                  experience_years,
                  primary_mobile_number,
                  alternative_mobile_number, 
                  email_id,
                  permanent_address,
                  division_fid,
                  division_allocated,
                  supervisor_fid,
                  supervisor_name,
                  work_location_fid,
                  work_location,
                  position,
                  'PuVVNL' AS discom,
                  1 AS discom_fid,
                  'Active' AS service_status, 
                  record_status
                FROM eim_meter_reader_registration 
                WHERE record_status = 1 
                  AND primary_mobile_number = @MobileNumber
                  AND service_status IN ('Active', 'Inactive')";


        var result = await db.QueryFirstOrDefaultAsync<EmployeeLoginInfo>(sql, new { MobileNumber = mobileNumber });

        if (result == null)
        {
            return new EmployeeLoginInfo
            {
                Trx_Code = "01",
                Trx_Status = "Failure transaction no record found for this meter reader"
            };
        }

        if (result.Service_Status?.ToLower() == "inactive")
        {
            result.Trx_Code = "02";
            result.Trx_Status = "Inactive Meter Reader - No longer working with the agency Fluentgrid Limited";
        }
        else
        {
            int randomNumber = random.Next(1000, 10000);
            result.OTP_Code = randomNumber.ToString();
            result.Trx_Code = "00";
            result.Trx_Status = "Successful transaction";

            EmailSender emailSender = new EmailSender();
            await emailSender.SendEmailAsync("bhaskar.p@fluentgrid.com", "OTP to login into iPower Mobile ", result.OTP_Code + " is your OTP to login into iPower Mobile app. Do not share with anyone.");

            string strSMS = "OTP for your iPower Mobile app is " + result.OTP_Code + " OTP is confidential. Please do not share with anyone. OTP will automatically expire within 5 minutes. Fluentgrid";
            await emailSender.SendEmailAsync("sairam.p@fluentgrid.com", "OTP to login into iPower Mobile ", strSMS);

            string strUrl = sendSMS(strSMS, mobileNumber, "1707172889271402713");

            try
            {
                using HttpClientHandler handler = new HttpClientHandler
                {
                    SslProtocols = System.Security.Authentication.SslProtocols.Tls12
                };

                using HttpClient client = new HttpClient(handler);
                string smsResponse = await client.GetStringAsync(strUrl);

                // Optional: Log or check smsResponse content if your SMS API provides status
                if (string.IsNullOrWhiteSpace(smsResponse) || smsResponse.ToLower().Contains("error"))
                {
                    result.Trx_Code = "03";
                    result.Trx_Status = "SMS sending failed. Please try again.";
                }
            }
            catch (Exception ex)
            {
                // Log ex.Message if needed
                result.Trx_Code = "03";
                result.Trx_Status = "SMS sending failed: " + ex.Message;
            }
        }

        return result;
    }


    //public async Task<EmployeeLoginInfo?> GetEmployeeLoginInfoAsync(string mobileNumber)
    //{
    //    using var db = Connection;
    //    string sql = @"SELECT pid,  employee_id, offer_letter_no, CAST(employee_access_id AS CHAR) as employee_access_id, full_name, photograph_url, mobile_number, email, permanent_address, 
    //        date_of_joining, department, designation, father_name, blood_group, 
    //        work_location_fid, working_location, discom, discom_fid, division, division_fid,
    //        reporting_officer_name, reporting_officer_contact, agency_name, office_address, id_card_validity,  service_status 
    //    FROM eim_employee 
    //    WHERE service_status='Active' and record_status=1 and mobile_number = @MobileNumber";

    //    var result = await db.QueryFirstOrDefaultAsync<EmployeeLoginInfo>(sql, new { MobileNumber = mobileNumber });


    //    if (result == null)
    //    {
    //        return new EmployeeLoginInfo
    //        {
    //            Trx_Code = "01",
    //            Trx_Status = "Failure transaction no record found for this meter reader"
    //        };
    //    }


    //    if (result.Service_Status?.ToLower() == "inactive")
    //    {
    //        result.Trx_Code = "02";
    //        result.Trx_Status = "Inactive Meter Reader - No longer working with the agency Fluentgrid Limited";
    //    }
    //    else
    //    {
    //        int randomNumber = random.Next(1000, 10000);
    //        result.OTP_Code = randomNumber.ToString(); 
    //        result.Trx_Code = "00";
    //        result.Trx_Status = "Successful transaction";

    //        EmailSender emailSender = new EmailSender();

    //        await emailSender.SendEmailAsync("bhaskar.p@fluentgrid.com", "OTP to login into iPower Mobile ", result.OTP_Code + " is your OTP to login into iPower Mobile app. Do not share with anyone.");
    //        //"MPPKVVCL ATPM Pre-Paid wallet balance is low. Please top up your wallet to avoid discontinuity of collection on ATPM. Wallet balance available Rs. " + employeelogin.verificationcode + " MPPKVCCL"
    //        //"OTP for your iPower Mobile app is " + employeelogin.verificationcode + " OTP is confidential. Please do not share with anyone. OTP will automatically expire within 5 minutes. Fluentgrid"
    //        string strSMS = "";
    //        strSMS = "OTP for your iPower Mobile app is " + result.OTP_Code + " OTP is confidential. Please do not share with anyone. OTP will automatically expire within 5 minutes. Fluentgrid";

    //        await emailSender.SendEmailAsync("sairam.p@fluentgrid.com", "OTP to login into iPower Mobile ", strSMS);

    //        string strUrl = sendSMS(strSMS, mobileNumber, "1707172889271402713");
    //        HttpClientHandler handler = new HttpClientHandler();
    //        handler.SslProtocols = System.Security.Authentication.SslProtocols.Tls12;
    //        HttpClient client = new HttpClient(handler);
    //        await client.GetStringAsync(strUrl);

    //    }

    //    return result;
    //}

    private string sendSMS(string message, string phoneNo, string template_id)
    {
        string strUrl = "";
        string Password = "india@002";
        string Msg = message;
        string userdetails = "sairam.p";
        string OPTINS = "FGIPWR";
        string MobileNumber = phoneNo;
        strUrl = "https://login.bulksmsgateway.in/sendmessage.php?&user=" + userdetails + "&password=" + Password + "&mobile=" + MobileNumber + "&message=" + Msg + "&sender=" + OPTINS + "&type=" + 3 + "&template_id=" + template_id;

        return strUrl;
    }


}