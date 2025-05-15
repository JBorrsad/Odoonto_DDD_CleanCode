import { IPresenter } from "nucleus";
import { EntityBase, EntityType } from "wf.extra.domain.mimetic-manager";

export abstract class ICreateEditModalPresenter extends IPresenter {
  /**
   * Inicializa el presentador
   */
  abstract init(): void;
  
  /**
   * Configura el tipo de entidad, modo y entidad (opcional)
   */
  abstract setup(
    mode: 'create' | 'edit',
    entityType: EntityType,
    entity?: EntityBase,
    existingNames?: string[]
  ): void;
  
  /**
   * Valida el nombre de la entidad
   */
  abstract validateName(name: string): void;
  
  /**
   * Guarda la entidad (creando o actualizando)
   */
  abstract save(): Promise<string>;
  
  /**
   * Limpia los recursos
   */
  abstract override dispose(): void;
} 