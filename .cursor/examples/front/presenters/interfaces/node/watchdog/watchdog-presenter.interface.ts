
export abstract class IWatchdogPresenter {
    abstract createWatchdog(pipelineId: string, nodeId: string,  name: string, statusCode: string, mediaType: string, content: string, script: string): Promise<void>;

    abstract editWatchdog(pipelineId: string, nodeId: string, name: string, statusCode: string ,mediaType: string ,content: string, script: string,isActived: boolean): Promise<void>;
}