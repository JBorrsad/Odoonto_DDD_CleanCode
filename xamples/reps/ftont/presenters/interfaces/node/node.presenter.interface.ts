import { IPresenter } from "nucleus";
import { NodeModel } from "wf.extra.application.mimetic-manager";

export abstract class INodePresenter extends IPresenter {
    abstract getById(id: string): Promise<NodeModel>;
    abstract removeNode(pipelineId: string, nodeId: string): Promise<void>;
} 