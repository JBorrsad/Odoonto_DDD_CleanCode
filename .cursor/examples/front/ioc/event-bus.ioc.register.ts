import { DomainEventBus, Injector, PresentationEventBus } from "nucleus";

export class EventBusIOCRegister {
    static registerDependencies() {
        Injector.addSingleton(DomainEventBus);
        Injector.addSingleton(PresentationEventBus);
    }
}