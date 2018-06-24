using Persistence.Interfaces;

namespace Persistence.Grains
{
	public class EmployeeState
	{
		public int Level { get; set; }
		public IManager Manager { get; set; }
	}
}