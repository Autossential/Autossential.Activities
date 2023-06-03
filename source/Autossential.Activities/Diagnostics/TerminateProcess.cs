using Autossential.Activities.Properties;
using Autossential.Shared;
using System;
using System.Activities;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace Autossential.Activities
{
    public sealed class TerminateProcess : CodeActivity
    {
        public InArgument<int> Timeout { get; set; } = 30000;

        public InArgument ProcessName { get; set; }

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            base.CacheMetadata(metadata);
            if (ProcessName == null)
            {
                metadata.AddValidationError(Resources.Validation_ValueErrorFormat(nameof(ProcessName)));
            }
            else if (ProcessName.IsArgumentTypeAnyCompatible<string, IEnumerable<string>>())
            {
                metadata.AddRuntimeArgument(ProcessName, ProcessName.ArgumentType, nameof(ProcessName), true);
            }
        }

        protected override void Execute(CodeActivityContext context)
        {
            var timeout = Timeout.Get(context);
            var names = ProcessName.Get(context);
            if (names is string)
                names = new string[] { names.ToString() };

            var timer = System.Diagnostics.Stopwatch.StartNew();

            foreach (var name in (IEnumerable<string>)names)
            {
                var processes = Process.GetProcessesByName(name);
                if (processes.Length == 0)
                    continue;

                var closedGUI = CloseProcesses(processes, timer, timeout);
                if (closedGUI)
                {
                    processes = Process.GetProcessesByName(name);
                    if (processes.Length > 0)
                    {
                        // holds for possible delayed GUI termination
                        Thread.Sleep(1000);
                    }

                    KillProcesses(processes);
                    continue;
                }

                KillProcesses(processes);
            }
        }

        private void KillProcesses(Process[] processes)
        {
            foreach (var proc in processes)
            {
                if (!proc.HasExited)
                    proc.Kill();
            }
        }

        bool CloseProcesses(Process[] processes, System.Diagnostics.Stopwatch timer, int timeout)
        {
            string processName = null;
            foreach (var process in processes)
            {
                if (process.HasExited || process.MainWindowHandle == IntPtr.Zero)
                    continue;

                if (process.CloseMainWindow())
                {
                    processName = process.ProcessName;
                }

                process.Close();
            }

            if (processName != null && timer.ElapsedMilliseconds <= timeout)
            {
                CloseProcesses(Process.GetProcessesByName(processName), timer, timeout);
                return true;
            }

            return false;
        }
    }
}