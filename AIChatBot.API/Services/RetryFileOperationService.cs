using Microsoft.AspNetCore.Mvc;

namespace AIChatBot.API.Services
{
    public class RetryFileOperationService
    {
        public IActionResult RetryFileOperation(Func<IActionResult> operation, int maxRetries = 3, int delayMilliseconds = 200)
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
                        return new ObjectResult("File is currently in use. Please try again later.")
                        {
                            StatusCode = StatusCodes.Status423Locked
                        };
                    Thread.Sleep(delayMilliseconds);
                }
            }
        }
    }
}
