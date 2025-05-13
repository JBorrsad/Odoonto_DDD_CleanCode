// EJEMPLO DE CLASE BASE ENTITY (Domain Core Layer)
// Ruta: src/Domain/TuProyecto.Domain.Core/Models/Entity.cs

namespace TuProyecto.Domain.Core.Models;

using System;

/// <summary>
/// Características clave de la clase base Entity en DDD:
/// 1. Define propiedades comunes para todas las entidades
/// 2. Implementa igualdad basada en Id
/// 3. Proporciona propiedades de auditoría
/// 4. Puede incluir métodos de utilidad comunes
/// 5. Establece la base para toda la jerarquía de entidades
/// </summary>
public abstract class Entity
{
    // Identificador único de la entidad
    public Guid Id { get; protected set; }

    // Propiedades de auditoría
    public DateTime CreationDate { get; protected set; }
    public DateTime EditDate { get; protected set; }

    // Constructor protegido, solo accesible desde clases derivadas
    protected Entity(Guid id)
    {
        Id = id;
        var now = DateTime.UtcNow;
        CreationDate = now;
        EditDate = now;
    }

    // Constructor protegido sin parámetros para EF Core
    protected Entity()
    {
        Id = Guid.Empty;
        CreationDate = DateTime.UtcNow;
        EditDate = DateTime.UtcNow;
    }

    // Método para actualizar la fecha de edición
    public void UpdateEditDate()
    {
        EditDate = DateTime.UtcNow;
    }

    // Sobrescritura de Equals para comparación de entidades por Id
    public override bool Equals(object obj)
    {
        if (obj is null)
            return false;

        if (ReferenceEquals(this, obj))
            return true;

        if (obj.GetType() != this.GetType())
            return false;

        Entity other = (Entity)obj;
        return Id.Equals(other.Id);
    }

    // Sobrescritura de GetHashCode basada en Id
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    // Sobrecarga del operador de igualdad
    public static bool operator ==(Entity left, Entity right)
    {
        if (left is null && right is null)
            return true;

        if (left is null || right is null)
            return false;

        return left.Equals(right);
    }

    // Sobrecarga del operador de desigualdad
    public static bool operator !=(Entity left, Entity right)
    {
        return !(left == right);
    }
}