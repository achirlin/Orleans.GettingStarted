using System;

using Orleans;
using Orleans.Runtime.Configuration;
using Orleans.Runtime.Host;
using Interaction.Interfaces;

namespace Interaction.Host
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

			var msft = factory.GetGrain<IStockGrain>("MSFT");

			var price = msft.GetPrice().Result;

			Console.WriteLine(price);
			Console.ReadLine();
		}
	}
}
