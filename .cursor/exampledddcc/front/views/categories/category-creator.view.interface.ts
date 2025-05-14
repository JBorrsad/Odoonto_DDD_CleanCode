import { IView } from "nucleus";

export abstract class ICategoryCreatorView extends IView {
    abstract createCategory(): void;
    abstract goBack(): void;
} 