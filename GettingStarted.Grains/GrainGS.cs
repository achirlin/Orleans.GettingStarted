using System.Threading.Tasks;
using Orleans;
using GettingStarted.Interfaces;

namespace GettingStarted.Grains
{
    public class GrainGS : Grain, IGrainGS
    {
        int MyProperty { get; set; }

        public Task<string> HellowWorld()
        {
            return Task.FromResult($"Hello World: {MyProperty}");
        }

        public Task<int> Increment()
        {
            ++MyProperty;
            return Task.FromResult(MyProperty);
        }

    }
}
