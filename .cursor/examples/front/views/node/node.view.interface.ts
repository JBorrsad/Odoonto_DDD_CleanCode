import { View } from 'core-infrastructure';
import { Node } from 'wf.extra.domain.mimetic-manager';

export interface NodeView extends View {
  displayNode(node: Node): void;
} 