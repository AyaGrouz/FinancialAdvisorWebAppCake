using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialAdvisorWebApp.Models
{
    public class ModelQuestionnaire
    {
      public List<Questionss> ListQuestions { get; set; }
    }

    public class Questionss
    { 
        public string Code_question { get; set; }
        public string question { get; set; }
        public List<ChoiceItem> choiceList { get; set; }
    }

    public class ChoiceItem
    {
        public string Code_choice{ get; set; }
        public string Choice{ get; set; }
        public int Weight{ get; set; }
        public bool Ischecked { get; set; }

    }
}
