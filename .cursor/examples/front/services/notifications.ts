import { Injectable, signal, WritableSignal } from '@angular/core';
import { NotificationMessage } from '../core/notifications/models/notification-message.model';
import {
	Notification,
	NotificationType,
} from '../core/notifications/models/notification.model';

const AUTO_REMOVE_TIMER: number = 3000;

@Injectable({
	providedIn: 'root',
})
export class NotificationService {
	private _notifications: WritableSignal<Notification[]> = signal<Notification[]>([]);
	get notifications(): WritableSignal<Notification[]> {
		return this._notifications;
	}

	constructor() { }

	public removeAllNotifications(removeSuccessToo: boolean = false): void {

		if (removeSuccessToo) {
			this.notifications.set([]);
			return;
		}

		this.notifications.set(this._notifications().filter(
			(n) => n.type === NotificationType.Success
		));
	}

	public addNotificationSuccess(
		content: NotificationMessage[],
		notificationID?: string
	): void {
		const notification: Notification = {
			type: NotificationType.Success,
			content: content,
			expirationTime: AUTO_REMOVE_TIMER,
		};
		if (notificationID) notification.nameID = notificationID;
		this.addNotification(notification);
	}

	public addNotificationError(
		content: NotificationMessage[],
		notificationID?: string
	): void {
		const notification: Notification = {
			type: NotificationType.Error,
			content: content,
		};
		if (notificationID) notification.nameID = notificationID;
		this.addNotification(notification);
	}

	public addNotificationWarning(
		content: NotificationMessage[],
		notificationID?: string
	): void {
		const notification: Notification = {
			type: NotificationType.Warning,
			content: content,
		};
		if (notificationID) notification.nameID = notificationID;
		this.addNotification(notification);
	}

	public addNotificationInfo(
		content: NotificationMessage[],
		notificationID?: string,
		expirationTime?: number
	): void {
		const notification: Notification = {
			type: NotificationType.Info,
			content: content,
		};
		if (notificationID) notification.nameID = notificationID;
		if (expirationTime) notification.expirationTime = expirationTime;
		this.addNotification(notification);
	}

	private addNotification(notification: Notification): void {

		this._notifications.set([...this.notifications(), notification]);

		if (notification.expirationTime) {

			setTimeout(() => {
				const index: number = this._notifications().indexOf(notification);
				this.removeNotificationByIndex(index);
			}, notification.expirationTime);
		}
	}

	removeNotificationByIndex(index: number): void {
		this._notifications.set(this._notifications().filter(
			(n, i) => i !== index
		));
	}

	removeNotificationByNameID(nameID: string): void {
		let notif: Notification | undefined = this._notifications().find(
			(n) => n.nameID === nameID
		);
		if (notif) {
			const index: number = this._notifications().indexOf(notif);
			this._notifications.set(this._notifications().filter(
				(n, i) => i !== index
			));
		}
	}
}
