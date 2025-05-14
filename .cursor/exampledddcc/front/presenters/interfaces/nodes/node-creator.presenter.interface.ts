import { IPresenter } from 'nucleus';
import { Node } from 'wf.extra.domain.mimetic-manager';

/**
 * Interfaz para el presentador de creación de nodos
 */
export interface INodeCreatorPresenter extends IPresenter {

  present(node: Node): void;
  
  addNodeChild(pipelineId: string, nodeName: string, type: string)
} 