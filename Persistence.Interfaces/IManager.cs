using System.Collections.Generic;
using System.Threading.Tasks;
using Orleans;

namespace Persistence.Interfaces
{
    public interface IManager : IGrainWithStringKey
    {
        Task<IEmployee> AsEmployee();
        Task<List<IEmployee>> GetDirectReports();
        Task AddEmployee(IEmployee employee);
    }

}
