import { Injector, IPresenter } from "nucleus";
import { ICategoriesView as ICategoriesView } from "../../../views/categories/categories.view.interface";
import { ICategoriesPresenter as ICategoriesPresenter } from "../../interfaces/categories/categories.interface";
import {
	CategoryReadModel,
	ICategoryLoaderService,
	IPipelineLoaderService,
	PipelineReadModel
} from "wf.extra.application.mimetic-manager";

// Interfaces adicionales para manejar la discrepancia entre flows/pipelines
//interface CategoryWithPipelines { no se usa
//	id: string;
//	name: string;
//	pipelines?: Array<{id: string, name: string}>;
//	flows?: Array<{id: string, name: string}>;
//}

export class CategoriesPresenter extends IPresenter implements ICategoriesPresenter {
	get view(): ICategoriesView {
		return this._view as ICategoriesView;
	}

	// Caché para optimizar el rendimiento
	private _categoriesCache: CategoryReadModel[] = [];
	private _pipelinesCache: PipelineReadModel[] = [];
	private _uncategorizedFlows: PipelineReadModel[] = [];
	private _isDataLoaded: boolean = false;
	private _categoriesWithPipelinesCache: Map<string, PipelineReadModel[]> = new Map();

	constructor(
		protected override readonly _view: ICategoriesView,
		private readonly _categoryLoaderService: ICategoryLoaderService,
		private readonly _pipelineLoaderService: IPipelineLoaderService
	) {
		super(_view);
		//this._categoryCreateUseCase = Injector.get(ApplicationTypeSymbols.UseCases.ICategoryCreateUseCase as any) as ICategoryCreateUseCase;
	}

	static create(view: ICategoriesView): CategoriesPresenter {
		const categoryLoaderService = Injector.get<ICategoryLoaderService>(ICategoryLoaderService);
		const pipelineLoaderService = Injector.get(IPipelineLoaderService);
		return new CategoriesPresenter(
			view,
			categoryLoaderService,
			pipelineLoaderService
		);
	}

	/**
	 * Carga todas las categorías, usando caché si es posible
	 */
	async loadCategories(): Promise<void> {
		// Si ya tenemos los datos en caché, usarlos
		if (this._isDataLoaded && this._categoriesCache.length > 0) {
			console.log("Usando datos en caché para categorías");
			this.view.renderCategories(this._categoriesCache);
			return;
		}

		try {
			// 1. Obtener las categorías normales (excluyendo "Sin categoría")
			const normalCategories = await this._categoryLoaderService.getAllCategories();
			
			// 2. Normalizar las categorías para asegurar una estructura consistente
			this._categoriesCache = this.normalizeCategories(normalCategories);
			
			// 3. Obtener los pipelines sin categoría
			this._uncategorizedFlows = await this.identifyUncategorizedPipelines();
			
			// 4. Solo si se encontraron pipelines sin categoría, añadir esa categoría
			if (this._uncategorizedFlows.length > 0) {
				const uncategorizedCategory: CategoryReadModel = {
					id: "sin-categoria",
					name: "Sin categoría",
					flows: this._uncategorizedFlows.map(pipeline => ({
						id: pipeline.id,
						name: pipeline.name
					})),
					createDate: new Date(),
					editDate: new Date()
				};
				
				// Añadir la categoría de pipelines sin categoría al inicio
				this._categoriesCache = [uncategorizedCategory, ...this._categoriesCache];
			}
			
			// Marcar datos como cargados
			this._isDataLoaded = true;
			
			// 5. Renderizar las categorías
			this.view.renderCategories(this._categoriesCache);
			
			console.log(`Categorías cargadas: ${this._categoriesCache.length}`);
		} catch (error) {
			console.error("Error al cargar categorías:", error);
			// Renderizar un estado mínimo para evitar errores en cascada
			this.view.renderCategories([]);
			throw error; // Propagar el error para manejo adecuado
		}
	}

