export { IGateView } from "./views/node/gate.view.interface";
export { IPrinterView } from "./views/node/printer.view.interface";
export { IRequestView } from "./views/node/request.view.interface";
export { IResponseView } from "./views/node/response.view.interface";
export { IScripterView } from "./views/node/scripter.view.interface";
export { ISerializerView } from "./views/node/serializer.view.interface";
export { ISwitcherView } from "./views/node/switcher.view.interface";
export { IWatchdogView } from "./views/node/watchdog.view.interface";

// Exportaciones para node y nodes
export { INodePresenter } from "./presenters/interfaces/node/node.presenter.interface";
export { NodePresenter } from "./presenters/presenters/node/node.presenter";

export { NodeUpdaterPresenter } from "./presenters/presenters/node/node-updater.presenter";

export { NodesPresenter } from "./presenters/presenters/nodes/nodes.presenter";

export { NodeCreatorPresenter } from "./presenters/presenters/nodes/node-creator.presenter";

// Exportación de vistas para node y nodes

export { INodeCreateModalView } from "./views/node/inode-create-modal.view";
export { INodesListView } from "./views/nodes/nodes-list.view";

// Exportación de presentadores específicos de nodos
export { GatePresenter } from "./presenters/presenters/node/gate/gate.presenter";
export { PrinterPresenter } from "./presenters/presenters/node/printer/printer.presenter";
export { RequestPresenter } from "./presenters/presenters/node/request/request.presenter";
export { WatchdogPresenter } from "./presenters/presenters/node/watchdog/watchdog.presenter";
export { ResponsePresenter } from "./presenters/presenters/node/response/response.presenter";
export { ScripterPresenter } from "./presenters/presenters/node/scripter/scripter.presenter";
export { SerializerPresenter } from "./presenters/presenters/node/serializer/serializer.presenter";
export { SwitcherPresenter } from "./presenters/presenters/node/switcher/switcher.presenter";



// Nuevas exportaciones para reorganizacion
export { IPipelineView } from "./views/pipeline/pipeline.view.interface"
export { IPipelinesView } from "./views/pipelines/pipelines.view.interface";
export { IPipelineCreatorView } from "./views/pipelines/pipeline-creator.view.interface";
export { IPipelineUpdaterView } from "./views/pipeline/pipeline-updater.view.interface";
export { IPipelineItemView } from "./views/pipelines/pipeline-item.view.interface"
export { IPipelineListView } from "./views/pipelines/pipeline-list.view.interface"

export { IPipelinesPresenter } from "./presenters/interfaces/pipelines/pipelines.presenter.interface";
export { IPipelineCreatorPresenter } from "./presenters/interfaces/pipelines/pipeline-creator.presenter.interface";
export { IPipelinePresenter } from "./presenters/interfaces/pipeline/pipeline.presenter.interface";
export { IPipelineUpdaterPresenter } from "./presenters/interfaces/pipeline/pipeline-updater.presenter.interface";
export { PipelineItemPresenter } from "./presenters/presenters/pipelines/pipeline-item.presenter"
export { IPipelineListPresenter } from "./presenters/interfaces/pipelines/pipeline-list.presenter.interface"
export { PipelineListPresenter } from "./presenters/presenters/pipelines/pipeline-list.presenter"

export { PipelinePresenter} from "./presenters/presenters/pipeline/pipeline.presenter";
export { PipelinesPresenter } from "./presenters/presenters/pipelines/pipelines.presenter";
export { PipelineCreatorPresenter } from "./presenters/presenters/pipelines/pipeline-creator.presenter";
export { PipelineUpdaterPresenter } from "./presenters/presenters/pipeline/pipeline-updater.presenter";

// Exportaciones de modelos y utilidades
export { NodeInputValidator } from "./shared/node-input.validator";
export { PipelineJsonMapping } from "./shared/pipeline-json-mapping";
export { ExportActions } from "./shared/export-actions"

// Exportación de interfaces de categorías
export { ICategoriesView } from "./views/categories/categories.view.interface";
export { ICategoryView } from "./views/category/category.view.interface";

