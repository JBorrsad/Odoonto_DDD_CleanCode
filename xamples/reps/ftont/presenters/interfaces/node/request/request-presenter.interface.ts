
export abstract class IRequestPresenter {
    
    abstract createRequest(pipelineId: string, nodeId: string,  name: string, body: string, route: string, mediaType: string, method: string): Promise<void>;
    
    abstract editRequest( pipelineId: string, nodeId: string, name: string, body: string, route: string, mediaType: string, method: string, isActived: boolean): Promise<void>;
}