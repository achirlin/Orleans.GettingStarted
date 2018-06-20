using System.Threading.Tasks;
using Orleans;

namespace Minimal.Interfaces
{
    public interface IHello : Orleans.IGrainWithIntegerKey
    {
        Task<string> SayHello(string msg);
    }
}
