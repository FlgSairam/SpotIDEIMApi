using System.Text;
using System.Text.Json;
using System.Net.Http.Headers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using DapperAuthApi.Interfaces;
using DapperAuthApi.Models;

namespace DapperAuthApi.Services
{
    public class AttendanceReminderService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<AttendanceReminderService> _logger;

        public AttendanceReminderService(IServiceScopeFactory scopeFactory, ILogger<AttendanceReminderService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var now = DateTime.Now;

                if (now.Hour == 10 && now.Minute == 30) // Send daily at 10:30 AM
                {
                    try
                    {
                        using var scope = _scopeFactory.CreateScope();
                        var attendanceRepo = scope.ServiceProvider.GetRequiredService<IAttendanceRepository>();
                        var pushService = scope.ServiceProvider.GetRequiredService<IPushNotification>();

                        var today = DateTime.Today.ToString("yyyyMMdd");

                        var absentees = await attendanceRepo.GetEmployeesWithoutAttendance(today);

                        foreach (var employee in absentees)
                        {
                            var tokens = await pushService.GetById(employee.employee_id);

                            foreach (var token in tokens)
                            {
                                await SendPushNotificationAsync(token, "Fluentgrid Services", $"Hi {employee.full_name}, please submit your today attendance.");
                                _logger.LogInformation("Push sent to {full_name}", employee.full_name);
                            }
                        }

                        // Sleep 1 minute to avoid double sending
                        await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error in AttendanceReminderService");
                    }
                }

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken); // Poll every minute
            }
        }

        private async Task SendPushNotificationAsync(string expoPushToken, string title, string message)
        {
            if (string.IsNullOrWhiteSpace(expoPushToken) || !expoPushToken.StartsWith("ExponentPushToken"))
                return; // invalid token

            var payload = new
            {
                to = expoPushToken,
                sound = "default",
                title = title,
                body = message,
                badge = 1
            };

            using var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("https://exp.host/--/api/v2/push/send", content);
            var result = await response.Content.ReadAsStringAsync();

            // Log result (optional)
            Console.WriteLine($"Expo Response: {result}");
        }
    }
}
