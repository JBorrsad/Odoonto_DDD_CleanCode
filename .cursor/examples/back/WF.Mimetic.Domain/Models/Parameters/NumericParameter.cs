namespace WF.Mimetic.Domain.Models.Parameters;

using System;
using WF.Mimetic.Domain.Core.Models.Exceptions;

public class NumericParameter : Parameter
{
    public static NumericParameter Default => new NumericParameter();

    public override ParameterType Type => ParameterType.Numeric;

    private NumericParameter() : base(Guid.Empty, Guid.Empty)
    {

    }

    private NumericParameter(Guid id, Guid fatherId) : base(id, fatherId)
    {
    }

    internal static NumericParameter Create(Guid id, ParameterAggregate parameterAggregate)
    {
        if (Guid.Empty.Equals(id))
        {
            throw new InvalidValueException("The parameter id cant be empty.");
        }

        if (parameterAggregate is null)
        {
            throw new InvalidValueException("The aggregate cant be null.");
        }

        return new NumericParameter(id, parameterAggregate.Id);
    }

    public override bool IsCorrectValueType(object value)
    {
        return value is double;
    }

    public override bool TryParse(string value, out object result)
    {
        bool isParsed = double.TryParse(value, out double resultParsed);

        result = resultParsed;
        return isParsed;
    }
}
