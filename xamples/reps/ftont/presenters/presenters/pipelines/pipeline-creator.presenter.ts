import { Injector } from "nucleus";
import { 
    IPipelineCreateUseCase, 
    IPipelineLoaderService, 
    ICategoryLoaderService,
    IAddFlowToCategoryUseCase,
    CategoryReadModel 
} from "wf.extra.application.mimetic-manager";
import { IPipelineCreatorPresenter } from "../../interfaces/pipelines/pipeline-creator.presenter.interface";
import { IPipelineCreatorView } from "../../../views/pipelines/pipeline-creator.view.interface";

export class PipelineCreatorPresenter extends IPipelineCreatorPresenter {
    private constructor(
        view: IPipelineCreatorView,
        private readonly _pipelineCreateUseCase: IPipelineCreateUseCase,
        private readonly _pipelineLoaderService: IPipelineLoaderService,
        private readonly _categoryLoaderService: ICategoryLoaderService,
        private readonly _addFlowToCategoryUseCase: IAddFlowToCategoryUseCase
    ) {
        super(view);
    }

    static create(view: IPipelineCreatorView) {
        const _pipelineCreateUseCase = Injector.get(IPipelineCreateUseCase);
        const _pipelineLoaderService = Injector.get(IPipelineLoaderService);
        const _categoryLoaderService = Injector.get(ICategoryLoaderService);
        const _addFlowToCategoryUseCase = Injector.get(IAddFlowToCategoryUseCase);
        
        return new PipelineCreatorPresenter(
            view, 
            _pipelineCreateUseCase, 
            _pipelineLoaderService,
            _categoryLoaderService,
            _addFlowToCategoryUseCase
        );
    }

    async createPipeline(name: string, isPublic: boolean = false, categoryId: string = "sin-categoria"): Promise<string> {
        // 1. Crear la pipeline (el backend actualmente ignora isPublic)
        const pipelineId = await this._pipelineCreateUseCase.execute(name);
        
        // 2. Si hay una categoría seleccionada y no es "sin-categoria", añadir la pipeline a esa categoría
        if (categoryId && categoryId !== "sin-categoria") {
            try {
                await this._addFlowToCategoryUseCase.execute(categoryId, pipelineId);
            } catch (error) {
                console.error("Error añadiendo pipeline a categoría:", error);
                // No bloqueamos el flujo si hay un error, solo lo registramos
            }
        }
        
        return pipelineId;
    }
    
    async loadCategories(): Promise<CategoryReadModel[]> {
        try {
            return await this._categoryLoaderService.getAllCategories();
        } catch (error) {
            console.error("Error cargando categorías:", error);
            return [];
        }
    }
    
    async isNameUnique(name: string): Promise<boolean> {
        if (!name || name.trim() === '') {
            return false;
        }
        
        const pipelines = await this._pipelineLoaderService.getAllPipelines();
        return !pipelines.some(pip => pip.name.toLowerCase() === name.toLowerCase().trim());
    }

    async validateAndCreatePipeline(name: string, isPublic: boolean = false, categoryId: string = "sin-categoria"): Promise<{success: boolean, pipelineId?: string, errorMessage?: string}> {
        // Validar primero
        if (!name || name.trim() === '') {
            return {
                success: false,
                errorMessage: "El nombre de la pipeline no puede estar vacío"
            };
        }

        // Verificar unicidad
        const isUnique = await this.isNameUnique(name);
        if (!isUnique) {
            return {
                success: false,
                errorMessage: "Ya existe una pipeline con este nombre"
            };
        }

        try {
            // Crear pipeline
            const pipelineId = await this.createPipeline(name, isPublic, categoryId);
            return {
                success: true,
                pipelineId
            };
        } catch (error) {
            return {
                success: false,
                errorMessage: `Error al crear la pipeline: ${error?.data?.Message || error}`
            };
        }
    }
} 