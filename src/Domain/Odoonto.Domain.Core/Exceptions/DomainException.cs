namespace Odoonto.Domain.Core.Exceptions;

using System;

public abstract class DomainException : Exception
{
    protected DomainException(string message) : base(message)
    {
    }
}

public class InvalidValueException : DomainException
{
    public InvalidValueException(string message) : base(message)
    {
    }
}

public class WrongOperationException : DomainException
{
    public WrongOperationException(string message) : base(message)
    {
    }
}

public class DuplicatedValueException : DomainException
{
    public DuplicatedValueException(string message) : base(message)
    {
    }
}

public class ValueNotFoundException : DomainException
{
    public ValueNotFoundException(string message) : base(message)
    {
    }
}