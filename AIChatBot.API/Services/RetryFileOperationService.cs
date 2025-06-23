using Microsoft.AspNetCore.Mvc;

namespace AIChatBot.API.Services
{
    public class RetryFileOperationService
    {
        public IActionResult RetryFileOperation(Func<IActionResult> operation, ControllerBase controller, int maxRetries = 3, int delayMilliseconds = 200)
        {
            int retries = 0;
            while (true)
            {
                try
                {
                    return operation();
                }
                catch (IOException ex) when ((ex.HResult & 0x0000FFFF) == 32)
                {
                    if (++retries >= maxRetries)
                        return controller.Problem("File is currently in use. Please try again later.");
                    Thread.Sleep(delayMilliseconds);
                }
            }
        }
    }
}
