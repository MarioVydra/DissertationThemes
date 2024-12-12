using DissertationThemes.WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace DissertationThemes.WebApi.Controllers
{
    [ApiController]
    [Route("/themesyears")]
    public class ThemesYearsController : Controller
    {
        private readonly ThemeService themeService;

        public ThemesYearsController(ThemeService themeService)
        {
            this.themeService = themeService;
        }

        [HttpGet]
        public async Task<ActionResult<List<int>>> GetUniqueYears()
        {
            try
            {
                var years = await this.themeService.GetUniqueYears();
                return Ok(years);
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
