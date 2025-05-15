import { IView } from "nucleus";

export interface ImportModalViewModel {
  name: string;
  fileName: string;
  isValid: boolean;
  errorMessage: string;
  titleText: string;
  subtitleText: string;
  importButtonText: string;
  labelText: string;
  tooltipText: string;
}

export abstract class IImportModalView extends IView {
  abstract render(viewModel: ImportModalViewModel): void;
} 