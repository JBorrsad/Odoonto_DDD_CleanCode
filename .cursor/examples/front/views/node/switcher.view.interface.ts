import { IView } from 'nucleus';
import { SwitcherModel } from 'wf.extra.application.mimetic-manager';
import { Node } from 'wf.extra.domain.mimetic-manager';

export abstract class ISwitcherView extends IView {
  abstract displaySwitcher(node: SwitcherModel): void;
}