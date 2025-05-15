import { CommonModule } from '@angular/common';
import { Component, OnInit, signal, WritableSignal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { startup as ateka_startup, IAtekaOidcManager, IAtekaOidcSettings } from 'wf.xtra.ateka-oidc-client';
import { environment } from '../../../environments/environment';
import { FooterComponent } from '../footer-component/footer.component';
import { NotificationsComponent } from '../notifications-component/notifications.component';
import { PageLoaderComponent } from '../page-loader-component/page-loader.component';
import { TopMenuComponent } from '../top-menu-component/top-menu.component';

@Component({
    selector: 'app-root',
    standalone: true,
    imports: [
        RouterOutlet,
        TopMenuComponent,
        FooterComponent,
        PageLoaderComponent,
        NotificationsComponent,
        CommonModule,
    ],
    template: `
      	<app-page-loader />
	<ng-container *ngIf="isAuthorized()">
              <app-top-menu />
              <app-notifications />
              <div class="app-wrapper">
                  <router-outlet></router-outlet>
              </div>
             <app-footer />
	</ng-container>
    `,
})
export class AppComponent implements OnInit {
    atekaOidcManager: IAtekaOidcManager;
    isAuthorized: WritableSignal<boolean> = signal(false);

    async ngOnInit() {
        const atekaConf: IAtekaOidcSettings = {
            authority: environment.ateka.authority,
            client_id: environment.ateka.client_id,
            scope: environment.ateka.scope,
            redirect_uri: window.location.href, //window.location.origin + environment.baseHref,
            post_logout_redirect_uri: window.location.href, //window.location.origin + environment.baseHref,
            automatic_silent_renew: true,
            token_changed_callback: (access_token: string) => {
                localStorage.setItem('ateka_token', access_token);
            }
        };

        this.atekaOidcManager = await ateka_startup(atekaConf);
        this.isAuthorized.set(true);
    }
}