import { IView } from "nucleus";

export interface CreateEditModalViewModel {
  name: string;
  isNameValid: boolean;
  errorMessage: string;
  titleText: string;
  subtitleText: string;
  saveButtonText: string;
  labelText: string;
  tooltipText: string;
}

export abstract class ICreateEditModalView extends IView {
  abstract render(viewModel: CreateEditModalViewModel): void;
} 