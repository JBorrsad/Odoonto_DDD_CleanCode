# Reglas de Implementación DDD

## Info

- **Autor**: Cursor
- **Versión**: 1.0
- **Categoría**: Implementación
- **Tags**: #DDD #Rules #BestPractices #Implementación

## Contexto

Este documento define reglas concretas para implementar correctamente Domain-Driven Design (DDD) en un proyecto con Firebase y frontend React.

## Reglas por Capa

### Capa de Dominio

#### Entidades

- ✅ Usar propiedades con getters públicos y setters privados
- ✅ Implementar métodos factory estáticos (Create)
- ✅ Validar en los métodos que modifican estado
- ✅ Lanzar excepciones específicas de dominio
- ✅ Encapsular lógica de negocio en métodos
- ❌ No permitir estados inválidos
- ❌ No tener dependencias externas

```csharp
public class Category : Entity
{
    public string Name { get; private set; }

    private Category(Guid id) : base(id)
    {
        Name = null;
    }

    public static Category Create(Guid id)
    {
        if (id.Equals(Guid.Empty))
        {
            throw new InvalidValueException("The Category id can't be empty.");
        }

        Category category = new Category(id);
        category.UpdateEditDate();
        return category;
    }

    public void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new InvalidValueException("The Category name can't be null/empty.");
        }

        Name = name;
        UpdateEditDate();
    }
}
```

#### Interfaces de Repositorio

- ✅ Definir en el dominio
- ✅ Incluir métodos CRUD básicos
- ✅ Añadir métodos específicos del dominio
- ✅ Usar Task/async para operaciones asíncronas
- ❌ No incluir detalles de implementación

```csharp
public interface ICategoryRepository : IRepository<Category>
{
    Task<Category> GetByIdWithFlows(Guid id);
    Task<bool> ExistsByName(string name);
    Task AddFlowToCategory(Guid categoryId, Guid flowId);
}
```

### Capa de Aplicación

#### Interfaces de Servicio

- ✅ Definir contratos basados en casos de uso
- ✅ Usar DTOs para entrada/salida
- ✅ Cada método debe representar una operación específica
- ❌ No exponer entidades de dominio

```csharp
public interface ICategoryAppService
{
    Task<CategoryReadDto> GetById(Guid id);
    Task<IEnumerable<CategoryQueryDto>> GetAll();
    Task Create(CategoryCreateDto data);
    Task Update(Guid id, CategoryUpdateDto data);
    Task Remove(Guid id);
    Task AddFlowToCategory(Guid categoryId, AddFlowToCategoryDto data);
}
```

#### DTOs

- ✅ Crear DTOs específicos para cada operación (Create, Update, Read, Query)
- ✅ Usar anotaciones de validación
- ✅ Incluir solo los datos necesarios
- ❌ No incluir lógica de negocio

```csharp
public class CategoryCreateDto
{
    public Guid? Id { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 3)]
    public string Name { get; set; }
}

public class CategoryReadDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime EditDate { get; set; }
    public IEnumerable<FlowInCategoryDto> Flows { get; set; }
}
```

#### Servicios de Aplicación

- ✅ Inyectar repositorios y mapper
- ✅ Orquestar operaciones entre entidades
- ✅ Mapear entre DTOs y entidades
- ✅ Validar datos de entrada
- ❌ No implementar lógica de negocio (delegarla al dominio)

```csharp
public class CategoryAppService : ICategoryAppService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    public CategoryAppService(ICategoryRepository categoryRepository, IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    public async Task Create(CategoryCreateDto data)
    {
        if (data is null)
        {
            throw new InvalidValueException("The request is not valid.");
        }

        Category category = Category.Create(data.Id ?? Guid.NewGuid());
        category.SetName(data.Name);

        await _categoryRepository.Create(category);
    }

    // Más implementaciones...
}
```

### Capa de Datos

#### Repositorios Firebase

