using System;
using System.Collections.Specialized;
using System.Xml;

namespace QueryException
{
	#region Publishing Interfaces
	/// <summary>
	/// Interface to publish exception information.  All exception information is passed as the chain of exception objects.
	/// </summary>
	public interface IExceptionPublisher
	{
		/// <summary>
		/// Method used to publish exception information and additional information.
		/// </summary>
		/// <param name="exception">The exception object whose information should be published.</param>
		/// <param name="additionalInfo">A collection of additional data that should be published along with the exception information.</param>
		/// <param name="configSettings">A collection of name/value attributes specified in the config settings.</param>
		void Publish(Exception exception, NameValueCollection additionalInfo, NameValueCollection configSettings);
	}

	/// <summary>
	/// Interface to publish exception information.  All exception information is passed as XML.
	/// </summary>
	public interface IExceptionXmlPublisher
	{
		/// <summary>
		/// Method used to publish exception information and any additional information in XML.
		/// </summary>
		/// <param name="exceptionInfo">An XML Document containing the all exception information.</param>
		/// <param name="configSettings">A collection of name/value attributes specified in the config settings.</param>
		void Publish(XmlDocument exceptionInfo, NameValueCollection configSettings);
	}
	#endregion
}
