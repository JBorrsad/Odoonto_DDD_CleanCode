# Arquitectura con API REST y Firebase en el Backend

Este documento describe la arquitectura donde el frontend React se comunica con un backend .NET mediante una API REST, y es este backend quien interactúa con Firebase como sustituto de una base de datos relacional.

## 1. Visión General de la Arquitectura

La arquitectura sigue un diseño Domain-Driven Design (DDD) puro:

```
┌─────────────────────┐       ┌───────────────────────────────────────────────────┐       ┌───────────────┐
│                     │       │                   BACKEND (.NET)                   │       │               │
│                     │       │                                                   │       │               │
│                     │       │ ┌───────────┐  ┌───────────┐  ┌───────────┐       │       │               │
│    FRONTEND         │       │ │           │  │           │  │           │       │       │               │
│    (React/          │  API  │ │ Presenta- │  │ Applica-  │  │  Domain   │       │       │               │
│    TypeScript)      ├───────┼─► tion      ├──► tion      ├──►           │       │       │               │
│                     │  REST │ │ Layer     │  │ Layer     │  │  Layer    │       │       │    Firebase   │
│                     │       │ │ (API)     │  │           │  │           │       │       │    Firestore  │
│                     │       │ │           │  │           │  │           │       │       │               │
│                     │       │ └─────┬─────┘  └─────┬─────┘  └─────┬─────┘       │       │               │
│                     │       │       │              │              │             │       │               │
│                     │       │       │              │              │             │       │               │
│                     │       │       └──────────────┼──────────────┘             │       │               │
│                     │       │                      │                            │       │               │
│                     │       │              ┌───────▼─────┐                      │       │               │
│                     │       │              │             │                      │       │               │
│                     │       │              │ Data Layer  ├──────────────────────┼───────►               │
│                     │       │              │ (Firebase)  │                      │       │               │
│                     │       │              │             │                      │       │               │
│                     │       │              └─────────────┘                      │       │               │
│                     │       │                                                   │       │               │
└─────────────────────┘       └───────────────────────────────────────────────────┘       └───────────────┘
```

## 2. Componentes de la Arquitectura

### 2.1. Frontend (React/TypeScript)

El frontend es una aplicación React/TypeScript completamente separada que:

- Se comunica EXCLUSIVAMENTE con el backend a través de la API REST
- Nunca accede directamente a Firebase
- Sigue patrón MVP (Model-View-Presenter) internamente
- Se sitúa ÚNICAMENTE en la capa de Presentación según DDD

```typescript
// Ejemplo de servicio en el frontend para comunicarse con la API
// src/services/api/category.service.ts
import axios from "axios";
import { CategoryDto } from "../models/category.model";

export class CategoryApiService {
  private readonly baseUrl = "https://your-api.com/api/categories";

  async getAll(): Promise<CategoryDto[]> {
    const response = await axios.get(this.baseUrl);
    return response.data;
  }

  async getById(id: string): Promise<CategoryDto> {
    const response = await axios.get(`${this.baseUrl}/${id}`);
    return response.data;
  }

  // Más métodos para comunicarse con la API...
}
```

### 2.2. Backend API (ASP.NET Core)

La API REST es la capa de Presentación del backend:

- Expone endpoints documentados con Swagger
- Se implementa con controladores ASP.NET Core
- Utiliza servicios de la capa de Aplicación
- No contiene lógica de negocio

```csharp
// Ejemplo de controlador API
[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryAppService _categoryService;

    public CategoriesController(ICategoryAppService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAll()
    {
        return Ok(await _categoryService.GetAll());
    }

    // Más endpoints...
}
```

### 2.3. Capa de Datos con Firebase

La capa de Datos es donde se implementa la interacción con Firebase:

- Implementa las interfaces de repositorio definidas en el dominio
- Utiliza el SDK de Firebase para C#
- Abstrae completamente los detalles de Firebase del resto de la aplicación

