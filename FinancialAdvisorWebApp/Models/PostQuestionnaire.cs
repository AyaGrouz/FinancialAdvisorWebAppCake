using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialAdvisorWebApp.Models
{
    public class PostQuestionnaire
    {
        public string Id_invest { get; set; }
        public int Version { get; set; }
        public List<Choicess> answer { get; set; }
        public string RoomSid { get; set; }

    }

    public class Choicess
    {
        public string Choice { get; set; }
        public int Id_choice { get; set; }
    }

   
}
