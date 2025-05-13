namespace WF.Mimetic.Application.Services.Rules;

using WF.Mimetic.Application.DTO.Engine;
using WF.Mimetic.Application.Interfaces.Rules;
using WF.Mimetic.Domain.Core.Models.Exceptions;
using WF.Mimetic.Domain.Interfaces.Rules;
using WF.Mimetic.Domain.Models.Nodes;
using WF.Mimetic.Domain.Models.Rules;

public class EngineAppService : IEngineAppService
{
    private readonly IRulesEngine _engine;

    public EngineAppService(IRulesEngine engine)
    {
        _engine = engine;
    }

    public string RunScript(ScriptRunDto script)
    {
        if (script is null)
        {
            throw new InvalidValueException("The script cant be null.");
        }

        if (!_engine.IsValidScript(script.Script))
        {
            throw new InvalidValueException("The script is not valid.");
        }

        string msg = Moustacher.WrapJson("msg", script.Payload);

        return _engine.Execute(script.Script, msg);
    }
}
