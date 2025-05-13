namespace WF.Mimetic.UI.Server.Controllers.Endpoints;

using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WF.Mimetic.Application.DTO.Flows.FlowResults;
using WF.Mimetic.Application.Interfaces.Listeners;

[Route("api/dynamic/")]
[ApiController]
[ApiExplorerSettings(IgnoreApi = true)]
public class GetListenerController : ControllerBase
{
    private readonly IListenerAppService _gateListener;

    public GetListenerController(IListenerAppService gateListener)
    {
        _gateListener = gateListener;
    }

    [HttpGet("{first}")]
    public Task<ContentResult> ListenGet(string first, [FromQuery] IDictionary<string, string> query)
    {
        return RunGetListenner(query, first);
    }

    [HttpGet("{first}/{second}")]
    public Task<ContentResult> ListenGet(string first, string second, [FromQuery] IDictionary<string, string> query)
    {
        return RunGetListenner(query, first, second);
    }

    [HttpGet("{first}/{second}/{third}")]
    public Task<ContentResult> ListenGet(string first, string second, string third, [FromQuery] IDictionary<string, string> query)
    {
        return RunGetListenner(query, first, second, third);
    }

    [HttpGet("{first}/{second}/{third}/{fourth}")]
    public Task<ContentResult> ListenGet(string first, string second, string third, string fourth, [FromQuery] IDictionary<string, string> query)
    {
        return RunGetListenner(query, first, second, third, fourth);
    }

    [HttpGet("{first}/{second}/{third}/{fourth}/{fifth}")]
    public Task<ContentResult> ListenGet(string first, string second, string third, string fourth, string fifth, [FromQuery] IDictionary<string, string> query)
    {
        return RunGetListenner(query, first, second, third, fourth, fifth);
    }

    [HttpGet("{first}/{second}/{third}/{fourth}/{fifth}/{sixth}")]
    public Task<ContentResult> ListenGet(string first, string second, string third, string fourth, string fifth, string sixth, [FromQuery] IDictionary<string, string> query)
    {
        return RunGetListenner(query, first, second, third, fourth, fifth, sixth);
    }

    [HttpGet("{first}/{second}/{third}/{fourth}/{fifth}/{sixth}/{seventh}")]
    public Task<ContentResult> ListenGet(string first, string second, string third, string fourth, string fifth, string sixth, string seventh, [FromQuery] IDictionary<string, string> query)
    {
        return RunGetListenner(query, first, second, third, fourth, fifth, sixth, seventh);
    }

    [HttpGet("{first}/{second}/{third}/{fourth}/{fifth}/{sixth}/{seventh}/{eighth}")]
    public Task<ContentResult> ListenGet(string first, string second, string third, string fourth, string fifth, string sixth, string seventh, string eighth, [FromQuery] IDictionary<string, string> query)
    {
        return RunGetListenner(query, first, second, third, fourth, fifth, sixth, seventh, eighth);
    }

    private async Task<ContentResult> RunGetListenner(IDictionary<string, string> query, params string[] segments)
    {
        FlowResultReadDto result = await _gateListener.ListenGet(query, segments);
        return new ContentResult()
        {
            StatusCode = result.StatusCode,
            Content = result.Content,
            ContentType = result.ContentType
        };
    }
}
