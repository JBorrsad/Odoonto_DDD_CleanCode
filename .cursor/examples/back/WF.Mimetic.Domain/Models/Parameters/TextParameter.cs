namespace WF.Mimetic.Domain.Models.Parameters;

using System;
using WF.Mimetic.Domain.Core.Models.Exceptions;

public class TextParameter : Parameter
{
    public static TextParameter Default => new TextParameter();

    public override ParameterType Type => ParameterType.Text;

    private TextParameter() : base(Guid.Empty, Guid.Empty)
    {

    }

    private TextParameter(Guid id, Guid fatherId) : base(id, fatherId)
    {
    }

    public override bool IsCorrectValueType(object value)
    {
        return value is string;
    }

    public override bool TryParse(string value, out object result)
    {
        result = value;
        return true;
    }

    internal static TextParameter Create(Guid id, ParameterAggregate parameterAggregate)
    {
        if (Guid.Empty.Equals(id))
        {
            throw new InvalidValueException("The parameter id cant be empty.");
        }

        if (parameterAggregate is null)
        {
            throw new InvalidValueException("The aggregate cant be null.");
        }

        return new TextParameter(id, parameterAggregate.Id);
    }
}
