import { ComponentDefinition, IPresenter, IView, Injector, PresentationEventBus } from 'nucleus';
import { Node } from 'wf.extra.domain.mimetic-manager';

import { NodeUpdaterView } from '../../../views/node/node-updater.view.interface';
import { INodeLoaderService, IEditNodeUseCase, NodeModel } from 'wf.extra.application.mimetic-manager';
import { INodeUpdaterPresenter } from '../../interfaces/node/node-updater.presenter.interface';

/**
 * Presentador para la actualización de nodos
 */
export class NodeUpdaterPresenter extends IPresenter implements INodeUpdaterPresenter {
  private nodeData: NodeModel;
  
  constructor(
    private readonly nodeUpdaterView: NodeUpdaterView,
    private readonly nodeLoaderService: INodeLoaderService,
    private readonly editNodeUseCase: IEditNodeUseCase,
  ) {
    super(nodeUpdaterView);
  }

  /**
   * Crea una instancia del presentador
   * @param view Vista asociada
   */
  static create(view: NodeUpdaterView): NodeUpdaterPresenter {
    const nodeLoaderService = Injector.get<INodeLoaderService>(INodeLoaderService);
    const editNodeUseCase = Injector.get<IEditNodeUseCase>(IEditNodeUseCase);
    return new NodeUpdaterPresenter(view, nodeLoaderService, editNodeUseCase);
  }

  /**
   * Presenta la actualización de un nodo
   * @param node Modelo del nodo actualizado
   */
  present(node: Node): void {
    this.nodeUpdaterView.displayUpdatedNode(node);
  }

  /**
   * Carga los datos de un nodo para su edición
   * @param nodeId ID del nodo a editar
   */
  async loadNodeData(nodeId: string): Promise<void> {
    this.nodeData = await this.nodeLoaderService.getNodeById(nodeId);
    this.nodeUpdaterView.renderNodeData(this.nodeData as unknown as Node);
  }

  /**
   * Guarda los cambios realizados en un nodo
   * @param nodeData Datos del nodo a guardar
   */
  async saveNodeChanges(nodeData: any): Promise<void> {
    // Dependiendo del tipo de nodo, llamar al método adecuado de editNodeUseCase
    // Esta es una implementación simplificada, la real debería determinar el tipo de nodo
    const nodeType = nodeData.type?.toLowerCase() || 'unknown';
    
    switch (nodeType) {
      case 'gate':
        await this.editNodeUseCase.editGate(
          nodeData.pipelineId, nodeData.id, nodeData.name,
          nodeData.route, nodeData.method, nodeData.isActived || true
        );
        break;
      case 'printer':
        await this.editNodeUseCase.editPrinter(
          nodeData.pipelineId, nodeData.id, nodeData.name,
          nodeData.script, nodeData.isActived || true
        );
        break;
      // Agregar otros tipos según sea necesario
      default:
        console.error(`Tipo de nodo no soportado para actualización: ${nodeType}`);
    }
    
    this.nodeUpdaterView.notifyUpdateSuccess();
  }
} 