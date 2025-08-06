using DapperAuthApi.Interfaces;
using DapperAuthApi.Models;
using Expo.Server.Client;
using Expo.Server.Models;
using Microsoft.AspNetCore.Authorization;

//using FirebaseAdmin;
//using FirebaseAdmin.Messaging;
//using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Mvc;


namespace iPowerMobileAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PushNotificationController : ControllerBase
    {
        private readonly PushApiClient _expoClient;
        private readonly IPushNotification _pushNotification;

        public PushNotificationController(IPushNotification pushNotification)
        {
            _pushNotification = pushNotification;
            _expoClient = new PushApiClient();
        }

        [Authorize]
        [HttpPost("DeviceInfo")]
        public async Task<IActionResult> InsertAsync([FromBody] DeviceInfo entity)
        {

            if (ModelState.IsValid)
            {
                var vData = await _pushNotification.InsertAsync(entity);

                if (vData != null)
                {
                    // If data is found, return it with a 200 OK status
                    return Ok(vData);
                }
                else
                {
                    // If data is not found, return a 404 Not Found status
                    return NotFound();
                }
            }
            // If model state is not valid, return HTTP 400 Bad Request with validation errors
            return BadRequest(ModelState);
        }

        [Authorize]
        [HttpPost("send")]
        public async Task<IActionResult> SendPushNotification(int employeeFK, string message)
        {
            var response = await _pushNotification.GetById(employeeFK);

            if (response == null || !response.Any())
            {
                return BadRequest("Expo Push Token is required.");
            }

            var request = new PushNotificationRequest
            {
                ExpoPushToken = response.FirstOrDefault(), // Use the first token from the list
                Title = "Fluentgrid Connect",
                Message = message,
                BadgeCount = 1
            };

            try
            {
                var pushTicketReq = new PushTicketRequest()
                {
                    PushTo = new List<string?> { request.ExpoPushToken },
                    PushTitle = request.Title,
                    PushBody = request.Message,
                    PushBadgeCount = request.BadgeCount ?? 1,
                    PushSound = "default"
                };

                var result = await _expoClient.PushSendAsync(pushTicketReq);

                if (result?.PushTicketErrors?.Count > 0)
                {
                    return BadRequest(result.PushTicketErrors);
                }

                return Ok(new { Message = "Notification sent successfully.", Tickets = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error sending notification.", Error = ex.Message });
            }
        }

        //[HttpPost("sendfirebase")]
        //public async Task<IActionResult> SendFirebaseNotification([FromBody] PushNotification request)
        //{
        //    //if (string.IsNullOrEmpty(request.Topic))
        //    //    return BadRequest("Firebase token is required");

        //    var message = new Message()
        //    {
        //        Topic = "all-users", // Send to a topic (e.g., package name)
        //        Notification = new Notification()
        //        {
        //            Title = "title",
        //            Body = "body"
        //        }
        //    };


        //    try
        //    {
        //        // Ensure Firebase is initialized
        //        //if (FirebaseApp.DefaultInstance == null)
        //        //{
        //        //    FirebaseApp.Create(new AppOptions()
        //        //    {
        //        //        Credential = GoogleCredential.FromFile("D:\\Projects\\iPowerMobileAPI\\fluentgrid-connect-firebase-adminsdk-fbsvc-13cc11ee03.json")
        //        //    });
        //        //}
        //        // Send the message
        //        string response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
        //        return Ok(new { Message = "Broadcast notification sent", Response = response });
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new { Message = "Error sending notification", Error = ex.Message });
        //    }
        //}

        public class PushNotification
        {
            public string? Topic { get; set; }
            public string? Title { get; set; } = "Notification";
            public string? Message { get; set; }
        }


    }
}