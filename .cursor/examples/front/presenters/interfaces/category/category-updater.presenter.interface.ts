import { IPresenter } from "nucleus";
import { CategoryReadModel } from "wf.extra.application.mimetic-manager";

export abstract class ICategoryUpdaterPresenter extends IPresenter {
    abstract getCategory(id: string): Promise<CategoryReadModel>;
    abstract updateCategory(id: string, name: string): Promise<void>;
    abstract isNameUnique(id: string, name: string): Promise<boolean>;
} 