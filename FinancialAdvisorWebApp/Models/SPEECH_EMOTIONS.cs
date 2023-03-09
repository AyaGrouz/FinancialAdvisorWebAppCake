using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialAdvisorWebApp.Models
{
    public class SPEECH_EMOTIONS
    {
        [Key]
        public int ID_SPEECH_EMOTION { get; set; }
        public string ID_INVEST { get; set; }
        public int VERSION { get; set; }
        public string emotion { get; set; }
    }
}
