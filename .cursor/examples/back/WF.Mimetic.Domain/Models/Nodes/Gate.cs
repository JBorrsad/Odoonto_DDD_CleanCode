using System;
using System.Linq;
using System.Threading.Tasks;
using WF.Mimetic.Domain.Core.Models.Exceptions;
using WF.Mimetic.Domain.Interfaces.Rules;
using WF.Mimetic.Domain.Models.Flows;
using WF.Mimetic.Domain.Shared;

namespace WF.Mimetic.Domain.Models.Nodes;

public class Gate : Node
{
    public static Gate Default => new Gate();
    public static string RouteRoot => "/api/dynamic";

    public string Route { get; private set; }
    public Method Method { get; private set; }

    public override NodeType Type => NodeType.Gate;

    private Gate() : base(Guid.Empty, Guid.Empty)
    {
    }

    private Gate(Guid id, Guid flowId) : base(id, flowId)
    {
    }

    internal static Gate Create(Guid id, Flow flow)
    {
        if (Guid.Empty.Equals(id))
        {
            throw new InvalidValueException("The gate id cant be empty.");
        }

        if (flow is null)
        {
            throw new WrongOperationException("The flow cant be null.");
        }

        return new Gate(id, flow.Id);
    }

    public void SetRoute(string route)
    {
        if (route.First() != '/')
        {
            route = '/' + route;
        }

        if(!RouteValidator.ValidateRoute(route))
        {
            throw new InvalidValueException("The route is not valid.");
        }

        if (route.First() != '/')
        {
            route = '/' + route;
        }

        Route = route;
    }

    public void SetMethod(Method method)
    {
        if (Method.None.Equals(method))
        {
            throw new InvalidValueException("The gate method is not valid.");
        }

        Method = method;
    }

    public override Task<string> Run(IRulesEngine rulesEngine, string msg)
    {
        if (msg is null)
        {
            throw new WrongOperationException("The msg cant be null.");
        }

        return Task.FromResult(msg);
    }
}
