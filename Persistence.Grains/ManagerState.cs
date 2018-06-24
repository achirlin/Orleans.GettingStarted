using System.Collections.Generic;
using Persistence.Interfaces;

namespace Persistence.Grains
{
	public class ManagerState
	{
		public List<IEmployee> Reports { get; set; }
	}
}