import { IPresenter } from "nucleus";
import { ICategoriesView } from "../../../views/categories/categories.view.interface";

/**
 * Interfaz para el presenter de lista de categorías
 */
export abstract class ICategoriesPresenter extends IPresenter {
	/**
	 * Vista asociada al presenter
	 */
	readonly view: ICategoriesView;
	
	/**
	 * Carga todas las categorías
	 */
	abstract loadCategories(): Promise<void>;
}
