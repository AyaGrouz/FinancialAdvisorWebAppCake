using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialAdvisorWebApp.Models
{
    public class FACIAL_EMOTIONS
    {
        [Key]
        public int ID_FACE_EMOTION { get; set; }
        public string ID_INVEST { get; set; }
        public int VERSION { get; set; }
        public float angry { get; set; }
        public float disgust { get; set; }
        public float scared { get; set; }
        public float happy { get; set; }
        public float sad { get; set; }
        public float surprised { get; set; }
        public float neutral { get; set; }
        public float deviation { get; set; }


    }
}
