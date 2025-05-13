import { IPresenter } from "nucleus";
import { ICategoryView } from '../../../views/category/category.view.interface';
import { CategoryReadModel, PipelineReadModel } from "wf.extra.application.mimetic-manager";

/**
 * Interfaz para el presenter de categoría
 */
export abstract class ICategoryPresenter extends IPresenter {
	/**
	 * Vista asociada al presenter
	 */
	readonly view: ICategoryView;
	
	/**
	 * Carga una categoría por su ID
	 * @param categoryId ID de la categoría
	 */
	abstract loadCategory(categoryId: string): Promise<void>;

	abstract init(): Promise<void>;
	abstract getPipelines(): PipelineReadModel[];
	abstract getCategory(): CategoryReadModel | null;
	
	/**
	 * Gestiona la acción de eliminar categoría 
	 * Incluye llamar al caso de uso y manejar la respuesta
	 */
	abstract onDeleteClicked(): Promise<void>;
	
	/**
	 * Elimina un pipeline por su ID
	 * @param pipelineId ID del pipeline a eliminar
	 * @returns Promesa que se resuelve cuando se completa la eliminación
	 */
	abstract deletePipeline(pipelineId: string): Promise<void>;
}
