// EJEMPLOS DE DTOs (Application Layer)
// Ruta: src/Application/TuProyecto.Application/DTO/Categories/

// DTO para Creación
namespace TuProyecto.Application.DTO.Categories;

using System;

/// <summary>
/// Características clave de los DTOs en DDD:
/// 1. Clases simples con propiedades públicas (POCO)
/// 2. Representan datos para una operación específica
/// 3. Se usan para transferir datos entre capas
/// 4. No contienen lógica de negocio
/// 5. Se dividen en tipos según su propósito (Create, Read, Update, Query)
/// </summary>

// Usado para crear una nueva categoría
public class CategoryCreateDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
}

// Usado para actualizar una categoría existente
public class CategoryUpdateDto
{
    public string Name { get; set; }
}

// Usado para respuesta detallada (lectura completa)
public class CategoryReadDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime LastEditDate { get; set; }
    public IEnumerable<FlowInCategoryDto> Flows { get; set; } = new List<FlowInCategoryDto>();
}

// Usado para listados (menos detalle)
public class CategoryQueryDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int FlowCount { get; set; }
}

// DTO para operaciones específicas
public class AddFlowToCategoryDto
{
    public Guid FlowId { get; set; }
}

// DTO auxiliar para relaciones
public class FlowInCategoryDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int NodeCount { get; set; }
}