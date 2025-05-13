namespace WF.Mimetic.UI.Server.Controllers.Endpoints;

using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using WF.Mimetic.Application.DTO.Flows.FlowResults;
using WF.Mimetic.Application.Interfaces.Listeners;

[Route("api/dynamic/")]
[ApiController]
[ApiExplorerSettings(IgnoreApi = true)]
public class PostListenerController : ControllerBase
{
    private readonly IListenerAppService _gateListener;

    public PostListenerController(IListenerAppService gateListener)
    {
        _gateListener = gateListener;
    }

    [HttpPost("{first}")]
    public Task<ContentResult> ListenPost(string first, [FromQuery] IDictionary<string, string> query)
    {
        return RunPostListenner(query, first);
    }

    [HttpPost("{first}/{second}")]
    public Task<ContentResult> ListenPost(string first, string second, [FromQuery] IDictionary<string, string> query)
    {
        return RunPostListenner(query, first, second);
    }

    [HttpPost("{first}/{second}/{third}")]
    public Task<ContentResult> ListenPost(string first, string second, string third, [FromQuery] IDictionary<string, string> query)
    {
        return RunPostListenner(query, first, second, third);
    }

    [HttpPost("{first}/{second}/{third}/{fourth}")]
    public Task<ContentResult> ListenPost(string first, string second, string third, string fourth, [FromQuery] IDictionary<string, string> query)
    {
        return RunPostListenner(query, first, second, third, fourth);
    }

    [HttpPost("{first}/{second}/{third}/{fourth}/{fifth}")]
    public Task<ContentResult> ListenPost(string first, string second, string third, string fourth, string fifth, [FromQuery] IDictionary<string, string> query)
    {
        return RunPostListenner(query, first, second, third, fourth, fifth);
    }

    [HttpPost("{first}/{second}/{third}/{fourth}/{fifth}/{sixth}")]
    public Task<ContentResult> ListenPost(string first, string second, string third, string fourth, string fifth, string sixth, [FromQuery] IDictionary<string, string> query)
    {
        return RunPostListenner(query, first, second, third, fourth, fifth, sixth);
    }

    [HttpPost("{first}/{second}/{third}/{fourth}/{fifth}/{sixth}/{seventh}")]
    public Task<ContentResult> ListenPost(string first, string second, string third, string fourth, string fifth, string sixth, string seventh, [FromQuery] IDictionary<string, string> query)
    {
        return RunPostListenner(query, first, second, third, fourth, fifth, sixth, seventh);
    }

    [HttpPost("{first}/{second}/{third}/{fourth}/{fifth}/{sixth}/{seventh}/{eighth}")]
    public Task<ContentResult> ListenPost(string first, string second, string third, string fourth, string fifth, string sixth, string seventh, string eighth, [FromQuery] IDictionary<string, string> query)
    {
        return RunPostListenner(query, first, second, third, fourth, fifth, sixth, seventh, eighth);
    }

    private async Task<ContentResult> RunPostListenner(IDictionary<string, string> query, params string[] segments)
    {
        string body = await new StreamReader(Request.Body).ReadToEndAsync();
        FlowResultReadDto result = await _gateListener.ListenPost(body, query, segments);
        return new ContentResult()
        {
            StatusCode = result.StatusCode,
            Content = result.Content,
            ContentType = result.ContentType
        };
    }
}
