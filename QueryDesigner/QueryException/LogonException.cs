using System;
using System.Runtime.Serialization;

namespace QueryException
{
	[Serializable]
	public class LogonException : BaseApplicationException
	{
		// Default constructor
		public LogonException() : base()
		{
		}
		// Constructor with exception message
		public LogonException(string message) : base(message)
		{
		}
		// Constructor with message and inner exception
		public LogonException(string message, Exception inner) : base(message,inner)
		{
		}
		// Protected constructor to de-serialize data
		protected LogonException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}