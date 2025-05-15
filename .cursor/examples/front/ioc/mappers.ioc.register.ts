import { Injector } from 'nucleus';
import {
	FormModelMapper,
	LineModelMapper,
	RecordModelMapper,
} from 'wf.extra.application.cronos';
import {
	AlertModelMapping,
	BoardModelMapper,
	ButtonModelMapping,
	CheckModelMapping,
	ColumnModelMapper,
	DateModelMapping,
	FieldModelMapperContainer,
	FileInputSublayoutModelMapper,
	NumericModelMapping,
	PreviewModelMapper,
	PreviewOptionModelMapper,
	RowModelMapper,
	SelectMultipleSublayoutModelMapper,
	SelectSingleModelMapping,
	SelectSingleOptionsModelMapping,
	SublayoutModelMapper,
	SubrowModelMapper,
	TextAreaModelMapping,
	TextModelMapping,
	YearModelMapping,
} from 'wf.extra.application.surface';
import {
	FileInputSubformEntityMapper,
	FormEntityMapper,
	LineEntityMapping,
	RecordEntityMapper,
	SubformEntityMapper,
} from 'wf.extra.data.cronos';
import {
	AlertEntityMapping,
	BoardDefinitionMapper,
	BoardEntityMapper,
	ButtonEntityMapping,
	CheckEntityMapping,
	ColumnEntityMapping,
	ControlDefinitionMapper,
	ControlEntityMapper,
	DataOriginEntityMapping,
	DateEntityMapping,
	DesignEntityMappersContainer,
	DesignEntityMapping,
	EntryEntityMapper,
	FieldEntityMappersContainer,
	FieldOptionEntityMapping,
	FileInputEntityMapping,
	IntegrationEntityMappersContainer,
	IntegrationEntityMapping,
	IntegrationParameterEntityMapping,
	LambdaCalculatedEntityMapping,
	LambdaEntityMappersContainer,
	LambdaFieldParameterEntityMapping,
	LambdaFieldRequiredEntityMapping,
	LambdaLayoutParameterEntityMapping,
	LambdaLayoutRequiredEntityMapping,
	LambdaValidationEntityMapping,
	LambdaVisibilityEntityMapping,
	LayoutEntityMappersContainer,
	LayoutEntityMapping,
	ListEntityMapping,
	NumericEntityMapping,
	PreviewEntityMapping,
	PreviewFieldOptionEntityMapping,
	PreviewLayoutOptionEntityMapping,
	RadioEntityMapping,
	RowEntityMapping,
	SelectMultipleEntityMapping,
	SelectSingleEntityMapping,
	SubrowEntityMapping,
	TableEntityMapping,
	TextAreaEntityMapping,
	TextEntityMapping,
	YearEntityMapping,
} from 'wf.extra.data.surface';
import {
	BooleanParameterEntityMapping,
	CategoryEntityMapper,
	CategoryQueryMapper,
	DateParameterEntityMapping,
	GateEntityMapper,
	NodeEntityTypeParse,
	NumericParameterEntityMapping,
	ParameterEntityMapping,
	PipelineEntityMapper,
	PipelineQueryMapper,
	PrinterEntityMapper,
	RequestEntityMapper,
	ResponseEntityMapper,
	ScripterEntityMapper,
	SerializerEntityMapper,
	SwitcherEntityMapper,
	TextParameterEntityMapping,
	WatchdogEntityMapper,
} from 'wf.extra.data.mimetic-manager';
import {
	GateModelMapping,
	NodeTypeMapping,
	ParameterModelMapping,
	PrinterModelMapping,
	ReadPipelineModelMapper,
	RequestModelMapping,
	ResponseModelMapping,
	ScripterModelMapping,
	SerializerModelMapping,
	SwitcherModelMapping,
	WatchdogModelMapping,
} from 'wf.extra.application.mimetic-manager';
export class MapperIOCRegister {
	static registerDependencies() {
		this._registerEntityMappers();
		this._registerModelsMappers();
	}

