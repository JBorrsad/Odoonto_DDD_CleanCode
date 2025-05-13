
export abstract class IResponsePresenter {

    abstract createResponse(pipelineId: string, nodeId: string,  name: string, statusCode: string, mediaType: string, content: string): Promise<void>;
    
    abstract editResponse( pipelineId: string, nodeId: string, name: string, statusCode: string, mediaType: string, content: string, isActived: boolean): Promise<void>;
}