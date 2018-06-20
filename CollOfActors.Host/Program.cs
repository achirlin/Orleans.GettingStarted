using System;
using CollOfActors.Interfaces;
using Orleans;
using Orleans.Runtime.Configuration;
using Orleans.Runtime.Host;

namespace CollOfActors.Host
{
    /// <summary>
    /// Orleans test silo host
    /// </summary>
    public class Program
    {
        static void Main(string[] args)
        {
            // Orleans comes with a rich XML and programmatic configuration. Here we're just going to set up with basic programmatic config
            var config = Orleans.Runtime.Configuration.ClientConfiguration.LocalhostSilo(30000);
            GrainClient.Initialize(config);

            var factory = GrainClient.GrainFactory;

            var e0 = factory.GetGrain<IEmployee>(Guid.NewGuid());
            var e1 = factory.GetGrain<IEmployee>(Guid.NewGuid());
            var e2 = factory.GetGrain<IEmployee>(Guid.NewGuid());
            var e3 = factory.GetGrain<IEmployee>(Guid.NewGuid());
            var e4 = factory.GetGrain<IEmployee>(Guid.NewGuid());

            var m0 = factory.GetGrain<IManager>(Guid.NewGuid());
            var m1 = factory.GetGrain<IManager>(Guid.NewGuid());

            var m0e = m0.AsEmployee().Result;
            var m1e = m1.AsEmployee().Result;

            m0e.Promote(10);
            m1e.Promote(11);

            m0.AddDirectReport(e0).Wait();
            m0.AddDirectReport(e1).Wait();
            m0.AddDirectReport(e2).Wait();

            m1.AddDirectReport(m0e).Wait();
            m1.AddDirectReport(e3).Wait();

            m1.AddDirectReport(e4).Wait();

            var dr = m0.GetDirectReports().Result;


            Console.WriteLine("Orleans Silo is running.\nPress Enter to terminate...");
            Console.ReadLine();
        }
    }
}
