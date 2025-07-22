import {
  Component,
  Input,
  TestBed,
  __decorate,
  init_core,
  init_testing,
  init_tslib_es6
} from "./chunk-5RTIE2IS.js";

// src/app/components/header/header.component.spec.ts
init_testing();

// src/app/components/header/header.component.ts
init_tslib_es6();

// angular:jit:template:src/app/components/header/header.component.html
var header_component_default = `<header class="page-header py-3 px-4 bg-light border-bottom">
    <h4 class="mb-0">Welcome {{ userName || '' }}</h4>
</header>`;

// angular:jit:style:src/app/components/header/header.component.css
var header_component_default2 = "/* src/app/components/header/header.component.css */\n.page-header {\n  background: #f8f9fa;\n  border-bottom: 1px solid #dee2e6;\n}\n/*# sourceMappingURL=header.component.css.map */\n";

// src/app/components/header/header.component.ts
init_core();
var HeaderComponent = class HeaderComponent2 {
  userName;
  static propDecorators = {
    userName: [{ type: Input }]
  };
};
HeaderComponent = __decorate([
  Component({
    selector: "app-header",
    template: header_component_default,
    standalone: false,
    styles: [header_component_default2]
  })
], HeaderComponent);

// src/app/components/header/header.component.spec.ts
describe("HeaderComponent", () => {
  let component;
  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [HeaderComponent]
    });
    const fixture = TestBed.createComponent(HeaderComponent);
    component = fixture.componentInstance;
  });
  it("should create", () => {
    expect(component).toBeTruthy();
  });
});
//# sourceMappingURL=spec-app-components-header-header.component.spec.js.map
