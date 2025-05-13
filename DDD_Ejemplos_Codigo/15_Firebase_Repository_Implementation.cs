// EJEMPLO DE IMPLEMENTACIÓN DE REPOSITORIO PARA FIREBASE (Data Layer)
// Ruta: src/Data/TuProyecto.Data/Repositories/Categories/CategoryRepository.cs

namespace TuProyecto.Data.Repositories.Categories;

using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TuProyecto.Data.Core.Firebase;
using TuProyecto.Domain.Core.Models.Exceptions;
using TuProyecto.Domain.Models.Categories;
using TuProyecto.Domain.Models.Flows;
using TuProyecto.Domain.Repositories.Categories;

/// <summary>
/// Características clave de una implementación de repositorio para Firebase en DDD:
/// 1. Implementa la interfaz definida en el dominio
/// 2. Utiliza Firebase SDK para acceso a datos
/// 3. Gestiona la conversión entre documentos de Firebase y entidades de dominio
/// 4. Maneja las relaciones entre entidades aunque Firebase sea NoSQL
/// 5. Implementa transacciones cuando es necesario
/// </summary>
public class CategoryRepository : ICategoryRepository
{
    private readonly FirebaseDbContext _firebaseContext;
    private readonly string _collectionName = "categories";

    public CategoryRepository(FirebaseDbContext firebaseContext)
    {
        _firebaseContext = firebaseContext;
    }

    public async Task<Category> GetById(Guid id)
    {
        DocumentSnapshot document = await _firebaseContext.FirestoreDb
            .Collection(_collectionName)
            .Document(id.ToString())
            .GetSnapshotAsync();

        if (!document.Exists)
        {
            return null;
        }

        var categoryData = document.ConvertTo<CategoryDocument>();
        return MapToDomainEntity(categoryData);
    }

    public async Task<Category> GetByIdOrThrow(Guid id)
    {
        Category category = await GetById(id);

        if (category is null)
        {
            throw new ValueNotFoundException($"The {nameof(Category)} (Id: {id}) not found.");
        }

        return category;
    }

    public async Task<IEnumerable<Category>> GetAll()
    {
        QuerySnapshot querySnapshot = await _firebaseContext.FirestoreDb
            .Collection(_collectionName)
            .GetSnapshotAsync();

        if (querySnapshot.Count == 0)
        {
            return Enumerable.Empty<Category>();
        }

        var categories = new List<Category>();
        foreach (DocumentSnapshot document in querySnapshot.Documents)
        {
            var categoryData = document.ConvertTo<CategoryDocument>();
            categories.Add(MapToDomainEntity(categoryData));
        }

        return categories;
    }

    public async Task<Category> GetByIdWithFlowsOrThrow(Guid id)
    {
        Category category = await GetByIdOrThrow(id);

        // Cargar los flows relacionados
        QuerySnapshot flowsSnapshot = await _firebaseContext.FirestoreDb
            .Collection("flows")
            .WhereEqualTo("categoryId", id.ToString())
            .GetSnapshotAsync();

        foreach (DocumentSnapshot flowDoc in flowsSnapshot.Documents)
        {
            var flowData = flowDoc.ConvertTo<FlowDocument>();
            Flow flow = MapFlowToDomainEntity(flowData);

            // Como la entidad ya tiene el flujo configurado con su categoría,
            // usamos un método interno que no dispare el evento SetCategory
            AddFlowWithoutSettingCategory(category, flow);
        }

        return category;
    }

    public async Task<Category> GetByIdWithoutFlows(Guid id)
    {
        return await GetByIdOrThrow(id);
    }

    public async Task Create(Category entity)
    {
        var categoryDocument = MapToDocument(entity);

        await _firebaseContext.FirestoreDb
            .Collection(_collectionName)
            .Document(entity.Id.ToString())
            .SetAsync(categoryDocument);
    }

    public async Task Update(Category entity)
    {
        var categoryDocument = MapToDocument(entity);

        await _firebaseContext.FirestoreDb
            .Collection(_collectionName)
            .Document(entity.Id.ToString())
            .SetAsync(categoryDocument, SetOptions.MergeAll);
    }

    public async Task Delete(Category entity)
    {
        await _firebaseContext.FirestoreDb
            .Collection(_collectionName)
            .Document(entity.Id.ToString())
            .DeleteAsync();
    }

    public async Task<bool> Exists(Guid id)
    {
        DocumentSnapshot document = await _firebaseContext.FirestoreDb
            .Collection(_collectionName)
            .Document(id.ToString())
            .GetSnapshotAsync();

        return document.Exists;
    }

