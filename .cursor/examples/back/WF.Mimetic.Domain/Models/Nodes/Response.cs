namespace WF.Mimetic.Domain.Models.Nodes;

using Newtonsoft.Json;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WF.Mimetic.Domain.Core.Models.Exceptions;
using WF.Mimetic.Domain.Interfaces.Rules;
using WF.Mimetic.Domain.Models.Flows;

public class Response : Node
{
    public static Response Default => new Response();

    public string StatusCode { get; private set; }
    public string MediaType { get; private set; }
    public string Content { get; private set; }

    public override NodeType Type => NodeType.Response;

    protected Response() : base(Guid.Empty, Guid.Empty)
    {
        StatusCode = string.Empty;
        Content = string.Empty;
        MediaType = "text/plain";
    }

    protected Response(Guid id, Guid flowId) : base(id, flowId)
    {
        StatusCode = string.Empty;
        Content = string.Empty;
        MediaType = "text/plain";
    }

    internal static Response Create(Guid id, Flow flow)
    {
        if (Guid.Empty.Equals(id))
        {
            throw new InvalidValueException("The response id cant be empty.");
        }

        if (flow is null)
        {
            throw new WrongOperationException("The flow cant be null.");
        }

        return new Response(id, flow.Id);
    }

    public void SetContent(string content)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            Content = string.Empty;
            return;
        }

        Content = content;
    }

    public void SetStatusCode(string statusCode)
    {
        if (string.IsNullOrWhiteSpace(statusCode))
        {
            StatusCode = string.Empty;
            return;
        }

        if (int.TryParse(statusCode, out int _))
        {
            StatusCode = statusCode;
            return;
        }

        Regex regex = new Regex(@"^[A-Za-z_][A-Za-z0-9_\-]*(\.([A-Za-z_][A-Za-z0-9_\-]*|\[[0-9]*\]))*$");

        if (!regex.IsMatch(statusCode))
        {
            throw new InvalidValueException("The status code is not valid.");
        }

        StatusCode = statusCode;
    }

    public void SetMediaType(string mediaType)
    {
        if (string.IsNullOrEmpty(mediaType))
        {
            throw new InvalidValueException("The media type cant be empty/null.");
        }

        MediaType = mediaType;
    }

    public override Task<string> Run(IRulesEngine rulesEngine, string msg)
    {
        if (msg is null)
        {
            throw new WrongOperationException("The msg cant be null.");
        }

        int statusCode = GetStatusCode(msg);
        string content = GetContent(msg);

        FlowResult result = new FlowResult(statusCode, content, MediaType);

        return Task.FromResult(JsonConvert.SerializeObject(result));
    }

    private string GetContent(string msg)
    {
        if(string.IsNullOrWhiteSpace(Content))
        {
            return "";
        }

        Regex regex = new Regex(@"^[A-Za-z_][A-Za-z0-9_\-]*(\.([A-Za-z_][A-Za-z0-9_\-]*|\[[0-9]*\]))*$");

        if (regex.IsMatch(Content))
        {
            return Moustacher.GetValueFromJson(msg, Content);
        }

        if (Content == "::global")
        {
            return msg;
        }

        return Content;
    }

    private int GetStatusCode(string msg)
    {
        if (int.TryParse(StatusCode, out int statusCode))
        {
            return statusCode;
        }

        string result = Moustacher.GetValueFromJson(msg, StatusCode);

        if (!int.TryParse(result, out statusCode))
        {
            return 200;
        }

        return statusCode;
    }
}
