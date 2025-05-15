namespace WF.Mimetic.UI.Server.Controllers.Endpoints;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WF.Mimetic.Application.DTO.Flows.FlowResults;
using WF.Mimetic.Application.Interfaces.Listeners;

[Route("api/dynamic/")]
[ApiController]
[ApiExplorerSettings(IgnoreApi = true)]
public class DeleteListenerController : ControllerBase
{
    private readonly IListenerAppService _gateListener;

    public DeleteListenerController(IListenerAppService gateListener)
    {
        _gateListener = gateListener;
    }

    [HttpDelete("{first}")]
    public Task<ContentResult> ListenDelete(string first, [FromQuery] IDictionary<string, string> query)
    {
        return RunDeleteListenner(query, first);
    }

    [HttpDelete("{first}/{second}")]
    public Task<ContentResult> ListenDelete(string first, string second, [FromQuery] IDictionary<string, string> query)
    {
        return RunDeleteListenner(query, first, second);
    }

    [HttpDelete("{first}/{second}/{third}")]
    public Task<ContentResult> ListenDelete(string first, string second, string third, [FromQuery] IDictionary<string, string> query)
    {
        return RunDeleteListenner(query, first, second, third);
    }

    [HttpDelete("{first}/{second}/{third}/{fourth}")]
    public Task<ContentResult> ListenDelete(string first, string second, string third, string fourth, [FromQuery] IDictionary<string, string> query)
    {
        return RunDeleteListenner(query, first, second, third, fourth);
    }

    [HttpDelete("{first}/{second}/{third}/{fourth}/{fifth}")]
    public Task<ContentResult> ListenDelete(string first, string second, string third, string fourth, string fifth, [FromQuery] IDictionary<string, string> query)
    {
        return RunDeleteListenner(query, first, second, third, fourth, fifth);
    }

    [HttpDelete("{first}/{second}/{third}/{fourth}/{fifth}/{sixth}")]
    public Task<ContentResult> ListenDelete(string first, string second, string third, string fourth, string fifth, string sixth, [FromQuery] IDictionary<string, string> query)
    {
        return RunDeleteListenner(query, first, second, third, fourth, fifth, sixth);
    }

    [HttpDelete("{first}/{second}/{third}/{fourth}/{fifth}/{sixth}/{seventh}")]
    public Task<ContentResult> ListenDelete(string first, string second, string third, string fourth, string fifth, string sixth, string seventh, [FromQuery] IDictionary<string, string> query)
    {
        return RunDeleteListenner(query, first, second, third, fourth, fifth, sixth, seventh);
    }

    [HttpDelete("{first}/{second}/{third}/{fourth}/{fifth}/{sixth}/{seventh}/{eighth}")]
    public Task<ContentResult> ListenDelete(string first, string second, string third, string fourth, string fifth, string sixth, string seventh, string eighth, [FromQuery] IDictionary<string, string> query)
    {
        return RunDeleteListenner(query, first, second, third, fourth, fifth, sixth, seventh, eighth);
    }

    private async Task<ContentResult> RunDeleteListenner(IDictionary<string, string> query, params string[] segments)
    {
        FlowResultReadDto result = await _gateListener.ListenDelete(query, segments);
        return new ContentResult()
        {
            StatusCode = result.StatusCode,
            Content = result.Content,
            ContentType = result.ContentType
        };
    }
}
