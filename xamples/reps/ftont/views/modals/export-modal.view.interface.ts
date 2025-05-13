import { IView } from "nucleus";

export interface ExportModalViewModel {
  name: string;
  fileName: string;
  isValid: boolean;
  errorMessage: string;
  titleText: string;
  subtitleText: string;
  saveButtonText: string;
  exportContent?: string;
}

export abstract class IExportModalView extends IView {
  abstract render(viewModel: ExportModalViewModel): void;
} 