```csharp
// Implementación de repositorio para Firebase
public class CategoryRepository : ICategoryRepository
{
    private readonly FirebaseDbContext _firebaseContext;
    private readonly string _collectionName = "categories";

    public CategoryRepository(FirebaseDbContext firebaseContext)
    {
        _firebaseContext = firebaseContext;
    }

    public async Task<IEnumerable<Category>> GetAll()
    {
        // Implementación usando Firebase SDK
        var snapshot = await _firebaseContext.FirestoreDb
            .Collection(_collectionName)
            .GetSnapshotAsync();

        return snapshot.Documents
            .Select(doc => ConvertToEntity(doc))
            .ToList();
    }

    // Más métodos...
}
```

## 3. Flujo de Datos en la Aplicación

1. **Usuario interactúa con el frontend**:

   - El usuario realiza una acción en la interfaz
   - El componente React llama a un presentador
   - El presentador llama al servicio API

2. **Frontend se comunica con el backend**:

   - El servicio API realiza una petición HTTP al endpoint correspondiente
   - La petición incluye datos en formato JSON (si es necesario)
   - La petición puede incluir tokens de autenticación

3. **El controlador API procesa la petición**:

   - Valida los datos de entrada
   - Llama al servicio de aplicación correspondiente
   - Retorna una respuesta HTTP

4. **El servicio de aplicación ejecuta la lógica de negocio**:

   - Orquesta las operaciones necesarias
   - Utiliza entidades del dominio
   - Llama a repositorios para persistencia

5. **El repositorio interactúa con Firebase**:

   - Traduce operaciones del dominio a operaciones Firestore
   - Ejecuta consultas, transacciones o actualizaciones
   - Convierte documentos Firestore a entidades de dominio

6. **El resultado vuelve al usuario**:
   - Respuesta HTTP → Servicio frontend → Presentador → Vista

## 4. Ejemplo Concreto: Crear una Categoría

1. **En el frontend**:

   ```typescript
   // Vista (React Component)
   const handleCreate = async () => {
     const newCategory = { name: categoryName };
     try {
       await categoryPresenter.createCategory(newCategory);
       // Actualizar UI
     } catch (error) {
       // Manejar error
     }
   };

   // Presentador
   const createCategory = async (data) => {
     return await categoryService.create(data);
   };

   // Servicio API
   async create(category) {
     const response = await axios.post(this.baseUrl, category);
     return response.data;
   }
   ```

2. **En el backend (API Controller)**:

   ```csharp
   [HttpPost]
   public async Task<ActionResult> Create([FromBody] CategoryCreateDto dto)
   {
       var id = await _categoryService.Create(dto);
       return CreatedAtAction(nameof(GetById), new { id }, null);
   }
   ```

3. **En el servicio de aplicación**:

   ```csharp
   public async Task<Guid> Create(CategoryCreateDto dto)
   {
       var category = Category.Create(Guid.NewGuid());
       category.SetName(dto.Name);

       await _categoryRepository.Create(category);
       return category.Id;
   }
   ```

4. **En el repositorio Firebase**:

   ```csharp
   public async Task Create(Category entity)
   {
       var data = new Dictionary<string, object>
       {
           { "name", entity.Name },
           { "creationDate", entity.CreationDate },
           { "editDate", entity.EditDate }
       };

       await _firebaseContext.FirestoreDb
           .Collection(_collectionName)
           .Document(entity.Id.ToString())
           .SetAsync(data);
   }
   ```

## 5. Ventajas de Esta Arquitectura

1. **Separación clara de responsabilidades**: Cada capa tiene un propósito específico.
2. **Fácil sustitución de tecnologías**: Se puede reemplazar Firebase sin afectar el frontend.
3. **Seguridad mejorada**: El frontend nunca tiene acceso directo a Firebase.
4. **Centralización de la lógica de negocio**: Toda la lógica está en el backend.
5. **Testabilidad**: Cada componente puede probarse de forma aislada.
6. **Escalabilidad**: El frontend y backend pueden escalar independientemente.

## 6. Consideraciones de Implementación

1. **Gestión de autenticación**: Implementar JWT u OAuth para autenticar peticiones.
2. **Optimización de queries**: Firestore tiene limitaciones en consultas complejas.
3. **Manejo de transacciones**: Implementar transacciones para operaciones que modifiquen múltiples documentos.
4. **Mapeo de datos**: Crear mappers bidireccionales entre documentos Firestore y entidades de dominio.

Esta arquitectura mantiene los principios de DDD mientras reemplaza solo la capa de datos por Firebase, dejando el resto del flujo de datos intacto.
