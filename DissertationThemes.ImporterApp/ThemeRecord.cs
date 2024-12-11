using DissertationThemes.SharedLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DissertationThemes.ImporterApp
{
    public class ThemeRecord
    {
        public string Name { get; set; }
        public string Supervisor { get; set; }
        public string StProgram { get; set; }
        public string FieldOfStudy {  get; set; }
        public bool IsFullTimeStudy { get; set; }
        public bool IsExternalStudy {  get; set; }
        public string ResearchType { get; set; }
        public string Descrition {  get; set; }
        public string Created {  get; set; }
    }
}
