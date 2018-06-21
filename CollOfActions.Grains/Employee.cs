using System.Threading.Tasks;
using Orleans;
using CollOfActors.Interfaces;
using System;
using Orleans.Concurrency;

namespace CollOfActors.Grains
{
	[Reentrant]
	public class Employee : Grain, IEmployee
	{
		private int _level;
		private IManager _manager;

		public Task<int> GetLevel()
		{
			return Task.FromResult(_level);
		}

		public Task Promote(int newLevel)
		{
			_level = newLevel;
			return Task.CompletedTask;
		}

		public Task<IManager> GetManager()
		{
			return Task.FromResult(_manager);
		}

		public Task SetManager(IManager manager)
		{
			_manager = manager;
			return Task.CompletedTask;
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
