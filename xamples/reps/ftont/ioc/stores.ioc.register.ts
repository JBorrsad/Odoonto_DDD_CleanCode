import { Injector } from 'nucleus';
import {
	FormStoreFake,
	IFormStore,
	IIntegrationStore,
	IntegrationStoreFake,
	IRecordStore,
	IRGEStore,
	RecordStoreFake,
	RGEStoreFake,
} from 'wf.extra.data.cronos';
import {
	IPipelineStore,
	INodeStore,
	PipelineStore,
	NodeStore,
} from 'wf.extra.data.mimetic-manager';
import { ICategoryStore, CategoryStore } from 'wf.extra.data.mimetic-manager';
import {
	BoardStore,
	ControlStore,
	DesignStore,
	IBoardStore,
	IControlStore,
	IDesignStore,
	ILayoutStore,
	ITagStore,
	LayoutStore,
	TagStore,
} from 'wf.extra.data.surface';

export class StoresIOCRegister {
	static registerDependencies() {
		Injector.addSingleton(IntegrationStoreFake, IIntegrationStore);
		Injector.addSingleton(FormStoreFake, IFormStore);
		Injector.addSingleton(RecordStoreFake, IRecordStore);
		Injector.addSingleton(RGEStoreFake, IRGEStore);

		Injector.addSingleton(BoardStore, IBoardStore);
		Injector.addSingleton(ControlStore, IControlStore);
		Injector.addSingleton(DesignStore, IDesignStore);
		Injector.addSingleton(LayoutStore, ILayoutStore);
		Injector.addSingleton(TagStore, ITagStore);

		Injector.addSingleton(PipelineStore, IPipelineStore);
		Injector.addSingleton(NodeStore, INodeStore);
		Injector.addSingleton(CategoryStore, ICategoryStore);
	}
}
