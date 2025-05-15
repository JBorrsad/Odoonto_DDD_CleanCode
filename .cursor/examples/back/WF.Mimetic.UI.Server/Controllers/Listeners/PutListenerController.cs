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
public class PutListenerController : ControllerBase
{
    private readonly IListenerAppService _gateListener;

    public PutListenerController(IListenerAppService gateListener)
    {
        _gateListener = gateListener;
    }

    [HttpPut("{first}")]
    public Task<ContentResult> ListenPut(string first, [FromQuery] IDictionary<string, string> query)
    {
        return RunPutListenner(query, first);
    }

    [HttpPut("{first}/{second}")]
    public Task<ContentResult> ListenPut(string first, string second, [FromQuery] IDictionary<string, string> query)
    {
        return RunPutListenner(query, first, second);
    }

    [HttpPut("{first}/{second}/{third}")]
    public Task<ContentResult> ListenPut(string first, string second, string third, [FromQuery] IDictionary<string, string> query)
    {
        return RunPutListenner(query, first, second, third);
    }

    [HttpPut("{first}/{second}/{third}/{fourth}")]
    public Task<ContentResult> ListenPut(string first, string second, string third, string fourth, [FromQuery] IDictionary<string, string> query)
    {
        return RunPutListenner(query, first, second, third, fourth);
    }

    [HttpPut("{first}/{second}/{third}/{fourth}/{fifth}")]
    public Task<ContentResult> ListenPut(string first, string second, string third, string fourth, string fifth, [FromQuery] IDictionary<string, string> query)
    {
        return RunPutListenner(query, first, second, third, fourth, fifth);
    }

    [HttpPut("{first}/{second}/{third}/{fourth}/{fifth}/{sixth}")]
    public Task<ContentResult> ListenPut(string first, string second, string third, string fourth, string fifth, string sixth, [FromQuery] IDictionary<string, string> query)
    {
        return RunPutListenner(query, first, second, third, fourth, fifth, sixth);
    }

    [HttpPut("{first}/{second}/{third}/{fourth}/{fifth}/{sixth}/{seventh}")]
    public Task<ContentResult> ListenPut(string first, string second, string third, string fourth, string fifth, string sixth, string seventh, [FromQuery] IDictionary<string, string> query)
    {
        return RunPutListenner(query, first, second, third, fourth, fifth, sixth, seventh);
    }

    [HttpPut("{first}/{second}/{third}/{fourth}/{fifth}/{sixth}/{seventh}/{eighth}")]
    public Task<ContentResult> ListenPut(string first, string second, string third, string fourth, string fifth, string sixth, string seventh, string eighth, [FromQuery] IDictionary<string, string> query)
    {
        return RunPutListenner(query, first, second, third, fourth, fifth, sixth, seventh, eighth);
    }

    private async Task<ContentResult> RunPutListenner(IDictionary<string, string> query, params string[] segments)
    {
        string body = await new StreamReader(Request.Body).ReadToEndAsync();
        FlowResultReadDto result = await _gateListener.ListenPut(body, query, segments);
        return new ContentResult()
        {
            StatusCode = result.StatusCode,
            Content = result.Content,
            ContentType = result.ContentType
        };
    }
}
