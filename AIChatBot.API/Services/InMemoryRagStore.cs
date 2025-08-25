using AIChatBot.API.Interfaces.Services;
using System.Text;

namespace AIChatBot.API.Services
{
    public class InMemoryRagStore : IRagStore
    {
        private readonly Dictionary<string, Dictionary<string, List<string>>> _userDocuments;
        private readonly object _lock = new object();

        public InMemoryRagStore()
        {
            _userDocuments = new Dictionary<string, Dictionary<string, List<string>>>();
        }

        public Task IndexAsync(string userId, string docId, string content)
        {
            return Task.Run(() =>
            {
                lock (_lock)
                {
                    if (!_userDocuments.ContainsKey(userId))
                    {
                        _userDocuments[userId] = new Dictionary<string, List<string>>();
                    }

                    var chunks = ChunkContent(content);
                    _userDocuments[userId][docId] = chunks;
                }
            });
        }

        public Task<List<string>> SearchAsync(string userId, string query, int topK = 3)
        {
            return Task.Run(() =>
            {
                lock (_lock)
                {
                    var results = new List<(string chunk, double score)>();

                    if (_userDocuments.ContainsKey(userId))
                    {
                        foreach (var (docId, chunks) in _userDocuments[userId])
                        {
                            foreach (var chunk in chunks)
                            {
                                var score = CalculateSimpleSimilarity(query, chunk);
                                if (score > 0.1) // Basic threshold
                                {
                                    results.Add((chunk, score));
                                }
                            }
                        }
                    }

                    return results
                        .OrderByDescending(x => x.score)
                        .Take(topK)
                        .Select(x => x.chunk)
                        .ToList();
                }
            });
        }

        public Task<List<string>> GetDocumentIdsAsync(string userId)
        {
            return Task.Run(() =>
            {
                lock (_lock)
                {
                    if (_userDocuments.ContainsKey(userId))
                    {
                        return _userDocuments[userId].Keys.ToList();
                    }
                    return new List<string>();
                }
            });
        }

        public Task<bool> DeleteDocumentAsync(string userId, string docId)
        {
            return Task.Run(() =>
            {
                lock (_lock)
                {
                    if (_userDocuments.ContainsKey(userId) && _userDocuments[userId].ContainsKey(docId))
                    {
                        _userDocuments[userId].Remove(docId);
                        return true;
                    }
                    return false;
                }
            });
        }

        private List<string> ChunkContent(string content)
        {
            const int chunkSize = 500; // Characters per chunk
            const int overlap = 50;   // Character overlap between chunks

            var chunks = new List<string>();
            var lines = content.Split('\n');
            var currentChunk = new StringBuilder();
            
            foreach (var line in lines)
            {
                if (currentChunk.Length + line.Length > chunkSize && currentChunk.Length > 0)
                {
                    chunks.Add(currentChunk.ToString().Trim());
                    
                    // Start new chunk with some overlap
                    var overlapText = currentChunk.ToString();
                    if (overlapText.Length > overlap)
                    {
                        overlapText = overlapText.Substring(overlapText.Length - overlap);
                    }
                    currentChunk = new StringBuilder(overlapText);
                }
                
                currentChunk.AppendLine(line);
            }
            
            if (currentChunk.Length > 0)
            {
                chunks.Add(currentChunk.ToString().Trim());
            }

            return chunks.Count > 0 ? chunks : new List<string> { content };
        }

        private double CalculateSimpleSimilarity(string query, string text)
        {
            // Simple keyword-based similarity
            var queryWords = query.ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var textWords = text.ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            
            var matches = queryWords.Count(qw => textWords.Any(tw => tw.Contains(qw) || qw.Contains(tw)));
            return (double)matches / queryWords.Length;
        }
    }
}