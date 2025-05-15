import { ICreateNodesUseCase, IEditNodeUseCase, INodeLoaderService, IRemoveNodeUseCase} from "wf.extra.application.mimetic-manager";
import { ComponentDefinition, Injector, IState, IView } from "nucleus";


import { Parameter } from "wf.extra.domain.mimetic-manager";
import { ISwitcherPresenter } from "../../../interfaces/node/switcher/switcher-presenter.interface";
import { NodePresenter } from "../node.presenter";


export class SwitcherPresenter extends NodePresenter implements ISwitcherPresenter{

    private constructor(
        _view: IView,
        private readonly _nodesCreateUseCase: ICreateNodesUseCase,
        private readonly _nodesLoaderService: INodeLoaderService,
        private readonly _nodeEditService: IEditNodeUseCase,
        protected readonly _removeNodeUseCase: IRemoveNodeUseCase,
        private readonly _state: IState
    ){
        super(_view, _nodesLoaderService, _removeNodeUseCase);
    }

    static create(view: IView) {
        const nodeCreateUseCase = Injector.get(ICreateNodesUseCase);
        const nodeLoaderService = Injector.get(INodeLoaderService);
        const nodeEditService = Injector.get(IEditNodeUseCase);
        const removeNodeUseCase = Injector.get(IRemoveNodeUseCase);
        const state = Injector.get(IState);
        return new SwitcherPresenter(view, nodeCreateUseCase, nodeLoaderService,nodeEditService, removeNodeUseCase, state);
    }

    onInit(){
        this.getAllChildren();
    }

    createSwitcher(pipelineId: string,nodeId: string, name: string): Promise<void> {
        return this._nodesCreateUseCase.addSwitcher(pipelineId,nodeId, name);
    }

    editSwitcher(pipelineId: string, nodeId: string, name: string, isActived: boolean): Promise<void> {
        return this._nodeEditService.editSwitcher(pipelineId, nodeId, name,isActived)
    }
    
    private getAllChildren(){
        const parameters = this._state.getAll(Parameter);
        var children = parameters.map(param => {
            const parameter: ComponentDefinition = {
                id: param.id.toString(),
                type: IView,
                props: {
                    nodeId: param.fatherId.toString(), 
                    id: param.id.toString(),
                    name: param.name,
                }
            }
            return parameter;
        }).forEach(param => this.addChild(param));
    }

    

}