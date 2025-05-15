import { IView } from "nucleus";
import { INodeLoaderService } from "wf.extra.application.mimetic-manager";

import { IRemoveNodeUseCase } from "wf.extra.application.mimetic-manager";
import { INodePresenter } from "../../interfaces/node/node.presenter.interface";

export abstract class NodePresenter extends INodePresenter {
    constructor(
        view: IView,
        protected readonly _nodeLoaderService: INodeLoaderService,
        protected readonly _removeNodeUseCaseN: IRemoveNodeUseCase
    ) {
        super(view);
    }

    getById(id: string) {
        return this._nodeLoaderService.getNodeById(id);
    }

    removeNode(pipelineId: string, nodeId: string): Promise<void> {
        return this._removeNodeUseCaseN.execute(pipelineId, nodeId);
    }

    removeUncreatedChild(pipId: string, nodeId: string): void {
        this._removeNodeUseCaseN.executeChild(pipId, nodeId);
    }
} 