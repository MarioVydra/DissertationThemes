using DissertationThemes.WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace DissertationThemes.WebApi.Controllers
{
    [ApiController]
    [Route("/theme")]
    public class ThemeController : ControllerBase
    {
        private readonly ThemeService themeService;

        public ThemeController(ThemeService themeService)
        {
            this.themeService = themeService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetThemeById(int id)
        {
            try
            {
                var theme = await this.themeService.GetThemeById(id);
                return Ok(theme);
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
