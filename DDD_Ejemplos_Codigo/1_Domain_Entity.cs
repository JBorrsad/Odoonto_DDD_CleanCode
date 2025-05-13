// EJEMPLO DE ENTIDAD DE DOMINIO (Domain Layer)
// Ruta: src/Domain/Odoonto.Domain/Models/Categories/Category.cs

namespace Odoonto.Domain.Models.Categories;

using System;
using System.Collections.Generic;
using Odoonto.Domain.Core.Models;
using Odoonto.Domain.Core.Models.Exceptions;
using Odoonto.Domain.Core.Models.SortedLists;
using Odoonto.Domain.Models.Flows;

/// <summary>
/// Características clave de una entidad en DDD:
/// 1. Hereda de la clase base Entity que contiene Id y propiedades de auditoría
/// 2. Propiedades con getters públicos y setters privados (encapsulamiento)
/// 3. Constructor privado y método Factory estático (Create)
/// 4. Validaciones de negocio dentro de los métodos
/// 5. Métodos que representan comportamiento de negocio
/// </summary>
public class Category : Entity
{
    public string Name { get; private set; }

    // Colección privada para mantener el encapsulamiento
    private readonly ISortedEntityList<Flow> _flows;

    // Acceso de solo lectura a la colección
    public IEnumerable<Flow> Flows => _flows.AsEnumerable();

    // Constructor privado - solo accesible desde el método factory
    private Category(Guid id) : base(id)
    {
        Name = null;
        _flows = new SortedEntityList<Flow>();
    }

    // Método factory estático para crear instancias válidas
    public static Category Create(Guid id)
    {
        if (id.Equals(Guid.Empty))
        {
            throw new InvalidValueException("The category id can't be empty.");
        }

        Category category = new Category(id);
        category.UpdateEditDate();
        return category;
    }

    // Método de comportamiento con validaciones
    public void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new InvalidValueException("The category name can't be null/empty.");
        }

        Name = name;
        UpdateEditDate();
    }

    // Método para agregar relación con validaciones
    public void AddFlow(Flow flow)
    {
        if (flow is null)
        {
            throw new WrongOperationException("The flow can't be null.");
        }

        if (_flows.Exists(f => f.Id == flow.Id))
        {
            throw new DuplicatedValueException($"The flow (Id: {flow.Id}) already belongs to the category.");
        }

        flow.SetCategory(Id);
        _flows.Add(flow);
        UpdateEditDate();
    }

    // Método para eliminar relación
    public void RemoveFlow(Flow flow)
    {
        if (flow is null)
        {
            throw new WrongOperationException("The flow can't be null.");
        }

        if (!_flows.Exists(f => f.Id == flow.Id))
        {
            throw new WrongOperationException("The flow doesn't belong to the category.");
        }

        flow.RemoveCategory();
        _flows.Remove(flow);
        UpdateEditDate();
    }
}