using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialAdvisorWebApp.Models
{
    public class QUESTION
    {
        [Key]
        public int ID_QUESTION { get; set; }
        public string QUEST { get; set; }
        public string CODE_QUESTION { get; set; }
        public ICollection<CHOICE> CHOICES { get; set; }
    }
}
