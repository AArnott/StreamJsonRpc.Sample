using System;
using System.IO.Pipes;
using System.Threading.Tasks;

namespace StreamJsonRpc.Sample.Client
{
    class Client
    {
        static async Task Main()
        {
            Console.WriteLine("Connecting to server...");
            using (var stream = new NamedPipeClientStream(".", "StreamJsonRpcSamplePipe", PipeDirection.InOut, PipeOptions.Asynchronous))
            {
                await stream.ConnectAsync();
                Console.WriteLine("Connected. Sending request...");
                var jsonRpc = JsonRpc.Attach(stream);
                int sum = await jsonRpc.InvokeAsync<int>("Add", 3, 5);
                Console.WriteLine($"3 + 5 = {sum}");
                Console.WriteLine("Terminating stream...");
            }
        }
    }
}
