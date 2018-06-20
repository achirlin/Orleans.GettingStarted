using System.Threading.Tasks;
using Orleans;

namespace CollOfActors.Interfaces
{
    public interface IEmployee : IGrainWithGuidKey
    {
        Task<int> GetLevel();
        Task Promote(int newLevel);
        Task<IManager> GetManager();
        Task SetManager(IManager manager);
    }
}
