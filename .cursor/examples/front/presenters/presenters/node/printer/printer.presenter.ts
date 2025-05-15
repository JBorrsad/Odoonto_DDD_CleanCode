import { ICreateNodesUseCase, IEditNodeUseCase, INodeLoaderService, IRemoveNodeUseCase } from "wf.extra.application.mimetic-manager";
import { Injector, IView } from "nucleus";
import { NodePresenter } from "../node.presenter";
import { IPrinterPresenter } from "../../../interfaces/node/printer/printer-presenter.interface";


export class PrinterPresenter extends NodePresenter implements IPrinterPresenter{

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
        return new PrinterPresenter(view, nodeCreateUseCase, nodeLoaderService, nodeEditService, removeNodeUseCase);
    }

    createPrinter(flowId: string, nodeId: string, name: string, script: string): Promise<void> {
        return this._nodesCreateUseCase.addPrinter(flowId,nodeId, name, script);
    }

    editPrinter(pipelineId: string, nodeId: string, name: string, script: string, isActived: boolean): Promise<void> {
        return this._nodeEditService.editPrinter(pipelineId,nodeId, name,script,isActived)
    }

    
    
}