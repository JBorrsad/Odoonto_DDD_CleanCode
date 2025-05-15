namespace WF.Mimetic.Domain.Models.Categories;

using System;
using System.Collections.Generic;
using System.Xml.Linq;
using WF.Mimetic.Domain.Core.Models;
using WF.Mimetic.Domain.Core.Models.Exceptions;
using WF.Mimetic.Domain.Core.Models.SortedLists;
using WF.Mimetic.Domain.Models.Flows;

public class Category : Entity
{
    public string Name { get; private set; }

    private readonly ISortedEntityList<Flow> _flows;

    public IEnumerable<Flow> Flows => _flows.AsEnumerable();

    private Category(Guid id) : base(id)
    {
        Name = null;
        _flows = new SortedEntityList<Flow>();
    }

    public static Category Create(Guid id)
    {
        if (id.Equals(Guid.Empty))
        {
            throw new InvalidValueException("The category id can't be empty.");
        }

        Category category = new Category(id);
        category.UpdateEditDate();
        return category;
    }

    public void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new InvalidValueException("The category name can't be null/empty.");
        }

        Name = name;
        UpdateEditDate();
    }

    public void AddFlow(Flow flow)
    {
        if (flow is null)
        {
            throw new WrongOperationException("The flow can't be null.");
        }

        if (_flows.Exists(f => f.Id == flow.Id))
        {
            throw new DuplicatedValueException($"The flow (Id: {flow.Id}) already belongs to the category.");
        }

        flow.SetCategory(Id);
        _flows.Add(flow);
        UpdateEditDate();
    }

    public void RemoveFlow(Flow flow)
    {
        if (flow is null)
        {
            throw new WrongOperationException("The flow can't be null.");
        }

        if (!_flows.Exists(f => f.Id == flow.Id))
        {
            throw new WrongOperationException("The flow doesn't belong to the category.");
        }

        flow.RemoveCategory();
        _flows.Remove(flow);
        UpdateEditDate();
    }
}





