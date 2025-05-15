import { Injector } from 'nucleus';
import {
	FormRepository,
	IFormRepository,
	IInstanceRepository,
	IIntegrationRepository,
	InstanceRepository,
	IntegrationRepository,
	IRecordRepository,
	IRGERepository,
	RecordRepository,
	RGERepository,
} from 'wf.extra.data.cronos';
import {
	CategoryRepository,
	ICategoryRepository,
	INodeRepository,
	IPipelineRepository,
	NodeRepository,
	PipelineRepository,
} from 'wf.extra.data.mimetic-manager';
import { BoardRepository, IBoardRepository } from 'wf.extra.data.surface';


export class RepositoriesIOCRegister {
	static registerDependencies() {
		Injector.addSingleton(RecordRepository, IRecordRepository);
		Injector.addSingleton(FormRepository, IFormRepository);
		Injector.addSingleton(RGERepository, IRGERepository);
		Injector.addSingleton(IntegrationRepository, IIntegrationRepository);
		Injector.addSingleton(InstanceRepository, IInstanceRepository);
		Injector.addSingleton(BoardRepository, IBoardRepository);

		Injector.addSingleton(PipelineRepository, IPipelineRepository);
		Injector.addSingleton(NodeRepository, INodeRepository);
		Injector.addSingleton(CategoryRepository, ICategoryRepository);
	}
}
