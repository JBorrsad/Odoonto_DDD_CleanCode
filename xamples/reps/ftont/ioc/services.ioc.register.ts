import { Injector } from 'nucleus';
import {
	ButtonFieldClickUseCase,
	CalculatedLambdaSubscriptionHandler,
	ClearInvisibleElementsUseCase,
	ClearRecordDataFromStateUseCase,
	CreateFileInputSubformUseCase,
	CreateSubformUseCase,
	CreatSelectMultipleSubformUseCase,
	DataOriginFieldChangedSubscriptionHandler,
	DataOriginFieldLoadedSubscriptionHandler,
	DataOriginSublayoutChangedSubscriptionHandler,
	DataOriginSublayoutLoadedSubscriptionHandler,
	DataOriginSubscriptionService,
	DeleteFileInputSubformUseCase,
	DiscardChangesSubformUseCase,
	DiscardDocsFileInputSubformUseCase,
	EmitFieldLoadedEventUseCase,
	EmitSublayoutLoadedEventUseCase,
	FieldChangedSubscriptionHandler,
	FieldRequiredLambdaSubscriptionHandler,
	FieldValidatedSubscriptionHandler,
	FormMementoLoaderService,
	FormWriterUseCase,
	GetOrCreateInstanceUseCase,
	IButtonFieldClickUseCase,
	IClearInvisibleElementsUseCase,
	IClearRecordDataFromStateUseCase,
	ICreateFileInputSubformUseCase,
	ICreateSubformUseCase,
	ICreatSelectMultipleSubformUseCase,
	IDeleteFileInputSubformUseCase,
	IDiscardChangedSubformUseCase,
	IDiscardDocsFileInputSubformUseCase,
	IEmitFieldLoadedEventUseCase,
	IEmitSublayoutLoadedEventUseCase,
	IFormMementoLoaderService,
	IFormWriterUseCase,
	IGetOrCreateInstanceUseCase,
	ILineLoaderService,
	IntegrationFieldChangedSubscriptionHandler,
	IntegrationFieldClickedSubscriptionHandler,
	IntegrationFieldLoadedSubscriptionHandler,
	IntegrationNullHandler,
	IntegrationSublayoutLoadedSubscriptionHandler,
	IntegrationSubscriptionService,
	IPersistFormMementoService,
	IPersistSubformUseCase,
	IPreviewFileInputLoaderService,
	IPreviewListLoaderService,
	IPreviewSelectMultipleLoaderService,
	IPreviewTableLoaderService,
	IRecordLoaderService,
	IRecordValidationsLoaderService,
	IRemoveSubformUseCase,
	IRgeService,
	ISubformLoaderService,
	IUpdateDescriptionFileInputSubformUseCase,
	IUploadFileInputSubformUseCase,
	IValidateFormUseCase,
	IValidateRecordUseCase,
	LambdaSubscriptionService,
	LayoutRequiredLambdaSubscriptionHandler,
	LineLoaderService,
	PersistFormMementoService,
	PersistSubformUseCase,
	PreviewFileInputLoaderService,
	PreviewListLoaderService,
	PreviewSelectMultipleLoaderService,
	PreviewTableLoaderService,
	RecordLoaderService,
	RecordValidationsLoaderService,
	RemoveSubformUseCase,
	RgeService,
	SubformLoaderService,
	SublayoutChangedSubscriptionHandler,
	SublayoutValidatedSubscriptionHandler,
	UpdateDescriptionFileInputSubformUseCase,
	UploadFileInputSubformUseCase,
	ValidateFormUseCase,
	ValidateRecordUseCase,
	ValidationLambdaSubscriptionHandler,
	VisibilityLambdaSubscriptionHandler,
} from 'wf.extra.application.cronos';
import {

	CreateNodesUseCase,
	EditNodeUseCase,
	ICreateNodesUseCase,
	IEditNodeUseCase,
	INodeLoaderService,
	IPipelineCreateUseCase,
	IPipelineDeleteUseCase,
	IPipelineEditUseCase,
	IPipelineLoaderService,
	IRemoveNodeUseCase,
	NodeLoaderService,
	PipelineCreateUseCase,
	PipelineDeleteUseCase,
	PipelineEditUseCase,
	PipelineLoaderService,
	PipelineNodeCreateHandler,
	PipelineNodeRemoveHandler,
	RemoveNodeUseCase,
	PipelineExportUseCase,
	PipelineImportUseCase,
	IPipelineExportUseCase,
	IPipelineImportUseCase
} from 'wf.extra.application.mimetic-manager';
import {

	BoardLoaderService,
	ChangeFaceUseCase,
	ColumnLoaderService,
	CreateReceiptUseCase,
	CreateRecordUseCase,
	DesignLoaderService,
	ExportRecordDataUseCase,
	FaceSelectorService,
	FieldLoaderService,
	IBoardLoaderService,
	IChangeFaceUseCase,
	IColumnLoaderService,
	ICreateReceiptUseCase,
	ICreateRecordUseCase,
	IDesignLoaderService,
	IExportRecordDataUseCase,
	IFaceSelectorService,
	IFieldLoaderService,
	ILayoutLoaderService,
	IRowLoaderService,
	LayoutLoaderService,
	RowLoaderService,
} from 'wf.extra.application.surface';
import {

	CategoryLoaderService,
	ICategoryLoaderService,
	CategoryModelMapper,
	CategoryCreateUseCase,
	CategoryDeleteUseCase,
	CategoryEditUseCase,
	AddFlowToCategoryUseCase,
	RemoveFlowFromCategoryUseCase,
	ICategoryCreateUseCase,
	ICategoryEditUseCase,
	ICategoryDeleteUseCase,
	IAddFlowToCategoryUseCase,
	IRemoveFlowFromCategoryUseCase
} from 'wf.extra.application.mimetic-manager';

