namespace DissertationThemes.WebApi.Dtos
{
    public class ThemeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Supervisor { get; set; }
        public int StProgramId { get; set; }
        public bool IsFullTimeStudy { get; set; }
        public bool IsExternalStudy { get; set; }
        public string ResearchType { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
    }
}