	/**
	 * Inicializa el presentador y carga las categorías y pipelines
	 */
	async init(order: "asc" | "desc" = "asc"): Promise<void> {
		// Si los datos ya están cargados, usarlos
		if (this._isDataLoaded) {
			console.log("Usando datos en caché");
			this._view.renderCategories(this._categoriesCache);
			return;
		}
		
		try {
			// Cargar categorías y pipelines en paralelo para optimizar
			console.log("Cargando datos desde el servidor...");
			const [normalCategories, allPipelines] = await Promise.all([
				this._categoryLoaderService.getAllCategories(),
				this._pipelineLoaderService.getAllPipelines()
			]);
			
			// Guardar en caché
			this._categoriesCache = this.normalizeCategories(normalCategories);
			this._pipelinesCache = allPipelines;
			
			// Identificar pipelines sin categoría usando los datos en caché
			this._uncategorizedFlows = this.identifyUncategorizedPipelinesFromCache(
				this._pipelinesCache, 
				this._categoriesCache
			);
			
			// Añadir categoría "sin-categoria" si hay pipelines sin categorizar
			if (this._uncategorizedFlows.length > 0) {
				const uncategorizedCategory: CategoryReadModel = {
					id: "sin-categoria",
					name: "Sin categoría",
					flows: this._uncategorizedFlows.map(pipeline => ({
						id: pipeline.id,
						name: pipeline.name
					})),
					createDate: new Date(),
					editDate: new Date()
				};
				
				// Añadir al inicio del array
				this._categoriesCache.unshift(uncategorizedCategory);
			}
			
			// Marcar como datos cargados
			this._isDataLoaded = true;
			
			// Renderizar en la vista
			this._view.renderCategories(this._categoriesCache);
			
			console.log(`Categorías cargadas: ${this._categoriesCache.length}, Pipelines cargadas: ${this._pipelinesCache.length}`);
		} catch (error) {
			console.error("Error al inicializar categorías:", error);
			// Renderizar un estado mínimo para evitar errores en cascada
			this._view.renderCategories([]);
		}
	}

	/**
	 * Filtra las categorías según el término de búsqueda
	 */
	filterCategories(searchTerm: string): CategoryReadModel[] {
		// Si no hay término de búsqueda, devolver todas las categorías
		if (!searchTerm || !searchTerm.trim()) {
			return [...this._categoriesCache];
		}
		
		const term = searchTerm.toLowerCase();
		
		// Filtrar categorías que coincidan con el término de búsqueda por nombre
		return this._categoriesCache.filter(category => 
			category.name.toLowerCase().includes(term)
		);
	}

	/**
	 * Exporta todas las categorías a JSON
	 */
	exportCategories(): Promise<string> {
		return this._categoryLoaderService.exportCategories();
	}
	
	/**
	 * Obtiene pipelines de una categoría específica (usando caché)
	 */
	async getPipelinesForCategory(categoryId: string): Promise<PipelineReadModel[]> {
		// Verificar si ya tenemos estos datos en caché
		if (this._categoriesWithPipelinesCache.has(categoryId)) {
			return this._categoriesWithPipelinesCache.get(categoryId)!;
		}
		
		// Buscar la categoría en nuestro caché
		const category = this._categoriesCache.find(c => c.id === categoryId);
		
		if (!category || !category.flows || category.flows.length === 0) {
			return [];
		}
		
		try {
			// Convertir IDs de flujos a pipelines completos
			const pipelinePromises = category.flows.map(flow => {
				// Primero buscar en caché
				const cachedPipeline = this._pipelinesCache.find(p => p.id === flow.id);
				if (cachedPipeline) return Promise.resolve(cachedPipeline);
				
				// Si no está en caché, obtener del servicio
				return this._pipelineLoaderService.getPipelineById(flow.id);
			});
			
			const pipelines = await Promise.all(pipelinePromises);
			
			// Filtrar nulls y almacenar en caché
			const validPipelines = pipelines.filter(p => p !== null) as PipelineReadModel[];
			this._categoriesWithPipelinesCache.set(categoryId, validPipelines);
			
			return validPipelines;
		} catch (error) {
			console.error(`Error obteniendo pipelines para categoría ${categoryId}:`, error);
			return [];
		}
	}

	/**
	 * Normaliza las categorías para asegurar un formato consistente
	 */
	private normalizeCategories(categories: any[]): CategoryReadModel[] {
		return categories.map(category => this.normalizeCategory(category));
	}
	
