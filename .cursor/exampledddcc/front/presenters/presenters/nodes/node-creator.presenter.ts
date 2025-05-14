import { DomainEventBus, DomainEventFactory, DomainEventType, Guid, IPresenter, Injector } from 'nucleus';
import { Node, Pipeline } from 'wf.extra.domain.mimetic-manager';

import { INodeCreateModalView } from '../../../views/node/inode-create-modal.view';
import { ICreateNodesUseCase, INodeCreate } from 'wf.extra.application.mimetic-manager';
import { INodeCreatorPresenter } from '../../interfaces/nodes/node-creator.presenter.interface';


export class NodeCreatorPresenter extends IPresenter implements INodeCreatorPresenter {

  constructor(
    view: INodeCreateModalView,
    private readonly createNodesUseCase: ICreateNodesUseCase,
    private readonly _domainEventBus: DomainEventBus
  ) {
    super(view);
  }

  static create(view: INodeCreateModalView, flowId: string): NodeCreatorPresenter {
    const createNodesUseCase = Injector.get<ICreateNodesUseCase>(ICreateNodesUseCase);
    const domainEventBus = Injector.get(DomainEventBus)
    return new NodeCreatorPresenter(view, createNodesUseCase, domainEventBus);
  }

  get view(): INodeCreateModalView {
    return this._view as INodeCreateModalView;
  }

  present(node: Node): void {
    this.view.closeModal();
  }

  addNodeChild(pipelineId: string, nodeName: string, type: string) {
    const newNode: INodeCreate = {
      pipelineId: pipelineId,
      name: nodeName,
      type: type
    }
    const createEvent = DomainEventFactory.create(Pipeline, DomainEventType.Validated, Guid.parse(pipelineId), newNode);
    this._domainEventBus.publish(createEvent);
  }
} 