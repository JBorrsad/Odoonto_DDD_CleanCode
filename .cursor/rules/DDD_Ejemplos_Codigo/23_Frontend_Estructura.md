# Estructura de Frontend con React y TypeScript siguiendo DDD y MVP

Este documento describe la estructura recomendada para el frontend de la aplicación, que será un proyecto separado del backend pero manteniendo el enfoque de Domain-Driven Design (DDD) adaptado al frontend y el patrón Model-View-Presenter (MVP).

## 1. Estructura de Directorios del Proyecto Frontend

```
frontend/
  ├── public/                 # Archivos estáticos públicos
  ├── src/                    # Código fuente
  │   ├── models/             # Modelos de dominio (capa de dominio)
  │   │   ├── common/         # Interfaces y tipos comunes
  │   │   ├── categories/     # Modelos de categorías
  │   │   ├── flows/          # Modelos de flujos
  │   │   └── nodes/          # Modelos de nodos
  │   │
  │   ├── services/           # Servicios de datos (capa de infraestructura)
  │   │   ├── api/            # Servicios de API REST
  │   │   │   ├── base/       # Funcionalidad base para servicios de API
  │   │   │   ├── categories/ # Servicios específicos para categorías
  │   │   │   └── ...         # Otros servicios de API
  │   │   │
  │   │   └── utils/          # Utilidades y funciones auxiliares
  │   │
  │   ├── presenters/         # Presentadores (capa de aplicación)
  │   │   ├── categories/     # Presentadores para categorías
  │   │   ├── flows/          # Presentadores para flujos
  │   │   ├── nodes/          # Presentadores para nodos
  │   │   └── hooks/          # Hooks personalizados para lógica reutilizable
  │   │
  │   ├── views/              # Vistas (componentes de UI)
  │   │   ├── components/     # Componentes reutilizables
  │   │   │   ├── atoms/      # Componentes atómicos (botones, inputs, etc.)
  │   │   │   ├── molecules/  # Componentes moleculares (tarjetas, formularios, etc.)
  │   │   │   └── organisms/  # Componentes organizacionales (tablas, paneles, etc.)
  │   │   │
  │   │   ├── pages/          # Páginas/rutas de la aplicación
  │   │   │   ├── categories/ # Páginas relacionadas con categorías
  │   │   │   ├── flows/      # Páginas relacionadas con flujos
  │   │   │   └── ...         # Otras páginas
  │   │   │
  │   │   └── layouts/        # Layouts y estructura de la aplicación
  │   │
  │   ├── context/            # Contextos de React (estado global)
  │   │   ├── auth/           # Contexto de autenticación
  │   │   └── ...             # Otros contextos
  │   │
  │   ├── config/             # Configuración de la aplicación
  │   │   ├── routes.tsx      # Configuración de rutas
  │   │   └── theme.ts        # Configuración del tema
  │   │
  │   ├── styles/             # Estilos globales y temas
  │   │   ├── global.css      # Estilos globales
  │   │   └── variables.css   # Variables CSS
  │   │
  │   ├── locales/            # Archivos de internacionalización
  │   │   ├── es/             # Traducciones en español
  │   │   └── en/             # Traducciones en inglés
  │   │
  │   ├── App.tsx             # Componente principal
  │   └── index.tsx           # Punto de entrada
  │
  ├── tests/                  # Pruebas
  │   ├── unit/               # Pruebas unitarias
  │   ├── integration/        # Pruebas de integración
  │   └── e2e/                # Pruebas end-to-end
  │
  ├── .env                    # Variables de entorno
  ├── .env.development        # Variables específicas para desarrollo
  ├── .env.production         # Variables específicas para producción
  ├── package.json            # Dependencias y scripts
  ├── tsconfig.json           # Configuración de TypeScript
  └── README.md               # Documentación
```

## 2. Enfoque de DDD en el Frontend

Aunque DDD se concibió originalmente para el backend, sus principios pueden adaptarse al frontend, especialmente en aplicaciones complejas:

