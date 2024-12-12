using DissertationThemes.WebApi.Dtos;
using Microsoft.EntityFrameworkCore;
using DissertationThemes.SharedLibrary;

namespace DissertationThemes.WebApi.Services
{
    public class StProgramService
    {
        private readonly DissertationThemesContext context;
        public StProgramService(DissertationThemesContext context) 
        {
            this.context = context;
        }

        public async Task<List<StProgramDto>> GetStprograms()
        { 
            var programs = await this.context.StProgram
                .OrderBy(p => p.Name)
                .Select(p => new StProgramDto
                { 
                    Id = p.Id,
                    Name = p.Name,
                    FieldOfStudy = p.FieldOfStudy
                }).ToListAsync();

            if (programs == null || !programs.Any()) 
            {
                throw new InvalidOperationException("No StProgram records found.");
            }

            return programs;
        }
    }
}
