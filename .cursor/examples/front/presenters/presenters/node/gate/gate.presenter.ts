import { ICreateNodesUseCase, IEditNodeUseCase, INodeLoaderService, IRemoveNodeUseCase } from "wf.extra.application.mimetic-manager";

import { Injector, IPresenter, IView } from "nucleus";
import { NodePresenter } from "../node.presenter";
import { IGatePresenter } from "../../../interfaces/node/gate/gate-presenter.interface";

export class GatePresenter extends NodePresenter implements IGatePresenter{

    private constructor(
        _view: IView,
        private readonly _nodesCreateUseCase: ICreateNodesUseCase,
        private readonly _nodesLoaderService: INodeLoaderService,
        private readonly _nodeEditService: IEditNodeUseCase,
        protected readonly _removeNodeUseCase: IRemoveNodeUseCase
    ){
        super(_view, _nodesLoaderService, _removeNodeUseCase);
    }
    get presenter(): IPresenter {
        throw new Error("Method not implemented.");
    }


    static create(view: IView) {
        const nodeCreateUseCase = Injector.get(ICreateNodesUseCase);
        const nodeLoaderService = Injector.get(INodeLoaderService);
        const nodeEditService = Injector.get(IEditNodeUseCase);
        const removeNodeUseCase = Injector.get(IRemoveNodeUseCase)
        return new GatePresenter(view, nodeCreateUseCase, nodeLoaderService, nodeEditService, removeNodeUseCase);
    }

    createGate(flowId: string, nodeId: string , name: string, route: string, method: string): Promise<void> {
        return this._nodesCreateUseCase.addGate(flowId, nodeId, name, route, method)
    }

    editGate(pipelineId: string, nodeId: string, name: string, route: string, method: string, isActived: boolean): Promise<void> {
        return this._nodeEditService.editGate(pipelineId, nodeId, name, route, method, isActived);
    }
    
}