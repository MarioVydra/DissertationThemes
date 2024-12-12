using DissertationThemes.WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace DissertationThemes.WebApi.Controllers
{
    [ApiController]
    [Route("/themes")]
    public class ThemesWithParametersController : ControllerBase
    {
        private readonly ThemeService themeService;

        public ThemesWithParametersController(ThemeService themeService)
        {
            this.themeService = themeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetThemes([FromQuery] int? year, [FromQuery] int? stProgramId)
        {
            try
            {
                var theme = await this.themeService.GetThemes(year, stProgramId);
                return Ok(theme);
            }
            catch (InvalidOperationException ex)
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