export class ServiceIOCRegister {
	static registerDependencies() {
		this._registerServices();
		this._registerEventHandlers();
	}

	private static _registerServices() {
		Injector.addSingleton(RecordLoaderService, IRecordLoaderService);
		Injector.addSingleton(FormWriterUseCase, IFormWriterUseCase);
		Injector.addSingleton(LineLoaderService, ILineLoaderService);
		Injector.addSingleton(
			RecordValidationsLoaderService,
			IRecordValidationsLoaderService
		);
		Injector.addSingleton(
			FormMementoLoaderService,
			IFormMementoLoaderService
		);
		Injector.addSingleton(
			PersistFormMementoService,
			IPersistFormMementoService
		);
		Injector.addSingleton(SubformLoaderService, ISubformLoaderService);
		Injector.addSingleton(
			PreviewFileInputLoaderService,
			IPreviewFileInputLoaderService
		);
		Injector.addSingleton(
			PreviewListLoaderService,
			IPreviewListLoaderService
		);
		Injector.addSingleton(
			PreviewSelectMultipleLoaderService,
			IPreviewSelectMultipleLoaderService
		);
		Injector.addSingleton(
			PreviewTableLoaderService,
			IPreviewTableLoaderService
		);
		Injector.addSingleton(LambdaSubscriptionService);
		Injector.addSingleton(IntegrationSubscriptionService);
		Injector.addSingleton(DataOriginSubscriptionService);
		Injector.addSingleton(ValidateFormUseCase, IValidateFormUseCase);
		Injector.addSingleton(CreateSubformUseCase, ICreateSubformUseCase);
		Injector.addSingleton(
			CreatSelectMultipleSubformUseCase,
			ICreatSelectMultipleSubformUseCase
		);
		Injector.addSingleton(
			DiscardChangesSubformUseCase,
			IDiscardChangedSubformUseCase
		);
		Injector.addSingleton(PersistSubformUseCase, IPersistSubformUseCase);
		Injector.addSingleton(RemoveSubformUseCase, IRemoveSubformUseCase);
		Injector.addSingleton(ValidateRecordUseCase, IValidateRecordUseCase);
		Injector.addSingleton(
			ClearInvisibleElementsUseCase,
			IClearInvisibleElementsUseCase
		);
		Injector.addSingleton(
			ButtonFieldClickUseCase,
			IButtonFieldClickUseCase
		);
		Injector.addSingleton(
			CreateFileInputSubformUseCase,
			ICreateFileInputSubformUseCase
		);
		Injector.addSingleton(
			UpdateDescriptionFileInputSubformUseCase,
			IUpdateDescriptionFileInputSubformUseCase
		);
		Injector.addSingleton(
			DiscardDocsFileInputSubformUseCase,
			IDiscardDocsFileInputSubformUseCase
		);
		Injector.addSingleton(
			UploadFileInputSubformUseCase,
			IUploadFileInputSubformUseCase
		);
		Injector.addSingleton(
			DeleteFileInputSubformUseCase,
			IDeleteFileInputSubformUseCase
		);
		Injector.addSingleton(RgeService, IRgeService);
		Injector.addSingleton(
			GetOrCreateInstanceUseCase,
			IGetOrCreateInstanceUseCase
		);
		Injector.addSingleton(CreateReceiptUseCase, ICreateReceiptUseCase);

		Injector.addSingleton(
			EmitFieldLoadedEventUseCase,
			IEmitFieldLoadedEventUseCase
		);
		Injector.addSingleton(
			EmitSublayoutLoadedEventUseCase,
			IEmitSublayoutLoadedEventUseCase
		);

		Injector.addSingleton(CreateRecordUseCase, ICreateRecordUseCase);
		Injector.addSingleton(
			ExportRecordDataUseCase,
			IExportRecordDataUseCase
		);
		Injector.addSingleton(ChangeFaceUseCase, IChangeFaceUseCase);

		Injector.addSingleton(DesignLoaderService, IDesignLoaderService);
		Injector.addSingleton(RowLoaderService, IRowLoaderService);
		Injector.addSingleton(FieldLoaderService, IFieldLoaderService);
		Injector.addSingleton(ColumnLoaderService, IColumnLoaderService);
		Injector.addSingleton(LayoutLoaderService, ILayoutLoaderService);
		Injector.addSingleton(BoardLoaderService, IBoardLoaderService);

		Injector.addSingleton(FaceSelectorService, IFaceSelectorService);

		// Servicios relacionados con Pipelines
		Injector.addSingleton(PipelineLoaderService, IPipelineLoaderService);
		// Registro por interfaz
		Injector.addSingleton(PipelineCreateUseCase, IPipelineCreateUseCase);
		Injector.addSingleton(PipelineEditUseCase, IPipelineEditUseCase);
		Injector.addSingleton(PipelineDeleteUseCase, IPipelineDeleteUseCase);

		// Registro por clase concreta
		Injector.addSingleton(PipelineCreateUseCase);
		Injector.addSingleton(PipelineEditUseCase);
		Injector.addSingleton(PipelineDeleteUseCase);
		Injector.addSingleton(PipelineExportUseCase);
		Injector.addSingleton(PipelineImportUseCase);

		// Servicios relacionados con Categorías
		Injector.addSingleton(CategoryLoaderService, ICategoryLoaderService);
		Injector.addSingleton(CategoryCreateUseCase, ICategoryCreateUseCase);
		Injector.addSingleton(CategoryEditUseCase, ICategoryEditUseCase);
		Injector.addSingleton(CategoryDeleteUseCase, ICategoryDeleteUseCase);
		Injector.addSingleton(CategoryModelMapper);

		// Asegurar que estos servicios se registren correctamente
		Injector.addSingleton(AddFlowToCategoryUseCase, IAddFlowToCategoryUseCase);
		Injector.addSingleton(RemoveFlowFromCategoryUseCase, IRemoveFlowFromCategoryUseCase);

		// También registramos por clase concreta para mayor seguridad
		Injector.addSingleton(AddFlowToCategoryUseCase);
		Injector.addSingleton(RemoveFlowFromCategoryUseCase);
		Injector.addSingleton(CategoryDeleteUseCase);

		// Servicios relacionados con Nodos
		Injector.addSingleton(NodeLoaderService, INodeLoaderService);
		Injector.addSingleton(CreateNodesUseCase, ICreateNodesUseCase);
		Injector.addSingleton(EditNodeUseCase, IEditNodeUseCase);
		Injector.addSingleton(RemoveNodeUseCase, IRemoveNodeUseCase);

		Injector.addSingleton(
			ClearRecordDataFromStateUseCase,
			IClearRecordDataFromStateUseCase
		);

		Injector.addSingleton(PipelineExportUseCase, IPipelineExportUseCase)
	}

