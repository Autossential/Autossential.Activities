using System.Activities;
using System.Diagnostics;

namespace Autossential.Activities
{
    public sealed class TerminateProcess : CodeActivity
    {
        [RequiredArgument]
        public InArgument<IReadOnlyList<string>> ProcessNames { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            var processNames = ProcessNames.Get(context);
            var sessionId = Process.GetCurrentProcess().SessionId;

            foreach (var processName in processNames)
            {
                CloseProcessMainWindow(processName, sessionId);
                KillProcess(processName, sessionId);
            }
        }

        private static void KillProcess(string processName, int sessionId)
        {
            var runningProcesses = GetProcessesByNameAndSessionId(processName, sessionId);
            foreach (var process in runningProcesses)
            {
                try
                {
                    if (!process.HasExited)
                    {
                        process.Kill();
                        process.WaitForExit(2000); // small delay to ensure the process was terminated
                    }
                }
                catch (Exception e)
                {
                    Trace.WriteLine($"Error killing process {processName} (PID {process.Id}): {e.Message}");
                }
                finally
                {
                    process.Dispose();
                }
            }
        }

        private static void CloseProcessMainWindow(string processName, int sessionId)
        {
            var runningProcesses = GetProcessesByNameAndSessionId(processName, sessionId);
            foreach (var process in runningProcesses)
            {
                try
                {
                    if (!process.HasExited && process.Responding && process.MainWindowHandle != IntPtr.Zero)
                    {
                        if (process.CloseMainWindow())
                        {
                            if (process.WaitForExit(2000))
                            {
                                process.Close(); // free resources associated with process
                            }
                            else
                            {
                                Trace.WriteLine($"Process {process.ProcessName} (PID {process.Id}) did not exit gracefully.");
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Trace.WriteLine($"Error closing process {processName} (PID {process.Id}): {e.Message}");
                }
                finally
                {
                    process.Dispose();
                }
            }
        }

        private static Process[] GetProcessesByNameAndSessionId(string processName, int sessionId)
        {
            return Process.GetProcessesByName(processName).Where(p => p.SessionId == sessionId).ToArray();
        }
    }
}