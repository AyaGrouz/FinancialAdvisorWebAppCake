using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialAdvisorWebApp.Models
{
    public class QUESTIONNAIRE
    {
        [Key]
        public int ID_QUESTIONNAIRE { get; set; }
        public string ID_INVEST { get; set; }
        public int VERSION { get; set; }
        public string CODE_CHOIX { get; set; }
        public int ID_CHOIX { get; set; }
        public CHOICE CHOICE { get; set; }

        public ICollection<QUEST_INVEST> Quest_Invest { get; set; }
    }
}
