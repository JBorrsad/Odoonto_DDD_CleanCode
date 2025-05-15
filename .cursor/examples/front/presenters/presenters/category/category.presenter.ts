import { ICategoryView } from "../../../views/category/category.view.interface";
import {
	ICategoryLoaderService,
	IPipelineLoaderService,
	PipelineReadModel,
	CategoryReadModel,
	ICategoryDeleteUseCase,
	IPipelineDeleteUseCase
} from "wf.extra.application.mimetic-manager";
import { Injector, IPresenter, Guid } from "nucleus";
import { ICategoryPresenter } from "../../interfaces/category/category.presenter.interface";
import { Pipeline } from "wf.extra.domain.mimetic-manager";

export class CategoryPresenter
	extends IPresenter
	implements ICategoryPresenter
{
	private _pipelines: PipelineReadModel[] = [];
	private _categoryId: string = "";
	private _category: CategoryReadModel | null = null;

	get view(): ICategoryView {
		return this._view as ICategoryView;
	}

	constructor(
		protected override readonly _view: ICategoryView,
		private readonly _pipelineLoaderService: IPipelineLoaderService,
		private readonly _categoryLoaderService: ICategoryLoaderService,
		private readonly _categoryDeleteUseCase: ICategoryDeleteUseCase,
		private readonly _pipelineDeleteUseCase: IPipelineDeleteUseCase,
		categoryId?: string
	) {
		super(_view);
		if (categoryId) {
			this._categoryId = categoryId;
		}
	}

	static create(view: ICategoryView, categoryId: string): CategoryPresenter {
		const pipelineLoaderService = Injector.get<IPipelineLoaderService>(
			IPipelineLoaderService
		);
		const categoryLoaderService = Injector.get<ICategoryLoaderService>(
			ICategoryLoaderService
		);
		const categoryDeleteUseCase = Injector.get<ICategoryDeleteUseCase>(
			ICategoryDeleteUseCase
		);
		const pipelineDeleteUseCase = Injector.get<IPipelineDeleteUseCase>(
			IPipelineDeleteUseCase
		);
		return new CategoryPresenter(
			view,
			pipelineLoaderService,
			categoryLoaderService,
			categoryDeleteUseCase,
			pipelineDeleteUseCase,
			categoryId
		);
	}

	async init(): Promise<void> {
		try {
			// Limpiar pipelines antes de cargar
			this._pipelines = [];

			// Determinar qué cargar basado en el ID de categoría
			if (this._categoryId === "sin-categoria") {
				// Caso especial: cargar solo pipelines sin categoría
				await this.loadUncategorizedPipelines();

				// Para el caso de "sin-categoria", crear una categoría ficticia
				const uncategorizedCategory: CategoryReadModel = {
					id: "sin-categoria",
					name: "Sin Categoría",
					flows: [],
					createDate: new Date(),
					editDate: new Date(),
				};

				// Guardar la categoría ficticia
				this._category = uncategorizedCategory;

				// Actualizar la vista con la categoría ficticia
				this._view.render(uncategorizedCategory);
			} else if (this._categoryId) {
				// Cargar pipelines de una categoría específica
				await this.loadCategoryPipelines();

				// Obtener la categoría actual
				this._category = await this._categoryLoaderService.getCategoryById(
					this._categoryId
				);
				// Actualizar la vista con la categoría
				this._view.render(this._category);
			}
		} catch (error) {
			console.error(
				`Error al cargar pipelines para categoría ${this._categoryId}:`,
				error
			);
			this._pipelines = [];
			throw error;
		}
	}

	private async loadCategoryPipelines(): Promise<void> {
		try {
			// Obtener la categoría completa por su ID
			const category = await this._categoryLoaderService.getCategoryById(
				this._categoryId
			);

			// Si la categoría tiene pipelines asociados, cargarlos
			if (category && category.flows && category.flows.length > 0) {
				// Crear una lista de promesas para cargar los pipelines
				const pipelinePromises = category.flows.map((flow) => {
					if (flow && flow.id) {
						return this._pipelineLoaderService
							.getPipelineById(flow.id)
							.catch((error) => {
								console.error(`Error al cargar pipeline ${flow.id}:`, error);
								return null;
							});
					}
					return Promise.resolve(null);
				});

				// Esperar a que todas las promesas se resuelvan
				const pipelines = await Promise.all(pipelinePromises);

				// Filtrar pipelines nulos (que fallaron al cargarse)
				this._pipelines = pipelines.filter(
					(p) => p !== null
				) as PipelineReadModel[];
			}
		} catch (error) {
			console.error(
				`Error al cargar pipelines para categoría ${this._categoryId}:`,
				error
			);
			this._pipelines = [];
		}
	}

	private async loadUncategorizedPipelines(): Promise<void> {
		try {
			// 1. Obtener todos los pipelines disponibles
			const allPipelines = await this._pipelineLoaderService.getAllPipelines();

			// 2. Obtener todas las categorías
			const categories = await this._categoryLoaderService.getAllCategories();

			// 3. Crear un conjunto con los IDs de pipelines que pertenecen a alguna categoría
			const categorizedIds = new Set<string>();

			// 4. Para cada categoría, obtener su detalle completo para acceder a los flujos asociados
			for (const categoryInfo of categories) {
				// Saltar si es la categoría "sin-categoria"
				if (categoryInfo.id === "sin-categoria") continue;

				try {
					// Obtener los detalles completos de la categoría
					const categoryDetail =
						await this._categoryLoaderService.getCategoryById(categoryInfo.id);

					// Añadir todos los IDs de flujos al conjunto de categorizados
					if (
						categoryDetail &&
						categoryDetail.flows &&
						Array.isArray(categoryDetail.flows)
					) {
						categoryDetail.flows.forEach((flow) => {
							if (flow && flow.id) {
								categorizedIds.add(flow.id);
							}
						});
					}
				} catch (err) {
					console.error(
						`Error al obtener detalles de categoría ${categoryInfo.id}:`,
						err
					);
				}
			}

			// 5. Filtrar para obtener solo los pipelines sin categoría
			this._pipelines = allPipelines.filter(
				(pipeline) =>
					pipeline && pipeline.id && !categorizedIds.has(pipeline.id)
			);
		} catch (error) {
			console.error("Error al cargar pipelines sin categoría:", error);
			this._pipelines = [];
		}
	}

	getPipelines(): PipelineReadModel[] {
		return [...this._pipelines];
	}

	getCategory(): CategoryReadModel | null {
		return this._category;
	}

	override dispose(): void {
		this._pipelines = [];
		this._category = null;
	}

	async loadCategory(categoryId: string): Promise<void> {
		this._category = await this._categoryLoaderService.getCategoryById(
			categoryId
		);
		this.view.render(this._category);
	}

	async onDeleteClicked(): Promise<void> {
		if (!this._categoryId || this._categoryId === "sin-categoria") {
			this._view.responseOk("No se puede eliminar la categoría 'Sin Categoría'", true);
			return;
		}

		try {
			console.log(`Iniciando eliminación de categoría ${this._categoryId}...`);
			
			// Ejecutar el caso de uso para eliminar la categoría
			await this._categoryDeleteUseCase.execute(this._categoryId);
			
			console.log(`Categoría ${this._categoryId} eliminada correctamente`);
			
			// Informar al usuario que la eliminación fue exitosa
			this._view.responseOk("La categoría ha sido eliminada correctamente", false);
			
			// Esperar un momento antes de navegar para permitir que el toast se muestre
			setTimeout(() => {
				// Navegar de vuelta a la lista de categorías
				(this._view as ICategoryView).navigateToCategories(true);
			}, 500);
			
		} catch (error) {
			console.error(`Error al eliminar la categoría ${this._categoryId}:`, error);
			
			// Proporcionar un mensaje de error más descriptivo
			let errorMessage = "Error al eliminar la categoría";
			if (error instanceof Error) {
				errorMessage += `: ${error.message}`;
			} else if (typeof error === 'string') {
				errorMessage += `: ${error}`;
			}
			
			this._view.responseOk(errorMessage, true);
		}
	}

	exportCategory(id: string){
		return this._categoryLoaderService.exportCategory(id)
	}

	/**
	 * Elimina un pipeline por su ID
	 * @param pipelineId ID del pipeline a eliminar
	 * @returns Promesa que se resuelve cuando se completa la eliminación
	 */
	async deletePipeline(pipelineId: string): Promise<void> {
		try {
			// Ejecutar el caso de uso para eliminar el pipeline
			console.log(`Iniciando eliminación de pipeline ${pipelineId}...`);
			// Convertir el ID a Guid (requerido por IPipelineDeleteUseCase)
			await this._pipelineDeleteUseCase.execute(pipelineId as unknown as Guid);
			
			console.log(`Pipeline ${pipelineId} eliminado correctamente`);
			
			// Recargar pipelines después de la eliminación
			await this.init();
			
		} catch (error) {
			console.error(`Error al eliminar el pipeline ${pipelineId}:`, error);
			
			// Propagar el error para que sea manejado por la vista
			throw error;
		}
	}
}
