using System;

namespace FastGenerator
{
	public class TiaException : Exception
	{
		public TiaException()
		{
        
		}

		public TiaException(string message) : base(message)
		{
		}

		public TiaException(string message, Exception inner) : base(message, inner)
		{
        
		}
	}
}