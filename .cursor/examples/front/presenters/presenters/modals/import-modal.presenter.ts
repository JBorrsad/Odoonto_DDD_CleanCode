/*import { Injector } from "nucleus";
import { IImportModalPresenter } from "../../interfaces/modals/import-modal.presenter.interface";
import { IImportModalView } from "../../../views/modals/import-modal.view.interface";
import { 
  EntityType,
  ImportModalModel
} from "wf.extra.domain.mimetic-manager";
import { 
  IPipelineImportUseCase,
  ICategoryImportUseCase,
  JSONCategoryReadModel,
  CategoryReadModel,
  PipelineImportUseCase,
  CategoryImportUseCase
} from "wf.extra.application.mimetic-manager";

export class ImportModalPresenter extends IImportModalPresenter {
  private _model: ImportModalModel;
  
  constructor(
    protected override readonly _view: IImportModalView,
    private _pipelineImportUseCase?: IPipelineImportUseCase,
    private _categoryImportUseCase?: ICategoryImportUseCase
  ) {
    super(_view);
    this._model = new ImportModalModel();
    this._injectDependencies();
  }
  
  static create(view: IImportModalView): ImportModalPresenter {
    return new ImportModalPresenter(view);
  }
  

  private _injectDependencies(): void {
    if (!this._pipelineImportUseCase) {
      try {
        // Enfoque SOLID: obtenemos la implementación concreta
        this._pipelineImportUseCase = Injector.get(PipelineImportUseCase);
      } catch (error) {
        console.warn('PipelineImportUseCase no está disponible, utilizando fallback');
        // Implementación temporal
        this._pipelineImportUseCase = {
          execute: async (name: string, json: string) => {
            console.log(`Simulando importación de pipeline: ${name}`);
            return 'pipeline-importado-id';
          }
        } as IPipelineImportUseCase;
      }
    }
    
    if (!this._categoryImportUseCase) {
      try {
        // Enfoque SOLID: obtenemos la implementación concreta
        this._categoryImportUseCase = Injector.get(CategoryImportUseCase);
      } catch (error) {
        console.warn('CategoryImportUseCase no está disponible, utilizando fallback');
        // Implementación temporal
        this._categoryImportUseCase = {
          import: async (jsonCategories: JSONCategoryReadModel[]) => {
            console.log(`Simulando importación de categorías`, jsonCategories);
            const mockCategory = {
              id: 'categoria-importada-id',
              name: 'Categoría importada',
              flows: [],
              createDate: new Date().toISOString(),
              editDate: new Date().toISOString()
            } as CategoryReadModel;
            return [mockCategory];
          }
        } as ICategoryImportUseCase;
      }
    }
  }
  

  override init(): void {
    this._updateViewModel();
  }
  

  override setup(entityType: EntityType, existingNames: string[] = []): void {
    this._model.entityType = entityType;
    this._model.existingNames = existingNames;
    this._model.isValid = false;
    this._model.errorMessage = '';
    this._model.name = '';
    this._model.fileName = '';
    this._model.fileContent = '';
    
    this._updateViewModel();
  }
  

  override validateName(name: string): void {
    this._model.name = name;
    
    // Validación básica: no puede estar vacío
    if (!name || name.trim() === '') {
      this._model.isValid = false;
      this._model.errorMessage = "El nombre no puede estar vacío.";
      this._updateViewModel();
      return;
    }
    
    const normalizedName = name.toLowerCase().trim();
    const existingNormalizedNames = this._model.existingNames.map(n => n.toLowerCase().trim());
    
    // Verificar que no exista ya otra entidad con ese nombre
    if (existingNormalizedNames.includes(normalizedName)) {
      this._model.isValid = false;
      this._model.errorMessage = `Ya existe ${this._model.entityType === 'category' ? 'una categoría' : 'un pipeline'} con ese nombre.`;
      this._updateViewModel();
      return;
    }
    
    this._validateComplete();
  }
  

  override handleFileSelection(file: File | null): void {
    if (!file) {
      this._model.fileName = '';
      this._model.fileContent = '';
      this._model.isValid = false;
      this._updateViewModel();
      return;
    }
    
    this._model.fileName = file.name;
    
    // Leer el contenido del archivo
    const reader = new FileReader();
    reader.onload = (e) => {
      try {
        const content = e.target?.result as string;
        
        // Validar que es un JSON válido
        JSON.parse(content);
        
        this._model.fileContent = content;
        this._validateComplete();
      } catch (error) {
        this._model.isValid = false;
        this._model.errorMessage = "El archivo no contiene un JSON válido.";
        this._updateViewModel();
      }
    };
    
    reader.onerror = () => {
      this._model.isValid = false;
      this._model.errorMessage = "Error al leer el archivo.";
      this._updateViewModel();
    };
    
    reader.readAsText(file);
  }
  

  override async import(): Promise<string> {
    if (!this._model.isValid) {
      return Promise.reject(new Error(this._model.errorMessage));
    }
    
    const name = this._model.name.trim();
    const fileContent = this._model.fileContent;
    
    try {
      if (this._model.entityType === 'category') {
        // Parsear el JSON para obtener la categoría
        let categoryData;
        try {
          categoryData = JSON.parse(fileContent);
        } catch (error) {
          throw new Error("El archivo no contiene un JSON válido.");
        }
        
        // Asegurar que categoryData es un array (si no lo es, convertirlo)
        const categories = Array.isArray(categoryData) ? categoryData : [categoryData];
        
        // Aplicar el nuevo nombre a la primera categoría
        if (categories.length > 0) {
          categories[0].name = name;
        }
        
        // Importar la categoría
        const result = await this._categoryImportUseCase!.import(categories);
        return result[0]?.id || 'error';
      } else {
        return this._pipelineImportUseCase!.execute(name, fileContent);
      }
    } catch (error) {
      console.error(`Error al importar ${this._model.entityType}:`, error);
      throw error;
    }
  }
  

  private _validateComplete(): void {
    if (this._model.name && this._model.fileContent) {
      this._model.isValid = true;
      this._model.errorMessage = '';
    } else {
      this._model.isValid = false;
      this._model.errorMessage = '';
    }
    
    this._updateViewModel();
  }
  

  private _updateViewModel(): void {
    const viewModel = {
      name: this._model.name,
      fileName: this._model.fileName,
      isValid: this._model.isValid,
      errorMessage: this._model.errorMessage,
      titleText: this._getTitle(),
      subtitleText: this._getSubtitle(),
      importButtonText: 'Importar',
      labelText: `Nombre ${this._model.entityType === 'category' ? 'de la categoría' : 'del pipeline'}`,
      tooltipText: `Inserte un nombre único ${this._model.entityType === 'category' ? 'de la categoría' : 'del pipeline'}`
    };
    
    this._view.render(viewModel);
  }

  private _getTitle(): string {
    return `Importar ${this._model.entityType === 'category' ? 'una categoría' : 'un pipeline'} desde archivo JSON`;
  }
  

  private _getSubtitle(): string {
    return `Seleccione un archivo JSON que contenga ${this._model.entityType === 'category' ? 'una categoría' : 'un pipeline'} exportado previamente`;
  }

  override dispose(): void {
    // Liberar recursos si es necesario
  }
} */