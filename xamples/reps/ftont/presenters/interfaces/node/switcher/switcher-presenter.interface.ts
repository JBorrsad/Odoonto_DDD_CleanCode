
export abstract class ISwitcherPresenter {
    abstract createSwitcher(pipelineId: string, nodeId: string,  name: string): Promise<void>;
    
    abstract editSwitcher( pipelineId: string, nodeId: string, name: string, isActived: boolean): Promise<void>;
}