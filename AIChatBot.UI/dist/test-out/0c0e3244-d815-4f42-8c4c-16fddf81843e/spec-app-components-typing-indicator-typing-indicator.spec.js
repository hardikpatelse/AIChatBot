import {
  Component,
  TestBed,
  __async,
  __commonJS,
  __decorate,
  __esm,
  init_core,
  init_testing,
  init_tslib_es6
} from "./chunk-5RTIE2IS.js";

// angular:jit:template:src/app/components/typing-indicator/typing-indicator.html
var typing_indicator_default;
var init_typing_indicator = __esm({
  "angular:jit:template:src/app/components/typing-indicator/typing-indicator.html"() {
    typing_indicator_default = '<div class="typing-indicator">\n    <span class="dot"></span>\n    <span class="dot"></span>\n    <span class="dot"></span>\n</div>';
  }
});

// angular:jit:style:src/app/components/typing-indicator/typing-indicator.css
var typing_indicator_default2;
var init_typing_indicator2 = __esm({
  "angular:jit:style:src/app/components/typing-indicator/typing-indicator.css"() {
    typing_indicator_default2 = "/* src/app/components/typing-indicator/typing-indicator.css */\n.typing-indicator {\n  display: flex;\n  align-items: center;\n  height: 24px;\n  padding-left: 8px;\n}\n.typing-indicator .dot {\n  width: 8px;\n  height: 8px;\n  margin: 0 3px;\n  border-radius: 50%;\n  background-color: #4fa3f7;\n  animation: blink 1.4s infinite both;\n}\n.typing-indicator .dot:nth-child(2) {\n  animation-delay: 0.2s;\n}\n.typing-indicator .dot:nth-child(3) {\n  animation-delay: 0.4s;\n}\n@keyframes blink {\n  0%, 80%, 100% {\n    transform: scale(0);\n    opacity: 0.3;\n  }\n  40% {\n    transform: scale(1);\n    opacity: 1;\n  }\n}\n/*# sourceMappingURL=typing-indicator.css.map */\n";
  }
});

// src/app/components/typing-indicator/typing-indicator.ts
var TypingIndicator;
var init_typing_indicator3 = __esm({
  "src/app/components/typing-indicator/typing-indicator.ts"() {
    "use strict";
    init_tslib_es6();
    init_typing_indicator();
    init_typing_indicator2();
    init_core();
    TypingIndicator = class TypingIndicator2 {
    };
    TypingIndicator = __decorate([
      Component({
        selector: "app-typing-indicator",
        standalone: false,
        template: typing_indicator_default,
        styles: [typing_indicator_default2]
      })
    ], TypingIndicator);
  }
});

// src/app/components/typing-indicator/typing-indicator.spec.ts
var require_typing_indicator_spec = __commonJS({
  "src/app/components/typing-indicator/typing-indicator.spec.ts"(exports) {
    init_testing();
    init_typing_indicator3();
    describe("TypingIndicator", () => {
      let component;
      let fixture;
      beforeEach(() => __async(null, null, function* () {
        yield TestBed.configureTestingModule({
          imports: [TypingIndicator]
        }).compileComponents();
        fixture = TestBed.createComponent(TypingIndicator);
        component = fixture.componentInstance;
        fixture.detectChanges();
      }));
      it("should create", () => {
        expect(component).toBeTruthy();
      });
    });
  }
});
export default require_typing_indicator_spec();
//# sourceMappingURL=spec-app-components-typing-indicator-typing-indicator.spec.js.map