export { ICategoryPresenter } from "./presenters/interfaces/category/category.presenter.interface";

// Exportación de presentadores de categorías - usando aliases para mantener compatibilidad
export { CategoriesPresenter} from "./presenters/presenters/categories/categories.presenter";
export { CategoryPresenter } from "./presenters/presenters/category/category.presenter";


// Exportación de interfaces y clases del modal
export { IModalPresenter } from "./presenters/interfaces/modals/modal.presenter.interface";

export { IModalView } from "./views/shared/modal/modal.view.interface";


// Usamos el modelo desde el dominio en lugar de uno local
export { BaseModalModel } from "wf.extra.domain.mimetic-manager";
export {
  EntityType
} from "wf.extra.domain.mimetic-manager";

// Modals - Presenters
export { ICreateEditModalPresenter } from "./presenters/interfaces/modals/create-edit-modal.presenter.interface";
export { IExportModalPresenter } from "./presenters/interfaces/modals/export-modal.presenter.interface";

export { CreateEditModalPresenter } from "./presenters/presenters/modals/create-edit-modal.presenter";


// NODOS
export { IGatePresenter } from "./presenters/interfaces/node/gate/gate-presenter.interface";
export { IPrinterPresenter } from "./presenters/interfaces/node/printer/printer-presenter.interface";
export { IRequestPresenter } from "./presenters/interfaces/node/request/request-presenter.interface";
export { IResponsePresenter } from "./presenters/interfaces/node/response/response-presenter.interface";
export { IScripterPresenter } from "./presenters/interfaces/node/scripter/scripter-presenter.interface";
export { ISerializerPresenter } from "./presenters/interfaces/node/serializer/serializer-presenter.interface";
export { ISwitcherPresenter } from "./presenters/interfaces/node/switcher/switcher-presenter.interface";
export { IWatchdogPresenter } from "./presenters/interfaces/node/watchdog/watchdog-presenter.interface";
export { IParameterPresenter } from "./presenters/interfaces/node/switcher/parameter-presenter.interface";

// FORMULARIOS
export { GatePresenter as NodeFormGatePresenter } from "./presenters/presenters/node/gate/gate.presenter";
export { PrinterPresenter as NodeFormPrinterPresenter } from "./presenters/presenters/node/printer/printer.presenter";
export { RequestPresenter as NodeFormRequestPresenter } from "./presenters/presenters/node/request/request.presenter";
export { ResponsePresenter as NodeFormResponsePresenter } from "./presenters/presenters/node/response/response.presenter";
export { ScripterPresenter as NodeFormScripterPresenter } from "./presenters/presenters/node/scripter/scripter.presenter";
export { SerializerPresenter as NodeFormSerializerPresenter } from "./presenters/presenters/node/serializer/serializer.presenter";
export { SwitcherPresenter as NodeFormSwitcherPresenter } from "./presenters/presenters/node/switcher/switcher.presenter";
export { WatchdogPresenter as NodeFormWatchdogPresenter } from "./presenters/presenters/node/watchdog/watchdog.presenter";
export { NodeCreatorPresenter as NodeFormCreatePresenter } from "./presenters/presenters/nodes/node-creator.presenter";


//CATEGORIAS
export { ICategoryCreatorView } from "./views/categories/category-creator.view.interface";
export { CategoryCreatorPresenter } from "./presenters/presenters/categories/category-creator.presenter";
export { ICategoriesPresenter } from "./presenters/interfaces/categories/categories.interface";
export { ICategoryCreatorPresenter } from "./presenters/interfaces/categories/category-creator.presenter.interface";

// CATEGORIAS - EDICIÓN
export { ICategoryUpdaterView } from "./views/category/category-updater.view.interface";
export { ICategoryUpdaterPresenter } from "./presenters/interfaces/category/category-updater.presenter.interface";
export { CategoryUpdaterPresenter } from "./presenters/presenters/category/category-updater.presenter";
