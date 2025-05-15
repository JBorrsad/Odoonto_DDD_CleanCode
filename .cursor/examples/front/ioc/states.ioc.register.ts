import { Injector, IState, State } from "nucleus";
import { CronosStateIndexes } from "wf.extra.data.cronos";
import { DisclosureStateIndexes } from "wf.extra.data.disclosure";
import { SurfaceStateIndexes } from "wf.extra.data.surface";
import { MimeticManagerState} from "wf.extra.data.mimetic-manager";

class StateIndexed extends State {
	constructor() {
		super();
		CronosStateIndexes.addTo(this);
		SurfaceStateIndexes.addTo(this);
		DisclosureStateIndexes.addTo(this);
		MimeticManagerState.addTo(this)
	}
}

export class StatesIOCRegister {
	static registerDependencies() {
		Injector.addSingleton(StateIndexed, IState);
	}
}
