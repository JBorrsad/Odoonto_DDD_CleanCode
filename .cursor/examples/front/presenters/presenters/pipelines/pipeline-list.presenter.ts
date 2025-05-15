import { PipelineReadModel } from "wf.extra.application.mimetic-manager";
import { IPipelineItemView } from "../../../views/pipelines/pipeline-item.view.interface";
import { IPipelineListPresenter } from "../../interfaces/pipelines/pipeline-list.presenter.interface"
import { ComponentDefinition } from "nucleus";
import { IPipelineListView } from "../../../views/pipelines/pipeline-list.view.interface";

export class PipelineListPresenter extends IPipelineListPresenter {

    private constructor(
        view: IPipelineListView
    ) {
        super(view)

    }

    static create(view: IPipelineItemView) {

        return new PipelineListPresenter(view)
    }

    setChildren(pipelines: PipelineReadModel[]) {
        this.clearChildren();

        const children = pipelines.map(pip => {
            const child: ComponentDefinition = {
                id: pip.id,
                type: IPipelineItemView,
                props: {
                    pipelineId: pip.id,
                    name: pip.name,
                    isPublic: pip.isPublic
                }
            }
            return child
        })

        this.addChildren(children);
        console.log("Hijos: ",this.getChildren())
    }
}