using System;

using Orleans;
using Orleans.Runtime.Configuration;

using GettingStarted.Interfaces;

namespace StandAlone.Host
{
    /// <summary>
    /// Orleans test silo host
    /// </summary>
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Waiting for Orleans Silo to start. Press Enter to proceed...");
            Console.ReadLine();

            // Orleans comes with a rich XML and programmatic configuration. Here we're just going to set up with basic programmatic config
            var config = ClientConfiguration.LocalhostSilo(30000);

            GrainClient.Initialize(config);

            var friend = GrainClient.GrainFactory.GetGrain<IGrainGS>(0);

            var result = friend.HellowWorld().Result;
            Console.WriteLine(result);

            result = friend.HellowWorld().Result;
            friend.Increment();
            Console.WriteLine(result);
        }
    }
}
