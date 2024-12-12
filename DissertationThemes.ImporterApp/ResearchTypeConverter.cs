using DissertationThemes.SharedLibrary;

namespace DissertationThemes.ImporterApp
{
    public static class ResearchTypeConverter
    {
        public static ResearchType FromSlovakString(string value)
        {
            switch (value) 
            {
                case "základný výskum":
                    return ResearchType.BasicResearch;
                case "aplikovaný výskum":
                    return ResearchType.AppliedResearch;
                case "aplikovaný výskum a experimentálny vývoj":
                    return ResearchType.AppliedResearchExpDevelopment;
                default:
                    throw new ArgumentException($"Invalid research type: {value}");
            }
        }

        public static string ToSlovakString(ResearchType value)
        {
            switch (value)
            {
                case ResearchType.BasicResearch:
                    return "základný výskum";
                case ResearchType.AppliedResearch:
                    return "aplikovaný výskum";
                case ResearchType.AppliedResearchExpDevelopment:
                    return "aplikovaný výskum a experimentálny vývoj";
                default:
                    throw new ArgumentException($"Invalid research type: {value}");
            }
        }
    }
}
