import { CommonModule } from "@angular/common";
import { Component, signal, WritableSignal } from "@angular/core";
import { PageLoaderService } from "@services/page-loader";
import { RecordPreviewSelectorView } from "manager-library";

@Component({
    selector: 'record-preview-selector',
    standalone: true,
    imports: [
        CommonModule,
        RecordPreviewSelectorView
    ],
    template: `
        <record-preview-selector-view *ngIf="!isLoading()" />
    `,
    styles: [``]
})
export class RecordPreviewSelectorComponent {
    isLoading: WritableSignal<boolean> = signal<boolean>(true);

    constructor(
        private readonly _pageLoaderService: PageLoaderService
    ) {
        this.isLoading = _pageLoaderService.isLoading;
    }
}
