import { IView } from 'nucleus';

export abstract class INodeCreateModalView extends IView {
    
    abstract get children(): string[];
    
    abstract closeModal(): void;
} 