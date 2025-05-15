import { ComponentDefinition, Guid, Injector, IState, IView, PresentationEventBus, PresentationEventBusEventTypes } from "nucleus";
import { IPipelineLoaderService, IPipelineDeleteUseCase, PipelineReadModel, PipelineNodeCreateHandler, PipelineNodeRemoveHandler, INodeCreate } from "wf.extra.application.mimetic-manager";

import { IPipelinePresenter } from "../../interfaces/pipeline/pipeline.presenter.interface";
import { IPipelineView } from "../../../views/pipeline/pipeline.view.interface";

import { IGateView } from "../../../views/node/gate.view.interface";
import { IPrinterView } from "../../../views/node/printer.view.interface";
import { IRequestView } from "../../../views/node/request.view.interface";
import { IResponseView } from "../../../views/node/response.view.interface";
import { IScripterView } from "../../../views/node/scripter.view.interface";
import { ISerializerView } from "../../../views/node/serializer.view.interface";
import { ISwitcherView } from "../../../views/node/switcher.view.interface";
import { IWatchdogView } from "../../../views/node/watchdog.view.interface";

import { Node, NodeType } from "wf.extra.domain.mimetic-manager";

export class PipelinePresenter extends IPipelinePresenter {

    private _pipelineId: string;


    private constructor(
        view: IPipelineView,
        private readonly _pipelineLoaderService: IPipelineLoaderService,
        private readonly _pipelineDeleteUseCase: IPipelineDeleteUseCase,
        private readonly _nodeCreateHandler: PipelineNodeCreateHandler,
        protected readonly _nodeDeleteHandler: PipelineNodeRemoveHandler,
        private readonly _state: IState
    ) {
        super(view);
    }

    static create(view: IPipelineView) {
        const _pipelineLoaderService = Injector.get(IPipelineLoaderService);
        const _pipelineDeleteUseCase = Injector.get(IPipelineDeleteUseCase);
        const nodeCreateHandler = Injector.get(PipelineNodeCreateHandler);
        const nodeDeleteHandler = Injector.get(PipelineNodeRemoveHandler);
        const state = Injector.get(IState)

        return new PipelinePresenter(
            view,
            _pipelineLoaderService,
            _pipelineDeleteUseCase,
            nodeCreateHandler,
            nodeDeleteHandler,
            state
        );
    }

    async onInit(pipelineId: string) {
        this._pipelineId = pipelineId;
        const pipe = await this.getPipeline();
        (this._view as IPipelineView).render(pipe);
        this._nodeCreateHandler.publishSubscribe(this._pipelineId);
        this._nodeDeleteHandler.publishSubscribe(this._pipelineId);
        this.addSubscription();
    }

    async getPipeline(): Promise<PipelineReadModel> {
        const pipeline = await this._pipelineLoaderService.getPipelineById(this._pipelineId);
        const children = this.getChildrenNodes();
        (this._view as IPipelineView).setChildren(children)
        this.addChildren(children)
        return pipeline
    }

    async deletePipeline(id: Guid): Promise<void> {
        await this._pipelineDeleteUseCase.execute(id);
    }

    addSubscription() {

        this.subscribeToEvent(
            PresentationEventBus.createEventName(
                PresentationEventBusEventTypes.removeChild,
                this._pipelineId
            ),
            (playload) => {
                const view = (this._view as IPipelineView)
                try {
                    this.removeChild((playload as { nodeId: Guid }).nodeId.toString());
                    view.message('El nodo ha sido eliminado correctamente', false);
                    (this._view as IPipelineView).setChildren(this.getChildren());
                } catch (error) {
                    view.message('No de ha encontrado el nodo');
                }
            }
        )

        this.subscribeToEvent(
            PresentationEventBus.createEventName(
                PresentationEventBusEventTypes.addChild,
                this._pipelineId
            ),
            (playload) => {
                const newNode: INodeCreate = (playload as INodeCreate)
                const newId = Guid.create().toString();
                const newChild: ComponentDefinition = {
                    id: newId,
                    type: this.getIView(newNode.type),
                    props: {
                        pipelineId: newNode.pipelineId,
                        nodeId: newId,
                        createName: newNode.name
                    }
                }
                this.addChild(newChild);
                (this._view as IPipelineView).setChildren(this.getChildren());
            }
        )
    }

    //private checkPendingToast(): void { no se usa
    //    const msg = localStorage.getItem('pipeline_toast_message');
    //    const type = localStorage.getItem('pipeline_toast_type');

    //    if (!msg) { return; }

    //    const isError = type !== 'success';
    //    (this._view as IPipelineView).message(msg, isError);

    //    localStorage.removeItem('pipeline_toast_message');
    //    localStorage.removeItem('pipeline_toast_type');
    //}

    getChildrenNodes() {
        const nodes = this._state.getAll(Node);

        return nodes.map(node => {
            const child: ComponentDefinition = {
                id: node.id.toString(),
                type: this.getIView(node.type),
                props: {
                    pipelineId: node.flowId.toString(),
                    nodeId: node.id.toString()
                }
            }
            return child
        })
    }

    private getIView(type: string) {
        switch (type.toLowerCase()) {
            case NodeType.Gate:
                return IGateView
            case 'printer':
                return IPrinterView
            case 'scripter':
                return IScripterView
            case 'request':
                return IRequestView
            case 'response':
                return IResponseView
            case 'serializer':
                return ISerializerView
            case 'switcher':
                return ISwitcherView
            case 'watchdog':
                return IWatchdogView
        }
        return IView
    }

    override dispose(): void {
        super.dispose();
    }

} 