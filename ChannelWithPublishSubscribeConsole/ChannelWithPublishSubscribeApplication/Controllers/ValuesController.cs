using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChannelWithPublishSubscribeApplication.Controllers
{
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IBackgroundTaskQueue _backgroundTaskQueue;
        public ValuesController(IBackgroundTaskQueue backgroundTaskQueue)
        {
            _backgroundTaskQueue = backgroundTaskQueue;
        }
        [HttpGet("/api/values/dowork")]
        public async Task<IActionResult> DoWork()
        {
            Console.WriteLine($"I was written to queue {DateTime.Now}");
            await _backgroundTaskQueue.QueueBackgroundWorkItemAsync(_ =>
            {
                //This wont run untill the reader reads and calls it
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
