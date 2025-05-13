import { animate, group, style, transition, trigger } from '@angular/animations';
import { CommonModule } from '@angular/common';
import { Component, CUSTOM_ELEMENTS_SCHEMA, OnDestroy, OnInit, ViewEncapsulation, WritableSignal } from '@angular/core';
import { IPresenter } from 'nucleus';
import { INotificationPresenter, NotificationReadModel, NotificationType } from 'wf.xtra.core-manager';
import { Notification } from '../../core/notifications/models/notification.model';
import { NotificationService } from './../../services/notifications';
import { NotificationMessage } from './models/notification-message.model';

@Component({
	selector: 'app-notifications',
	standalone: true,
	imports: [CommonModule],
	schemas: [
		CUSTOM_ELEMENTS_SCHEMA
	],
	encapsulation: ViewEncapsulation.None,
	styles: [
		`
      .notification-wrapper {
        position: fixed;
        z-index: 9999;
        left: 50%;
        top: 3rem;
        transform: translate(-50%);
        display: flex;
        flex-direction: row;
      }
      xtra-toast {
        position: absolute;
        right: 0;
        top: 0;
        pointer-events: none;
        transition: all 0.2s linear;
        &:nth-child(1) {
          z-index: 10;
          right: 0;
          transition-delay: 0s;
          transform: scale(1) translateX(50%);
        }
        &:nth-child(2) {
          z-index: 9;
          right: 1rem;
          transition-delay: 0.1s;
          transform: scale(0.95) translateX(50%);
        }
        &:nth-child(3) {
          z-index: 8;
          right: 2rem;
          transition-delay: 0.2s;
          transform: scale(0.9) translateX(50%);
        }
        &:nth-child(4) {
          z-index: 7;
          right: 3rem;
          transition-delay: 0.3s;
          transform: scale(0.85) translateX(50%);
        }
        &:nth-child(5) {
          z-index: 6;
          right: 4rem;
          transition-delay: 0.4s;
          transform: scale(0.8) translateX(50%);
        }
        &:nth-child(6) {
          z-index: 5;
          right: 5rem;
          transition-delay: 0.5s;
          transform: scale(0.75) translateX(50%);
        }
        &:nth-child(7) {
          z-index: 4;
          right: 6rem;
          transition-delay: 0.6s;
          transform: scale(0.7) translateX(50%);
        }
        &:nth-child(8) {
          z-index: 3;
          right: 7rem;
          transition-delay: 0.7s;
          transform: scale(0.65) translateX(50%);
        }
        &:nth-child(9) {
          z-index: 2;
          right: 8rem;
          transition-delay: 0.8s;
          transform: scale(0.6) translateX(50%);
        }
        &:nth-child(10) {
          z-index: 1;
          right: 9rem;
          transition-delay: 0.9s;
          transform: scale(0.55) translateX(50%);
        }
        &.ng-animating {
          .notification-wrapper {
            .mat-expansion-panel-header {
              &.mat-expanded {
                & + .mat-expansion-panel-content {
                  height: auto !important;
                }
              }
              & + .mat-expansion-panel-content {
                height: 0 !important;
              }
            }
          }
        }
      }
    `,
	],
	animations: [
		trigger('myInsertRemoveTrigger', [
			transition(':enter', [
				style({
					opacity: 0,
					transform: 'translateX(-10rem)',
				}),
				group([
					animate(
						'0.4s cubic-bezier(0.33, 1, 0.68, 1)',
						style({
							transform: 'translateX(50%)',
						})
					),
					animate(
						'0.2s linear',
						style({
							opacity: 1,
						})
					),
				]),
			]),
			transition(':leave', [
				style({
					opacity: 1,
					transform: 'translateX(50%)',
				}),
				group([
					animate(
						'0.4s cubic-bezier(0.33, 1, 0.68, 1)',
						style({
							transform: 'translateX(30rem)',
						})
					),
					animate(
						'0.2s linear',
						style({
							opacity: 0,
						})
					),
				]),
			]),
		]),
	],
	template: `
		<div class="notification-wrapper">
		<ng-container *ngIf="notificationList">
			<xtra-toast
			*ngFor="let notification of notificationList(); let index = index"
			@myInsertRemoveTrigger
			[type]="notification.type"
			(close)="onCloseNotification(index)"
			>
			<xtra-toast-message
				*ngFor="let message of notification.content"
				[text]="message.text"
				[isLink]="message.isLink"
				(click)="goTo(message.id)"
			></xtra-toast-message>
			</xtra-toast>
		</ng-container>
		</div>
	`,
})
export class NotificationsComponent implements OnInit, OnDestroy {
	public notificationList: WritableSignal<Notification[]>;

	private _presenter: INotificationPresenter;
	get presenter(): IPresenter { return this._presenter as any; }

	constructor(private notificationService: NotificationService) {
		this.notificationList = this.notificationService.notifications;
	}

	ngOnInit(): void {
		// this._presenter = NotificationPresenter.create(this);
		// this.initPresenter();
	}

	render(data: NotificationReadModel): void {
		if (data.type == NotificationType.error) {
			this.showError(data);
		}
		else if (data.type == NotificationType.validationError) {
			this.showValidationError(data);
		}
		else if (data.type == NotificationType.success) {
			this.showSuccess(data);
		}
		else if (data.type == NotificationType.normal) {
			this.showInfo(data);
		}

	}

	private showError(data: NotificationReadModel): void {
		const message: string = data.description;
		this.notificationService.addNotificationError(
			[
				{
					text: message
				},
			],
			data.name
		);
	}

	private showValidationError(data: NotificationReadModel): void {
		const notificationMessage: NotificationMessage = {
			text: data.description,
			id: data.name,
			isLink: true
		};
		this.notificationService.addNotificationError([notificationMessage]);
	}

	private showSuccess(data: NotificationReadModel): void {
		const message: string = data.description;
		this.notificationService.addNotificationSuccess(
			[
				{
					text: message
				},
			],
			data.name
		);
	}
	private showInfo(data: NotificationReadModel): void {
		const message: string = data.description;
		this.notificationService.addNotificationInfo(
			[
				{
					text: message
				},
			],
			data.name
		);
	}

	clearValidationError(emitterId: string): void {
		this.notificationService.removeNotificationByNameID(emitterId);
	}

	onCloseNotification(index: number): void {
		this.notificationService.removeNotificationByIndex(index);
	}

	goTo(id: string): void {
		const field: HTMLElement | null = document.getElementById(id);

		try {
			(field as any).focusField();
		} catch (error) {
			console.warn(`El campo ${id} no tiene un método "focusField" definido`);
		}
	}

	private getMessage(data: string): string {
		let message = data.split('#')[1];
		message = message.replaceAll(')', '');
		return message;
	}

	ngOnDestroy(): void {
		this._presenter.dispose();
	}
}
