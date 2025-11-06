using Dapper;
using DapperAuthApi.Models;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System.Data;

namespace DapperAuthApi.Repositories
{
    public class PuvnlDataReportRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string? _connectionString;

        public PuvnlDataReportRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        private IDbConnection Connection => new MySqlConnection(_connectionString);

        public async Task<IEnumerable<PuvnlDataReport>> GetPuvnlDataAsync(long mrMobileNo, int qryMonth)
        {
            using var db = Connection;

            string sql = @"
                SELECT  
                    DATE_FORMAT(p.StartDate, '%d-%m-%Y') AS BillingDate,
                    p.MRMobileNo,
                    m.reader_name AS Full_Name,
                    p.CircleName,
                    p.DivisionCode,
                    p.DivisionName,
                    p.AutoCount,
                    p.ManualCount,
                    p.IDFCount,
                    p.TDMRCount,
                    p.ProbeCount,
                    p.TotalCount
                FROM eim_tbl_puvnl_data PARTITION (P{0}) p
                LEFT JOIN eim_meter_reader_registration m 
                        ON m.primary_mobile_number = p.MRMobileNo
                WHERE 
                    p.record_status = 1
                    AND m.record_status = 1
                    AND p.MRMobileNo = @MrMobileNo
                UNION ALL
                SELECT 
                    'Grand Total' AS BillingDate,
                    p.MRMobileNo,
                    m.reader_name AS Full_Name,
                    p.CircleName,
                    p.DivisionCode,
                    p.DivisionName,
                    SUM(p.AutoCount) AS AutoCount,
                    SUM(p.ManualCount) AS ManualCount,
                    SUM(p.IDFCount) AS IDFCount,
                    SUM(p.TDMRCount) AS TDMRCount,
                    SUM(p.ProbeCount) AS ProbeCount,
                    SUM(p.TotalCount) AS TotalCount
                FROM eim_tbl_puvnl_data PARTITION (P{0}) p
                LEFT JOIN eim_meter_reader_registration m 
                        ON m.primary_mobile_number = p.MRMobileNo
                WHERE 
                    p.record_status = 1
                    AND m.record_status = 1
                    AND p.MRMobileNo = @MrMobileNo
                GROUP BY p.MRMobileNo, m.reader_name, p.CircleName, p.DivisionCode, p.DivisionName;";

            sql = string.Format(sql, qryMonth);

            var result = await db.QueryAsync<PuvnlDataReport>(sql, new { MrMobileNo = mrMobileNo });
            return result;
        }

        // 🆕 New method: Fetch overall summary for given date range & circle
        public async Task<PuvnlDataSummaryReport?> GetPuvnlSummaryAsync(string? circleName, int fromDate, int toDate, int qryMonth)
        {
            using var db = Connection;

            string sql = @"
        SELECT 
            COUNT(DISTINCT q.MRMobileNo) AS MRCount,
            CONCAT(CAST(SUM(q.AUTOCOUNT) AS CHAR), ' (', 
                   CAST(ROUND(SUM(q.AUTOCOUNT) * 100.0 / NULLIF(SUM(q.TOTALCOUNT), 0), 2) AS CHAR), '%)') AS AutoCount,
            CONCAT(CAST(SUM(q.MANUALCOUNT) AS CHAR), ' (', 
                   CAST(ROUND(SUM(q.MANUALCOUNT) * 100.0 / NULLIF(SUM(q.TOTALCOUNT), 0), 2) AS CHAR), '%)') AS ManualCount,
            CONCAT(CAST(SUM(q.IDFCOUNT) AS CHAR), ' (', 
                   CAST(ROUND(SUM(q.IDFCOUNT) * 100.0 / NULLIF(SUM(q.TOTALCOUNT), 0), 2) AS CHAR), '%)') AS IDFCount,
            CONCAT(CAST(SUM(q.TDMRCOUNT) AS CHAR), ' (', 
                   CAST(ROUND(SUM(q.TDMRCOUNT) * 100.0 / NULLIF(SUM(q.TOTALCOUNT), 0), 2) AS CHAR), '%)') AS TDMRCount,
            CONCAT(CAST(SUM(q.PROBECOUNT) AS CHAR), ' (', 
                   CAST(ROUND(SUM(q.PROBECOUNT) * 100.0 / NULLIF(SUM(q.TOTALCOUNT), 0), 2) AS CHAR), '%)') AS ProbeCount,
            SUM(q.TOTALCOUNT) AS TotalCount
        FROM eim_tbl_puvnl_data PARTITION (P{0}) q
        LEFT JOIN eim_meter_reader_registration m 
                        ON m.primary_mobile_number = q.MRMobileNo
        WHERE 
            q.record_status = 1 
            AND m.record_status = 1 
            AND q.qrydate BETWEEN @FromDate AND @ToDate
            AND (@CircleName IS NULL OR @CircleName = '' OR q.CircleName = @CircleName);";

            sql = string.Format(sql, qryMonth);

            return await db.QueryFirstOrDefaultAsync<PuvnlDataSummaryReport>(sql, new
            {
                CircleName = circleName,
                FromDate = fromDate,
                ToDate = toDate
            });
        }


