using System.Threading.Tasks;
using Orleans;
using Persistence.Interfaces;
using System;
using Orleans.Concurrency;
using Orleans.Providers;

namespace Persistence.Grains
{
	[Reentrant]
	[StorageProvider(ProviderName = "FileStore")]
	public class Employee : Grain<EmployeeState>, IEmployee
	{
		public Task<int> GetLevel()
		{
			return Task.FromResult(State.Level);
		}

		public Task Promote(int newLevel)
		{
			State.Level = newLevel;
			return WriteStateAsync();
		}

		public Task<IManager> GetManager()
		{
			return Task.FromResult(State.Manager);
		}

		public Task SetManager(IManager manager)
		{
			State.Manager = manager;
			return WriteStateAsync();
		}

		public async Task Greeting(GreetingData data)
		{
			Console.WriteLine("{0} said: {1}", data.From, data.Message);

			// stop this from repeating endlessly
			if (data.Count >= 3)
				return;

			// send a message back to the sender
			// acknowlge the greeting with the message
			await GrainFactory
				.GetGrain<IEmployee>(data.From)
				.Greeting(new GreetingData
				{
					From = this.GetPrimaryKeyString(),
					Message = "Thanks!",
					Count = data.Count + 1
				});
		}
	}
}
