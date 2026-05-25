using System;
using Microsoft.Diagnostics.Tracing;
using Microsoft.Diagnostics.Tracing.Parsers;
using Microsoft.Diagnostics.Tracing.Session;

namespace ETWApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // 建立新的 ETW session
            using (var session = new TraceEventSession("ETWConsoleTestSession"))
            {
                session.StopOnDispose = true;

                // 啟用 Kernel Process provider
                session.EnableKernelProvider(KernelTraceEventParser.Keywords.Process);

                // 訂閱 ProcessStart/Stop 事件
                session.Source.Kernel.ProcessStart += data =>
                {
                    Console.WriteLine($"[START] {data.ProcessName} (PID={data.ProcessID})");
                };

                session.Source.Kernel.ProcessStop += data =>
                {
                    Console.WriteLine($"[STOP] {data.ProcessName} (PID={data.ProcessID})");
                };

                Console.WriteLine("Listening for process start/stop events...");
                Console.WriteLine("Try opening and closing Notepad or Calculator.");
                Console.WriteLine("Press Ctrl+C to exit.");

                // 開始事件 loop
                session.Source.Process();
            }
        }
    }
}
