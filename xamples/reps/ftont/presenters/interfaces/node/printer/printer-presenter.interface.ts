

export abstract class IPrinterPresenter {
    abstract createPrinter(flowId: string, nodeId: string, name: string, script: string): Promise<void>;

    abstract editPrinter( pipelineId: string, nodeId: string, name: string, script: string, isActived: boolean): Promise<void>;
}