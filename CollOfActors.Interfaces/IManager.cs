using System.Collections.Generic;
using System.Threading.Tasks;
using Orleans;

namespace CollOfActors.Interfaces
{
    public interface IManager : IGrainWithStringKey
    {
        Task<IEmployee> AsEmployee();
        Task<List<IEmployee>> GetDirectReports();
        Task AddEmployee(IEmployee employee);
    }

}
