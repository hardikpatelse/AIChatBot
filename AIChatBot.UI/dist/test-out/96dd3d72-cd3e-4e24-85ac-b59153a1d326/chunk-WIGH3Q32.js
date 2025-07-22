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

// src/app/services/user.service.ts
init_tslib_es6();
init_core();
init_http();
var UserService = class UserService2 {
  http;
  apiUrl = "https://localhost:7154/api";
  constructor(http) {
    this.http = http;
  }
  getUserByEmail(email) {
    return this.http.get(`${this.apiUrl}/user/by-email?email=${encodeURIComponent(email)}`);
  }
  registerUser(name, email) {
    return this.http.post(`${this.apiUrl}/user/register`, { "name": name, "email": email });
  }
  static ctorParameters = () => [
    { type: HttpClient }
  ];
};
UserService = __decorate([
  Injectable({
    providedIn: "root"
  })
], UserService);

export {
  UserService
};
//# sourceMappingURL=chunk-WIGH3Q32.js.map
