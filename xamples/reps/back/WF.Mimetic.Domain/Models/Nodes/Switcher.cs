namespace WF.Mimetic.Domain.Models.Nodes;

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WF.Mimetic.Domain.Core.Models.Exceptions;
using WF.Mimetic.Domain.Interfaces.Rules;
using WF.Mimetic.Domain.Models.Flows;
using WF.Mimetic.Domain.Models.Parameters;
using WF.Mimetic.Domain.Models.Rules;

public class Switcher : Node
{
    public static Switcher Default => new Switcher();

    public override NodeType Type => NodeType.Switcher;

    private Switcher() : base(Guid.Empty, Guid.Empty)
    {
    }

    private Switcher(Guid id, Guid flowId) : base(id, flowId)
    {
    }

    internal static Switcher Create(Guid id, Flow flow)
    {
        if (Guid.Empty.Equals(id))
        {
            throw new InvalidValueException("The switcher id cant be empty.");
        }

        if (flow is null)
        {
            throw new WrongOperationException("The flow cant be null.");
        }

        return new Switcher(id, flow.Id);
    }

    public override Task<string> Run(IRulesEngine rulesEngine, string msg)
    {
        IDictionary<string, object> result = new Dictionary<string, object>();

        if (msg is null)
        {
            throw new WrongOperationException("The msg cant be null.");
        }

        foreach (Parameter parameter in Parameters)
        {
            if (!parameter.TryGetValueByTarget(msg, out object value))
            {
                value = parameter.DefaultValue;
            }

            if (IsNullRuleBreaked(value, parameter))
            {
                throw new InvalidValueException($"The parameter ({parameter.Name}) cant be null.");
            }

            result.Add(parameter.Name, value);
        }

        return Task.FromResult(Moustacher.SetObjectToJson(result, "msg", msg));
    }

    private bool IsNullRuleBreaked(object value, Parameter parameter)
    {
        return value is null && !parameter.IsNullable;
    }
}
