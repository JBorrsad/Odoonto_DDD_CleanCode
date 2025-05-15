import { CUSTOM_ELEMENTS_SCHEMA, Component } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { CommonService } from '../../services/common';

@Component({
	selector: 'app-top-menu',
	styles: [
		`
			.logo-container {
				display: flex;
				justify-content: center;
				align-items: center;
				height: 50px;
				margin: auto 0;
			}
		`,
	],
	standalone: true,
	imports: [RouterModule],
	schemas: [CUSTOM_ELEMENTS_SCHEMA],
	template: `
		<xtra-nav-bar [theme]="'dark'" logo="assets/images/logo.svg">
			<div class="logo-container" slot="logo">
				<img class="logo" src="assets/images/logo1.svg" alt="Logo" />
			</div>
			<xtra-nav-menu
				slot="menu"
				text="Mimetic"
				image="assets/images/catalogue.svg"
				[routerLink]="'/categories'"
			>
			</xtra-nav-menu>
			<xtra-nav-menu
				slot="menu"
				text="Catálogo"
				image="assets/images/catalogue.svg"
			>
				<xtra-nav-menu-list-item
					slot="items"
					[routerLink]="'/boards'"
					text="Datos Específicos"
				></xtra-nav-menu-list-item>
				<xtra-nav-menu-list-item
					slot="items"
					[routerLink]="'/controls'"
					text="Controles"
				></xtra-nav-menu-list-item>
			</xtra-nav-menu>

			<xtra-nav-menu
				slot="menu"
				text="Ayuda"
				image="assets/images/life-saver.svg"
				(click)="goToUserManual()"
			/>
		</xtra-nav-bar>
	`,
})
export class TopMenuComponent {
	constructor(private readonly router: Router) {}

	goToUserManual() {
		CommonService.navUserManualUrl('');
	}
}
