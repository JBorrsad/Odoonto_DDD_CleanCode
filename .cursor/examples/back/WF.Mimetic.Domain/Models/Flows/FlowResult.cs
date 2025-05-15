namespace WF.Mimetic.Domain.Models.Flows;

public class FlowResult
{
    public static FlowResult Default => new FlowResult();

    public int? StatusCode { get; set; }
    public string Content { get; set; }
    public string ContentType { get; set; }

    private FlowResult()
    {
        StatusCode = null;
        Content = null;
        ContentType = null;
    }

    public FlowResult(int? statusCode)
    {
        StatusCode = statusCode;
        Content = null;
        ContentType = null;
    }

    public FlowResult(int? statusCode, string content)
    {
        StatusCode = statusCode;
        Content = content;
        ContentType = "text/plain";
    }

    public FlowResult(int? statusCode, string content, string contentType)
    {
        StatusCode = statusCode;
        Content = content;
        ContentType = contentType;
    }
}
