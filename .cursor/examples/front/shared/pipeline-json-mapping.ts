import { Guid, Injector } from "nucleus";
import { Pipeline } from "../../../wf.extra.domain.mimetic-manager/src/models/flows/Pipeline";
import {
	ICreateNodesUseCase,
	IPipelineCreateUseCase,
	IPipelineEditUseCase,
} from "wf.extra.application.mimetic-manager";

export class PipelineJsonMapping {
	private constructor(
		private readonly _nodeCreateUseCase: ICreateNodesUseCase,
		private readonly _pipelineCreateUseCase: IPipelineCreateUseCase,
		private readonly _pipelineEditUseCase: IPipelineEditUseCase
	) {}

	static create() {
		const nodeCreateUseCase = Injector.get(ICreateNodesUseCase);
		const pipelineCreateUseCase = Injector.get(IPipelineCreateUseCase);
		const pipelineEditUseCase = Injector.get(IPipelineEditUseCase);
		return new PipelineJsonMapping(
			nodeCreateUseCase,
			pipelineCreateUseCase,
			pipelineEditUseCase
		);
	}

	async fromJSON(obj: any) {
		console.log(obj);
		try {
			const pipeline = Pipeline.create(Guid.parse(obj.id));
			pipeline["_name"] = obj.name;
			pipeline.setIsPublic(obj.isPublic);

			const idFlow = await this._pipelineCreateUseCase.execute(pipeline.name);
			await this._pipelineEditUseCase.execute(
				idFlow,
				pipeline.name,
				pipeline["isPublic"]
			);

			// Mapear nodos
			if (obj.nodes) {
				for (const n of obj.nodes) {
					await this.nodeParser(n, idFlow);
				}
			}
			return Promise.resolve();
		} catch (error) {
			//throw new Error(`Error al parsear pipeline: ${error.message}`);
			return Promise.reject(error);
		}
	}

	async fromJSONArray(arr: any[]) {
		for (const obj of arr) {
			await this.fromJSON(obj);
		}
	}

	nodeParser(node: string, flowId: string) {
		switch (node["type"].toLowerCase()) {
			case "gate":
				return this.gateParser(node, flowId);
			case "request":
				return this.requestParser(node, flowId);
			case "response":
				return this.responseParser(node, flowId);
			case "printer":
				return this.printerParser(node, flowId);
			case "serializer":
				return this.serializerParser(node, flowId);
			case "watchdog":
				return this.watchdogParser(node, flowId);
			case "switcher":
				return this.switcherParser(node, flowId);
			case "scripter":
				return this.scripterParser(node, flowId);
			default:
				throw new Error(`No se ha encontrado el parser para el tipo de nodo`);
		}
	}

	//nodos
	gateParser(node: string, flowId: string) {
		return this._nodeCreateUseCase.addGate(
			flowId,
			Guid.create().toString(),
			node["name"],
			node["route"],
			node["method"]
		);
	}

	requestParser(node: string, flowId: string) {
		return this._nodeCreateUseCase.addRequest(
			flowId,
			Guid.create().toString(),
			node["name"],
			node["body"],
			node["route"],
			node["mediaType"],
			node["method"]
		);
	}

	responseParser(node: string, flowId: string) {
		return this._nodeCreateUseCase.addResponse(
			flowId,
			Guid.create().toString(),
			node["name"],
			node["statusCode"],
			node["mediaType"],
			node["content"]
		);
	}

	printerParser(node: string, flowId: string) {
		return this._nodeCreateUseCase.addPrinter(
			flowId,
			Guid.create().toString(),
			node["name"],
			node["script"]
		);
	}

	serializerParser(node: string, flowId: string) {
		return this._nodeCreateUseCase.addSerializer(
			flowId,
			Guid.create().toString(),
			node["name"],
			node["serializationType"]
		);
	}

	watchdogParser(node: string, flowId: string) {
		return this._nodeCreateUseCase.addWatchdog(
			flowId,
			Guid.create().toString(),
			node["name"],
			node["statusCode"],
			node["mediaType"],
			node["content"],
			node["script"]
		);
	}

	switcherParser(node: string, flowId: string) {
		//return this._nodeCreateUseCase.addSwitcher(flowId, Guid.create().toString(), node['name']);
		new Error("No se pueden crear Switchers, está en mantenimiento.");
	}

	scripterParser(node: string, flowId: string) {
		return this._nodeCreateUseCase.addScripter(
			flowId,
			Guid.create().toString(),
			node["name"],
			node["script"]
		);
	}
}
