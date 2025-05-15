namespace WF.Mimetic.Domain.Models.Nodes;

using HandlebarsDotNet;
using HandlebarsDotNet.Extension.NewtonsoftJson;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using WF.Mimetic.Domain.Core.Models.Exceptions;
using WF.Mimetic.Domain.Interfaces.Rules;
using WF.Mimetic.Domain.Models.Flows;
using WF.Mimetic.Domain.Models.Rules;

public class Printer : Node
{
    public static Printer Default => new Printer();

    public override NodeType Type => NodeType.Printer;

    private Printer() : base(Guid.Empty, Guid.Empty)
    {
    }

    private Printer(Guid id, Guid flowId) : base(id, flowId)
    {
    }

    internal static Printer Create(Guid id, Flow flow)
    {
        if (Guid.Empty.Equals(id))
        {
            throw new InvalidValueException("The scripter id cant be empty.");
        }

        if (flow is null)
        {
            throw new WrongOperationException("The flow cant be null.");
        }

        return new Printer(id, flow.Id);
    }

    public override Task<string> Run(IRulesEngine rulesEngine, string msg)
    {
        if (msg is null)
        {
            throw new WrongOperationException("The msg cant be null.");
        }

        string result = Moustacher.PrintFromJson(msg, Script);

        return Task.FromResult(Moustacher.SetStringToJson(result, "msg", msg));
    }
}
