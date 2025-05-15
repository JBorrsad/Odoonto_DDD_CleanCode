import { IView } from 'nucleus';
import { NodeModel } from 'wf.extra.application.mimetic-manager';

/**
 * Interfaz para la vista de edición de nodo
 */
export abstract class INodeEditView extends IView {
    /** ID del nodo */
    abstract get nodeId(): string;
    
    /** ID del pipeline al que pertenece el nodo */
    abstract get pipelineId(): string;
    
    /** Renderiza los datos del nodo */
    abstract render(node: NodeModel): void;
} 