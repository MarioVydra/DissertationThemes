
using CsvHelper;
using DissertationThemes.ImporterApp;
using DissertationThemes.SharedLibrary;
using DissertationThemes.WebApi.Dtos;
using Microsoft.EntityFrameworkCore;
using Spire.Doc;
using System.Globalization;


namespace DissertationThemes.WebApi.Services
{
    public class ThemeService
    {
        private readonly DissertationThemesContext context;
        private readonly string templatePath;

        public ThemeService(DissertationThemesContext context, IConfiguration configuration)
        {
            this.context = context;
            string? configuredPath = configuration["TemplatePaths:DefaultTemplate"];
            if (string.IsNullOrEmpty(configuredPath))
            {
                this.templatePath = "PhD_temy_sablona.docx";
            }
            else
            {
                this.templatePath = configuredPath;
            }
        }

        public async Task<ThemeDto> GetThemeById(int id)
        {
            var theme = await this.context.Themes
                .Include(t => t.Supervisor)
                .Include(t => t.StProgram)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (theme == null)
            {
                throw new ArgumentException($"Theme with ID {id} does not exists.");
            }

            return new ThemeDto
            {
                Id = theme.Id,
                Name = theme.Name,
                Supervisor = theme.Supervisor.FullName,
                StProgramId = theme.StProgram.Id,
                IsFullTimeStudy = theme.IsFullTimeStudy,
                IsExternalStudy = theme.IsExternalStudy,
                ResearchType = theme.ResearchType.ToString(),
                Description = theme.Description,
                Created = theme.Created
            };

        }

        public async Task<List<int>> GetUniqueYears()
        {
            var years = await this.context.Themes
               .Select(t => t.Created.Year)
               .Distinct()
               .OrderByDescending(y => y)
               .ToListAsync();

            if (years == null || !years.Any())
            {
                throw new InvalidOperationException("No Year records found.");
            }

            return years;
        }

        public async Task<List<ThemeDto>> GetThemes(int? year, int? stProgramId)
        {
            var themes = await this.ApplyFilters(year, stProgramId);

            var themeDtos = themes.Select(t => new ThemeDto
            {
                Id = t.Id,
                Name = t.Name,
                Supervisor = t.Supervisor.FullName,
                StProgramId = t.StProgram.Id,
                IsFullTimeStudy = t.IsFullTimeStudy,
                IsExternalStudy = t.IsExternalStudy,
                ResearchType = t.ResearchType.ToString(),
                Description = t.Description,
                Created = t.Created
            }).ToList();

            return themeDtos;
        }

        public async Task<byte[]> GetThemesCsv(int? year, int? stProgramId)
        {
            var themes = await this.ApplyFilters(year, stProgramId);

            using (var memoryStream = new MemoryStream())
            using (var writer = new StreamWriter(memoryStream))
            {
                var config = new CsvHelper.Configuration.CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    Delimiter = ";",
                    NewLine = "\r\n"
                };

                using (var csv = new CsvWriter(writer, config))
                {
                    csv.WriteHeader<ThemeCsvRecord>();
                    csv.NextRecord();

                    foreach (var theme in themes)
                    {
                        var themeRecord = new ThemeCsvRecord
                        {
                            Name = theme.Name,
                            Supervisor = theme.Supervisor.FullName,
                            StProgram = theme.StProgram.Name,
                            FieldOfStudy = theme.StProgram.FieldOfStudy,
                            IsFullTimeStudy = theme.IsFullTimeStudy ? "TRUE" : "FALSE",
                            IsExternalStudy = theme.IsExternalStudy ? "TRUE" : "FALSE",
                            ResearchType = ResearchTypeConverter.ToSlovakString(theme.ResearchType),
                            Description = theme.Description.Replace(Environment.NewLine, "<br>"),
                            Created = theme.Created.ToString("d.M.yyyy HH:mm"),
                        };

                        csv.WriteRecord(themeRecord);
                        csv.NextRecord();
                    }
                }
                return memoryStream.ToArray();
            }

        }

        /**
         * https://www.e-iceblue.com/Tutorials/Spire.Doc/Spire.Doc-Program-Guide/Spire.Doc-Program-Guide-Content.html?gad_source=1&gclid=Cj0KCQiA0--6BhCBARIsADYqyL8THqvvCafhiBubG5J5BUcRfNdFElwpQ403O_UNq_u0b-MjYRL-ROwaAp3wEALw_wcB
         */
        public async Task<(byte[] fileContent, string fileName)> GenerateDocxforTheme(int id)
        {
            var theme = await context.Themes
                .Include(t => t.Supervisor)
                .Include(t => t.StProgram)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (theme == null)
            {
                throw new ArgumentException($"Theme with ID {id} does not exists.");
            }

            string themeName = theme.Name;
            string supervisor = theme.Supervisor.FullName;
            string studyProgram = theme.StProgram.Name;
            string fieldOfStudy = theme.StProgram.FieldOfStudy;
            string researchType = ResearchTypeConverter.ToSlovakString(theme.ResearchType);
            string description = theme.Description;

            Document doc = new Document();
            doc.LoadFromFile(templatePath);

            doc.Replace("#=ThemeName=#", themeName, true, true);
            doc.Replace("#=Supervisor=#", supervisor, true, true);
            doc.Replace("#=StProgram=#", studyProgram, true, true);
            doc.Replace("#=FieldOfStudy=#", fieldOfStudy, true, true);
            doc.Replace("#=ResearchType=#", researchType, true, true);
            doc.Replace("#=Description=#", description, true, true);

            using (var memoryStream = new MemoryStream())
            {
                doc.SaveToStream(memoryStream, FileFormat.Docx);
                byte[] fileContent = memoryStream.ToArray();
                string fileName = $"Dissertation_Theme_{id}.docx";
                return (fileContent, fileName);
            }
        }

        private async Task<List<Theme>> ApplyFilters(int? year, int? stProgramId)
        {
            var query = this.context.Themes.AsQueryable();

            if (year.HasValue)
            {
                query = query.Where(t => t.Created.Year == year.Value);
            }

            if (stProgramId.HasValue)
            {
                query = query.Where(t => t.StProgram.Id == stProgramId.Value);
            }

            var themes = await query
                .Include(t => t.Supervisor)
                .Include(t => t.StProgram)
                .ToListAsync();

            if (themes == null || !themes.Any())
            {
                throw new InvalidOperationException("No Theme records found.");
            }

            return themes;
        }
    }
}
