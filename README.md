# 🤖 AIChatBot

AIChatBot is a local and online AI-powered chatbot built using open-source language models. This project supports both locally hosted models (via [Ollama](https://ollama.com)) and cloud-based models via [OpenRouter](https://openrouter.ai). It demonstrates the integration of AI with a .NET 8 API and Angular 20 frontend.

## ✨ Latest Features

### 🆕 RAG (Retrieval-Augmented Generation) Integration
Transform your chatbot experience with **Knowledge-Based AI**! Upload your documents and get contextually-aware responses based on your own content.

**Key Highlights:**
- 📄 **Multi-format Document Support**: Upload `.txt`, `.md`, and `.pdf` files
- 🔍 **Intelligent Document Search**: Advanced retrieval algorithms find relevant content
- 💬 **Context-Aware Responses**: AI answers based on your uploaded documents
- 📊 **Source Attribution**: See exactly which documents informed each response  
- 🗂️ **Document Management**: Easy upload, view, and delete capabilities
- 👤 **User-Specific Collections**: Each user maintains their own document library

Experience AI that truly understands YOUR content!

### 🔥 Firestore Database Connector
Seamlessly integrate with Google Cloud Firestore for scalable data storage! The new Firestore connector provides a complete alternative to Entity Framework Core.

**Key Features:**
- 🚀 **Complete CRUD Operations**: Full support for all data entities
- 🔄 **Interface Abstraction**: Drop-in replacement for Entity Framework implementations
- 📄 **Entity Mapping**: Automatic conversion between EF Core entities and Firestore documents  
- ⚙️ **Flexible Configuration**: Support for production, emulator, and local development
- 🛡️ **Error Handling**: Comprehensive logging and exception handling
- ✅ **Unit Tested**: Full test coverage for all components

Switch between SQL Server and Firestore with a simple configuration change!

---

## 🎬 Demo Video

Watch the AIChatBot in action on YouTube:  
📺 [AIChatBot Demo](https://youtu.be/ReTiWZABiD8)

---


## 🛠️ Prerequisites

To run this project locally, ensure the following:

- Windows 10/11 with WSL support
- Installed Ubuntu 20.04.6 LTS (via Microsoft Store)
- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download)
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

### 🔧 Backend (API - .NET 8)

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

## 📚 RAG (Retrieval-Augmented Generation) Features

The AIChatBot includes advanced RAG capabilities that allow AI models to answer questions based on your uploaded documents. This feature significantly enhances the AI's ability to provide contextually relevant and accurate responses.

### 🏗️ RAG Architecture

The RAG system consists of several key components:

1. **Document Processing Pipeline**:
   - File upload handling for multiple formats
   - Text extraction from PDF, TXT, and MD files
   - Content chunking for efficient retrieval
   - In-memory indexing with similarity search

2. **Retrieval System**:
   - Semantic search across document chunks
   - Top-K retrieval (configurable, default: 3 chunks)
   - Relevance scoring and ranking
   - Source attribution and metadata tracking

3. **Generation Enhancement**:
   - Context-aware prompt construction
   - Integration with all supported AI models
   - Source citation in responses
   - Fallback to general knowledge when needed

### 🎛️ RAG Configuration

The RAG system supports various AI models with different levels of effectiveness:

| Model Category | Models | RAG Performance |
|---------------|--------|-----------------|
| **Best RAG Support** | GPT-3.5 Turbo<br>Gemini Flash 2.0 (Unlimited)<br>Gemini Flash 2.0 (Limited) | ⭐⭐⭐⭐⭐ |
| **Good RAG Support** | DeepSeek v3<br>Gemma 3 27B<br>LLaMA 3 | ⭐⭐⭐⭐ |
| **Basic RAG Support** | PHI-3<br>Mistral 7B | ⭐⭐⭐ |

### 🚀 Using RAG Features

1. **Upload Documents**:
   ```bash
   # Supported formats
   - Plain text files (.txt)
   - Markdown files (.md) 
   - PDF documents (.pdf)
   ```

2. **Document Management**:
   - View all uploaded documents
   - Delete individual documents
   - User-specific document collections
   - Automatic indexing upon upload

3. **RAG-Enhanced Chat**:
   - Select "Knowledge-Based (RAG)" mode
   - Ask questions about your uploaded content
   - Receive responses with source attribution
   - Contextual answers based on document content

### 💾 Document Storage

Currently, the RAG system uses in-memory storage (`InMemoryRagStore`), which provides:
- Fast retrieval performance
- Simple deployment setup
- Automatic cleanup on application restart
- User-isolated document collections

*Note: For production deployments, consider implementing persistent storage solutions.*

---

## 🧪 Model & Environment Summary

| Model                          | Type   | Source        | Access       | RAG Support |
| ------------------------------ | ------ | ------------- | ------------ | ----------- |
| PHI-3\:latest                  | Local  | Ollama        | `ollama run` | ⭐⭐⭐       |
| Mistral\:latest                | Local  | Ollama        | `ollama run` | ⭐⭐⭐       |
| Gemma:2b                       | Local  | Ollama        | `ollama run` | ⭐⭐⭐       |
| Llama3\:latest                 | Local  | Ollama        | `ollama run` | ⭐⭐⭐⭐     |
| google/gemma-3-27b-it\:free    | Online | OpenRouter.ai | API Key      | ⭐⭐⭐⭐     |
| deepseek/deepseek-chat-v3-0324 | Online | OpenRouter.ai | API Key      | ⭐⭐⭐⭐     |
| google/gemini-2.0-flash-exp    | Online | OpenRouter.ai | API Key      | ⭐⭐⭐⭐⭐   |
| openai/gpt-3.5-turbo-0613      | Online | OpenRouter.ai | API Key      | ⭐⭐⭐⭐⭐   |
| google/gemini-2.0-flash-001    | Online | OpenRouter.ai | API Key      | ⭐⭐⭐⭐⭐   |

---

## 📂 Project Structure

```
AIChatBot/
│
├── AIChatBot.API/           # .NET 8 API for chatbot
│   ├── Controllers/         # API endpoints
│   │   └── DocumentsController.cs    # RAG document upload/management
│   ├── Services/           # Business logic services
│   │   ├── RagChatService.cs         # RAG-enabled chat functionality
│   │   ├── InMemoryRagStore.cs       # Document indexing and search
│   │   ├── AgentService.cs           # AI tool integration
│   │   └── ChatService.cs            # Standard chat functionality
│   ├── Interfaces/         # Service contracts
│   │   └── IRagStore.cs              # RAG storage interface
│   └── Migrations/         # Database schema updates for RAG
├── AIChatBot.UI/           # Angular 20 UI frontend
│   └── src/app/components/
│       ├── document-upload/          # RAG document upload component
│       ├── chat/                     # Main chat interface
│       └── model-selector/           # AI model and mode selection
└── README.md               # Project documentation
```

---
## 🧠 AI Tools & Agent Integration

The AIChatBot supports three advanced operation modes beyond simple chat:

### 1. 🛠️ Tool-Enabled AI
In this mode, the AI can recognize specific tasks in user prompts and use internal tools (functions) to perform actions. Integrated tools include:

| Tool Function    | Description                                  | Example Prompt                                                   |
|------------------|----------------------------------------------|------------------------------------------------------------------|
| `CreateFile`     | Creates a text file with given content       | "Create a file called `report.txt` with the text `Hello world`." |
| `FetchWebData`   | Fetches the HTML/content of a public URL     | "Fetch the content of https://example.com"                       |
| `SendEmail`      | Simulates sending an email (console-logged)  | "Send an email to john@example.com with subject `Hello`."        |

These functions are executed server-side in `.NET`, with input parsed from natural language prompts.

### 2. 📚 Knowledge-Based (RAG) Mode
**NEW FEATURE:** The RAG (Retrieval-Augmented Generation) mode enables AI to answer questions based on your uploaded documents. This powerful feature allows you to:

#### 🎯 Key Capabilities:
- **Upload Documents**: Support for `.txt`, `.md`, and `.pdf` files
- **Intelligent Retrieval**: Automatically finds relevant content from your documents
- **Source Attribution**: AI responses include references to source documents
- **Document Management**: View, organize, and delete uploaded documents
- **User-Specific Storage**: Each user has their own document collection

#### 📄 Supported File Formats:
- **Text Files**: `.txt` - Plain text documents
- **Markdown Files**: `.md` - Formatted markdown documents  
- **PDF Documents**: `.pdf` - Portable document format

#### 🚀 How to Use RAG Mode:
1. **Upload Documents**: Use the drag-and-drop interface or browse to upload documents
2. **Select RAG Mode**: Choose "Knowledge-Based (RAG)" from the chat mode dropdown
3. **Ask Questions**: Query your documents using natural language
4. **Get Contextual Answers**: Receive AI responses enriched with your document content

#### 💡 Example RAG Interaction:
```
User: "What are the key findings in the latest market report?"
AI: Based on the provided context from "market_analysis_2025.pdf", the key findings include:
1. Consumer spending increased by 15% in Q4
2. Digital transformation investments rose by 23%
3. Supply chain disruptions decreased significantly

*Sources: 1 document(s) referenced*
```

### 3. 🤖 AI Agent Mode (Planning + Action)
The AI agent is capable of:
- Understanding high-level tasks
- Selecting and invoking appropriate tools
- Providing intelligent responses based on the outcome

This is powered by an `AgentService` that works with both **local LLMs** (via Ollama) and **cloud models** (via OpenRouter) to determine the right function to execute and handle the response.

You can toggle between AI modes via the UI:
- **Chat-Only Mode**
- **AI + Tools Mode**
- **Knowledge-Based (RAG) Mode**
- **Agent Mode** (multi-step planning, coming soon)

---

## 🚀 Get Started

### Quick Start Guide

1. **Choose your preferred model type** (local or online)
2. **Start the backend** using `.NET 8`
3. **Start the frontend** using Angular CLI  
4. **Access AIChatBot** at `http://localhost:4200/`

### Using RAG Features (NEW!)

5. **Upload Documents**: 
   - Click the "Documents for RAG" section in the sidebar
   - Drag & drop or browse to upload `.txt`, `.md`, or `.pdf` files
   - Wait for successful upload confirmation

6. **Enable Knowledge-Based Mode**:
   - Select "Knowledge-Based (RAG)" from the chat mode dropdown
   - Choose an AI model with good RAG support (⭐⭐⭐⭐ or ⭐⭐⭐⭐⭐)

7. **Start Asking Questions**:
   ```
   Examples:
   "Summarize the key points from my uploaded documents"
   "What does the report say about market trends?"
   "Find information about [specific topic] in my files"
   ```

8. **Review Source Attribution**:
   - AI responses will include "*Sources: X document(s) referenced*"
   - Responses are enriched with content from your uploaded documents

---

## 💖 Contributing

Pull requests and suggestions are welcome! Feel free to fork the repo and enhance it.

---

## 📄 License

This project is open-source and available under the [MIT License](LICENSE).

