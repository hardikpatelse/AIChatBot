using AIChatBot.API.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace AIChatBot.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DocumentsController : ControllerBase
    {
        private readonly IRagStore _ragStore;

        public DocumentsController(IRagStore ragStore)
        {
            _ragStore = ragStore;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadDocument(IFormFile file, [FromForm] string userId)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return BadRequest("No file provided or file is empty");
                }

                if (!IsValidFileType(file.FileName))
                {
                    return BadRequest("Unsupported file type. Supported types: .txt, .md, .pdf");
                }

                var content = await ExtractContentFromFile(file);
                var docId = $"{Path.GetFileNameWithoutExtension(file.FileName)}_{DateTime.UtcNow:yyyyMMdd_HHmmss}";

                await _ragStore.IndexAsync(userId, docId, content);

                return Ok(new { 
                    message = "Document uploaded and indexed successfully", 
                    documentId = docId,
                    fileName = file.FileName,
                    status = "Ready"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error processing document: {ex.Message}");
            }
        }

        [HttpGet("{userId}/documents")]
        public async Task<IActionResult> GetUserDocuments(string userId)
        {
            try
            {
                var documentIds = await _ragStore.GetDocumentIdsAsync(userId);
                return Ok(new { documents = documentIds });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving documents: {ex.Message}");
            }
        }

        [HttpDelete("{userId}/documents/{docId}")]
        public async Task<IActionResult> DeleteDocument(string userId, string docId)
        {
            try
            {
                var success = await _ragStore.DeleteDocumentAsync(userId, docId);
                if (success)
                {
                    return Ok(new { message = "Document deleted successfully" });
                }
                return NotFound("Document not found");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error deleting document: {ex.Message}");
            }
        }

        private bool IsValidFileType(string fileName)
        {
            var allowedExtensions = new[] { ".txt", ".md", ".pdf" };
            var extension = Path.GetExtension(fileName)?.ToLower();
            return allowedExtensions.Contains(extension);
        }

        private async Task<string> ExtractContentFromFile(IFormFile file)
        {
            var extension = Path.GetExtension(file.FileName)?.ToLower();
            
            using var stream = file.OpenReadStream();
            
            switch (extension)
            {
                case ".txt":
                case ".md":
                    using (var reader = new StreamReader(stream, Encoding.UTF8))
                    {
                        return await reader.ReadToEndAsync();
                    }
                
                case ".pdf":
                    // For now, return a placeholder. In production, you'd use a PDF library
                    return "PDF content extraction would require additional dependencies like iTextSharp or PdfPig.";
                
                default:
                    throw new NotSupportedException($"File type {extension} is not supported");
            }
        }
    }
}