export abstract class IGatePresenter  {
    abstract createGate(flowId: string,nodeId: string, name: string, route: string, method: string): Promise<void>

    abstract editGate( pipelineId: string, nodeId: string, name: string, route: string, method: string, isActived: boolean): Promise<void>;
}