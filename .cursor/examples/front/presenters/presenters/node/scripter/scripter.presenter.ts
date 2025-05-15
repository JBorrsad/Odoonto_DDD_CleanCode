import { ICreateNodesUseCase, IEditNodeUseCase, INodeLoaderService, IRemoveNodeUseCase } from "wf.extra.application.mimetic-manager";
import { Injector, IView } from "nucleus";
import { IScripterPresenter } from "../../../interfaces/node/scripter/scripter-presenter.interface";
import { NodePresenter } from "../node.presenter";


export class ScripterPresenter extends NodePresenter implements IScripterPresenter{

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
        return new ScripterPresenter(view, nodeCreateUseCase, nodeLoaderService, nodeEditService, removeNodeUseCase);
    }

    createScripter(pipelineId: string, nodeId: string,  name: string, script: string): Promise<void> {
        return this._nodesCreateUseCase.addScripter(pipelineId,pipelineId, name, script);
    }

    editScripter(pipelineId: string, nodeId: string, name: string, script: string, isActived: boolean): Promise<void> {
        return this._nodeEditService.editScripter(pipelineId, nodeId, name, script, isActived)
    }
    
}