import { IView } from 'nucleus';
import { GateModel } from 'wf.extra.application.mimetic-manager';

export abstract class IGateView extends IView {
  abstract displayGate(node: GateModel): void;
} 