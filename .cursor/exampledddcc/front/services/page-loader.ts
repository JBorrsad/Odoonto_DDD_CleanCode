import { Injectable, signal, WritableSignal } from '@angular/core';

@Injectable({
	providedIn: 'root',
})
export class PageLoaderService {
	private _isLoading: WritableSignal<boolean> = signal<boolean>(false);
	get isLoading(): WritableSignal<boolean> {
		return this._isLoading;
	}

	constructor() { }

	showLoader(): void {
		this._isLoading.set(true);
	}

	hideLoader(): void {
		this._isLoading.set(false);
	}
}