### 2.1. Capa de Dominio (Modelos)

- Define las entidades y objetos de valor del frontend.
- Implementa validaciones y lógica de negocio específica del cliente.
- Mantiene la integridad y coherencia de los datos en el frontend.

```typescript
// Ejemplo: src/models/categories/Category.model.ts
export interface CategoryBase {
  name: string;
}

export interface CategoryReadModel extends Entity, CategoryBase {
  flows: FlowInCategoryModel[];
}

// Clase con comportamiento para manipular categorías localmente
export class Category implements CategoryReadModel {
  id: string;
  name: string;
  creationDate: string;
  editDate: string;
  flows: FlowInCategoryModel[];

  constructor(data: CategoryReadModel) {
    // Inicialización
  }

  // Métodos para validación y manipulación local
  addFlow(flow: FlowInCategoryModel): void {
    // Lógica para agregar un flujo localmente
  }

  isValid(): boolean {
    // Lógica de validación
  }
}
```

### 2.2. Capa de Infraestructura (Servicios)

- Implementa la comunicación con la API REST del backend.
- Gestiona la serialización/deserialización de datos.
- Abstrae los detalles de comunicación HTTP.

```typescript
// Ejemplo: src/services/api/categories/category.service.ts
export class CategoryService {
  private http: AxiosInstance;
  private baseUrl: string;

  constructor() {
    // Inicialización
  }

  async getAll(): Promise<CategoryQueryModel[]> {
    // Implementación de llamada a la API REST
  }

  async getById(id: string): Promise<CategoryReadModel> {
    // Implementación de llamada a la API REST
  }

  // Otros métodos
}
```

### 2.3. Capa de Aplicación (Presentadores)

- Orquesta la lógica de presentación y casos de uso.
- Conecta los modelos con las vistas.
- Gestiona el estado de la aplicación.

```typescript
// Ejemplo: src/presenters/categories/useCategoryPresenter.ts
export function useCategoryPresenter() {
  // Estado
  const [categories, setCategories] = useState<Category[]>([]);

  // Métodos para cargar datos y ejecutar acciones
  const loadCategories = async () => {
    // Implementación usando el servicio API
  };

  const createCategory = async (data: CategoryCreateModel) => {
    // Implementación usando el servicio API
  };

  // Retornar estados y métodos para la vista
  return {
    categories,
    loading,
    error,
    loadCategories,
    createCategory,
    // ...etc.
  };
}
```

### 2.4. Capa de UI (Vistas)

- Implementa componentes de React responsables de la presentación.
- Se comunica exclusivamente con los presentadores.
- No contiene lógica de negocio.

```typescript
// Ejemplo: src/views/pages/categories/CategoryList.tsx
export const CategoryList: React.FC = () => {
  // Utilizar el presentador
  const { categories, loading, error, loadCategories, createCategory } =
    useCategoryPresenter();

  // Lógica de UI y renderizado
  return <div>{/* Implementación de la interfaz de usuario */}</div>;
};
```

## 3. Patrón MVP en React

El patrón MVP (Model-View-Presenter) se implementa de la siguiente manera:

1. **Model**: Representado por los modelos en `src/models/`.
2. **View**: Componentes React en `src/views/`.
3. **Presenter**: Custom hooks en `src/presenters/`.

Beneficios:

- **Separación de responsabilidades**: Cada componente tiene un propósito único.
- **Testabilidad**: Los presentadores pueden probarse independientemente de las vistas.
- **Reutilización**: La lógica de presentación puede compartirse entre vistas.
- **Mantenibilidad**: Cambios en la UI no afectan la lógica de negocio.

## 4. Comunicación con el Backend

### 4.1. Enfoque API REST

- Los servicios se comunican con el backend a través de endpoints REST.
- Utilizan axios u otra biblioteca para realizar peticiones HTTP.
- Las respuestas se mapean a modelos de dominio.
- Toda la lógica de negocio y acceso a datos está en el backend.
- El frontend nunca accede directamente a la base de datos.

