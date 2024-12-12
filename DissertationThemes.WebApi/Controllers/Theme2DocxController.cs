using DissertationThemes.WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace DissertationThemes.WebApi.Controllers
{
    [ApiController]
    [Route("/theme2docx")]
    public class Theme2DocxController : ControllerBase
    {
        private readonly ThemeService themeService;

        public Theme2DocxController(ThemeService themeService)
        {
            this.themeService = themeService;
        }

        /**
         * https://stackoverflow.com/questions/77563033/how-to-download-a-word-docx-file-that-is-stored-in-sql-from-api-in-c-sharp
         */
        [HttpGet("{id}")]
        public async Task<IActionResult> GetThemeAsDocx(int id)
        {
            try
            {
                var (fileContent, fileName) = await themeService.GenerateDocxforTheme(id);
                return File(fileContent, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", fileName);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
