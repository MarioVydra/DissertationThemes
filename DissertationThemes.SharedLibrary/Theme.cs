namespace DissertationThemes.SharedLibrary
{
    public class Theme
    {
        public DateTime Created { get; set; }
        public string Description { get; set; }
        public int Id { get; set; }
        public bool IsExternalStudy { get; set; }
        public bool IsFullTimeStudy { get; set; }
        public string Name { get; set; }
        public ResearchType ResearchType { get; set; }
        public StProgram StProgram { get; set; }
        public int StProgramId { get; set; }
        public Supervisor Supervisor { get; set; }
        public int SupervisorId { get; set; }
    }
}
