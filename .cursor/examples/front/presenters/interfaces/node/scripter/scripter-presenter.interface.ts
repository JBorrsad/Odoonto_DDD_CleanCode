
export abstract class IScripterPresenter {
    abstract createScripter(pipelineId: string, nodeId: string,  name: string, script: string): Promise<void>;

    abstract editScripter( pipelineId: string, nodeId: string, name: string, script: string, isActived: boolean): Promise<void>;
}