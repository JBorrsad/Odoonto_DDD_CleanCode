import { ComponentDefinition, IView } from "nucleus";
import { PipelineReadModel } from "wf.extra.application.mimetic-manager";

export abstract class IPipelineView extends IView {
    abstract render(pipeline: PipelineReadModel): void;
    abstract goHelpPipeline(): void;
    abstract goBack(): void;
    abstract message(message: string, isError?: boolean)
    abstract setChildren(children: ComponentDefinition[])
} 