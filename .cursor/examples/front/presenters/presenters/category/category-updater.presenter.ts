import { Injector } from 'nucleus';
import {
  ICategoryLoaderService,
  ICategoryEditUseCase,
  CategoryReadModel
} from 'wf.extra.application.mimetic-manager';

import { ICategoryUpdaterPresenter } from '../../interfaces/category/category-updater.presenter.interface';
import { ICategoryUpdaterView } from '../../../views/category/category-updater.view.interface';

export class CategoryUpdaterPresenter extends ICategoryUpdaterPresenter {
  private _categoryId!: string;

  private constructor(
    view: ICategoryUpdaterView,
    private readonly _loader: ICategoryLoaderService,
    private readonly _editor: ICategoryEditUseCase
  ) { super(view); }

  static create(view: ICategoryUpdaterView): CategoryUpdaterPresenter {
    return new CategoryUpdaterPresenter(
      view,
      Injector.get(ICategoryLoaderService),
      Injector.get(ICategoryEditUseCase)
    );
  }

  override dispose(): void { /* no subscriptions yet */ }

  async onInit(id: string): Promise<void> {
    this._categoryId = id;
    try {
      const cat = await this.getCategory();
      (this._view as ICategoryUpdaterView).render(cat);
    } catch (e: any) {
      this.showToast(e?.message ?? 'Error al cargar la categoría', true);
    }
  }

  public async getCategory(): Promise<CategoryReadModel> {
    return this._loader.getCategoryById(this._categoryId);
  }

  public async updateCategory(id: string, name: string): Promise<void> {
    try {
      await this._editor.execute(id, name);
      this.showToast('Categoría actualizada con éxito', false);
    } catch (e: any) {
      this.showToast(e?.data?.Message ?? 'Error al actualizar la categoría', true);
    }
  }

  private showToast(msg: string, isError = false): void {
    (this._view as ICategoryUpdaterView).responseOk(msg, isError);
  }

  public async isNameUnique(id: string, name: string): Promise<boolean> {
    // Implementa si tienes lógica de unicidad
    return true;
  }
} 