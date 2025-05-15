import { CUSTOM_ELEMENTS_SCHEMA, Component } from '@angular/core';

@Component({
	selector: 'app-footer',
	styleUrls: [],
	standalone: true,
	schemas: [
		CUSTOM_ELEMENTS_SCHEMA,
	],
	template: `
		<xtra-page-footer>
		<div slot="center">
			<img src="assets/images/gobierno_white.svg" height="30" />
		</div>
		<div slot="right"></div>
		</xtra-page-footer>
  	`,
})
export class FooterComponent { }
