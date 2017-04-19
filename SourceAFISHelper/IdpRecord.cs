using SourceAFIS.Simple;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceAFISHelper
{
    [Serializable]
    public class IdpRecord
    {
        public string ID { get; set; }

        public string DoB { get; set; }
        public string FirstName { get; set; }
        public string Gender { get; set; }
        public string LastName { get; set; }
        public string LGA { get; set; }
        public string MaritalStatus { get; set; }
        public string OtherNames { get; set; }
        public string Photo { get; set; }
        public string State { get; set; }
        public string YoB { get; set; }

        public string[] Fingers { get; set; } = new string[10];
    }
}