        public async Task<PuvnlMrExceptionSummaryReport?> GetPuvnlMrExceptionSummaryAsync(
     string? circleName, int fromDate, int toDate, int qryMonth)
        {
            using var db = Connection;

            string sql = @"
        SELECT 
            COUNT(DISTINCT q.mrid) AS MRCount,

            CONCAT(CAST(SUM(q.OkWithoutExceptions) AS CHAR), ' (',
                   CAST(ROUND(SUM(q.OkWithoutExceptions) * 100.0 / NULLIF(SUM(q.Total), 0), 2) AS CHAR), '%)') AS OKWithoutExceptions,

            CONCAT(CAST(SUM(q.IncorrectReading) AS CHAR), ' (',
                   CAST(ROUND(SUM(q.IncorrectReading) * 100.0 / NULLIF(SUM(q.Total), 0), 2) AS CHAR), '%)') AS IncorrectReading,

            CONCAT(CAST(SUM(q.UnclearImage) AS CHAR), ' (',
                   CAST(ROUND(SUM(q.UnclearImage) * 100.0 / NULLIF(SUM(q.Total), 0), 2) AS CHAR), '%)') AS UnclearImage,

            CONCAT(CAST(SUM(q.IncorrectParameter) AS CHAR), ' (',
                   CAST(ROUND(SUM(q.IncorrectParameter) * 100.0 / NULLIF(SUM(q.Total), 0), 2) AS CHAR), '%)') AS IncorrectParameter,

            CONCAT(CAST(SUM(q.InvalidImage) AS CHAR), ' (',
                   CAST(ROUND(SUM(q.InvalidImage) * 100.0 / NULLIF(SUM(q.Total), 0), 2) AS CHAR), '%)') AS InvalidImage,

            CONCAT(CAST(SUM(q.Spoof) AS CHAR), ' (',
                   CAST(ROUND(SUM(q.Spoof) * 100.0 / NULLIF(SUM(q.Total), 0), 2) AS CHAR), '%)') AS Spoof,

            SUM(q.Total) AS TotalCount

        FROM eim_tbl_puvnl_mr_exception_data PARTITION (P{0}) q
        LEFT OUTER JOIN eim_meter_reader_registration m 
            ON m.mr_uniqueid = q.MrId
        WHERE                       
            q.record_status = 1 
            AND m.record_status = 1 
            AND q.qrydate BETWEEN @FromDate AND @ToDate
            AND (@WorkLocation IS NULL OR @WorkLocation = '' OR m.work_location = @WorkLocation);";

            sql = string.Format(sql, qryMonth);

            return await db.QueryFirstOrDefaultAsync<PuvnlMrExceptionSummaryReport>(sql, new
            {
                WorkLocation = circleName,
                FromDate = fromDate,
                ToDate = toDate
            });
        }


