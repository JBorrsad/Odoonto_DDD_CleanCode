import { IView } from 'nucleus';
import { ScripterModel } from 'wf.extra.application.mimetic-manager';

export abstract class IScripterView extends IView {
  abstract displayScripter(node: ScripterModel): void;
}