import { IPresenter } from "nucleus";
import { EntityBase, EntityType } from "wf.extra.domain.mimetic-manager";

export abstract class IExportModalPresenter extends IPresenter {
  /**
   * Inicializa el presentador
   */
  abstract init(): void;
  
  /**
   * Configura el tipo de entidad y la entidad a exportar
   */
  abstract setup(entityType: EntityType, entity: EntityBase): void;
  
  /**
   * Valida el nombre del archivo de exportación
   */
  abstract validateFileName(fileName: string): void;
  
  /**
   * Realiza la exportación a archivo
   */
  abstract export(): Promise<void>;
  
  /**
   * Copia el contenido de exportación al portapapeles
   */
  abstract copyToClipboard(): Promise<void>;
  
  /**
   * Limpia los recursos
   */
  abstract override dispose(): void;
} 