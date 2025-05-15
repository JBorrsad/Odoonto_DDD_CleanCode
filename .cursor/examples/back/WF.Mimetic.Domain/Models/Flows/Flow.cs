namespace WF.Mimetic.Domain.Models.Flows;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WF.Mimetic.Domain.Core.Models;
using WF.Mimetic.Domain.Core.Models.Exceptions;
using WF.Mimetic.Domain.Core.Models.SortedLists;
using WF.Mimetic.Domain.Interfaces.Rules;
using WF.Mimetic.Domain.Models.Nodes;
using WF.Mimetic.Domain.Models.Categories;

public abstract class Flow : SortedEntity
{
    private readonly ISortedEntityList<Node> _nodes;

    public string Name { get; private set; }
    
    public Guid? CategoryId { get; private set; }

    public abstract FlowType Type { get; }

    public IEnumerable<Node> Nodes => _nodes.AsEnumerable();

    protected Flow(Guid id) : base(id)
    {
        Name = null;
        _nodes = new SortedEntityList<Node>();
        CategoryId = null;
    }

    public abstract Task<FlowResult> Run(IRulesEngine rulesEngine, string msg);

    public void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new InvalidValueException("The flow name can't be null/empty.");
        }

        Name = name;
    }

    public Node GetNode(Guid nodeId)
    {
        return _nodes.GetByIdOrDefault(nodeId);
    }

    public Node GetNodeOrThorw(Guid nodeId)
    {
        Node node = _nodes.GetByIdOrDefault(nodeId);

        if (node is null)
        {
            throw new ValueNotFoundException("The node doesnt belogn to the flow.");
        }

        return node;
    }

    public Tnode GetNode<Tnode>(Guid nodeId) where Tnode : Node
    {
        Node node = _nodes.GetByIdOrDefault(nodeId);

        if (node is null)
        {
            return null;
        }

        if (node is not Tnode)
        {
            return null;
        }

        return (Tnode)node;
    }

    public Tnode GetNodeOrThorw<Tnode>(Guid nodeId) where Tnode : Node
    {
        Tnode node = GetNode<Tnode>(nodeId);

        if (node is null)
        {
            throw new ValueNotFoundException("The node doesnt belogn to the flow.");
        }

        return node;
    }

    public virtual void AddGate(Guid id)
    {
        Gate gate = Gate.Create(id, this);
        AddNode(gate);
    }

    public void AddRequest(Guid id)
    {
        Request request = Request.Create(id, this);
        AddNode(request);
    }

    public void AddResponse(Guid id)
    {
        Response response = Response.Create(id, this);
        AddNode(response);
    }

    public void AddSwitcher(Guid id)
    {
        Switcher switcher = Switcher.Create(id, this);
        AddNode(switcher);
    }

    public void AddScripter(Guid id)
    {
        Scripter scripter = Scripter.Create(id, this);
        AddNode(scripter);
    }

    public void AddPrinter(Guid id)
    {
        Printer printer = Printer.Create(id, this);
        AddNode(printer);
    }

    public void AddSerializer(Guid id)
    {
        Serializer serializer = Serializer.Create(id, this);
        AddNode(serializer);
    }

    public void AddWatchdog(Guid id)
    {
        Watchdog watchdog = Watchdog.Create(id, this);
        AddNode(watchdog);
    }

    private void AddNode(Node node)
    {
        if (_nodes.Contains(node))
        {
            throw new DuplicatedValueException($"The node (Id: {node.Id}) already belongs to the flow.");
        }

        _nodes.Add(node);
    }

    public void MoveNode(Node node, int order)
    {
        if (node is null)
        {
            throw new WrongOperationException("The node cant be null.");
        }

        if (order < 0)
        {
            throw new InvalidValueException("The order is not valid.");
        }

        _nodes.Move(node, order);
    }

    public void RemoveNode(Node node)
    {
        if (node is null)
        {
            throw new WrongOperationException("The node cant be null.");
        }

        if (!_nodes.Contains(node))
        {
            throw new WrongOperationException("The node doesnt belong to the flow.");
        }

        _nodes.Remove(node);
    }

    public void SetCategory(Guid categoryId)
    {
        if (categoryId == Guid.Empty)
        {
            throw new InvalidValueException("The category id can't be empty.");
        }

        CategoryId = categoryId;
    }
    
    public void RemoveCategory()
    {
        CategoryId = null;
    }
}
