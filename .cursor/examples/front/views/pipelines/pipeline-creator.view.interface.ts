import { IView } from "nucleus";

export abstract class IPipelineCreatorView extends IView {
    abstract createPipeline(): void;
    abstract goBack(): void;
} 