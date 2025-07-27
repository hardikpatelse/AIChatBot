using AIChatBot.API.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace AIChatBot.API.Data
{
    public class ChatBotDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<ChatSession> ChatSessions { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<AIModel> AIModels { get; set; }
        public DbSet<ChatMode> ChatModes { get; set; }
        public DbSet<AIModelChatMode> AIModelChatModes { get; set; }
        public DbSet<AgentFile> AgentFiles { get; set; }

        public ChatBotDbContext(DbContextOptions<ChatBotDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ChatSession>()
                .HasMany(c => c.Messages)
                .WithOne()
                .HasForeignKey(m => m.ChatSessionId);

            modelBuilder.Entity<User>()
                .HasMany(u => u.ChatSessions)
                .WithOne()
                .HasForeignKey(c => c.UserId);

            modelBuilder.Entity<AgentFile>()
                .HasOne(af => af.User)
                .WithMany()
                .HasForeignKey(af => af.UserId);

            modelBuilder.Entity<AgentFile>()
                .HasOne(af => af.ChatSession)
                .WithMany()
                .HasForeignKey(af => af.ChatSessionId);

            modelBuilder.Entity<AIModelChatMode>()
                .HasKey(am => new { am.AIModelId, am.ChatModeId });

            modelBuilder.Entity<AIModelChatMode>()
                .HasOne(am => am.AIModel)
                .WithMany(m => m.ChatModes)
                .HasForeignKey(am => am.AIModelId);

            modelBuilder.Entity<AIModelChatMode>()
                .HasOne(am => am.ChatMode)
                .WithMany() // Remove navigation property to AIModels
                .HasForeignKey(am => am.ChatModeId);

            modelBuilder.Entity<ChatMode>().HasData(
                new ChatMode { Id = 1, Mode = "chat", Name = "Chat" },
                new ChatMode { Id = 2, Mode = "tools", Name = "Tools" },
                new ChatMode { Id = 3, Mode = "agent", Name = "Agent" },
                new ChatMode { Id = 4, Mode = "planner", Name = "Agent with Planning" },
                new ChatMode { Id = 5, Mode = "rag", Name = "Knowledge-Based (RAG)" }
            );

            // Seed AIModels
            modelBuilder.Entity<AIModel>().HasData(
                new AIModel { Id = 1, ModelName = "phi3", Name = "Phi-3 Mini", Company = "Microsoft", LogoUrl = "assets/images/model-icons/phi.png", Description = "Phi-3 is a family of lightweight 3B (Mini) and 14B (Medium) state-of-the-art open models by Microsoft.", ReferralSource = "Ollama", ReferenceLink = "https://ollama.com/library/phi3:latest" },
                new AIModel { Id = 2, ModelName = "llama3", Name = "LLaMA 3", Company = "Meta", LogoUrl = "assets/images/model-icons/llama.png", Description = "Meta Llama 3: The most capable openly available LLM to date.", ReferralSource = "Ollama", ReferenceLink = "https://ollama.com/library/llama3:latest" },
                new AIModel { Id = 3, ModelName = "mistral", Name = "Mistral 7B", Company = "Mistral AI", LogoUrl = "assets/images/model-icons/mistralai.png", Description = "The 7B model released by Mistral AI, updated to version 0.3.", ReferralSource = "Ollama", ReferenceLink = "https://ollama.com/library/mistral:latest" },
                new AIModel { Id = 4, ModelName = "gemma:2b", Name = "Gemma", Company = "Google", LogoUrl = "assets/images/model-icons/gemma.png", Description = "Gemma is a family of lightweight, state-of-the-art open models built by Google DeepMind. Updated to version 1.1.", ReferralSource = "Ollama", ReferenceLink = "https://ollama.com/library/gemma:2b" },
                new AIModel { Id = 5, ModelName = "deepseek/deepseek-chat-v3-0324:free", Name = "DeepSeek v3", Company = "DeepSeek", LogoUrl = "assets/images/model-icons/deepseek.png", Description = "DeepSeek V3, a 685B-parameter, mixture-of-experts model, is the latest iteration of the flagship chat model family from the DeepSeek team.\nIt succeeds the DeepSeek V3 model and performs really well on a variety of tasks.", ReferralSource = "OpenRouter", ReferenceLink = "https://openrouter.ai/deepseek/deepseek-chat-v3-0324:free" },
                new AIModel { Id = 6, ModelName = "google/gemma-3-27b-it:free", Name = "Gemma 3 27B", Company = "Google", LogoUrl = "assets/images/model-icons/gemma.png", Description = "Gemma 3 introduces multimodality, supporting vision-language input and text outputs. It handles context windows up to 128k tokens, understands over 140 languages, and offers improved math, reasoning, and chat capabilities, including structured outputs and function calling. Gemma 3 27B is Google's latest open source model, successor to Gemma 2.", ReferralSource = "OpenRouter", ReferenceLink = "https://openrouter.ai/google/gemma-3-27b-it:free" },
                new AIModel { Id = 7, ModelName = "google/gemini-2.0-flash-exp:free", Name = "Gemini Flash 2.0 - Limited", Company = "Google", LogoUrl = "assets/images/model-icons/gemini.png", Description = "Gemini Flash 2.0 offers a significantly faster time to first token (TTFT) compared to Gemini Flash 1.5, while maintaining quality on par with larger models like Gemini Pro 1.5. It introduces notable enhancements in multimodal understanding, coding capabilities, complex instruction following, and function calling. These advancements come together to deliver more seamless and robust agentic experiences.", ReferralSource = "OpenRouter", ReferenceLink = "https://openrouter.ai/google/gemini-2.0-flash-exp:free" },
                new AIModel { Id = 8, ModelName = "openai/gpt-3.5-turbo-0613", Name = "GPT-3.5 Turbo", Company = "OpenAI", LogoUrl = "assets/images/model-icons/chatgpt.png", Description = "GPT-3.5 Turbo is OpenAI's fastest model. It can understand and generate natural language or code, and is optimized for chat and traditional completion tasks.", ReferralSource = "OpenRouter", ReferenceLink = "https://openrouter.ai/openai/gpt-3.5-turbo-0613" },
                new AIModel { Id = 9, ModelName = "google/gemini-2.0-flash-001", Name = "Gemini Flash 2.0 - Unlimited", Company = "Google", LogoUrl = "assets/images/model-icons/gemini.png", Description = "Gemini Flash 2.0 offers a significantly faster time to first token (TTFT) compared to Gemini Flash 1.5, while maintaining quality on par with larger models like Gemini Pro 1.5. It introduces notable enhancements in multimodal understanding, coding capabilities, complex instruction following, and function calling. These advancements come together to deliver more seamless and robust agentic experiences.", ReferralSource = "OpenRouter", ReferenceLink = "https://openrouter.ai/google/gemini-2.0-flash-001" }
            );

            // Seed AIModelChatModes (model-mode relationships)
            modelBuilder.Entity<AIModelChatMode>().HasData(
                // Phi-3 Mini
                new AIModelChatMode { AIModelId = 1, ChatModeId = 1 },
                new AIModelChatMode { AIModelId = 1, ChatModeId = 2 },
                // LLaMA 3
                new AIModelChatMode { AIModelId = 2, ChatModeId = 1 },
                new AIModelChatMode { AIModelId = 2, ChatModeId = 2 },
                // Mistral 7B
                new AIModelChatMode { AIModelId = 3, ChatModeId = 1 },
                new AIModelChatMode { AIModelId = 3, ChatModeId = 2 },
                // Gemma
                new AIModelChatMode { AIModelId = 4, ChatModeId = 1 },
                new AIModelChatMode { AIModelId = 4, ChatModeId = 2 },
                // DeepSeek v3
                new AIModelChatMode { AIModelId = 5, ChatModeId = 1 },
                new AIModelChatMode { AIModelId = 5, ChatModeId = 2 },
                // Gemma 3 27B
                new AIModelChatMode { AIModelId = 6, ChatModeId = 1 },
                new AIModelChatMode { AIModelId = 6, ChatModeId = 2 },
                // Gemini Flash 2.0 - Limited
                new AIModelChatMode { AIModelId = 7, ChatModeId = 1 },
                new AIModelChatMode { AIModelId = 7, ChatModeId = 2 },
                // GPT-3.5 Turbo
                new AIModelChatMode { AIModelId = 8, ChatModeId = 1 },
                new AIModelChatMode { AIModelId = 8, ChatModeId = 2 },
                new AIModelChatMode { AIModelId = 8, ChatModeId = 3 },
                new AIModelChatMode { AIModelId = 8, ChatModeId = 4 },
                // Gemini Flash 2.0 - Unlimited
                new AIModelChatMode { AIModelId = 9, ChatModeId = 1 },
                new AIModelChatMode { AIModelId = 9, ChatModeId = 2 },
                new AIModelChatMode { AIModelId = 9, ChatModeId = 3 },
                new AIModelChatMode { AIModelId = 9, ChatModeId = 4 }
            );
        }
    }
}
