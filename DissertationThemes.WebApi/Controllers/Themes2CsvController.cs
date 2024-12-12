using DissertationThemes.WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace DissertationThemes.WebApi.Controllers
{
    [ApiController]
    [Route("/themes2csv")]
    public class Themes2CsvController : Controller
    {
        private readonly ThemeService themeService;

        public Themes2CsvController(ThemeService themeService)
        {
            this.themeService = themeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetThemesCsv([FromQuery] int? year, [FromQuery] int? stProgramId)
        {
            try
            {
                var csvData = await this.themeService.GetThemesCsv(year, stProgramId);
                return File(csvData, "text/csv", "themes.csv");
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
