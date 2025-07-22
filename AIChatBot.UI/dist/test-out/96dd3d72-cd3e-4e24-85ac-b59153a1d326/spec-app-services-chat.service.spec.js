import {
  ChatService,
  init_chat_service
} from "./chunk-LWW7JXJN.js";
import "./chunk-MFLGNGE5.js";
import "./chunk-I3YRUNTE.js";
import {
  TestBed,
  init_testing
} from "./chunk-5RTIE2IS.js";

// src/app/services/chat.service.spec.ts
init_testing();
init_chat_service();
describe("Chat", () => {
  let service;
  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ChatService);
  });
  it("should be created", () => {
    expect(service).toBeTruthy();
  });
});
//# sourceMappingURL=spec-app-services-chat.service.spec.js.map
