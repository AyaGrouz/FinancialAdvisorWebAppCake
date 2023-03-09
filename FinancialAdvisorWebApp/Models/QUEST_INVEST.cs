using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialAdvisorWebApp.Models
{
    public class QUEST_INVEST
    {
        public string ID_INVEST { get; set; }
        public INVESTISSEUR INVESTISSEUR { get; set; }
        public int ID_QUESTIONNAIRE { get; set; }
        public QUESTIONNAIRE QUESTIONNAIRE { get; set; }
    }
}
