using Autossential.Activities.Properties;
using Autossential.Shared;
using System;
using System.Activities;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;

namespace Autossential.Activities
{
    public sealed class TerminateProcess : CodeActivity
    {
        [DllImport("user32.dll")]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        static readonly IntPtr _hWND_BOTTOM = new IntPtr(1);

        const uint SWP_NOSIZE = 0x0001;
        const uint SWP_NOMOVE = 0x0002;
        const uint SWP_NOACTIVATE = 0x0010;

        public InArgument<int> Timeout { get; set; }

        public InArgument ProcessName { get; set; }

        public InArgument<bool> ContinueOnError { get; set; }

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

        const int WaitForExit = 3000;
        const int DelayForNext = 50;
        const int DelayNonGUI = 500;

        protected override void Execute(CodeActivityContext context)
        {
            var timeout = Timeout.Get(context);
            if (timeout <= 0)
                timeout = 30000;

            var names = ProcessName.Get(context);
            if (names is string)
                names = new string[] { names.ToString() };

            var timer = System.Diagnostics.Stopwatch.StartNew();
            var processes = Array.Empty<Process>();
            var switched = false;
            var hasGui = false;

            foreach (var name in (IEnumerable<string>)names)
            {
                do
                {
                    // Processes with GUI
                    processes = Process.GetProcessesByName(name)
                        .Where(p => p.MainWindowHandle != IntPtr.Zero).ToArray();

                    foreach (var process in processes)
                    {
                        if (process.HasExited)
                            continue;

                        hasGui = true;

                        if (process.CloseMainWindow())
                        {
                            process.Close();
                            Thread.Sleep(DelayForNext);
                            switched = false;
                            continue;
                        }

                        if (!switched)
                        {
                            SetWindowPos(process.MainWindowHandle, _hWND_BOTTOM, 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE | SWP_NOACTIVATE);
                            switched = true;
                            continue;
                        }

                        switched = false;
                        process.Kill();
                        process.WaitForExit(WaitForExit);
                        Thread.Sleep(DelayForNext);
                    }

                } while (processes.Length > 0 && timer.ElapsedMilliseconds <= timeout);

                if (hasGui)
                    Thread.Sleep(DelayNonGUI); // holds for a brief moment before search for non-GUI processes

                // Processes without GUI
                processes = Process.GetProcessesByName(name)
                    .Where(p => p.MainWindowHandle == IntPtr.Zero).ToArray();

                foreach (var process in processes)
                {
                    if (process.HasExited)
                        continue;

                    process.Kill();
                    process.WaitForExit(WaitForExit);
                }
            }
        }
    }
}