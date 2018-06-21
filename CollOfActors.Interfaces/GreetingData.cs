using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollOfActors.Interfaces
{
	public class GreetingData
	{
		/// <summary>
		/// The ID of grain sending the message
		/// </summary>
		public string From { get; set; }

		public string Message { get; set; }
		
		/// <summary>
		/// Count will be the number of times the message has been sent back and forth
		/// </summary>
		public int Count { get; set; }
	}
}
