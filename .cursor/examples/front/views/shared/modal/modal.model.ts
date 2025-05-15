import { BaseModalModel, EditableEntity, EntityType } from "wf.extra.domain.mimetic-manager";

export type ModalMode = 'create' | 'edit' | 'import' | 'export';

export class ModalModel extends BaseModalModel {
  mode: ModalMode = 'create';
  declare entityType: EntityType;
  declare entity?: EditableEntity;
  name: string = '';
  isValid: boolean = true;
  errorMessage: string = '';
  existingNames: string[] = [];
  file: File | null = null;
  fileContent: string = '';
} 