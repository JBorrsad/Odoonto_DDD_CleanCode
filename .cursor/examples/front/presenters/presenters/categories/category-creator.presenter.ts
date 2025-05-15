import { Injector, Guid } from "nucleus";
import {
  ICategoryCreateUseCase,
  ICategoryLoaderService
} from "wf.extra.application.mimetic-manager";
import { ICategoryCreatorPresenter } from "../../interfaces/categories/category-creator.presenter.interface";
import { ICategoryCreatorView } from "../../../views/categories/category-creator.view.interface";

export class CategoryCreatorPresenter extends ICategoryCreatorPresenter {
  private constructor(
    view: ICategoryCreatorView,
    private readonly _categoryCreateUseCase: ICategoryCreateUseCase,
    private readonly _categoryLoaderService: ICategoryLoaderService
  ) {
    super(view);
  }

  static create(view: ICategoryCreatorView) {
    const _categoryCreateUseCase = Injector.get(ICategoryCreateUseCase);
    const _categoryLoaderService = Injector.get(ICategoryLoaderService);
    return new CategoryCreatorPresenter(
      view,
      _categoryCreateUseCase,
      _categoryLoaderService
    );
  }

  async createCategory(name: string): Promise<string> {
    if (!name || name.trim() === "") {
      throw new Error("El nombre de la categoría no puede estar vacío");
    }

    const id = Guid.create().toString();

    await this._categoryCreateUseCase.execute(id, name);

    return id;
  }

  async isNameUnique(name: string): Promise<boolean> {
    if (!name || name.trim() === "") {
      return false;
    }

    const categories = await this._categoryLoaderService.getAllCategories();
    return !categories.some(
      (cat) => cat.name.toLowerCase() === name.toLowerCase().trim()
    );
  }
}
