using Microsoft.AspNetCore.Mvc;

namespace AIChatBot.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentController : ControllerBase
    {
        [HttpPost("upload")]
        public async Task<IActionResult> UploadDocument(IFormFile file)
        {
            var text = await _fileParser.ParseAsync(file);
            var chunks = _textChunker.Chunk(text);
            await _embeddingService.StoreChunksAsync(chunks);
            return Ok("Document indexed.");
        }
    }
}
