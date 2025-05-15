namespace WF.Mimetic.Domain.Models.Flows;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Threading.Tasks;
using WF.Mimetic.Domain.Core.Models.Exceptions;
using WF.Mimetic.Domain.Interfaces.Rules;
using WF.Mimetic.Domain.Models.Nodes;

public class Pipeline : Flow
{
    public static Pipeline Default => new Pipeline();

    public bool IsPublic { get; private set; }

    public override FlowType Type => FlowType.Pipeline;

    private Pipeline() : base(Guid.Empty)
    {
    }

    private Pipeline(Guid id) : base(id)
    {
        IsPublic = false;
    }

    public static Pipeline Create(Guid id)
    {
        if (id.Equals(Guid.Empty))
        {
            throw new InvalidValueException("The pipeline id cant be empty.");
        }

        return new Pipeline(id);
    }

    public void SetIsPublic(bool isPublic)
    {
        IsPublic = isPublic;
    }

    public override void AddGate(Guid id)
    {
        if (Nodes.OfType<Gate>().Any())
        {
            throw new InvalidValueException("The pipeline already has a gate.");
        }

        base.AddGate(id);
    }

    public override async Task<FlowResult> Run(IRulesEngine rulesEngine, string msg)
    {
        if (msg is null)
        {
            throw new WrongOperationException("The msg cant be null.");
        }

        string result = await RunPipeline(rulesEngine, msg);

        if (!TryParseFlowResult(result, out FlowResult flowResult))
        {
            return new FlowResult(200);
        }

        return flowResult;
    }

    private async Task<string> RunPipeline(IRulesEngine rulesEngine, string msg)
    {
        string result = msg;

        foreach (Node node in Nodes)
        {
            if(!node.IsActived)
            {
                continue;
            }

            result = await node.Run(rulesEngine, result);

            if (node is Response)
            {
                break;
            }
        }

        return result;
    }

    private bool TryParseFlowResult(string msg, out FlowResult flowResult)
    {
        try
        {
            flowResult = JsonConvert.DeserializeObject<FlowResult>(msg);
            return true;
        }
        catch
        {
            flowResult = null;
            return false;
        }
    }

    public string GetNodeGateRoute()
    {
        Gate gate = (Gate)Nodes.FirstOrDefault(n => n is Gate);
        if(gate is null)
        {
            return string.Empty;
        }

        return gate.Route;
    }

    public Method GetNodeGateMethod()
    {
        Gate gate = (Gate)Nodes.FirstOrDefault(n => n is Gate);
        if (gate is null)
        {
            return Method.None;
        }

        return gate.Method;
    }
}
