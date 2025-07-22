import {
  HttpClient,
  init_http
} from "./chunk-MFLGNGE5.js";
import {
  Injectable,
  __decorate,
  init_core,
  init_tslib_es6
} from "./chunk-5RTIE2IS.js";

// src/app/services/chat-session.service.ts
init_tslib_es6();
init_core();
init_http();
var ChatSessionService = class ChatSessionService2 {
  http;
  apiUrl = "https://localhost:7154/api";
  constructor(http) {
    this.http = http;
  }
  createSession(userId, name, modelId) {
    return this.http.post(`${this.apiUrl}/ChatSession/create`, { "name": name, "userId": userId, "modelId": modelId });
  }
  getSessions(userId) {
    return this.http.get(`${this.apiUrl}/ChatSession/list?userId=${encodeURIComponent(userId)}`);
  }
  static ctorParameters = () => [
    { type: HttpClient }
  ];
};
ChatSessionService = __decorate([
  Injectable({
    providedIn: "root"
  })
], ChatSessionService);

export {
  ChatSessionService
};
//# sourceMappingURL=chunk-DPJCSV3K.js.map
