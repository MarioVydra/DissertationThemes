using System.Collections.Generic;

namespace DissertationThemes.SharedLibrary
{
    public class Supervisor
    {
        public string FullName { get; set; }
        public int Id {  get; set; }
        public List<Theme> Themes { get; set; }
    }
}
