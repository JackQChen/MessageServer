using System;

namespace  QueryException
{
	/// <summary>
	/// ExceptionConst 的摘要说明。
	/// </summary>
	public class ExceptionConst
	{
		public ExceptionConst()
		{
			//
			// TODO: 在此处添加构造函数逻辑
			//
		}
		public const string RES_EXCEPTION_LOADING_CONFIGURATION = "Error loading exceptionManagement configuration.";
		public const string RES_CUSTOM_PUBLISHER_FAILURE_MESSAGE = "Custom Publisher failed to execute.";
		public const string RES_EXCEPTIONMANAGEMENT_PERMISSION_DENIED = "Permission Denied";
		public const string RES_EXCEPTIONMANAGEMENT_INFOACCESS_EXCEPTION = "Information could not be accessed.";
		public const string RES_EXCEPTIONMANAGEMENT_XMLSERIALIZATION_EXCEPTION = "Exception Manager could not serialize the Exception information into XML.";
		public const string RES_DEFAULTPUBLISHER_EVENTLOG_DENIED = "The event source {0} does not exist and cannot be created with the current permissions.";
		public const string RES_EXCEPTIONMANAGER_INTERNAL_EXCEPTIONS = "ExceptionManagerInternalException";
		public const string RES_EXCEPTIONMANAGER_PUBLISHED_EXCEPTIONS = "ExceptionManagerPublishedException";
		public const string RES_XML_ROOT = "ExceptionInformation";
		public const string RES_XML_ADDITIONAL_INFORMATION = "AdditionalInformationProperty";
		public const string RES_XML_EXCEPTION = "Exception";
		public const string RES_XML_STACK_TRACE = "StackTrace";
	}
}
