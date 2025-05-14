import { IView } from "nucleus";
import { CategoryReadModel } from "wf.extra.application.mimetic-manager";

/**
 * Interfaz para la vista de lista de categorías
 */
export abstract class ICategoriesView extends IView {
	/**
	 * Renderiza la lista de categorías
	 * @param categories Lista de categorías a renderizar
	 */
	abstract renderCategories(categories: CategoryReadModel[]): void;
}
