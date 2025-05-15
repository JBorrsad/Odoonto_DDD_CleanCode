namespace WF.Mimetic.UI.Server.Controllers.Rules;

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Threading.Tasks;
using WF.Mimetic.Application.DTO.Engine;
using WF.Mimetic.Application.Interfaces.Rules;

[Route("api/v0/[controller]")]
[ApiController]
public class EngineController : ControllerBase
{
    private readonly IEngineAppService _engineAppService;

    public EngineController(IEngineAppService engineAppService)
    {
        _engineAppService = engineAppService;
    }

    [HttpGet]
    public Task<string> Echo(string echo)
    {
        return Task.FromResult(echo);
    }

    [HttpPost]
    public ContentResult RunScript([FromBody] ScriptRunDto script)
    {
        string content = _engineAppService.RunScript(script);

        return new ContentResult()
        {
            StatusCode = 200,
            Content = content,
            ContentType = "application/json"
        };
    }
}
