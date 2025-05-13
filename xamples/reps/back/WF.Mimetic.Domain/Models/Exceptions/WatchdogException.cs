namespace WF.Mimetic.Domain.Models.Exceptions;

using System;
using System.Runtime.Serialization;
using WF.Mimetic.Domain.Core.Models.Exceptions;

[Serializable]
public class WatchdogException : BusinessLogicException
{
    public override uint ExceptionCode => 2007;

    public int StatusCode { get; private set; }
    public string MediaType { get; private set; }

    public WatchdogException() : base()
    {
        StatusCode = 400;
        MediaType = "text/plain";
    }

    public WatchdogException(string message) : base(message)
    {
        StatusCode = 400;
        MediaType = "text/plain";
    }

    public WatchdogException(int statusCode, string mediaType, string message) : base(message)
    {
        StatusCode = statusCode;
        MediaType = mediaType;
    }

    public WatchdogException(string message, Exception innerException) : base(message, innerException)
    {
        StatusCode = 400;
        MediaType = "text/plain";
    }

    public WatchdogException(int statusCode, string mediaType, string message, Exception innerException) : base(message, innerException)
    {
        StatusCode = statusCode;
        MediaType = mediaType;
    }

    protected WatchdogException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
