namespace WF.Mimetic.Domain.Models.Flows;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WF.Mimetic.Domain.Core.Models.EntityLists;
using WF.Mimetic.Domain.Core.Models.Exceptions;
using WF.Mimetic.Domain.Interfaces.Rules;
using WF.Mimetic.Domain.Models.Nodes;

public class Graph : Flow
{
    public static Graph Default => new Graph();

    private readonly IEntityList<Relation> _relations;

    public override FlowType Type => FlowType.Graph;

    public IEnumerable<Relation> Relations => _relations.AsEnumerable();

    private Graph() : base(Guid.Empty)
    {
        _relations = new EntityList<Relation>();
    }

    private Graph(Guid id) : base(id)
    {
        _relations = new EntityList<Relation>();
    }

    public static Graph Create(Guid id)
    {
        if (id.Equals(Guid.Empty))
        {
            throw new InvalidValueException("The graph id cant be empty.");
        }

        return new Graph(id);
    }

    public Relation GetRelation(Guid relationId)
    {
        return _relations.GetByIdOrDefault(relationId);
    }

    public Relation GetRelationOrThorw(Guid relationId)
    {
        Relation relation = _relations.GetByIdOrDefault(relationId);

        if (relation is null)
        {
            throw new ValueNotFoundException("The relation doesnt belogn to the flow.");
        }

        return relation;
    }

    public void AddRelation(Guid id, Node origin, Node destination)
    {
        if (origin is null)
        {
            throw new WrongOperationException("The origin node cant be null.");
        }

        if (destination is null)
        {
            throw new WrongOperationException("The destination node cant be null.");
        }

        if (!Nodes.Contains(origin))
        {
            throw new WrongOperationException("The origin node doesnt belong to the node.");
        }

        if (!Nodes.Contains(destination))
        {
            throw new WrongOperationException("The destination node doesnt belong to the node.");
        }

        Relation relation = Relation.Create(id, origin, destination);
        origin.AddRelation(relation);
        _relations.Add(relation);
    }

    public void RemoveRelation(Relation relation)
    {
        if (relation is null)
        {
            throw new WrongOperationException("The relation cant be null.");
        }

        if (!_relations.Contains(relation))
        {
            throw new WrongOperationException("The relation doesnt belong to the node.");
        }

        Node origin = GetNodeOrThorw(relation.OriginId);
        origin.RemoveRelation(relation);
        _relations.Remove(relation);
    }

    public override Task<FlowResult> Run(IRulesEngine rulesEngine, string msg)
    {
        throw new NotImplementedException();
    }
}
