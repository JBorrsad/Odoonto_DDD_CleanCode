namespace WF.Mimetic.Domain.Models.Nodes;

using System;
using System.Threading.Tasks;
using WF.Mimetic.Domain.Core.Models.Exceptions;
using WF.Mimetic.Domain.Interfaces.Rules;
using WF.Mimetic.Domain.Models.Flows;

public class Scripter : Node
{
    public static Scripter Default => new Scripter();

    public override NodeType Type => NodeType.Scripter;

    private Scripter() : base(Guid.Empty, Guid.Empty)
    {
    }

    private Scripter(Guid id, Guid flowId) : base(id, flowId)
    {
    }

    internal static Scripter Create(Guid id, Flow flow)
    {
        if (Guid.Empty.Equals(id))
        {
            throw new InvalidValueException("The scripter id cant be empty.");
        }

        if (flow is null)
        {
            throw new WrongOperationException("The flow cant be null.");
        }

        return new Scripter(id, flow.Id);
    }

    public override Task<string> Run(IRulesEngine rulesEngine, string msg)
    {
        if (msg is null)
        {
            throw new WrongOperationException("The msg cant be null.");
        }

        if (!rulesEngine.IsValidScript(Script))
        {
            throw new WrongOperationException("The script is not valid.");
        }

        string result = rulesEngine.Execute(Script, msg);

        if (Moustacher.IsJson(result))
        {
            return Task.FromResult(Moustacher.SetJsonToJson(result, "msg", msg));
        }

        return Task.FromResult(Moustacher.SetStringToJson(result, "msg", msg));
    }
}
