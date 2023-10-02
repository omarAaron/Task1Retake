using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task1Retake
{
    [Serializable]
      public class Semester
        {
            
            public string SemesterName { get; set; }
            public int NumofWeeks { get; set; }
            public DateTime StartDate { get; set; }
        }
}
