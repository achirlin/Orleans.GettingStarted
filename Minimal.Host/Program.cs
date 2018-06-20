using Orleans.Runtime.Host;
using Orleans;
using System.Net;
using System;
using Minimal.Interfaces;
using System.Threading.Tasks;

class Program
{

    static SiloHost siloHost;

    static void Main(string[] args)
    {
        // Orleans should run in its own AppDomain, we set it up like this
        AppDomain hostDomain = AppDomain.CreateDomain("OrleansHost", null,
            new AppDomainSetup()
            {
                AppDomainInitializer = InitSilo
            });
        
        // Orleans comes with a rich XML and programmatic configuration. 
        // Here we're just going to set up with basic programmatic config
        var config = Orleans.Runtime.Configuration.ClientConfiguration.LocalhostSilo(30000);
        GrainClient.Initialize(config);

        for (int i = 0; i < 1000; i++)
        {
            DoSomeClientWork(i);
            Task.Delay(10);
        }

        Console.WriteLine("Orleans Silo is running.\nPress Enter to terminate...");
        Console.ReadLine();

        // We do a clean shutdown in the other AppDomain
        hostDomain.DoCallBack(ShutdownSilo);
    }

    static void DoSomeClientWork(int counter)
    {
        var friend = GrainClient.GrainFactory.GetGrain<IHello>(0);
        var result = friend.SayHello($"Goodbye: {counter}").Result;
        Console.WriteLine(result);
    }

    static void InitSilo(string[] args)
    {
        siloHost = new SiloHost(Dns.GetHostName())
        {
            // The Cluster config is quirky and weird to configure in code
            // so we're going to use a config file
            ConfigFileName = "OrleansConfiguration.xml",
        };

        siloHost.InitializeOrleansSilo();

        //siloHost.NodeConfig.TraceToConsole = false;
        //siloHost.NodeConfig.DefaultTraceLevel = Orleans.Runtime.Severity.Error;

        bool startedok = siloHost.StartOrleansSilo();

        if (!startedok)
            throw new SystemException(
                $"Failed to start Orleans silo '{siloHost.Name}' as a {siloHost.Type} node");

    }

    static void ShutdownSilo()
    {
        if (siloHost != null)
        {
            siloHost.Dispose();
            GC.SuppressFinalize(siloHost);
            siloHost = null;
        }
    }
}