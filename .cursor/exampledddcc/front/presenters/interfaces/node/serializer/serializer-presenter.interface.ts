
export abstract class ISerializerPresenter {
    abstract createSerializer(pipelineId: string, nodeId: string, name: string, serializationType: string): Promise<void>;

    abstract editSerializer( pipelineId: string, nodeId: string, name: string, serializationType: string, isActived: boolean): Promise<void>;
}