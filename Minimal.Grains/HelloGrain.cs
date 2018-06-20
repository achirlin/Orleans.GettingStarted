using System.Threading.Tasks;
using Orleans;
using Minimal.Interfaces;

namespace Minimal.Grains
{
    class HelloGrain : Orleans.Grain, IHello
    {
        public Task<string> SayHello(string msg)
        {
            return Task.FromResult($"You said {msg}, I say: Hello!");
        }
    }
}
