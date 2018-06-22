using System.Threading.Tasks;
using Orleans;

namespace Persistence.Interfaces
{
    public interface IEmployee : IGrainWithStringKey
    {
        Task<int> GetLevel();
        Task Promote(int newLevel);
        Task<IManager> GetManager();
        Task SetManager(IManager manager);
        Task Greeting(GreetingData data);
    }
}
