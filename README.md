# 🤖 AIChatBot

AIChatBot is a local and online AI-powered chatbot built using open-source language models. This project supports both locally hosted models (via [Ollama](https://ollama.com)) and cloud-based models via [OpenRouter](https://openrouter.ai). It demonstrates the integration of AI with a .NET 9 API and Angular 20 frontend.

---

## 🛠️ Prerequisites

To run this project locally, ensure the following:

- Windows 10/11 with WSL support
- Installed Ubuntu 20.04.6 LTS (via Microsoft Store)
- [.NET 9 SDK](https://dotnet.microsoft.com/en-us/download)
- [Node.js (v20+)](https://nodejs.org/)
- [Angular CLI](https://angular.io/cli) (`npm install -g @angular/cli`)
- [Ollama](https://ollama.com/download) installed in Ubuntu for running local AI models
- Optional: Account on [https://openrouter.ai](https://openrouter.ai)

---

## 🧱 Environment Setup (Local Models using Ollama)

### 1. Install Ubuntu 20.04.6 LTS

- Go to Microsoft Store → Search for **Ubuntu 20.04.6 LTS** → Install.
- Open Ubuntu and create your UNIX user account.

### 2. Install Ollama inside Ubuntu

```bash
curl -fsSL https://ollama.com/install.sh | sh
```

### 3. Pull and Run Models

To pull and run the desired models:

```bash
# Pull models
ollama pull phi3:latest
ollama pull mistral:latest
ollama pull gemma:2b
ollama pull llama3:latest

# Run models
ollama run phi3
ollama run mistral
ollama run gemma:2b
ollama run llama3
```

### 4. Manage Models

```bash
# List all pulled models
ollama list

# Stop a running model
ollama stop phi3

# View running models
ps aux | grep ollama
```

### 5. Shutdown Ubuntu

From Ubuntu terminal:

```bash
shutdown now
```

Or simply close the terminal window if you don’t need a full shutdown.

---

## ☁️ Online Models via OpenRouter

1. Go to [https://openrouter.ai](https://openrouter.ai) and **sign up**.

2. Navigate to **API Keys** in your profile and **generate an API key**.

3. Set this key as an environment variable in your API project:

   ```bash
   export OPENROUTER_API_KEY=your_key_here
   ```

4. Models used:

   - `google/gemma-3-27b-it:free`
   - `deepseek/deepseek-chat-v3-0324:free`

API requests are routed via OpenRouter using this key, supporting seamless AI chat.

---

## 🤖 AIChatBot Integration

### 🔧 Backend (API - .NET 9)

1. Navigate to `AIChatBot.API/`
2. Run the following commands:

```bash
dotnet restore
dotnet build
dotnet run
```

- Ensure `appsettings.json` file includes:

```env
ApiKey=YOUR_KEY_HERE
```

### 💬 Frontend (UI - Angular 20)

1. Navigate to `AIChatBot.UI/`
2. Run:

```bash
npm install
ng serve
```

- Access the chatbot UI at `http://localhost:4200/`

---

## 🧪 Model & Environment Summary

| Model                          | Type   | Source        | Access       |
| ------------------------------ | ------ | ------------- | ------------ |
| PHI-3\:latest                  | Local  | Ollama        | `ollama run` |
| Mistral\:latest                | Local  | Ollama        | `ollama run` |
| Gemma:2b                       | Local  | Ollama        | `ollama run` |
| Llama3\:latest                 | Local  | Ollama        | `ollama run` |
| google/gemma-3-27b-it\:free    | Online | OpenRouter.ai | API Key      |
| deepseek/deepseek-chat-v3-0324 | Online | OpenRouter.ai | API Key      |

---

## 📂 Project Structure

```
AIChatBot/
│
├── AIChatBot.API/        # .NET 9 API for chatbot
├── AIChatBot.UI/         # Angular 20 UI frontend
└── README.md             # Project documentation
```

---

## 🚀 Get Started

1. Choose your preferred model type (local or online).
2. Start the backend using `.NET 9`
3. Start the frontend using Angular CLI
4. Interact with AIChatBot at `http://localhost:4200/`

---

## 💖 Contributing

Pull requests and suggestions are welcome! Feel free to fork the repo and enhance it.

---

## 📄 License

This project is open-source and available under the [MIT License](LICENSE).

