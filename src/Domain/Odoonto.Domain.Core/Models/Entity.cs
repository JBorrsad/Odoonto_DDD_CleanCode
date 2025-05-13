namespace Odoonto.Domain.Core.Models;

using System;

public abstract class Entity
{
    public Guid Id { get; private set; }
    public DateTime CreationDate { get; private set; }
    public DateTime EditDate { get; private set; }

    protected Entity(Guid id)
    {
        Id = id;
        CreationDate = DateTime.UtcNow;
        EditDate = CreationDate;
    }

    protected void UpdateEditDate()
    {
        EditDate = DateTime.UtcNow;
    }

    public override bool Equals(object obj)
    {
        if (obj is null)
            return false;

        if (ReferenceEquals(this, obj))
            return true;

        if (GetType() != obj.GetType())
            return false;

        var other = (Entity)obj;

        return Id.Equals(other.Id);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public static bool operator ==(Entity left, Entity right)
    {
        if (left is null && right is null)
            return true;

        if (left is null || right is null)
            return false;

        return left.Equals(right);
    }

    public static bool operator !=(Entity left, Entity right)
    {
        return !(left == right);
    }
}