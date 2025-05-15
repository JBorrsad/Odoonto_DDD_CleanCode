import { IView } from 'nucleus';
import { WatchdogModel } from 'wf.extra.application.mimetic-manager';

export abstract class IWatchdogView extends IView {
  abstract displayWatchdog(node: WatchdogModel): void;
}