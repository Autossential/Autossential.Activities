namespace Autossential.Activities.Extensions
{
    internal static class TaskExtensions
    {
        extension<T>(Task<T> task)
        {
            public async Task<T> WithTimeout(TimeSpan timeout, bool continueOnError, CancellationToken token)
            {
                try
                {
                    var timeoutTask = Task.Delay(timeout, token);
                    var completedTask = await Task.WhenAny(task, timeoutTask);
                    if (completedTask == timeoutTask)
                        throw new TimeoutException();

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