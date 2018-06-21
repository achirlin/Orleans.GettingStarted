using Orleans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interaction.Interfaces
{
	public interface IStockGrain : IGrainWithStringKey
	{
		Task<string> GetPrice();
	}
}