	private static _registerEntityMappers() {
		Injector.addSingleton(RecordEntityMapper);
		Injector.addSingleton(FormEntityMapper);
		Injector.addSingleton(SubformEntityMapper);
		Injector.addSingleton(FileInputSubformEntityMapper);
		Injector.addSingleton(LineEntityMapping);

		Injector.addSingleton(BoardDefinitionMapper);
		Injector.addSingleton(ControlDefinitionMapper);
		Injector.addSingleton(BoardEntityMapper);
		Injector.addSingleton(EntryEntityMapper);
		Injector.addSingleton(ControlEntityMapper);

		Injector.addSingleton(LayoutEntityMapping);
		Injector.addSingleton(SelectMultipleEntityMapping);
		Injector.addSingleton(ListEntityMapping);
		Injector.addSingleton(TableEntityMapping);
		Injector.addSingleton(FileInputEntityMapping);

		Injector.addSingleton(PreviewEntityMapping);

		Injector.addSingleton(DesignEntityMapping);
		Injector.addSingleton(RowEntityMapping);
		Injector.addSingleton(SubrowEntityMapping);
		Injector.addSingleton(ColumnEntityMapping);

		Injector.addSingleton(AlertEntityMapping);
		Injector.addSingleton(ButtonEntityMapping);
		Injector.addSingleton(CheckEntityMapping);
		Injector.addSingleton(NumericEntityMapping);
		Injector.addSingleton(RadioEntityMapping);
		Injector.addSingleton(TextAreaEntityMapping);
		Injector.addSingleton(TextEntityMapping);
		Injector.addSingleton(DateEntityMapping);
		Injector.addSingleton(YearEntityMapping);

		Injector.addSingleton(PreviewLayoutOptionEntityMapping);
		Injector.addSingleton(PreviewFieldOptionEntityMapping);

		Injector.addSingleton(LambdaValidationEntityMapping);
		Injector.addSingleton(LambdaCalculatedEntityMapping);
		Injector.addSingleton(LambdaVisibilityEntityMapping);
		Injector.addSingleton(LambdaFieldParameterEntityMapping);
		Injector.addSingleton(LambdaLayoutParameterEntityMapping);
		Injector.addSingleton(LambdaFieldRequiredEntityMapping);
		Injector.addSingleton(LambdaLayoutRequiredEntityMapping);

		Injector.addSingleton(SelectSingleEntityMapping);
		Injector.addSingleton(FieldOptionEntityMapping);
		Injector.addSingleton(DataOriginEntityMapping);
		Injector.addSingleton(IntegrationEntityMapping);
		Injector.addSingleton(IntegrationParameterEntityMapping);

		Injector.addSingleton(LayoutEntityMappersContainer);
		Injector.addSingleton(FieldEntityMappersContainer);
		Injector.addSingleton(DesignEntityMappersContainer);
		Injector.addSingleton(LambdaEntityMappersContainer);
		Injector.addSingleton(IntegrationEntityMappersContainer);

		Injector.addSingleton(PipelineEntityMapper);
		Injector.addSingleton(PipelineQueryMapper);
		Injector.addSingleton(CategoryEntityMapper);
		Injector.addSingleton(CategoryQueryMapper);

		Injector.addSingleton(GateEntityMapper);
		Injector.addSingleton(PrinterEntityMapper);
		Injector.addSingleton(RequestEntityMapper);
		Injector.addSingleton(ResponseEntityMapper);
		Injector.addSingleton(ScripterEntityMapper);
		Injector.addSingleton(SerializerEntityMapper);
		Injector.addSingleton(SwitcherEntityMapper);
		Injector.addSingleton(WatchdogEntityMapper);

		Injector.addSingleton(TextParameterEntityMapping);
		Injector.addSingleton(DateParameterEntityMapping);
		Injector.addSingleton(BooleanParameterEntityMapping);
		Injector.addSingleton(NumericParameterEntityMapping);
		Injector.addSingleton(NodeEntityTypeParse);
	}

	private static _registerModelsMappers() {
		Injector.addSingleton(BoardModelMapper);

		Injector.addSingleton(RecordModelMapper);
		Injector.addSingleton(FormModelMapper);
		Injector.addSingleton(LineModelMapper);

		Injector.addSingleton(ColumnModelMapper);
		Injector.addSingleton(RowModelMapper);
		Injector.addSingleton(SubrowModelMapper);

		Injector.addSingleton(SublayoutModelMapper);
		Injector.addSingleton(SelectMultipleSublayoutModelMapper);
		Injector.addSingleton(FileInputSublayoutModelMapper);

		Injector.addSingleton(TextModelMapping);
		Injector.addSingleton(TextAreaModelMapping);
		Injector.addSingleton(NumericModelMapping);
		Injector.addSingleton(CheckModelMapping);
		Injector.addSingleton(ButtonModelMapping);
		Injector.addSingleton(SelectSingleOptionsModelMapping);
		Injector.addSingleton(SelectSingleModelMapping);
		Injector.addSingleton(FieldModelMapperContainer);
		Injector.addSingleton(DateModelMapping);
		Injector.addSingleton(YearModelMapping);
		Injector.addSingleton(AlertModelMapping);

		Injector.addSingleton(PreviewModelMapper);
		Injector.addSingleton(PreviewOptionModelMapper);

		Injector.addSingleton(ReadPipelineModelMapper);

		Injector.addSingleton(GateModelMapping);
		Injector.addSingleton(PrinterModelMapping);
		Injector.addSingleton(RequestModelMapping);
		Injector.addSingleton(ResponseModelMapping);
		Injector.addSingleton(ScripterModelMapping);
		Injector.addSingleton(SerializerModelMapping);
		Injector.addSingleton(SwitcherModelMapping);
		Injector.addSingleton(WatchdogModelMapping);

		Injector.addSingleton(NodeTypeMapping);
		Injector.addSingleton(ParameterModelMapping);
	}
}
