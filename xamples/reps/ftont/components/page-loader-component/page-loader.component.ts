import { CommonModule } from '@angular/common';
import { CUSTOM_ELEMENTS_SCHEMA, Component, WritableSignal } from '@angular/core';
import { PageLoaderService } from './../../services/page-loader';

@Component({
	selector: 'app-page-loader',
	styles: [`
      .page-loader-wrapper {
        position: fixed;
        z-index: 99999;
        top: 0;
        left: 0;
        width: 100vw;
        height: 100vh;
        background-color: #ffffff;
      }
    `,
	],
	standalone: true,
	imports: [CommonModule],
	schemas: [
		CUSTOM_ELEMENTS_SCHEMA
	],
	template: `
		<div class="page-loader-wrapper" *ngIf="loaderIsVisible()">
			<xtra-loader-fancy />
		</div>
  `,
})
export class PageLoaderComponent {
	loaderIsVisible: WritableSignal<boolean>;

	constructor(private readonly _pageLoaderService: PageLoaderService) {
		this.loaderIsVisible = _pageLoaderService.isLoading;
	}
}
