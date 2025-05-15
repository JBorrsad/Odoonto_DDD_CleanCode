export class ExportActions {
    static async downloadExport(json: string, downloadEntity: string) {
        const jsonString = typeof json === 'string'
            ? json
            : JSON.stringify(json, null, 2);

        const blob = new Blob([jsonString], { type: 'application/json' });
        const url = window.URL.createObjectURL(blob);
        const a = document.createElement('a');

        a.href = url;
        a.download = downloadEntity + '/Mimetic.json';
        a.click();

        window.URL.revokeObjectURL(url);
    }

    static async copyJson(json: string) {
        if (navigator.clipboard) {
            await navigator.clipboard.writeText(json)
        }
    }

    static completeExport(json: string, downloadEntity: string) {
        this.copyJson(json);
        this.downloadExport(json, downloadEntity)
    }
}