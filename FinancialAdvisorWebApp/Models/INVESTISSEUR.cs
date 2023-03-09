using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialAdvisorWebApp.Models
{
    public class INVESTISSEUR
    {
        [Key]
        public string ID_INVEST { get; set; }
        public string NAME { get; set; }
        public string LASTNAME { get; set; }
        public float RISK { get; set; }
        public string CODE { get; set; }
        public ICollection<QUEST_INVEST> Quest_Invest { get; set; }
    }
}
