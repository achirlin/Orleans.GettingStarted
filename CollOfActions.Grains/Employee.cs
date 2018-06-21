using System.Threading.Tasks;
using Orleans;
using CollOfActors.Interfaces;
using System;

namespace CollOfActors.Grains
{
    public class Employee : Grain, IEmployee
    {
        private int _level;
        private IManager _manager;

        public Task<int> GetLevel()
        {
            return Task.FromResult(_level);
        }

        public Task Promote(int newLevel)
        {
            _level = newLevel;
            return Task.CompletedTask;
        }

        public Task<IManager> GetManager()
        {
            return Task.FromResult(_manager);
        }

        public Task SetManager(IManager manager)
        {
            _manager = manager;
            return Task.CompletedTask;
        }

        public Task Greeting(IEmployee from, string message)
        {

            Console.WriteLine("{0} said: {1}", from.GetPrimaryKeyString().ToString(), message);

            return Task.CompletedTask;
        }
    }
}
