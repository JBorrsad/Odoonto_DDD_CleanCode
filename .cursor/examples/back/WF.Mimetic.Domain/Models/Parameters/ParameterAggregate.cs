namespace WF.Mimetic.Domain.Models.Parameters;

using System;
using System.Collections.Generic;
using System.Linq;
using WF.Mimetic.Domain.Core.Models;
using WF.Mimetic.Domain.Core.Models.EntityLists;
using WF.Mimetic.Domain.Core.Models.Exceptions;
using WF.Mimetic.Domain.Core.Enums;

public abstract class ParameterAggregate : SortedEntity
{
    protected readonly IEntityList<Parameter> _parameters;

    protected ParameterAggregate(Guid id) : base(id)
    {
        _parameters = new EntityList<Parameter>();
    }

    public Parameter this[string name]
    {
        get => GetParameter(name);
    }

    public IEnumerable<string> Keys => _parameters.Select(parameter => parameter.Name);

    public IEnumerable<Parameter> Parameters => _parameters.AsEnumerable();

    public void AddBoolean(Guid id)
    {
        BooleanParameter parameter = BooleanParameter.Create(id, this);
        Add(parameter);
    }

    public void AddNumeric(Guid id)
    {
        NumericParameter parameter = NumericParameter.Create(id, this);
        Add(parameter);
    }

    public void AddText(Guid id)
    {
        TextParameter parameter = TextParameter.Create(id, this);
        Add(parameter);
    }

    public void AddDate(Guid id)
    {
        DateParameter parameter = DateParameter.Create(id, this);
        Add(parameter);
    }

    public void AddParameter(Guid id, ParameterType type)
    {
        switch (type)
        {
            case ParameterType.Boolean:
                AddBoolean(id);
                break;
            case ParameterType.Numeric:
                AddNumeric(id);
                break;
            case ParameterType.Date:
                AddDate(id);
                break;
            case ParameterType.Text:
                AddText(id);
                break;
            default:
                throw new InvalidValueException("The parameter type is not valid");
        }
    }

    private void Add(Parameter parameter)
    {
        string name = GenerateName(parameter.Type);

        while (Contains(name))
        {
            name = GenerateName(parameter.Type);
        }

        parameter.SetName(name);
        _parameters.Add(parameter);
    }

    public Parameter GetParameter(Guid id)
    {
        return _parameters.GetByIdOrDefault(id);
    }

    public Parameter GetParameter(string name)
    {
        return _parameters.FirstOrDefault(p => p.Name == name);
    }

    public Parameter GetParameterOrThrow(Guid id)
    {
        Parameter parameter = GetParameter(id);

        if (parameter is null)
        {
            throw new ValueNotFoundException("The parameter doesnt belong to the aggregate.");
        }

        return parameter;
    }

    public Parameter GetParameterOrThrow(string name)
    {
        Parameter parameter = GetParameter(name);

        if (parameter is null)
        {
            throw new ValueNotFoundException("The parameter doesnt belong to the aggregate.");
        }

        return parameter;
    }

    public void EditParameter(Parameter parameter, string name, bool isNullable, string defaultValue)
    {
        if (parameter is null)
        {
            throw new WrongOperationException("The parameter cant be null.");
        }

        if (!_parameters.Contains(parameter))
        {
            throw new WrongOperationException("The parameter doesnt belong to the aggregate.");
        }

        parameter.SetName(name);
        parameter.SetIsNullable(isNullable);
        parameter.SetDefaultValue(defaultValue);
    }

    public bool Remove(Parameter parameter)
    {
        if (parameter is null)
        {
            throw new InvalidValueException("The parameter cant be null.");
        }

        if (!_parameters.Contains(parameter))
        {
            return false;
        }

        return _parameters.Remove(parameter);
    }

    public bool TryGetValue(string name, out Parameter value)
    {
        value = GetParameter(name);
        return value is not null;
    }

    public bool Contains(string name)
    {
        return TryGetValue(name, out Parameter _);
    }

    private string GenerateName(ParameterType parameterType)
    {
        int randomId = new Random().Next(0, 999999);
        return $"{parameterType}_{string.Format("{0:000000}", randomId)}";
    }


    public static void AddParameter(ParameterAggregate aggregate, Guid id, string type)
    {
        if (aggregate is null)
        {
            throw new WrongOperationException("The aggregate cant be null.");
        }

        ParameterType parameterType = EnumsParser.Parse<ParameterType>(type);

        aggregate.AddParameter(id, parameterType);
    }

    public static void EditParameter(ParameterAggregate aggregate, Guid parameterId, string name, bool isNullable, string defaultValue)
    {
        if (aggregate is null)
        {
            throw new WrongOperationException("The aggregate cant be null.");
        }

        Parameter parameter = aggregate.GetParameterOrThrow(parameterId);
        aggregate.EditParameter(parameter, name, isNullable, defaultValue);
    }

    public static void RemoveParameter(ParameterAggregate aggregate, Guid parameterId)
    {
        if (aggregate is null)
        {
            throw new WrongOperationException("The aggregate cant be null.");
        }
        Parameter parameter = aggregate.GetParameterOrThrow(parameterId);
        aggregate.Remove(parameter);
    }
}
