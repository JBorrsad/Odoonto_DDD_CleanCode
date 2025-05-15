import { ICreateNodesUseCase, IEditNodeUseCase, INodeLoaderService, IRemoveNodeUseCase } from "wf.extra.application.mimetic-manager";
import { Injector, IView } from "nucleus";
import { NodePresenter } from "../node.presenter";
import { IWatchdogPresenter } from "../../../interfaces/node/watchdog/watchdog-presenter.interface";

export class WatchdogPresenter extends NodePresenter implements IWatchdogPresenter{

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
        return new WatchdogPresenter(view, nodeCreateUseCase, nodeLoaderService, nodeEditService, removeNodeUseCase);
    }

    createWatchdog(pipelineId: string, nodeId: string,  name: string, statusCode: string, mediaType: string, content: string, script: string){
        return this._nodesCreateUseCase.addWatchdog(pipelineId,nodeId, name, statusCode, mediaType, content, script);
    }

    editWatchdog(pipelineId: string, nodeId: string, name: string, statusCode: string, mediaType: string, content: string, script: string, isActived: boolean): Promise<void> {
        return this._nodeEditService.editWatchdog(pipelineId, nodeId, name, statusCode, mediaType, content, script, isActived)
    }
    
}