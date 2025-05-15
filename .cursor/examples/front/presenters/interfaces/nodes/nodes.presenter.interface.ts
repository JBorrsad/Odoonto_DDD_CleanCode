import { ComponentDefinition, IPresenter } from 'nucleus';


export abstract class INodesPresenter extends IPresenter {

  abstract loadNodes(children: ComponentDefinition[]);
} 