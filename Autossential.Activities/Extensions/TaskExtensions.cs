namespace Autossential.Activities.Extensions
{
    internal static class TaskExtensions
    {
        extension<T>(Task<T> task)
        {
            public async Task<T> WithTimeout(TimeSpan timeout, CancellationToken token)
            {
                var timeoutTask = Task.Delay(timeout, token);
                var completedTask = await Task.WhenAny(task, timeoutTask);
                if (completedTask == timeoutTask)
                    throw new TimeoutException();

                return await task;
            }

            public async Task<T> ContinueOnError(bool continueOnError)
            {
                try
                {
                    return await task;
                }
                catch
                {
                    if (continueOnError)
                        return default;

                    throw;
                }
            }
        }
    }
}