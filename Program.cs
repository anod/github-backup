
using Microsoft.Extensions.Hosting;

using IHost host = Host.CreateDefaultBuilder(args).Build();

Console.WriteLine("Hello, World!");

await host.RunAsync();