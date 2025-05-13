import { Newable } from "diod";
import { Injector, IState, LangType, LanguageDefinition } from "nucleus";
import { IAppConfig } from "wf.extra.application";
import { AppHttpClient, IAppHttpConfig, IMimeticHttpConfig, MimeticHttpClient } from "wf.extra.data";
import { FaceSelector, FaceType } from "wf.extra.domain.surface";
import { startup as CoreManagerStartup } from "wf.xtra.core-manager";
import { AppConf } from "../configuration/appConf";
import { FetchConf } from "../configuration/fetch-conf";
import { MimeticFetchConfig } from "../configuration/mimetic-conf";
import { EventBusIOCRegister } from "./event-bus.ioc.register";
import { MapperIOCRegister } from "./mappers.ioc.register";
import { RepositoriesIOCRegister } from "./repositories.ioc.register";
import { StatesIOCRegister } from "./states.ioc.register";
import { StoresIOCRegister } from "./stores.ioc.register";

export class AppIOCRegister {
    static registerDependencies(
        appConfig: Newable<IAppConfig>,
        appHttpConfig: Newable<IAppHttpConfig>,
        mimeticHttpConfig: Newable<IMimeticHttpConfig>
    ) {
        this.registerLanguageDependencies();
        this.registerAppConfigs(appConfig);
        this.registerHttpClients(appHttpConfig, mimeticHttpConfig);
        MapperIOCRegister.registerDependencies();
        StoresIOCRegister.registerDependencies();
        RepositoriesIOCRegister.registerDependencies();
        EventBusIOCRegister.registerDependencies();
        StatesIOCRegister.registerDependencies();

        Injector.build();

        // Registramos las dependencias para el manager actual
        // Este tira de core-infrastructure para registrar las dependencias
        // Cuando migremos a nucleus, eliminaremos esta linea
        CoreManagerStartup(AppConf, FetchConf, MimeticFetchConfig);


        this.initializeFaceSelector();
        this.initializeLanguageDefinition();
    }

    private static registerLanguageDependencies() {
        Injector.addSingleton(LanguageDefinition);
    }

    private static registerAppConfigs(appConfig: Newable<IAppConfig>) {
        Injector.addSingleton(appConfig, IAppConfig);
    }

    private static registerHttpClients(appHttpConfig: Newable<IAppHttpConfig>, mimeticHttpConfig: Newable<IMimeticHttpConfig>) {
        Injector.addSingleton(appHttpConfig, IAppHttpConfig);
        Injector.addSingleton(AppHttpClient);
        Injector.addSingleton(mimeticHttpConfig, IMimeticHttpConfig);
        Injector.addSingleton(MimeticHttpClient);
    }

    private static initializeFaceSelector() {
        const faceSelector = FaceSelector.create();
        faceSelector.setFace(FaceType.Intranet);
        const state = Injector.get(IState);
        state.setSingleton(FaceSelector, faceSelector);
    }

    private static initializeLanguageDefinition() {
        const languageDefinition = Injector.get(LanguageDefinition);
        languageDefinition.setLanguage(LangType.Spanish);
    }
}
