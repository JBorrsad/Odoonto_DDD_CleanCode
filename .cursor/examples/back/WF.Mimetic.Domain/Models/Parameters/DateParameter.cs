namespace WF.Mimetic.Domain.Models.Parameters;

using System;
using WF.Mimetic.Domain.Core.Models.Exceptions;

public class DateParameter : Parameter
{
    public static DateParameter Default => new DateParameter();

    public override ParameterType Type => ParameterType.Date;

    private DateParameter() : base(Guid.Empty, Guid.Empty)
    {

    }

    private DateParameter(Guid id, Guid fatherId) : base(id, fatherId)
    {
    }

    internal static DateParameter Create(Guid id, ParameterAggregate parameterAggregate)
    {
        if (Guid.Empty.Equals(id))
        {
            throw new InvalidValueException("The parameter id cant be empty.");
        }

        if (parameterAggregate is null)
        {
            throw new InvalidValueException("The aggregate cant be null.");
        }

        return new DateParameter(id, parameterAggregate.Id);
    }

    public override bool IsCorrectValueType(object value)
    {
        return value is DateTime;
    }

    public override bool TryParse(string value, out object result)
    {
        bool isParsed = DateTime.TryParse(value, out DateTime resultParsed);

        result = resultParsed;
        return isParsed;
    }
}
