using System.Collections.Generic;
using System.Threading.Tasks;
using Orleans;

namespace CollOfActors.Interfaces
{
    public interface IManager : IGrainWithGuidKey
    {
        Task<IEmployee> AsEmployee();
        Task<List<IEmployee>> GetDirectReports();
        Task AddDirectReport(IEmployee employee);
    }

}
