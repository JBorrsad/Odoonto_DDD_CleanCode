namespace WF.Mimetic.Domain.Models.Nodes;

using System;
using WF.Mimetic.Domain.Core.Models;
using WF.Mimetic.Domain.Core.Models.Exceptions;

public class Relation : Entity
{
    public static Relation Default => new Relation();

    public Guid FlowId { get; private set; }
    public Guid OriginId { get; private set; }
    public Guid DestinationId { get; private set; }

    public string Script { get; private set; }

    private Relation() : base(Guid.Empty)
    {
        FlowId = Guid.Empty;
        OriginId = Guid.Empty;
        DestinationId = Guid.Empty;
    }

    private Relation(Guid id, Guid flowId, Guid originId, Guid destinationId) : base(id)
    {
        FlowId = flowId;
        OriginId = originId;
        DestinationId = destinationId;
    }

    internal static Relation Create(Guid id, Node origin, Node destination)
    {
        if (Guid.Empty.Equals(id))
        {
            throw new InvalidValueException("The relation id cant be empty.");
        }

        if (origin is null)
        {
            throw new WrongOperationException("The origin node cant be null.");
        }

        if (destination is null)
        {
            throw new WrongOperationException("The destination node cant be null.");
        }

        if (origin.FlowId != destination.FlowId)
        {
            throw new WrongOperationException("The origin node flow is not equals than destination node flow.");
        }

        return new Relation(id, origin.FlowId, origin.Id, destination.Id);
    }

    public void SetScript(string script)
    {
        Script = script;
    }
}
