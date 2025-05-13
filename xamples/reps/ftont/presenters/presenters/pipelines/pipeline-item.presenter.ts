import { Injector } from "nucleus";
import { IPipelineItemPresenter } from "../../interfaces/pipelines/pipeline-item.presenter.interface";
import { IPipelineItemView } from "../../../views/pipelines/pipeline-item.view.interface";
import { IPipelineExportUseCase } from "wf.extra.application.mimetic-manager";

export class PipelineItemPresenter extends IPipelineItemPresenter{

    private constructor(
        view: IPipelineItemView,
        private readonly _pipelineExportUseCase: IPipelineExportUseCase
    ){
        super(view)

    }

    static create(view: IPipelineItemView){
        const pipelineExportUseCase = Injector.get(IPipelineExportUseCase)

        return new PipelineItemPresenter(view, pipelineExportUseCase)
    }

    async exportPipeline(id: string): Promise<string>{
        return await this._pipelineExportUseCase.execute(id)
    }
}