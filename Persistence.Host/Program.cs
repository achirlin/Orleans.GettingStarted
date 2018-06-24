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
			// Then configure and connect a client.
			var clientConfig = ClientConfiguration.LocalhostSilo(30000);
			GrainClient.Initialize(clientConfig);

			var factory = GrainClient.GrainFactory;

			Console.WriteLine("Client connected.");

			//---------------------------------------------------------------------------------------
			// Obtain a grain, invoke message and get result
			//---------------------------------------------------------------------------------------
			var e0 = factory.GetGrain<IEmployee>("e0");
			var e1 = factory.GetGrain<IEmployee>("e1");
			var e2 = factory.GetGrain<IEmployee>("e2");
			var e3 = factory.GetGrain<IEmployee>("e3");
			var e4 = factory.GetGrain<IEmployee>("e4");

			var m0 = factory.GetGrain<IManager>("m0");
			var m1 = factory.GetGrain<IManager>("m1");

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

		}
	}
}
