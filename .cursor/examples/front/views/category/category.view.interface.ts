import { IView } from "nucleus";
import { CategoryReadModel } from "wf.extra.application.mimetic-manager";


export abstract class ICategoryView extends IView {

	abstract render(category: CategoryReadModel): void;
	

	abstract create(): void;


	abstract navigateToCategories(deleted?: boolean): void;
	

	abstract responseOk(msg: string, isError?: boolean): void;
}
