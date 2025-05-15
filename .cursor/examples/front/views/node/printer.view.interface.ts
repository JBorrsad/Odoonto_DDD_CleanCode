import { IView } from 'nucleus';
import { PrinterModel } from 'wf.extra.application.mimetic-manager';

export abstract class IPrinterView extends IView {
  abstract displayPrinter(node: PrinterModel): void;
}