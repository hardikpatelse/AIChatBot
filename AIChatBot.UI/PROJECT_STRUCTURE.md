# AIChatBot.UI Project Structure

## Root
- angular.json, package.json, tsconfig.*.json, README.md, etc.
- public/
- src/

## src/
- main.ts
- index.html
- styles.css

### src/app/
- app.config.ts, app.css, app.html, app.ts, app.spec.ts
- app.routes.ts
- assets/
- components/
- entities/
- services/

#### src/app/components/
- chat/
- chat-history/
- chat-mode-selector/
- chat-session-list/
- header/
- model-details/
- model-selector/
- new-chat-session/
- typing-indicator/
- user-form/

Each feature folder contains:
- *.component.ts, *.component.html, *.component.css, *.module.ts, *.component.spec.ts

#### src/app/entities/
- aimodel-chatmode.ts
- chat-history.ts
- chatmode.ts
- chatsession.ts
- model.ts
- user.ts

#### src/app/services/
- chat-session.service.ts
- chat-session.service.spec.ts
- chat.service.ts
- chat.service.spec.ts
- signalr.service.ts
- signalr.service.spec.ts
- user.service.ts
- user.service.spec.ts

### src/environments/
- environment.ts

---

## Routing
- Defined in app.routes.ts
- Lazy loading for feature modules
- Default and wildcard routes redirect to chat

---

## Example Feature Module Structure
```
chat-session-list/
  chat-session-list.component.ts
  chat-session-list.component.html
  chat-session-list.component.css
  chat-session-list.module.ts
  chat-session-list.component.spec.ts
```

## Example Service Structure
```
services/
  chat.service.ts
  chat-session.service.ts
  user.service.ts
  signalr.service.ts
  chat.service.spec.ts
  ...
```

## Example Entity Structure
```
entities/
  user.ts
  chatsession.ts
  chat-history.ts
  model.ts
  ...
```

## Example Environment
```
environments/
  environment.ts
```

---

This scaffold provides a modular, maintainable Angular application structure, with clear separation of concerns for components, services, entities, and configuration.
