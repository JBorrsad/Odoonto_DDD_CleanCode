import { IView } from "nucleus";
import { CategoryReadModel } from "wf.extra.application.mimetic-manager";

export abstract class ICategoryUpdaterView extends IView {
    abstract render(category: CategoryReadModel): void;
    abstract updateCategory(): void;
    abstract goBack(): void;
    abstract responseOk(message: string, error?: boolean): void;
} 