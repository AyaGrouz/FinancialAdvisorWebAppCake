using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialAdvisorWebApp.Models
{
    public class CHOICE
    {
        [Key]
        public int ID_CHOIX { get; set; }
        public int WEIGHT { get; set; }
        public string CHOIX { get; set; }
        public int ID_QUESTION { get; set; }
        public QUESTION QUESTION { get; set; }
    }
}
