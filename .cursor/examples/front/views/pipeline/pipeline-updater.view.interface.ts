import { IView } from "nucleus";
import { PipelineReadModel } from "wf.extra.application.mimetic-manager";

export interface IEditPipeline {
    id: string;
    name: string;
    isPublic: boolean;
    categoryId: string;
}

export abstract class IPipelineUpdaterView extends IView {
    abstract render(pipeline: PipelineReadModel): void;
    abstract updatePipeline(updatedPipeline: IEditPipeline): void;
    abstract goBack(): void;
    abstract responseOk(message: string, error?: boolean): void;
} 