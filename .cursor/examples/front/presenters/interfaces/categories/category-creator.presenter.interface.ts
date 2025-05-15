import { IPresenter } from "nucleus";

export abstract class ICategoryCreatorPresenter extends IPresenter {
    abstract createCategory(name: string): Promise<string>;
} 