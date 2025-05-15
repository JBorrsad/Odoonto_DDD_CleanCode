import { IPresenter } from "nucleus";
import { IEditPipeline } from "../../../views/pipeline/pipeline-updater.view.interface";
import { PipelineReadModel } from "wf.extra.application.mimetic-manager";

export abstract class IPipelineUpdaterPresenter extends IPresenter {
    abstract getPipeline(id: string): Promise<PipelineReadModel>;
    abstract updatePipeline(id: string, pipeline: IEditPipeline): Promise<void>;
    abstract isNameUnique(id: string, name: string): Promise<boolean>;
} 