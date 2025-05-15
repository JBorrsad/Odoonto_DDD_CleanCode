import { CommonModule } from "@angular/common";
import { Component, OnInit, signal, WritableSignal } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { PageLoaderService } from "@services/page-loader";
import { XtraPlayerComponent } from "player-library";

@Component({
    selector: 'record-preview',
    standalone: true,
    imports: [
        CommonModule,
        XtraPlayerComponent
    ],
    template: `
        <ng-container *ngIf="!isLoading() &&recordId()">
            <div>Rendered Record Preview</div>
            <xtra-player [recordId]="recordId()"></xtra-player>
        </ng-container>
    `,
    styles: [``]
})
export class RecordPreviewComponent implements OnInit {
    recordId: WritableSignal<string> = signal<string>(null);
    isLoading: WritableSignal<boolean> = signal<boolean>(true);

    constructor(
        private readonly _activatedRoute: ActivatedRoute,
        private readonly _pageLoaderService: PageLoaderService
    ) {
        this.isLoading = _pageLoaderService.isLoading;
    }

    ngOnInit() {
        this._activatedRoute.params.subscribe(params => {
            this.recordId.set(params['recordId']);
            this._pageLoaderService.hideLoader();
        });
    }

}
