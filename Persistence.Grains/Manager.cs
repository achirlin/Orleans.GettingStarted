using Persistence.Interfaces;
using Orleans;
using System.Collections.Generic;
using System.Threading.Tasks;
using Orleans.Providers;

namespace Persistence.Grains
{
	/// <summary>
	/// 
	/// </summary>
	/// <remarks>
	/// The state will be loaded from storage and then OnActivateAsync is called so you can be sure that the state is loaded 
	/// when initializing your grain.
	/// Also in case of failure:
	/// http://dotnet.github.io/orleans/1.5/Tutorials/Declarative-Persistence.html#handling-failures-using-persistence
	/// </remarks>
	[StorageProvider(ProviderName = "FileStore")]
	public class Manager : Grain<ManagerState>, IManager
	{
		private IEmployee _me;

		public override Task OnActivateAsync()
		{
			// Therefore, the implementation should catch the activation event (a call to OnActivateAsync()) 
			// and perform any initialization steps necessary there.It is guaranteed to be called before any
			// method on the grain instance is called. In the Manager grain above, it was used to  establish
			// the reference to the Employee grain.

			// Each grain has a unique identity, also referred to as a grain key, within its type.
			_me = this.GrainFactory.GetGrain<IEmployee>(this.GetPrimaryKeyString());
			return base.OnActivateAsync();
		}

		public override Task OnDeactivateAsync()
		{
			// just to be aware of
			// http://dotnet.github.io/orleans/1.5/Tutorials/A-Service-is-a-Collection-of-Communicating-Actors.html#the-life-of-an-actor
			return base.OnDeactivateAsync();
		}

		public Task<List<IEmployee>> GetDirectReports()
		{
			return Task.FromResult(State.Reports);
		}

		public async Task AddEmployee(IEmployee employee)
		{
			if (State.Reports == null)
				State.Reports = new List<IEmployee>();

			State.Reports.Add(employee);

			await employee.SetManager(this);

			await employee.Greeting(new GreetingData
					{
						From = _me.GetPrimaryKeyString(),
						Message = "Welcome to my team!"
			});

			await WriteStateAsync();
		}

		public Task<IEmployee> AsEmployee()
		{
			return Task.FromResult(_me);
		}
	}
}
