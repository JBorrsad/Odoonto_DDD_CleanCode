import { IView } from "nucleus";
import { EntityType, EditableEntity, ModalMode } from "wf.extra.domain.mimetic-manager";

export abstract class IModalView extends IView {
  abstract show: boolean;
  abstract mode: ModalMode;
  abstract entityType: EntityType;
  abstract entity?: EditableEntity;
  abstract existingNames: string[];
  
  abstract render(viewModel: ModalViewModel): void;
  abstract close(): void;
}

export interface ModalViewModel {
  name: string;
  isNameValid: boolean;
  errorMessage: string;
  titleText: string;
  subtitleText: string;
  saveButtonText: string;
} 