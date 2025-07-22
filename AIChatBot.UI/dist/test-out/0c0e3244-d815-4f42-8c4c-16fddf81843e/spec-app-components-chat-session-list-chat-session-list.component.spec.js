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

// src/app/components/chat-session-list/chat-session-list.component.spec.ts
init_testing();

// src/app/components/chat-session-list/chat-session-list.component.ts
init_tslib_es6();

// angular:jit:template:src/app/components/chat-session-list/chat-session-list.component.html
var chat_session_list_component_default = '<div class="chat-session-list card shadow-sm">\n    <div class="card-header bg-primary text-white d-flex justify-content-between align-items-center">\n        <h5 class="mb-0">Sessions</h5>\n        <button class="btn btn-success btn-sm" (click)="onNewChatClick()">+ New Chat</button>\n    </div>\n    <ul class="list-group list-group-flush">\n        <li *ngFor="let session of sessions" class="list-group-item d-flex align-items-center"\n            (click)="onSessionClick(session)">\n            <span class="fw-bold me-2">\u{1F5E8}\uFE0F</span>\n            <span>{{ session.name }}</span>\n        </li>\n        <li *ngIf="sessions.length === 0" class="list-group-item text-muted text-center">\n            No sessions found.\n        </li>\n    </ul>\n</div>';

// angular:jit:style:src/app/components/chat-session-list/chat-session-list.component.css
var chat_session_list_component_default2 = "/* src/app/components/chat-session-list/chat-session-list.component.css */\n.chat-session-list {\n  max-width: 350px;\n  margin: 1rem auto;\n}\n.card-header {\n  background: #0d6efd;\n  color: #fff;\n}\n.list-group-item {\n  font-size: 1.05em;\n  cursor: pointer;\n  transition: background 0.2s;\n}\n.list-group-item:hover {\n  background: #f1f3f5;\n}\n/*# sourceMappingURL=chat-session-list.component.css.map */\n";

// src/app/components/chat-session-list/chat-session-list.component.ts
init_core();
var ChatSessionListComponent = class ChatSessionListComponent2 {
  sessions = [];
  sessionSelected = new EventEmitter();
  newChat = new EventEmitter();
  onSessionClick(session) {
    this.sessionSelected.emit(session);
  }
  onNewChatClick() {
    this.newChat.emit();
  }
  static propDecorators = {
    sessions: [{ type: Input }],
    sessionSelected: [{ type: Output }],
    newChat: [{ type: Output }]
  };
};
ChatSessionListComponent = __decorate([
  Component({
    selector: "app-chat-session-list",
    template: chat_session_list_component_default,
    standalone: false,
    styles: [chat_session_list_component_default2]
  })
], ChatSessionListComponent);

// src/app/components/chat-session-list/chat-session-list.component.spec.ts
describe("ChatSessionListComponent", () => {
  let component;
  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ChatSessionListComponent]
    });
    const fixture = TestBed.createComponent(ChatSessionListComponent);
    component = fixture.componentInstance;
  });
  it("should create", () => {
    expect(component).toBeTruthy();
  });
});
//# sourceMappingURL=spec-app-components-chat-session-list-chat-session-list.component.spec.js.map
