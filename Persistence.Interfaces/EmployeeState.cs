namespace Persistence.Interfaces
{
	public class EmployeeState
	{
		public int Level { get; set; }
		public IManager Manager { get; set; }
	}
}