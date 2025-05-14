import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import {
	ColumnReadModel,
	ElementReadModel,
	ElementType,
	FieldReadModel,
	FieldType,
	LayoutReadModel, SubrowReadModel, TriggerEventType
} from 'wf.xtra.core-manager';
import { PlainTabItem, ScriptElement } from 'wf.xtra.stencil-library';

export const DASH = '—';

@Injectable({
	providedIn: 'root',
})
export class CommonService {

	getSymbolMessage(symbol: Symbol): string | undefined {
		const messageSplit: string[] | undefined = symbol.description?.split('#');
		if (!messageSplit) {
			return symbol.description || '';
		}
		return messageSplit.length > 1 ? messageSplit[1] : messageSplit[0];
	}

	static getFieldIcon(fieldType: FieldType): string {
		switch (fieldType) {
			case FieldType.Text:
				return 'assets/images/icon_text.svg';
			case FieldType.Numeric:
				return 'assets/images/icon_number.svg';
			case FieldType.Date:
				return 'assets/images/icon_calendar.svg';
			case FieldType.TextArea:
				return 'assets/images/icon_textarea.svg';
			case FieldType.Check:
				return 'assets/images/icon_checkbox.svg';
			case FieldType.Radio:
				return 'assets/images/icon_radio.svg';
			case FieldType.SelectSingle:
				return 'assets/images/icon_drop_down.svg';
			case FieldType.SelectMultiple:
				return 'assets/images/icon_select_multiple.svg';
			case FieldType.Table:
				return 'assets/images/icon_table.svg';
			case FieldType.List:
				return 'assets/images/icon_card.svg';
			case FieldType.Button:
				return 'assets/images/icon_button.svg';
			case FieldType.Alert:
				return 'assets/images/icon_alert.svg';
			default:
				return fieldType;
		}
	}

	static orderElements(elements: ElementReadModel[]): ScriptElement[] {
		const parsedElements = elements.map(element => ({
			type: element.type as string === 'SelectSingle' ? 'Select' : element.type,
			text: element.name,
			subtext: element.label,
			value: element.lambdaValue
		} as ScriptElement));

		const orderedElements = parsedElements.sort((a, b) => {
			if (a.text > b.text) {
				return 1;
			} else if (a.text < b.text) {
				return -1;
			} else {
				return 0;
			}
		});

		return orderedElements;
	}

	static orderAndGroupElements(elements: ElementReadModel[]): ScriptElement[] {
		const orderedElements = elements.sort((a, b) => {
			if (!a.name || !b.name) {
				return -1;
			}
			return a.name.localeCompare(b.name);
		});

		const parsedElements = orderedElements.map(element => ({
			type: CommonService.getElementTypeParsed(element.type),
			text: element.name,
			subtext: element.label,
			value: element.lambdaValue
		} as ScriptElement));

		return parsedElements;
	}

	private static getElementTypeParsed(type: ElementType): string {
		if (type === ElementType.SelectSingle) {
			return 'Select';
		}
		if (type === ElementType.ExecVar) {
			return 'Gear';
		}

		return type as string;
	}

	static getTabsField(element: FieldReadModel, tabs: any): PlainTabItem[] {
		let tabsInfo: PlainTabItem[] = [
			{ src: "assets/images/parameters-white.svg", tooltip: "Parámetros" }
		];

		const hasInternationalization = element['translations'].length > 0;
		const internationalizationMessage = hasInternationalization ? "Internacionalización" : "Sin internacionalización";
		tabsInfo.push(
			{ src: "assets/images/language_grey_light.svg", tooltip: internationalizationMessage, highlighted: hasInternationalization }
		);

		if (tabs['required']) {
			const requiredMessage = this.propertyExists(element.requiredLambdaId) ? "Regla de requerido" : "Sin regla de requerido";
			tabsInfo.push(
				{ src: "assets/images/required_white.svg", tooltip: requiredMessage, highlighted: this.propertyExists(element.requiredLambdaId) }
			);
		}

		if (tabs['calculated']) {
			const calculatedMessage = this.propertyExists(element.calculatorLambdaId) ? "Regla de autocalculado" : "Sin regla de autocalculado";
			tabsInfo.push(
				{ src: "assets/images/calculator-white.svg", tooltip: calculatedMessage, highlighted: this.propertyExists(element.calculatorLambdaId) }
			);
		}

		if (tabs['validate']) {
			const validateMessage = this.propertyExists(element.checkerLambdaId) ? "Regla de validación" : "Sin regla de validación";
			tabsInfo.push(
				{ src: "assets/images/validation_white.svg", tooltip: validateMessage, highlighted: this.propertyExists(element.checkerLambdaId) }
			);
		}

		if (tabs['dataOrigin']) {
			const dataOriginMessage = this.propertyExists(element.dataOriginId) ? "Origen de datos" : "Sin origen de datos";
			tabsInfo.push(
				{ src: "assets/images/database_white.svg", tooltip: dataOriginMessage, highlighted: this.propertyExists(element.dataOriginId) }
			);
		}

		if (tabs['integration']) {
			const integrationMessage = this.propertyExists(element.integrationId) ? "Integración" : "Sin integración";
			tabsInfo.push(
				{ src: "assets/images/plug_white.svg", tooltip: integrationMessage, highlighted: this.propertyExists(element.integrationId) }
			);
		}

		return tabsInfo;
	}

