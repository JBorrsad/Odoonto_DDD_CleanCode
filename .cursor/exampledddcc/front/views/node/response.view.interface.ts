import { IView } from 'nucleus';
import { ResponseModel } from 'wf.extra.application.mimetic-manager';
export abstract class IResponseView extends IView {
  abstract displayResponse(node: ResponseModel): void;
}