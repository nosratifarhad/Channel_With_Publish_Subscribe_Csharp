using ChannelWithPublishSubscribeApplication.Channels.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChannelWithPublishSubscribeApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessengersController : ControllerBase
    {
        #region [ Field ]
        private readonly IBackgroundTaskQueue _backgroundTaskQueue;

        #endregion [ Field ]

        #region [ Ctor ]

        public MessengersController(IBackgroundTaskQueue backgroundTaskQueue)
        {
            _backgroundTaskQueue = backgroundTaskQueue;
        }

        #endregion  [Ctor]

        [HttpGet("/api/messengers/send-message")]
        public async Task<IActionResult> SendMessage(string message)
        {
            Console.WriteLine($"I was written to queue {DateTime.Now}");

            await _backgroundTaskQueue.QueueBackgroundWorkItemAsync(_ =>
            {
                return Task.Run(async () =>
                {
                    await Task.Delay(1000);

                    Console.WriteLine($"I ran {DateTime.Now}");
                });
            });

            return Ok();
        }
    }
}
