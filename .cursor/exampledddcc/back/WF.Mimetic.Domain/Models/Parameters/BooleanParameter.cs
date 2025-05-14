namespace WF.Mimetic.Domain.Models.Parameters;

using System;
using WF.Mimetic.Domain.Core.Models.Exceptions;

public class BooleanParameter : Parameter
{
    public static BooleanParameter Default => new BooleanParameter();

    public override ParameterType Type => ParameterType.Boolean;

    private BooleanParameter() : base(Guid.Empty, Guid.Empty)
    {

    }

    private BooleanParameter(Guid id, Guid fatherId) : base(id, fatherId)
    {
    }

    internal static BooleanParameter Create(Guid id, ParameterAggregate parameterAggregate)
    {
        if (Guid.Empty.Equals(id))
        {
            throw new InvalidValueException("The parameter id cant be empty.");
        }

        if (parameterAggregate is null)
        {
            throw new InvalidValueException("The aggregate cant be null.");
        }

        return new BooleanParameter(id, parameterAggregate.Id);
    }

    public override bool IsCorrectValueType(object value)
    {
        return value is bool;
    }

    public override bool TryParse(string value, out object result)
    {
        bool isParsed = bool.TryParse(value, out bool resultParsed);

        result = resultParsed;
        return isParsed;
    }
}