- ✅ Implementar interfaces del dominio
- ✅ Usar FirebaseDbContext
- ✅ Manejar conversión entre documentos y entidades
- ✅ Implementar transacciones cuando sea necesario
- ❌ No exponer detalles de Firebase fuera de la capa

```csharp
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

        var data = document.ConvertTo<CategoryDocument>();
        return MapToDomainEntity(data);
    }

    // Más implementaciones...
}
```

### Capa de Presentación

#### Controladores API

- ✅ Inyectar servicios de aplicación
- ✅ Usar atributos para definir rutas y métodos HTTP
- ✅ Documentar con Swagger/OpenAPI
- ✅ Traducir excepciones a códigos HTTP
- ❌ No incluir lógica de negocio

```csharp
[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryAppService _categoryAppService;

    public CategoriesController(ICategoryAppService categoryAppService)
    {
        _categoryAppService = categoryAppService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<CategoryQueryDto>), 200)]
    public async Task<ActionResult<IEnumerable<CategoryQueryDto>>> GetAll()
    {
        var result = await _categoryAppService.GetAll();
        return Ok(result);
    }

    // Más endpoints...
}
```

#### Frontend React (MVP)

- ✅ Separar Models, Services, Presenters y Views
- ✅ Comunicarse solo con la API REST
- ✅ Usar TypeScript para modelado fuerte
- ✅ Implementar validación local antes de enviar a la API
- ❌ No acceder directamente a Firebase
- ❌ No mezclar lógica de negocio con UI

```typescript
// Modelo
export class Category implements CategoryReadModel {
  // Propiedades y métodos...
}

// Servicio
export class CategoryService {
  async getAll(): Promise<CategoryQueryModel[]> {
    const response = await axios.get("/api/categories");
    return response.data;
  }
}

// Presentador
export function useCategoryPresenter() {
  const [categories, setCategories] = useState<Category[]>([]);
  // Lógica del presentador...
}

// Vista
export const CategoryList: React.FC = () => {
  const { categories, loading, createCategory } = useCategoryPresenter();
  // Renderizado UI...
};
```

## Convenciones de Nombrado

- **Entidades**: Nombres en singular, sin prefijos ni sufijos (Category, Product)
- **Repositorios**: Interfaz I[Entidad]Repository, implementación [Entidad]Repository
- **Servicios**: Interfaz I[Entidad]AppService, implementación [Entidad]AppService
- **DTOs**: [Entidad][Operación]Dto (CategoryCreateDto, CategoryReadDto)
- **Controladores**: [Entidad]Controller (en plural para las rutas)
- **Componentes React**: [Entidad][Vista] (CategoryList, CategoryDetail)

## Estructura de Carpetas

```
src/
  Domain/
    Proyecto.Domain/
      Models/            # Entidades
      Repositories/      # Interfaces de Repositorio
    Proyecto.Domain.Core/
  Application/
    Proyecto.Application/
      Services/          # Servicios de Aplicación
      Interfaces/        # Interfaces de Servicio
      DTO/               # DTOs
      AutoMapper/        # Configuración de mapeo
  Data/
    Proyecto.Data/
      Repositories/      # Implementación de Repositorios
    Proyecto.Data.Core/
      Firebase/          # Contexto Firebase
  Infrastructure/
    Proyecto.Infrastructure.IoC/
      Injectors/         # Configuración IoC
  Presentation/
    Proyecto.API/
      Controllers/       # Controladores API

frontend/
  src/
    models/              # Modelos frontend
    services/            # Servicios API
    presenters/          # Presentadores
    views/               # Componentes
```

## Reglas de Implementación Firebase

- Incluir manejo de transacciones para operaciones que afecten a múltiples documentos
- Implementar estrategias de cache para operaciones de lectura frecuentes
- Diseñar estructura de documentos teniendo en cuenta limitaciones de Firebase
- Gestionar relaciones entre entidades manualmente (no hay soporte nativo para joins)
- Utilizar reglas de seguridad para proteger los datos
