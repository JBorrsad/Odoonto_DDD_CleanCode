import { Injector } from "nucleus";
import { INodeLoaderService } from "wf.extra.application.mimetic-manager";

export class NodeInputValidator {

  private constructor(
    private readonly _nodeService: INodeLoaderService
  ){

  }

  static create(){
    const nodeService = Injector.get(INodeLoaderService);
    return new NodeInputValidator(nodeService)
  }

  validateText(text: string): boolean {
    return !String.isEmptyOrWhiteSpace(text);
  }

  validateRoute(route: string): boolean {
    const trimmed = route.trim();

    const regex = /^(http|\/).{1,}[^\/]$/;
    if (!regex.test(trimmed)) return false;

    const openBraces = (trimmed.match(/{{/g) || []).length;
    const closeBraces = (trimmed.match(/}}/g) || []).length;

    const singleOpenBraces = (trimmed.match(/{/g) || []).length;
    const singleCloseBraces = (trimmed.match(/}/g) || []).length;

    const hasOnlyDoubleBraces =
      (singleOpenBraces - openBraces * 2 === 0) &&
      (singleCloseBraces - closeBraces * 2 === 0) &&
      (openBraces === closeBraces);

    if (!hasOnlyDoubleBraces) return false;

    const braceContentMatches = trimmed.match(/{{[^{}]+}}/g) || [];
    if (braceContentMatches.length !== openBraces) return false;

    const fullPattern = /(?:^|\/){{[^{}\/]+}}(?:\/|$)/g;
    const validSegments = trimmed.match(fullPattern) || [];
    if (validSegments.length !== openBraces) return false;

    return true;
  }

  async isUnique(id: string, route: string){
    const trimmed = route.trim();
    return await this._nodeService.isUniqueRoute(id, trimmed)
  }

  validateSelect(options: string[], value: string): boolean {
    return options.map(x => x.toLowerCase()).includes(value.toLowerCase());
  }
}