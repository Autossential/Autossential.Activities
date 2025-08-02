using Autossential.Activities.Properties;
using Autossential.Shared;
using Autossential.Shared.Activities.Base;
using System;
using System.Activities;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Autossential.Activities
{
    public sealed class TerminateProcess : ContinuableAsyncTaskCodeActivity
    {
        public InArgument<int> Timeout { get; set; }
        public InArgument ProcessName { get; set; }

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            base.CacheMetadata(metadata);
            if (ProcessName == null)
            {
                metadata.AddValidationError(ResourcesFn.Validation_ValueErrorFormat(nameof(ProcessName)));
            }
            else if (ProcessName.IsArgumentTypeAnyCompatible<string, IEnumerable<string>>())
            {
                metadata.AddRuntimeArgument(ProcessName, ProcessName.ArgumentType, nameof(ProcessName), true);
            }
            else
            {
                metadata.AddValidationError(ResourcesFn.Validation_TypeErrorFormat("string or IEnumerable<string>", nameof(ProcessName)));
            }
        }

        private int _sessionId;

        protected override async Task<Action<AsyncCodeActivityContext>> ExecuteAsync(AsyncCodeActivityContext context, CancellationToken token)
        {
            var timeout = Timeout.Get(context);
            if (Timeout.Expression is null)
                timeout = 30000;

            var processNames = ProcessName.Get(context);
            if (processNames is string)
                processNames = new string[] { processNames.ToString() };

            _sessionId = Process.GetCurrentProcess().SessionId;

            await Task.Run(() =>
            {
                var timer = System.Diagnostics.Stopwatch.StartNew();
                var tasks = ((IEnumerable<string>)processNames).Select(name => TerminateProcessByNameAsync(name, timer, timeout));

                Task.WhenAll(tasks).Wait(token); // Executes all tasks asynchronously and waits for completion
            }, token);

            return null;
        }

        private IEnumerable<Process> GetProcessesByName(string processName)
            => Process.GetProcessesByName(processName).Where(p => p.SessionId == _sessionId);

        private async Task TerminateProcessByNameAsync(string processName, System.Diagnostics.Stopwatch timer, int timeout)
        {
            try
            {
                await CloseProcessesAsync(GetProcessesByName(processName), timer, timeout);
                await KillProcessesAsync(GetProcessesByName(processName));
            }
            catch (Exception e) when (e is InvalidOperationException || e is System.ComponentModel.Win32Exception)
            {
                Trace.WriteLine($"Error terminating process {processName}: {e.Message}");
            }
        }

        private static async Task KillProcessesAsync(IEnumerable<Process> processes)
        {
            foreach (var proc in processes)
            {
                try
                {
                    proc.Kill();
                    await Task.Delay(100); // small delay to ensure the process was terminated
                }
                catch (Exception e)
                {
                    Trace.WriteLine($"Failed to kill process {proc.ProcessName}: {e.Message}");
                }
            }
        }

        private async Task CloseProcessesAsync(IEnumerable<Process> processes, System.Diagnostics.Stopwatch timer, int timeout)
        {
            if (!processes.Any())
                return;

            string processName;

            do
            {
                // keep looping until all processes are closed or timeout is reached

                processName = null;
                foreach (var process in processes)
                {
                    if (process.HasExited || !process.Responding || process.MainWindowHandle == IntPtr.Zero)
                        continue;

                    if (process.CloseMainWindow())
                    {
                        processName = process.ProcessName;
                        process.Close(); // free resources associated with process
                    }

                    await Task.Delay(100);
                }

                if (processName != null && timer.ElapsedMilliseconds <= timeout)
                {
                    processes = GetProcessesByName(processName);
                }

            } while (processName != null);
        }
    }
}