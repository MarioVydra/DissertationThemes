using DissertationThemes.WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace DissertationThemes.WebApi.Controllers
{
    [ApiController]
    [Route("/stprograms")]
    public class StProgramController : ControllerBase
    {
        private readonly StProgramService programService;
        
        public StProgramController(StProgramService programService)
        {
            this.programService = programService;
        }

        [HttpGet]
        public async Task<IActionResult> GetStPrograms()
        {
            try
            {
                var programs = await this.programService.GetStprograms();
                return Ok(programs);
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
