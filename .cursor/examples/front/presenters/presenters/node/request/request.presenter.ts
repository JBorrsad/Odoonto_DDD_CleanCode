import { ICreateNodesUseCase, IEditNodeUseCase, INodeLoaderService, IRemoveNodeUseCase } from "wf.extra.application.mimetic-manager";
import { Injector, IView } from "nucleus";
import { NodePresenter } from "../node.presenter";
import { IRequestPresenter } from "../../../interfaces/node/request/request-presenter.interface";


export class RequestPresenter extends NodePresenter implements IRequestPresenter{

    private constructor(
        _view: IView,
        private readonly _nodesCreateUseCase: ICreateNodesUseCase,
        _nodesLoaderService: INodeLoaderService,
        private readonly _nodeEditService: IEditNodeUseCase,
        protected readonly _removeNodeUseCase: IRemoveNodeUseCase
    ){
        super(_view, _nodesLoaderService, _removeNodeUseCase);
    }

    static create(view: IView) {
        const nodeCreateUseCase = Injector.get(ICreateNodesUseCase);
        const nodeLoaderService = Injector.get(INodeLoaderService);
        const nodeEditService = Injector.get(IEditNodeUseCase);
        const removeNodeUseCase = Injector.get(IRemoveNodeUseCase)
        return new RequestPresenter(view, nodeCreateUseCase, nodeLoaderService, nodeEditService, removeNodeUseCase);
    }

    async createRequest(pipelineId: string, nodeId: string,  name: string, body: string, route: string, mediaType: string, method: string): Promise<void> {
                
        return await this._nodesCreateUseCase.addRequest(pipelineId,nodeId, name, body, route, mediaType, method);
    }

    editRequest(pipelineId: string, nodeId: string, name: string, body: string, route: string, mediaType: string, method: string, isActived: boolean): Promise<void> {
        return this._nodeEditService.editRequest(pipelineId, nodeId, name, body, route, mediaType,method,isActived)
    }
    
}