import { IPresenter } from "nucleus";

export abstract class IParameterPresenter extends IPresenter{

    abstract createParameter(pipelineId: string, switcherId: string, name: string, value: string): Promise<void>;

    abstract updateParameter(pipelineId: string, switcherId: string, paramId: string, name: string, value: string): Promise<void>;

    abstract deleteParameter(pipelineId: string, switcherId: string, paramId: string): Promise<void>;
}