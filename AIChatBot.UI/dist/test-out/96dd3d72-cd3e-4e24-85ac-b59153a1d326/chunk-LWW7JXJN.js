import {
  HttpClient,
  init_http
} from "./chunk-MFLGNGE5.js";
import {
  Injectable,
  __decorate,
  __esm,
  init_core,
  init_tslib_es6
} from "./chunk-5RTIE2IS.js";

// src/app/services/chat.service.ts
var ChatService;
var init_chat_service = __esm({
  "src/app/services/chat.service.ts"() {
    "use strict";
    init_tslib_es6();
    init_http();
    init_core();
    ChatService = class ChatService2 {
      http;
      apiUrl = "https://localhost:7154";
      constructor(http) {
        this.http = http;
      }
      getModels() {
        return this.http.get(`${this.apiUrl}/models`);
      }
      getHistory(modelId) {
        return this.http.get(`${this.apiUrl}/chat/history?modelId=${modelId}`);
      }
      sendMessage(userId, chatSessionIdentity, modelId, message, aIMode) {
        return this.http.post(`${this.apiUrl}/chat`, { userId, chatSessionIdentity, modelId, message, aIMode });
      }
      static ctorParameters = () => [
        { type: HttpClient }
      ];
    };
    ChatService = __decorate([
      Injectable({
        providedIn: "root"
      })
    ], ChatService);
  }
});

export {
  ChatService,
  init_chat_service
};
//# sourceMappingURL=chunk-LWW7JXJN.js.map
