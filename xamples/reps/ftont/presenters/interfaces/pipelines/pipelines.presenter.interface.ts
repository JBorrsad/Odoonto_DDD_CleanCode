import { PipelineReadModel } from "wf.extra.application.mimetic-manager";
import { IPresenter } from "nucleus";
import { CategoryReadModel } from "wf.extra.application.mimetic-manager";

export abstract class IPipelinesPresenter extends IPresenter {
	abstract getPipelines(): Promise<PipelineReadModel[]>;
	abstract getCategories(): Promise<CategoryReadModel[]>;
	abstract getPipelinesByCategory(categoryId: string): Promise<PipelineReadModel[]>;
	abstract orderPipelines(newOrder: string): Promise<PipelineReadModel[]>;
	abstract exportPipelines(): Promise<string>;
	abstract render(list: PipelineReadModel[]): void;
} 