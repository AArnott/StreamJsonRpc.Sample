using System;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Reflection;
using System.Threading.Tasks;
using Nerdbank.Streams;
using StreamJsonRpc;

class Program
{
    static bool useStdIo = true;

    static async Task Main()
    {
        if (useStdIo)
        {
            var psi = new ProcessStartInfo(FindPathToServer(), "stdio");
            psi.RedirectStandardInput = true;
            psi.RedirectStandardOutput = true;
            var process = Process.Start(psi);
            var stdioStream = FullDuplexStream.Splice(process.StandardOutput.BaseStream, process.StandardInput.BaseStream);
            await ActAsRpcClientAsync(stdioStream);
        }
        else
        {
            Console.WriteLine("Connecting to server...");
            using (var stream = new NamedPipeClientStream(".", "StreamJsonRpcSamplePipe", PipeDirection.InOut, PipeOptions.Asynchronous))
            {
                await stream.ConnectAsync();
                await ActAsRpcClientAsync(stream);
                Console.WriteLine("Terminating stream...");
            }
        }
    }

    private static async Task ActAsRpcClientAsync(Stream stream)
    {
        Console.WriteLine("Connected. Sending request...");
        using var jsonRpc = JsonRpc.Attach(stream);
        int sum = await jsonRpc.InvokeAsync<int>("Add", 3, 5);
        Console.WriteLine($"3 + 5 = {sum}");
    }

    private static string FindPathToServer()
    {
#if DEBUG
        const string Config = "Debug";
#else
        const string Config = "Release";
#endif
        return Path.Combine(
            Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
            $@"..\..\..\..\StreamJsonRpc.Sample.Server\bin\{Config}\netcoreapp3.1\StreamJsonRpc.Sample.Server.exe");
    }
}
