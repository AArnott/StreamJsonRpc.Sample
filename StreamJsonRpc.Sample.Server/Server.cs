using System;

internal class Server
{
    public int Add(int a, int b)
    {
        // Log to STDERR so as to not corrupt the STDOUT pipe that we may be using for JSON-RPC.
        Console.Error.WriteLine($"Received request: {a} + {b}");

        return a + b;
    }
}
