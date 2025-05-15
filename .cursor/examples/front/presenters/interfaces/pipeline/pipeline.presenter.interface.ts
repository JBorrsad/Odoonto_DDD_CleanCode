import { Guid, IPresenter } from "nucleus";
import { PipelineReadModel } from "wf.extra.application.mimetic-manager";

export abstract class IPipelinePresenter extends IPresenter {
    abstract getPipeline(id: string): Promise<PipelineReadModel>;
    abstract deletePipeline(id: Guid): Promise<void>;
} 