        public async Task<IEnumerable<PuvnlMrExceptionDetailReport>> GetPuvnlMrExceptionDetailAsync(long mrMobileNo, int qryMonth)
        {
            using var db = Connection;

            string sql = @" SELECT  
                                DATE_FORMAT(p.StartDate, '%d-%m-%Y') AS BillingDate,
                                m.primary_mobile_number AS MRMobileNo,
                                m.reader_name AS Full_Name,
                                m.work_location AS CircleName,
                                (SELECT DISTINCT divisioncode 
                                    FROM eim_puvnl_mr_mapping_data 
                                    WHERE division = m.division_allocated) AS DivisionCode,
                                m.division_allocated AS DivisionName,
                                m.mr_uniqueid AS Mr_UniqueId,
                                p.OkWithoutExceptions,
                                p.IncorrectReading,
                                p.UnclearImage,
                                p.IncorrectParameter,
                                p.InvalidImage,
                                p.Spoof,
                                p.Total
                            FROM eim_tbl_puvnl_mr_exception_data PARTITION (P{0}) p
                            LEFT OUTER JOIN eim_meter_reader_registration m 
                                ON m.mr_uniqueid = p.MrId
                            WHERE 
                                p.record_status = 1
                                AND m.record_status = 1
                                AND m.primary_mobile_number = @MrMobileNo
                            UNION ALL
                            SELECT  
                                'Grand Total' AS BillingDate,
                                '' AS MRMobileNo,
                                '' AS Full_Name,
                                '' AS CircleName,
                                '' AS DivisionCode,
                                '' AS DivisionName,
                                CAST(COUNT(DISTINCT m.mr_uniqueid) AS CHAR) AS Mr_UniqueId,
                                SUM(p.OkWithoutExceptions) AS OkWithoutExceptions,
                                SUM(p.IncorrectReading) AS IncorrectReading,
                                SUM(p.UnclearImage) AS UnclearImage,
                                SUM(p.IncorrectParameter) AS IncorrectParameter,
                                SUM(p.InvalidImage) AS InvalidImage,
                                SUM(p.Spoof) AS Spoof,
                                SUM(p.Total) AS Total
                            FROM eim_tbl_puvnl_mr_exception_data PARTITION (P{0}) p
                            LEFT OUTER JOIN eim_meter_reader_registration m 
                                ON m.mr_uniqueid = p.MrId
                            WHERE 
                                p.record_status = 1
                                AND m.record_status = 1
                                AND m.primary_mobile_number = @MrMobileNo;";

            sql = string.Format(sql, qryMonth);
            var result = await db.QueryAsync<PuvnlMrExceptionDetailReport>(sql, new { MrMobileNo = mrMobileNo });
            return result;
        }
        public async Task<IEnumerable<PuvnlZoneCircleSummaryReport>> GetPuvnlZoneCircleSummaryAsync()
        {
            using var db = Connection;

            string sql = @"
        SELECT 
            @srno := @srno + 1 AS SrNo,
            Zone,
            Circle,
            TotalMRs,
            MeterReaders,
            ROUND(MeterReaders / TotalMRs * 100, 2) AS PercentOfTotalMRs,
            AutoCount,
            ManualCount,
            IDFCount,
            TDMRCount,
            ProbeCount,
            TotalCount,
            (SELECT DISTINCT DATE_FORMAT(LastUpdated,'%d-%m-%Y %H:%i:%s') 
             FROM eim_tbl_coral_home_current_data
             ORDER BY LastUpdated DESC LIMIT 1) AS LastUpdated
        FROM
            (SELECT 
                IFNULL(mc.zone, 'GRAND TOTAL') AS Zone,
                CASE
                    WHEN mc.zone IS NULL THEN ''
                    ELSE IFNULL(mc.circle, 'SUB TOTAL')
                END AS Circle,
                SUM(c.TotalMRs) AS TotalMRs,
                SUM(c.MeterReaders) AS MeterReaders,
                SUM(c.AutoCount) AS AutoCount,
                SUM(c.ManualCount) AS ManualCount,
                SUM(c.IDFCount) AS IDFCount,
                SUM(c.TDMRCount) AS TDMRCount,
                SUM(c.ProbeCount) AS ProbeCount,
                SUM(c.TotalCount) AS TotalCount
             FROM spotideim.eim_tbl_coral_home_current_data c
             LEFT JOIN 
                (SELECT DISTINCT divisioncode, zone, circle
                 FROM eim_puvnl_mr_mapping_data) mc 
             ON mc.divisioncode = c.DivisionCode
             GROUP BY mc.zone, mc.circle WITH ROLLUP
            ) qry
        CROSS JOIN (SELECT @srno := 0) AS init;";

            var result = await db.QueryAsync<PuvnlZoneCircleSummaryReport>(sql);
            return result;
        }


    }
}
