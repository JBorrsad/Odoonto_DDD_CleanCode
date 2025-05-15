namespace WF.Mimetic.Domain.Models.Nodes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WF.Mimetic.Domain.Core.Models.EntityLists;
using WF.Mimetic.Domain.Core.Models.Exceptions;
using WF.Mimetic.Domain.Interfaces.Rules;
using WF.Mimetic.Domain.Models.Parameters;

public abstract class Node : ParameterAggregate
{
    private readonly IEntityList<Relation> _relations;

    public Guid FlowId { get; private set; }
    public string Name { get; private set; }
    public string Script { get; private set; }
    public bool IsActived { get; private set; }

    public IEnumerable<Relation> Relations => _relations.AsEnumerable();

    public abstract NodeType Type { get; }

    protected Node(Guid id, Guid flowId) : base(id)
    {
        FlowId = flowId;
        Name = null;
        Script = null;
        IsActived = true;
        _relations = new EntityList<Relation>();
        
    }

    public void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new InvalidValueException("The step name can't be null/empty.");
        }

        Name = name;
    }

    public void SetScript(string script)
    {
        Script = script;
    }

    public void SetIsActived(bool isActived)
    {
        IsActived = isActived;
    }

    public abstract Task<string> Run(IRulesEngine rulesEngine, string msg);

    internal void AddRelation(Relation relation)
    {
        if (relation is null)
        {
            throw new WrongOperationException("The relation cant be null.");
        }

        _relations.Add(relation);
    }

    internal void RemoveRelation(Relation relation)
    {
        if (relation is null)
        {
            throw new WrongOperationException("The relation cant be null.");
        }

        if (!_relations.Contains(relation))
        {
            throw new WrongOperationException("The relation doesnt belong to the node.");
        }

        _relations.Remove(relation);
    }
}