    public async Task AddFlowToCategory(Guid categoryId, Guid flowId)
    {
        // Implementar usando una transacción Firestore para garantizar consistencia
        await _firebaseContext.FirestoreDb.RunTransactionAsync(async transaction =>
        {
            // Obtener la categoría
            var categoryRef = _firebaseContext.FirestoreDb
                .Collection(_collectionName)
                .Document(categoryId.ToString());

            DocumentSnapshot categorySnapshot = await transaction.GetSnapshotAsync(categoryRef);

            if (!categorySnapshot.Exists)
            {
                throw new ValueNotFoundException($"The {nameof(Category)} (Id: {categoryId}) not found.");
            }

            // Obtener el flow
            var flowRef = _firebaseContext.FirestoreDb
                .Collection("flows")
                .Document(flowId.ToString());

            DocumentSnapshot flowSnapshot = await transaction.GetSnapshotAsync(flowRef);

            if (!flowSnapshot.Exists)
            {
                throw new ValueNotFoundException($"The {nameof(Flow)} (Id: {flowId}) not found.");
            }

            // Actualizar el flow para establecer su categoría
            transaction.Update(flowRef, new Dictionary<string, object>
            {
                { "categoryId", categoryId.ToString() }
            });
        });
    }

    public async Task RemoveFlowFromCategory(Guid categoryId, Guid flowId)
    {
        // Implementar usando una transacción Firestore para garantizar consistencia
        await _firebaseContext.FirestoreDb.RunTransactionAsync(async transaction =>
        {
            // Obtener la categoría
            var categoryRef = _firebaseContext.FirestoreDb
                .Collection(_collectionName)
                .Document(categoryId.ToString());

            DocumentSnapshot categorySnapshot = await transaction.GetSnapshotAsync(categoryRef);

            if (!categorySnapshot.Exists)
            {
                throw new ValueNotFoundException($"The {nameof(Category)} (Id: {categoryId}) not found.");
            }

            // Obtener el flow
            var flowRef = _firebaseContext.FirestoreDb
                .Collection("flows")
                .Document(flowId.ToString());

            DocumentSnapshot flowSnapshot = await transaction.GetSnapshotAsync(flowRef);

            if (!flowSnapshot.Exists)
            {
                throw new ValueNotFoundException($"The {nameof(Flow)} (Id: {flowId}) not found.");
            }

            // Verificar que el flujo pertenece a esta categoría
            var flowData = flowSnapshot.ConvertTo<FlowDocument>();
            if (flowData.CategoryId != categoryId.ToString())
            {
                throw new WrongOperationException("The flow doesn't belong to the category.");
            }

            // Actualizar el flow para remover su categoría
            transaction.Update(flowRef, new Dictionary<string, object>
            {
                { "categoryId", null }
            });
        });
    }

    // Clases privadas para mapeo entre Firebase y entidades de dominio
    private class CategoryDocument
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime EditDate { get; set; }
    }

    private class FlowDocument
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string CategoryId { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime EditDate { get; set; }
    }

    // Métodos de mapeo
    private Category MapToDomainEntity(CategoryDocument document)
    {
        var category = Category.Create(Guid.Parse(document.Id));
        category.SetName(document.Name);

        // En caso de que necesitemos establecer las fechas desde la base de datos
        // Esto requeriría métodos internos en la entidad o usar reflexión
        // SetPrivateProperty(category, "CreationDate", document.CreationDate);
        // SetPrivateProperty(category, "EditDate", document.EditDate);

        return category;
    }

    private Flow MapFlowToDomainEntity(FlowDocument document)
    {
        var flow = Flow.Create(Guid.Parse(document.Id));
        flow.SetName(document.Name);

        if (!string.IsNullOrEmpty(document.CategoryId))
        {
            flow.SetCategory(Guid.Parse(document.CategoryId));
        }

        return flow;
    }

    private CategoryDocument MapToDocument(Category entity)
    {
        return new CategoryDocument
        {
            Id = entity.Id.ToString(),
            Name = entity.Name,
            CreationDate = entity.CreationDate,
            EditDate = entity.EditDate
        };
    }

    // Este método es solo para uso interno del repositorio
    private void AddFlowWithoutSettingCategory(Category category, Flow flow)
    {
        // Se podría implementar usando reflexión para acceder al campo privado _flows
        // o idealmente, la entidad debería exponer un método para este caso específico
        var method = typeof(Category).GetMethod("AddFlowWithoutSettingCategory",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        if (method != null)
        {
            method.Invoke(category, new object[] { flow });
        }
    }
}