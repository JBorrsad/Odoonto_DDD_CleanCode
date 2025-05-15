import { IView } from 'nucleus';
import { Node } from 'wf.extra.domain.mimetic-manager';

/**
 * Interfaz para la vista de actualización de nodos
 */
export interface NodeUpdaterView extends IView {
  /**
   * Muestra un nodo actualizado
   * @param node Nodo actualizado
   */
  displayUpdatedNode(node: Node): void;
  
  /**
   * Muestra los datos de un nodo para su edición
   * @param node Nodo a editar
   */
  renderNodeData(node: Node): void;
  
  /**
   * Notifica que la actualización se ha completado con éxito
   */
  notifyUpdateSuccess(): void;
} 