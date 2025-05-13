import { IPresenter } from "nucleus";
import { CategoryReadModel } from "wf.extra.application.mimetic-manager";

export abstract class IPipelineCreatorPresenter extends IPresenter {
    abstract createPipeline(name: string, isPublic?: boolean, categoryId?: string): Promise<string>;
    abstract loadCategories(): Promise<CategoryReadModel[]>;
    abstract isNameUnique(name: string): Promise<boolean>;
    abstract validateAndCreatePipeline(name: string, isPublic?: boolean, categoryId?: string): Promise<{ success: boolean, pipelineId?: string, errorMessage?: string }>;
} 