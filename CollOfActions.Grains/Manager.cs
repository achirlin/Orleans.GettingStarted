using CollOfActors.Interfaces;
using Orleans;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CollOfActors.Grains
{
	public class Manager : Grain, IManager
	{
		private IEmployee _me;
		private List<IEmployee> _reports = new List<IEmployee>();

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
			return Task.FromResult(_reports);
		}

		public async Task AddEmployee(IEmployee employee)
		{
			_reports.Add(employee);

			await employee.SetManager(this);

			await employee.Greeting(new GreetingData
					{
						From = _me.GetPrimaryKeyString(),
						Message = "Welcome to my team!"
			});
		}

		public Task<IEmployee> AsEmployee()
		{
			return Task.FromResult(_me);
		}

	}
}
