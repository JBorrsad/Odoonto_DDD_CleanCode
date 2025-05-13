import { IView } from "nucleus";
import { CategoryReadModel, PipelineReadModel } from "wf.extra.application.mimetic-manager";

export abstract class IPipelinesView extends IView {
	abstract render(list: PipelineReadModel[]): void;
	abstract setCantityPipelines(pipelines: number): void;
	abstract goHelpControls(): void;
} 