import {
  ChatSessionService
} from "./chunk-DPJCSV3K.js";
import "./chunk-MFLGNGE5.js";
import "./chunk-I3YRUNTE.js";
import {
  Component,
  EventEmitter,
  Input,
  Output,
  TestBed,
  __decorate,
  init_core,
  init_testing,
  init_tslib_es6
} from "./chunk-5RTIE2IS.js";

// src/app/components/new-chat-session/new-chat-session.component.spec.ts
init_testing();

// src/app/components/new-chat-session/new-chat-session.component.ts
init_tslib_es6();

// angular:jit:template:src/app/components/new-chat-session/new-chat-session.component.html
var new_chat_session_component_default = '<div class="modal-body">\n    <form (ngSubmit)="createSession()">\n        <div class="mb-3">\n            <app-model-selector (modelSelected)="onModelSelected($event)"></app-model-selector>\n        </div>\n        <div class="mb-3">\n            <label for="chatName" class="form-label">Chat Name</label>\n            <input type="text" id="chatName" class="form-control" [(ngModel)]="chatName" name="chatName" required />\n        </div>\n        <div *ngIf="errorMessage" class="text-danger small mb-2">{{ errorMessage }}</div>\n        <button type="submit" class="btn btn-primary" [disabled]="isLoading || !chatName.trim()">Create</button>\n    </form>\n</div>';

// angular:jit:style:src/app/components/new-chat-session/new-chat-session.component.css
var new_chat_session_component_default2 = "/* src/app/components/new-chat-session/new-chat-session.component.css */\n.modal-body {\n  padding: 2rem;\n}\n.form-label {\n  font-weight: 500;\n}\n.btn-primary {\n  min-width: 100px;\n}\n/*# sourceMappingURL=new-chat-session.component.css.map */\n";

// src/app/components/new-chat-session/new-chat-session.component.ts
init_core();
var NewChatSessionComponent = class NewChatSessionComponent2 {
  chatSessionService;
  userId;
  sessionCreated = new EventEmitter();
  chatName = "";
  isLoading = false;
  errorMessage = "";
  selectedModel = null;
  constructor(chatSessionService) {
    this.chatSessionService = chatSessionService;
  }
  createSession() {
    if (!this.chatName.trim() || !this.userId || !this.selectedModel)
      return;
    this.isLoading = true;
    this.errorMessage = "";
    this.chatSessionService.createSession(this.userId, this.chatName, this.selectedModel.id).subscribe({
      next: (session) => {
        this.sessionCreated.emit(session);
        this.chatName = "";
        this.isLoading = false;
      },
      error: (err) => {
        this.errorMessage = "Failed to create chat session.";
        this.isLoading = false;
      }
    });
  }
  onModelSelected(model) {
    this.selectedModel = model;
  }
  static ctorParameters = () => [
    { type: ChatSessionService }
  ];
  static propDecorators = {
    userId: [{ type: Input }],
    sessionCreated: [{ type: Output }]
  };
};
NewChatSessionComponent = __decorate([
  Component({
    selector: "app-new-chat-session",
    template: new_chat_session_component_default,
    standalone: false,
    styles: [new_chat_session_component_default2]
  })
], NewChatSessionComponent);

// src/app/components/new-chat-session/new-chat-session.component.spec.ts
describe("NewChatSessionComponent", () => {
  let component;
  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [NewChatSessionComponent]
    });
    const fixture = TestBed.createComponent(NewChatSessionComponent);
    component = fixture.componentInstance;
  });
  it("should create", () => {
    expect(component).toBeTruthy();
  });
});
//# sourceMappingURL=spec-app-components-new-chat-session-new-chat-session.component.spec.js.map
