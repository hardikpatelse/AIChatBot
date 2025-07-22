import {
  ChatService,
  init_chat_service
} from "./chunk-LWW7JXJN.js";
import "./chunk-MFLGNGE5.js";
import "./chunk-I3YRUNTE.js";
import {
  Component,
  EventEmitter,
  Output,
  TestBed,
  __async,
  __commonJS,
  __decorate,
  __esm,
  init_core,
  init_testing,
  init_tslib_es6
} from "./chunk-5RTIE2IS.js";

// angular:jit:template:src/app/components/model-selector/model-selector.component.html
var model_selector_component_default;
var init_model_selector_component = __esm({
  "angular:jit:template:src/app/components/model-selector/model-selector.component.html"() {
    model_selector_component_default = '<div class="relative w-full max-w-sm">\n    <label for="model-select" class="block mb-1 text-sm font-small">Select Model</label>\n\n    <select id="model-select" [(ngModel)]="selectedModelId" (change)="onModelChange()" class="form-select">\n        <option *ngFor="let model of models" [value]="model.id">\n            {{ model.name }} ({{ model.company }})\n        </option>\n    </select>\n\n    <div class="absolute top-[40px] left-0 w-full mt-1 border bg-white z-10 rounded shadow" *ngIf="showDetails">\n        <div *ngFor="let model of models" (click)="selectModel(model)"\n            class="flex items-center gap-2 px-4 py-2 hover:bg-gray-100 cursor-pointer">\n            <img [src]="model.logoUrl" alt="{{model.name}}" class="w-6 h-6 object-contain" />\n            <div class="text-sm">\n                <div class="font-medium">{{ model.name }}</div>\n                <div class="text-xs text-gray-500">{{ model.company }}</div>\n            </div>\n        </div>\n    </div>\n</div>';
  }
});

// angular:jit:style:src/app/components/model-selector/model-selector.component.css
var model_selector_component_default2;
var init_model_selector_component2 = __esm({
  "angular:jit:style:src/app/components/model-selector/model-selector.component.css"() {
    model_selector_component_default2 = "/* src/app/components/model-selector/model-selector.component.css */\nselect {\n  cursor: pointer;\n}\nimg {\n  border-radius: 4px;\n}\n/*# sourceMappingURL=model-selector.component.css.map */\n";
  }
});

// src/app/components/model-selector/model-selector.component.ts
var ModelSelectorComponent;
var init_model_selector_component3 = __esm({
  "src/app/components/model-selector/model-selector.component.ts"() {
    "use strict";
    init_tslib_es6();
    init_model_selector_component();
    init_model_selector_component2();
    init_core();
    init_chat_service();
    ModelSelectorComponent = class ModelSelectorComponent2 {
      chatService;
      models = [];
      selectedModelId = 0;
      showDetails = false;
      modelSelected = new EventEmitter();
      constructor(chatService) {
        this.chatService = chatService;
      }
      ngOnInit() {
        this.getModels();
        const saved = localStorage.getItem("selectedModel");
        this.selectedModelId = saved ? Number(saved) : this.models[0]?.id;
        this.modelSelected.emit(this.getModelById(this.selectedModelId));
      }
      getModels() {
        this.chatService.getModels().subscribe((data) => {
          this.models = data;
          const savedModelId = localStorage.getItem("selectedModel");
          if (this.models.length > 0) {
            if (savedModelId && this.models.some((m) => m.id === Number(savedModelId))) {
              this.selectedModelId = Number(savedModelId);
            } else {
              this.selectedModelId = this.models[0].id;
            }
            this.modelSelected.emit(this.getModelById(this.selectedModelId));
          } else {
            this.selectedModelId = 0;
          }
        });
      }
      onModelChange() {
        localStorage.setItem("selectedModel", this.selectedModelId.toString());
        this.modelSelected.emit(this.getModelById(this.selectedModelId));
      }
      selectModel(model) {
        this.selectedModelId = model.id;
        this.showDetails = false;
        this.onModelChange();
      }
      getModelById(modelId) {
        return this.models.find((model) => model.id === modelId) || this.models[0];
      }
      static ctorParameters = () => [
        { type: ChatService }
      ];
      static propDecorators = {
        modelSelected: [{ type: Output }]
      };
    };
    ModelSelectorComponent = __decorate([
      Component({
        selector: "app-model-selector",
        standalone: false,
        template: model_selector_component_default,
        styles: [model_selector_component_default2]
      })
    ], ModelSelectorComponent);
  }
});

// src/app/components/model-selector/model-selector.component.spec.ts
var require_model_selector_component_spec = __commonJS({
  "src/app/components/model-selector/model-selector.component.spec.ts"(exports) {
    init_testing();
    init_model_selector_component3();
    describe("ModelSelector", () => {
      let component;
      let fixture;
      beforeEach(() => __async(null, null, function* () {
        yield TestBed.configureTestingModule({
          imports: [ModelSelectorComponent]
        }).compileComponents();
        fixture = TestBed.createComponent(ModelSelectorComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
      }));
      it("should create", () => {
        expect(component).toBeTruthy();
      });
    });
  }
});
export default require_model_selector_component_spec();
//# sourceMappingURL=spec-app-components-model-selector-model-selector.component.spec.js.map
