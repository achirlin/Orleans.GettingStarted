using CollOfActors.Interfaces;
using Orleans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollOfActors.Grains
{
    public class Manager : Grain, IManager
    {
        private IEmployee _me;
        private List<IEmployee> _reports = new List<IEmployee>();

        public override Task OnActivateAsync()
        {
            _me = this.GrainFactory.GetGrain<IEmployee>(this.GetPrimaryKey());
            return base.OnActivateAsync();
        }

        public Task<List<IEmployee>> GetDirectReports()
        {
            return Task.FromResult(_reports);
        }

        public Task AddDirectReport(IEmployee employee)
        {
            _reports.Add(employee);
            employee.SetManager(this);
            return Task.CompletedTask;
        }

        public Task<IEmployee> AsEmployee()
        {
            return Task.FromResult(_me);
        }

    }
}
