using System;
using Persistence.Interfaces;
using Orleans;
using Orleans.Runtime.Configuration;
using Orleans.Runtime.Host;

namespace Persistence.Host
{
	/// <summary>
	/// Orleans test silo host
	/// </summary>
	public class Program
	{
		static void Main(string[] args)
		{
			// First, configure and start a local silo
			var siloConfig = ClusterConfiguration.LocalhostPrimarySilo();

			var silo = new SiloHost("TestSilo", siloConfig);
			silo.InitializeOrleansSilo();
			silo.StartOrleansSilo();

			Console.WriteLine("Silo started.");

			// Then configure and connect a client.
			var clientConfig = ClientConfiguration.LocalhostSilo();
			var client = new ClientBuilder().UseConfiguration(clientConfig).Build();
			client.Connect().Wait();

			Console.WriteLine("Client connected.");

			//---------------------------------------------------------------------------------------
			// Obtain a grain, invoke message and get result
			//---------------------------------------------------------------------------------------
			var e0 = client.GetGrain<IEmployee>("e0");
			var e1 = client.GetGrain<IEmployee>("e1");
			var e2 = client.GetGrain<IEmployee>("e2");
			var e3 = client.GetGrain<IEmployee>("e3");
			var e4 = client.GetGrain<IEmployee>("e4");

			var m0 = client.GetGrain<IManager>("m0");
			var m1 = client.GetGrain<IManager>("m1");

			var m0e = m0.AsEmployee().Result;
			var m1e = m1.AsEmployee().Result;

			m0e.Promote(10);
			m1e.Promote(11);

			m0.AddEmployee(e0).Wait();
			m0.AddEmployee(e1).Wait();
			m0.AddEmployee(e2).Wait();

			m1.AddEmployee(m0e).Wait();
			m1.AddEmployee(e3).Wait();
			m1.AddEmployee(e4).Wait();

			var m0Team = m0.GetDirectReports().Result;

			Console.WriteLine("\nPress Enter to terminate...");
			Console.ReadLine();

			// Shut down
			client.Close();
			silo.ShutdownOrleansSilo();
		}
	}
}
