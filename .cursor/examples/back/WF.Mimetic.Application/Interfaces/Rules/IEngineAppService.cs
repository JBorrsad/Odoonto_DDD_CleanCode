namespace WF.Mimetic.Application.Interfaces.Rules;

using WF.Mimetic.Application.DTO.Engine;

public interface IEngineAppService
{
    string RunScript(ScriptRunDto script);
}