	/**
	 * Normaliza una categoría individual
	 */
	private normalizeCategory(category: any): CategoryReadModel {
		// Extraer los pipelines de la categoría (propiedad 'pipelines' o 'flows')
		const pipelinesList = this.extractPipelines(category);
		
		// Crear una versión normalizada con la estructura correcta
		return {
			id: category.id || '',
			name: category.name || '',
			flows: pipelinesList.map(p => ({
				id: p.id || '',
				name: p.name || ''
			})),
			createDate: category.createDate || new Date(),
			editDate: category.editDate || new Date()
		};
	}
	
	/**
	 * Extrae los pipelines de una categoría
	 */
	private extractPipelines(category: any): any[] {
		// Verificar si existe la propiedad 'pipelines' (format del API)
		if (category && category.pipelines && Array.isArray(category.pipelines)) {
			return category.pipelines;
		}
		// Verificar si existe la propiedad 'flows' (formato del modelo)
		else if (category && category.flows && Array.isArray(category.flows)) {
			return category.flows;
		}
		return [];
	}
	
	/**
	 * Identifica las pipelines sin categoría usando el API
	 */
	private async identifyUncategorizedPipelines(): Promise<PipelineReadModel[]> {
		try {
			// 1. Obtener todos los pipelines del sistema y guardar en caché
			const allPipelines = await this._pipelineLoaderService.getAllPipelines();
			this._pipelinesCache = allPipelines;
			
			if (!allPipelines || allPipelines.length === 0) {
				return [];
			}
			
			// 2. Obtener todas las categorías con sus detalles completos
			const categories = await this._categoryLoaderService.getAllCategories();
			if (!categories || categories.length === 0) {
				// Si no hay categorías, todos los pipelines están sin categorizar
				return allPipelines;
			}
			
			// 3. Crear un conjunto con los IDs de pipelines categorizados
			const categorizedIds = new Set<string>();
			
			// 4. Para cada categoría, obtener sus detalles completos y extraer los IDs de pipeline
			for (const categoryInfo of categories) {
				// Saltar si es la categoría "sin-categoria"
				if (categoryInfo.id === "sin-categoria") continue;
				
				try {
					// Obtener los detalles completos de la categoría
					const categoryDetail = await this._categoryLoaderService.getCategoryById(categoryInfo.id);
					
					// Añadir todos los IDs de flujos al conjunto de categorizados
					if (categoryDetail && categoryDetail.flows) {
						categoryDetail.flows.forEach(flow => {
							if (flow && flow.id) {
								categorizedIds.add(flow.id);
							}
						});
					}
				} catch (err) {
					console.error(`Error al obtener detalles de categoría ${categoryInfo.id}:`, err);
				}
			}
			
			// 5. Filtrar para obtener solo los pipelines sin categoría
			const uncategorizedPipelines = allPipelines.filter(pipeline => 
				pipeline && 
				pipeline.id && 
				!categorizedIds.has(pipeline.id)
			);
			
			console.log(`Pipelines sin categoría encontrados: ${uncategorizedPipelines.length}`);
			return uncategorizedPipelines;
			
		} catch (error) {
			console.error("Error al identificar pipelines sin categoría:", error);
			return [];
		}
	}
	
	/**
	 * Identifica pipelines sin categoría usando datos en caché
	 */
	private identifyUncategorizedPipelinesFromCache(
		allPipelines: PipelineReadModel[],
		categories: CategoryReadModel[]
	): PipelineReadModel[] {
		// Crear conjunto con IDs de pipelines categorizadas
		const categorizedIds = new Set<string>();
		
		// Recopilar todos los IDs de pipelines en categorías
		categories.forEach(category => {
			if (category.flows) {
				category.flows.forEach(flow => {
					if (flow.id) categorizedIds.add(flow.id);
				});
			}
		});
		
		// Filtrar pipelines sin categoría
		return allPipelines.filter(pipeline => 
			pipeline && pipeline.id && !categorizedIds.has(pipeline.id)
		);
	}

	/**
	 * Limpia el caché cuando sea necesario
	 */
	clearCache(): void {
		this._categoriesCache = [];
		this._pipelinesCache = [];
		this._categoriesWithPipelinesCache.clear();
		this._isDataLoaded = false;
		console.log("Caché limpiado. Se volverán a cargar los datos en la próxima solicitud.");
	}

	render(): void {
		this._view.renderCategories(this._categoriesCache);
	}

	override dispose(): void {
		// No limpiar caché al hacer dispose para mantener datos entre navegaciones
		this._uncategorizedFlows = [];
	}
}
