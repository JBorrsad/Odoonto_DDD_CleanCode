namespace WF.Mimetic.Domain.Models.Parameters;

using HandlebarsDotNet;
using HandlebarsDotNet.Extension.NewtonsoftJson;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Text.RegularExpressions;
using WF.Mimetic.Domain.Core.Models;
using WF.Mimetic.Domain.Core.Models.Exceptions;
using WF.Mimetic.Domain.Interfaces.Rules;
using WF.Mimetic.Domain.Models.Nodes;

public abstract class Parameter : Entity
{
    private string _defaultValue;

    public Guid FatherId { get; private set; }
    public string Name { get; private set; }
    public string Target { get; private set; }
    public bool IsNullable { get; private set; }
    public object DefaultValue => GetDefaultValue();
    public abstract ParameterType Type { get; }

    protected Parameter(Guid id, Guid fatherId) : base(id)
    {
        FatherId = fatherId;
        Name = null;
        IsNullable = true;
    }

    public abstract bool IsCorrectValueType(object value);
    public abstract bool TryParse(string value, out object result);

    public void SetName(string name)
    {
        Regex regex = new Regex(@"^[A-Za-z_][A-Za-z0-9_]*$");

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new InvalidValueException("The field name can't be null/empty.");
        }

        if (!regex.IsMatch(name))
        {
            throw new InvalidValueException("The field name is not valid (Regex: [A-Za-z_][A-Za-z0-9_]*).");
        }

        Name = name;
    }

    public void SetTarget(string target)
    {
        Regex regex = new Regex(@"^[A-Za-z_][A-Za-z0-9_\-]*(\.[A-Za-z_][A-Za-z0-9_\-]*)?$");

        if (string.IsNullOrWhiteSpace(target))
        {
            throw new InvalidValueException("The field target can't be null/empty.");
        }

        if (!regex.IsMatch(target))
        {
            throw new InvalidValueException("The field target is not valid (Regex: [A-Za-z_][A-Za-z0-9_.]*).");
        }

        Target = target;
    }

    public bool TryGetValueByTarget(string msg, out object value)
    {
        if (Target is null || msg is null)
        {
            value = null;
            return false;
        }

        string targetValue = Moustacher.GetValueFromJson(msg, Target);

        return TryParse(targetValue, out value);
    }

    public void SetIsNullable(bool isNullable)
    {
        IsNullable = isNullable;
    }

    public void SetDefaultValue(string value)
    {
        if (value is null)
        {
            _defaultValue = null;
            return;
        }

        if (!TryParse(value, out object _))
        {
            throw new InvalidValueException("The default value is not correct.");
        }

        _defaultValue = value;
    }

    private object GetDefaultValue()
    {
        if (_defaultValue is null)
        {
            return null;
        }

        if (!TryParse(_defaultValue, out object result))
        {
            return null;
        }

        return result;
    }
}
