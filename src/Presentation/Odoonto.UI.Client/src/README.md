# Frontend React con Patrón MVP

Este proyecto implementa un frontend React con TypeScript siguiendo el patrón Model-View-Presenter (MVP).

## Arquitectura MVP

### Models (src/models)
- Representan los datos de la aplicación
- Contienen lógica de validación básica
- Mapean los DTOs recibidos de la API a modelos de frontend
- No contienen lógica de negocio compleja

### Views (src/views)
- Componentes React puros (presentacionales)
- No tienen estado propio (o muy limitado)
- Reciben props de los presenters
- Delegan eventos al presenter (onClick, onChange, etc.)
- Pueden usar componentes UI reutilizables

### Presenters (src/presenters)
- Contienen la lógica de UI y estado
- Conectan los modelos con las vistas
- Gestionan el ciclo de vida de los componentes
- Manejan llamadas a servicios/API
- Implementan la lógica de presentación

## Flujo de Datos

1. Usuario interactúa con un componente View
2. View notifica al Presenter sobre la acción
3. Presenter actualiza el estado/modelo según sea necesario
4. Presenter llama a servicios API si es necesario
5. Cuando llegan datos de la API, el Presenter actualiza el modelo
6. Presenter pasa los datos actualizados a la View
7. View se renderiza con los nuevos datos

## Ejemplo de Implementación

```typescript
// src/models/category.model.ts
export interface CategoryModel {
  id: string;
  name: string;
  isActive: boolean;
}

// src/services/api/category.service.ts
export class CategoryApiService {
  async getCategories(): Promise<CategoryModel[]> {
    const response = await axios.get('/api/categories');
    return response.data;
  }
}

// src/presenters/category-list.presenter.ts
export class CategoryListPresenter {
  private categories: CategoryModel[] = [];
  private loading = false;
  private view: CategoryListView;
  private service: CategoryApiService;

  constructor(view: CategoryListView, service: CategoryApiService) {
    this.view = view;
    this.service = service;
  }

  async loadCategories() {
    this.loading = true;
    this.view.setLoading(true);
    
    try {
      this.categories = await this.service.getCategories();
      this.view.setCategories(this.categories);
    } catch (error) {
      this.view.showError("Error loading categories");
    } finally {
      this.loading = false;
      this.view.setLoading(false);
    }
  }
}

// src/views/category-list.view.tsx
interface CategoryListViewProps {
  presenter: CategoryListPresenter;
}

export const CategoryListView: React.FC<CategoryListViewProps> = ({ presenter }) => {
  const [categories, setCategories] = useState<CategoryModel[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    presenter.loadCategories();
  }, [presenter]);

  return (
    <div>
      {loading && <Spinner />}
      {error && <Alert variant="danger">{error}</Alert>}
      <ul>
        {categories.map(category => (
          <li key={category.id}>{category.name}</li>
        ))}
      </ul>
    </div>
  );
};
```

## Reglas de Implementación

1. Mantener separación clara entre Model, View y Presenter
2. No hacer llamadas a API directamente desde las Views
3. Los Presenters deben ser testables independientemente
4. Pasar referencias al Presenter a través de props o hooks
5. Evitar la lógica de negocio en las Views 