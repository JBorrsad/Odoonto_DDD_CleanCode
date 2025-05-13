/****************************************************************************************
 * IMPORTS
 ****************************************************************************************/
import { Injector } from 'nucleus';
import {
  IPipelineLoaderService,
  IPipelineEditUseCase,
  PipelineReadModel,
  IAddFlowToCategoryUseCase,
  IRemoveFlowFromCategoryUseCase,
  ICategoryLoaderService,
  CategoryReadModel
} from 'wf.extra.application.mimetic-manager';

import {
  IPipelineUpdaterPresenter
} from '../../interfaces/pipeline/pipeline-updater.presenter.interface';
import {
  IEditPipeline,
  IPipelineUpdaterView
} from '../../../views/pipeline/pipeline-updater.view.interface';


/****************************************************************************************
 * PRESENTER
 ****************************************************************************************/
export class PipelineUpdaterPresenter extends IPipelineUpdaterPresenter {

  /*********************************** General **************************************/
  private _pipelineId!: string;
  private _currentCategoryId: string = "sin-categoria";
  private _categories: CategoryReadModel[] = [];

  private constructor(
    view: IPipelineUpdaterView,
    private readonly _loader: IPipelineLoaderService,
    private readonly _editor: IPipelineEditUseCase,
    private readonly _categoryLoader: ICategoryLoaderService,
    private readonly _addFlowToCategory: IAddFlowToCategoryUseCase,
    private readonly _removeFlowFromCategory: IRemoveFlowFromCategoryUseCase
  ) { super(view); }

  static create(view: IPipelineUpdaterView): PipelineUpdaterPresenter {
    return new PipelineUpdaterPresenter(
      view,
      Injector.get(IPipelineLoaderService),
      Injector.get(IPipelineEditUseCase),
      Injector.get(ICategoryLoaderService),
      Injector.get(IAddFlowToCategoryUseCase),
      Injector.get(IRemoveFlowFromCategoryUseCase)
    );
  }

  override dispose(): void { /* no subscriptions yet */ }


  /*********************************** Pipelines *************************************/
  /** Arranca el flujo: guarda el id y pinta la vista */
  async onInit(id: string): Promise<void> {
    this._pipelineId = id;
    try {
      // Cargar las categorías
      this._categories = await this._categoryLoader.getAllCategories();
      
      // Cargar la pipeline
      const pip = await this.getPipeline();
      this._currentCategoryId = pip.categoryId || "sin-categoria";
      
      // Renderizar la vista
      (this._view as IPipelineUpdaterView).render(pip);
    } catch (e: any) {
      this.showToast(e?.message ?? 'Error al cargar la pipeline', true);
    }
  }

  public async getPipeline(): Promise<PipelineReadModel> {
    return this._loader.getPipelineById(this._pipelineId);
  }
  
  public getCategories(): CategoryReadModel[] {
    return this._categories;
  }

  /** Llamado por la vista al pulsar "Guardar cambios" */
  public async updatePipeline(id: string, dto: IEditPipeline): Promise<void> {
    try {
      // 1. Actualizar los datos básicos de la pipeline
      await this._editor.execute(id, dto.name, dto.isPublic);
      
      // 2. Gestionar la asignación de categoría
      const newCategoryId = dto.categoryId;
      
      // Si ha cambiado la categoría
      if (newCategoryId !== this._currentCategoryId) {
        
        // 2.1 Si la pipeline estaba en una categoría (no "sin-categoria"), eliminarla
        if (this._currentCategoryId !== "sin-categoria") {
          await this._removeFlowFromCategory.execute(this._currentCategoryId, id);
        }
        
        // 2.2 Si la nueva categoría no es "sin-categoria", añadirla a esta
        if (newCategoryId !== "sin-categoria") {
          await this._addFlowToCategory.execute(newCategoryId, id);
        }
        
        // 2.3 Actualizar la categoría actual para futuras ediciones
        this._currentCategoryId = newCategoryId;
      }
      
      this.showToast('Pipeline actualizada con éxito', false);
      // Permitir que la vista maneje la navegación basada en la categoría actual
      (this._view as IPipelineUpdaterView).goBack();
    } catch (e: any) {
      this.showToast(e?.data?.Message ?? 'Error al actualizar la pipeline', true);
    }
  }

  /*********************************** Errores **************************************/
  private showToast(msg: string, isError = false): void {
    (this._view as IPipelineUpdaterView).responseOk(msg, isError);
  }

  public async isNameUnique(name: string): Promise<boolean> {
    // Usa el servicio que tengas disponible; si no existe, stub temporal.
    if ((this._loader as any).isNameUnique) {
      return (this._loader as any).isNameUnique(name, this._pipelineId);
    }
    // Fallback: siempre "único" (evita error de compilación).
    return true;
  }
}
