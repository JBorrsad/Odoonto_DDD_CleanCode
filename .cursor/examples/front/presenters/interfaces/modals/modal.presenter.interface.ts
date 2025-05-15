import { IPresenter } from "nucleus";
import { EntityType, EditableEntity, ModalMode } from "wf.extra.domain.mimetic-manager";

// Modo del modal
export abstract class IModalPresenter extends IPresenter {
  /**
   * Inicializa el presentador
   */
  abstract init(): void;
  
  /**
   * Configura el presentador para un nuevo modo
   */
  abstract setup(
    mode: ModalMode,
    entityType: EntityType,
    entity?: EditableEntity,
    existingNames?: string[]
  ): void;
  
  /**
   * Valida el nombre ingresado
   */
  abstract validateName(name: string): void;
  
  /**
   * Maneja la selección de archivo (para importación)
   */
  abstract handleFileSelection(file: File | null): void;
  
  /**
   * Guarda los cambios según el modo y tipo de entidad
   */
  abstract save(): Promise<string | void>;
  
  /**
   * Exporta una entidad a un archivo
   */
  abstract export(): Promise<void>;
  
  /**
   * Limpia los recursos al destruir el presentador
   */
  abstract override dispose(): void;
} 