	private static _registerEventHandlers() {
		// Handlers de eventos de Lambda
		Injector.addSingleton(CalculatedLambdaSubscriptionHandler);
		Injector.addSingleton(ValidationLambdaSubscriptionHandler);
		Injector.addSingleton(VisibilityLambdaSubscriptionHandler);
		Injector.addSingleton(FieldRequiredLambdaSubscriptionHandler);
		Injector.addSingleton(LayoutRequiredLambdaSubscriptionHandler);

		// Handlers de eventos de campo y diseño
		Injector.addSingleton(FieldChangedSubscriptionHandler);
		Injector.addSingleton(FieldValidatedSubscriptionHandler);
		Injector.addSingleton(SublayoutValidatedSubscriptionHandler);
		Injector.addSingleton(SublayoutChangedSubscriptionHandler);

		// Handlers de eventos de integración
		Injector.addSingleton(IntegrationFieldChangedSubscriptionHandler);
		Injector.addSingleton(IntegrationFieldClickedSubscriptionHandler);
		Injector.addSingleton(IntegrationFieldLoadedSubscriptionHandler);
		Injector.addSingleton(IntegrationSublayoutLoadedSubscriptionHandler);
		Injector.addSingleton(IntegrationNullHandler);

		// Handlers de eventos de origen de datos
		Injector.addSingleton(DataOriginFieldLoadedSubscriptionHandler);
		Injector.addSingleton(DataOriginFieldChangedSubscriptionHandler);
		Injector.addSingleton(DataOriginSublayoutLoadedSubscriptionHandler);
		Injector.addSingleton(DataOriginSublayoutChangedSubscriptionHandler);

		// Handlers de eventos de pipeline
		Injector.addSingleton(PipelineNodeRemoveHandler);
		Injector.addSingleton(PipelineNodeCreateHandler);
	}
}
