import { IView } from 'nucleus';
import { SerializerModel } from 'wf.extra.application.mimetic-manager';

export abstract class ISerializerView extends IView {
  abstract displaySerializer(node: SerializerModel): void;
}