```typescript
// Ejemplo de servicio de API
export class CategoryApiService {
  private readonly baseUrl = "https://api.tuproyecto.com/api/categories";

  async getAll(): Promise<CategoryDto[]> {
    const response = await axios.get(this.baseUrl);
    return response.data;
  }

  async create(category: CategoryCreateDto): Promise<string> {
    const response = await axios.post(this.baseUrl, category);
    return response.data.id;
  }
}
```

## 5. Gestión del Estado

### 5.1. Estado Local (Componentes y Presentadores)

- Estado de UI específico de componentes: Usar `useState` de React.
- Estado de casos de uso: Gestionar en presentadores usando hooks personalizados.

### 5.2. Estado Global (Cuando sea necesario)

- Para estado compartido entre múltiples componentes: Usar Context API.
- Para estado global complejo: Considerar Redux o MobX si es necesario.

## 6. Arquitectura de Componentes (Atomic Design)

Se utiliza una arquitectura basada en Atomic Design para los componentes:

1. **Átomos**: Componentes básicos como botones, inputs, iconos.
2. **Moléculas**: Grupos de átomos como formularios, tarjetas, barras de búsqueda.
3. **Organismos**: Grupos de moléculas como tablas de datos, paneles complejos.
4. **Templates**: Layouts y estructuras de página.
5. **Pages**: Implementaciones específicas que representan rutas/vistas completas.

## 7. Implementación del Routing

```typescript
// src/config/routes.tsx
import { createBrowserRouter } from "react-router-dom";
import { CategoryList } from "../views/pages/categories/CategoryList";
import { CategoryDetail } from "../views/pages/categories/CategoryDetail";
// Más importaciones...

export const router = createBrowserRouter([
  {
    path: "/",
    element: <MainLayout />,
    children: [
      {
        path: "categories",
        element: <CategoryList />,
      },
      {
        path: "categories/:id",
        element: <CategoryDetail />,
      },
      // Más rutas...
    ],
  },
]);
```

## 8. Estrategia de Estilos

Se recomienda un enfoque de CSS-in-JS con styled-components o emotion, o utilizar una biblioteca de componentes como Material-UI, Chakra UI o Ant Design.

```typescript
// Ejemplo con styled-components
import styled from "styled-components";

const Button = styled.button`
  background-color: var(--primary-color);
  color: white;
  padding: 8px 16px;
  border-radius: 4px;

  &:hover {
    background-color: var(--primary-dark);
  }
`;

export default Button;
```

## 9. Internacionalización

Utilizar react-i18next o similar para gestionar traducciones:

```typescript
// src/locales/es/common.json
{
  "categories": {
    "title": "Categorías",
    "newButton": "Nueva Categoría",
    "emptyState": "No hay categorías disponibles"
  }
}

// Uso en componentes
import { useTranslation } from 'react-i18next';

function MyComponent() {
  const { t } = useTranslation();
  return <h1>{t('categories.title')}</h1>;
}
```

## 10. Testing

### 10.1. Pruebas Unitarias

- **Modelos**: Probar validaciones y lógica de negocio.
- **Presentadores**: Probar la lógica de presentación.
- **Servicios**: Probar la lógica de comunicación (mockear las peticiones).

### 10.2. Pruebas de Integración

- Probar la integración entre presentadores y vistas.
- Probar la integración entre servicios y presentadores.

### 10.3. Pruebas End-to-End

- Probar flujos completos de usuario con Cypress o Playwright.

## 11. Consideraciones Específicas para Typescript

- Utilizar tipos estrictos y evitar `any`.
- Definir interfaces para todos los modelos y props.
- Utilizar `discriminated unions` para manejar estados complejos.
- Aprovechar características avanzadas como `generics` y `utility types`.

## 12. Configuración y Despliegue

- Configurar variables de entorno para diferentes entornos.
- Implementar proceso de CI/CD.
- Estrategias de optimización de rendimiento (code splitting, lazy loading).
- Monitoreo y analítica.
