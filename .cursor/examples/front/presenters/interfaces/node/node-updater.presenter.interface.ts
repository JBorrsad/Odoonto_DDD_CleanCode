import { IPresenter } from 'nucleus';
import { Node } from 'wf.extra.domain.mimetic-manager';

/**
 * Interfaz para el presentador de actualización de nodos
 */
export interface INodeUpdaterPresenter extends IPresenter {
  /**
   * Presenta la actualización de un nodo
   * @param node Modelo del nodo actualizado
   */
  present(node: Node): void;
  
  /**
   * Carga los datos de un nodo para su edición
   * @param nodeId ID del nodo a editar
   */
  loadNodeData(nodeId: string): Promise<void>;
  
  /**
   * Guarda los cambios realizados en un nodo
   * @param nodeData Datos del nodo a guardar
   */
  saveNodeChanges(nodeData: any): Promise<void>;
} 