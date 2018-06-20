using System.Threading.Tasks;
using Orleans;

namespace GettingStarted.Interfaces
{
    public interface IGrainGS : IGrainWithIntegerKey
    {
        Task<int> Increment();

        Task<string> HellowWorld();
    }
}
