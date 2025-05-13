import { Guid, Injector } from 'nucleus';
import {
  IPipelineLoaderService,
  IPipelineDeleteUseCase,
  PipelineReadModel,
  CategoryReadModel
} from 'wf.extra.application.mimetic-manager';

import { IPipelinesPresenter } from '../../interfaces/pipelines/pipelines.presenter.interface';
import { IPipelinesView } from '../../../views/pipelines/pipelines.view.interface';

export class PipelinesPresenter extends IPipelinesPresenter {

  private _all: PipelineReadModel[] = [];

  private constructor(
    protected override readonly _view: IPipelinesView,
    private readonly _loader:  IPipelineLoaderService,
    private readonly _deleter: IPipelineDeleteUseCase
  ) { super(_view); }

  static create(view: IPipelinesView): PipelinesPresenter {
    return new PipelinesPresenter(
      view,
      Injector.get(IPipelineLoaderService),
      Injector.get(IPipelineDeleteUseCase)
    );
  }

  async load(): Promise<void> {
    this._all = await this._loader.getAllPipelines();
    this.render(this._all);
  }

  /* requerido por IPipelinesPresenter original */
  async getPipelines(): Promise<PipelineReadModel[]> { 
    await this.load(); 
    return this._all;
  }

  order(dir: 'asc'|'desc'): void {
    const list = [...this._all].sort((a,b)=>
      dir==='asc' ? a.name.localeCompare(b.name)
                  : b.name.localeCompare(a.name));
    this.render(list);
  }
  
  async orderPipelines(newOrder: string): Promise<PipelineReadModel[]> { 
    if (newOrder === 'asc' || newOrder === 'desc') {
      this.order(newOrder);
    }
    return this._all;
  }

  filter(term: string): void {
    const list = term
      ? this._all.filter(p=>p.name.toLowerCase().includes(term.toLowerCase()))
      : this._all;
    this.render(list);
  }

  async getPipelinesByCategory(categoryId: string): Promise<PipelineReadModel[]> { 
    // Implementar filtrado por categoría cuando sea necesario
    await this.load();
    // Asumimos que categoryId es una propiedad que existe en el modelo aunque no esté explícitamente definida
    // Este es un enfoque temporal hasta que se defina correctamente la estructura
    return this._all;
  }
  
  async getCategories(): Promise<CategoryReadModel[]> { 
    // En una implementación real, esto obtendría las categorías del repositorio
    return [];
  }
  
  async exportPipelines(): Promise<string> { 
    return this._loader.exportPipelines();
  }

  render(list: PipelineReadModel[]): void {
    this._view.render(list);
  }

  async deletePipeline(id: string): Promise<void> {
    await this._deleter.execute(id as unknown as Guid);
    await this.load();
    this._view.setCantityPipelines(this._all.length);
  }
}
