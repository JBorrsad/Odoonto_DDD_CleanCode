import { NotificationMessage } from './notification-message.model';

export interface Notification {
  type: NotificationType;
  content: NotificationMessage[];
  nameID?: string;
  expirationTime?: number;
}

export enum NotificationType {
  Info = 'info',
  Warning = 'warning',
  Error = 'error',
  Success = 'success',
}
