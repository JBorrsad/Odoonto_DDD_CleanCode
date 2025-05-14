import { ICreateNodesUseCase, IEditNodeUseCase, INodeLoaderService, IRemoveNodeUseCase } from "wf.extra.application.mimetic-manager";
import { Injector, IView } from "nucleus";

import { IResponsePresenter } from "../../../interfaces/node/response/response-presenter.interface";
import { NodePresenter } from "../node.presenter";


export class ResponsePresenter extends NodePresenter implements IResponsePresenter{

    private constructor(
        _view: IView,
        private readonly _nodesCreateUseCase: ICreateNodesUseCase,
        private readonly _nodesLoaderService: INodeLoaderService,
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
        return new ResponsePresenter(view, nodeCreateUseCase, nodeLoaderService, nodeEditService, removeNodeUseCase);
    }

    createResponse(pipelineId: string, nodeId: string, name: string, statusCode: string, mediaType: string, content: string): Promise<void> {
        return this._nodesCreateUseCase.addResponse(pipelineId,nodeId, name, statusCode, mediaType, content);
    }

    editResponse(pipelineId: string, nodeId: string, name: string, statusCode: string, mediaType: string, content: string, isActived: boolean): Promise<void> {
        return this._nodeEditService.editResponse(pipelineId, nodeId, name, statusCode, mediaType, content, isActived);
    }
    
}