using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task1Retake
{
    [Serializable]
    public class Module
    {
       
        public string ModuleCode { get; set; }
        public string ModuleName { get; set; }
        public int NumOfCredits { get; set; }
        public int ClassHours { get; set; }
        public int SelfStudy { get; set; }
    }
}
