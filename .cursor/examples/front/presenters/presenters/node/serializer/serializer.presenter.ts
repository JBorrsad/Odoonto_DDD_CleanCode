import { ICreateNodesUseCase, IEditNodeUseCase, INodeLoaderService, IRemoveNodeUseCase } from "wf.extra.application.mimetic-manager";
import { Injector, IView } from "nucleus";
import { NodePresenter } from "../node.presenter";
import { ISerializerPresenter } from "../../../interfaces/node/serializer/serializer-presenter.interface";


export class SerializerPresenter extends NodePresenter implements ISerializerPresenter{

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
        return new SerializerPresenter(view, nodeCreateUseCase, nodeLoaderService, nodeEditService, removeNodeUseCase);
    }

    createSerializer(pipelineId: string, nodeId: string,  name: string, type: string) {
        return this._nodesCreateUseCase.addSerializer(pipelineId,nodeId, name, type);
    }

    editSerializer(pipelineId: string, nodeId: string, name: string, serializationType: string, isActived: boolean): Promise<void> {
        return this._nodeEditService.editSerializer(pipelineId, nodeId, name, serializationType, isActived)
    }
    
}