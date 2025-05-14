import { IView } from 'nucleus';
import { RequestModel } from 'wf.extra.application.mimetic-manager';
export abstract class IRequestView extends IView {
  abstract displayRequest(node: RequestModel): void;
} 