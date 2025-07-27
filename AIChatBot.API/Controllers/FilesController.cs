using AIChatBot.API.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace AIChatBot.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly IFileService _fileService;

        public FilesController(IFileService fileService)
        {
            _fileService = fileService;
        }
        [HttpGet("{sessionId:int}")]
        public async Task<IActionResult> GetSessionFiles(int sessionId, [FromQuery] Guid userId)
        {
            try
            {
                if (userId == Guid.Empty)
                {
                    return BadRequest("User ID is required");
                }

                var files = await _fileService.GetSessionFilesAsync(sessionId, userId);
                return Ok(files);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving files: {ex.Message}");
            }
        }

        [HttpGet("download/{fileId:int}")]
        public async Task<IActionResult> DownloadFile(int fileId)
        {
            try
            {
                var agentFile = await _fileService.GetFileAsync(fileId);
                if (agentFile == null)
                {
                    return NotFound("File not found or access denied");
                }

                var fileStream = await _fileService.GetFileStreamAsync(fileId);
                if (fileStream == null)
                {
                    return NotFound("File content not found");
                }

                return File(fileStream, "application/octet-stream", agentFile.FileName);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error downloading file: {ex.Message}");
            }
        }
    }
}
