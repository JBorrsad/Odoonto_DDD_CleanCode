using System;
using WF.Mimetic.Domain.Core.Models.Exceptions;

namespace WF.Mimetic.Domain.Models.ApiDocs.Schemas;
public abstract class Schema
{
    public bool Nullable { get; private set; }

    public Schema IsNullable()
    {
        Nullable = true;
        return this;
    }

    public static Schema Create(Type type)
    {
        return (Schema) BooleanSchema.GetSchema(type)
            ?? (Schema) DateTimeSchema.GetSchema(type)
            ?? (Schema) GuidSchema.GetSchema(type)
            ?? (Schema) IntSchema.GetSchema(type)
            ?? (Schema) LongSchema.GetSchema(type)
            ?? (Schema) ShortSchema.GetSchema(type)
            ?? (Schema) StringSchema.GetSchema(type)
            ?? (Schema) ArraySchema.GetSchema(type)
            ?? (Schema) ObjectSchema.GetSchema(type)
            ?? throw new WrongOperationException("The schema type is not valid.");
    }
}