	static getTabsLayout(element: LayoutReadModel, tabs: any): PlainTabItem[] {
		let tabsInfo: PlainTabItem[] = [
			{ src: "assets/images/parameters-white.svg", tooltip: "Parámetros" }
		];

		const hasInternationalization = element['translations'].length > 0;
		const internationalizationMessage = hasInternationalization ? "Internacionalización" : "Sin internacionalización";
		tabsInfo.push(
			{ src: "assets/images/language_grey_light.svg", tooltip: internationalizationMessage, highlighted: hasInternationalization }
		);

		if (tabs['required']) {
			const requiredMessage = this.propertyExists(element.requiredLambdaId) ? "Regla de requerido" : "Sin regla de requerido";
			tabsInfo.push(
				{ src: "assets/images/required_white.svg", tooltip: requiredMessage, highlighted: this.propertyExists(element.requiredLambdaId) }
			);
		}

		if (tabs['dataOrigin']) {
			const dataOriginMessage = this.propertyExists(element.dataOriginId) ? "Origen de datos" : "Sin origen de datos";
			tabsInfo.push(
				{ src: "assets/images/database_white.svg", tooltip: dataOriginMessage, highlighted: this.propertyExists(element.dataOriginId) }
			);
		}

		return tabsInfo;
	}

	static getTabsSubrow(): PlainTabItem[] {
		let tabsInfo: PlainTabItem[] = [
			{ src: "assets/images/parameters-white.svg", tooltip: "Parámetros" }
		];

		return tabsInfo;
	}

	static getTabsDesignElement(element: SubrowReadModel | ColumnReadModel, tabs: any): PlainTabItem[] {
		let tabsInfo: PlainTabItem[] = [];

		if (tabs['parameters']) {
			tabsInfo.push(
				{ src: "assets/images/parameters-white.svg", tooltip: "Parámetros" }
			);
		}

		if (tabs['internationalization']) {
			const hasInternationalization = element.translations.length > 0;
			const internationalizationMessage = hasInternationalization ? "Internacionalización" : "Sin internacionalización";
			tabsInfo.push(
				{ src: "assets/images/language_grey_light.svg", tooltip: internationalizationMessage, highlighted: hasInternationalization }
			);
		}

		if (tabs['visibility']) {
			const visibilityMessage = this.propertyExists(element.visibilityLambdaId) ? 'Regla de visibilidad' : 'Sin regla de visibilidad'
			tabsInfo.push(
				{ src: "assets/images/visibility_white.svg", tooltip: visibilityMessage, highlighted: this.propertyExists(element.visibilityLambdaId) }
			);
		}

		return tabsInfo;
	}

	static propertyExists(property: string | null | undefined): boolean {
		return property !== null && property !== undefined && property !== '';
	}

	static getTriggerEventIcon(event: TriggerEventType): string {
		switch (event) {
			case TriggerEventType.Load:
				return 'assets/images/icon_load.svg';
			case TriggerEventType.Change:
				return 'assets/images/icon_change.svg';
			case TriggerEventType.Click:
				return 'assets/images/icon_click.svg';
			default:
				return '';
		}
	}

	static getTriggerEventText(event: TriggerEventType): string {
		switch (event) {
			case TriggerEventType.Load:
				return 'Carga inicial';
			case TriggerEventType.Change:
				return 'Cambio de valor';
			case TriggerEventType.Click:
				return 'Hacer clic';
			default:
				return '';
		}
	}

	static navUserManualUrl(path: string): void {
		const url = `${environment.baseHref}/UserManual/${path}`;
		window.open(url, 'user_manual');
	}
}
