import { IPresenter } from "nucleus";
import { EntityType } from "wf.extra.domain.mimetic-manager";

export abstract class IImportModalPresenter extends IPresenter {
  /**
   * Inicializa el presentador
   */
  abstract init(): void;
  
  /**
   * Configura el tipo de entidad a importar
   */
  abstract setup(entityType: EntityType, existingNames: string[]): void;
  
  /**
   * Valida el nombre de la entidad que se está importando
   */
  abstract validateName(name: string): void;
  
  /**
   * Maneja la selección de un archivo para importar
   */
  abstract handleFileSelection(file: File | null): void;
  
  /**
   * Realiza la importación
   */
  abstract import(): Promise<string>;
  
  /**
   * Limpia los recursos
   */
  abstract override dispose(): void;
} 