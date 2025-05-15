import { ComponentDefinition, IPresenter, IState, IView, Injector } from 'nucleus';
import { Node, NodeType } from 'wf.extra.domain.mimetic-manager';

import { INodesListView } from '../../../views/nodes/nodes-list.view';
import { INodesPresenter } from '../../interfaces/nodes/nodes.presenter.interface';
import { IGateView } from '../../../views/node/gate.view.interface';
import { IPrinterView } from '../../../views/node/printer.view.interface';
import { IRequestView } from '../../../views/node/request.view.interface';
import { IResponseView } from '../../../views/node/response.view.interface';
import { IScripterView } from '../../../views/node/scripter.view.interface';
import { ISerializerView } from '../../../views/node/serializer.view.interface';
import { ISwitcherView } from '../../../views/node/switcher.view.interface';
import { IWatchdogView } from '../../../views/node/watchdog.view.interface';


export class NodesPresenter extends IPresenter implements INodesPresenter {

  private constructor(
    view: INodesListView,
    private readonly _state: IState
  ) {
    super(view);
  }

  static create(view: INodesListView): NodesPresenter {
    const state = Injector.get(IState)
    return new NodesPresenter(view, state);
  }

  loadNodes(children: ComponentDefinition[]) {
    this.clearChildren();
    this.addChildren(children)
  }

  
} 
