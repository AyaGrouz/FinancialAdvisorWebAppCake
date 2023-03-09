using FinancialAdvisorWebApp.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialAdvisorWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RiskController : Controller
    {
        private readonly ApplicationContext _context;
        public RiskController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: api/Risk
        [EnableCors("AllowOrigin")]
        [HttpGet("{id_invest}")]
        public int GetRisk([FromRoute] string id_invest)
        {
            int weightSomme = 0;
            IGrouping<int, QUESTIONNAIRE> questionnaire = _context.Questionnaires
                .Where(x => x.ID_INVEST.Equals(id_invest)).OrderByDescending(x => x.VERSION)
                .AsEnumerable()
                .GroupBy(keySelector: x => x.VERSION).FirstOrDefault();
            foreach (var i in questionnaire)
            {
                var weightresult = _context.Choices.Find(i.ID_CHOIX).WEIGHT;
                weightSomme = weightSomme + weightresult;
            }
            return (weightSomme);
        }

        [EnableCors("AllowOrigin")]
        [HttpPut("{id_invest}")]
        public void PutRisk([FromBody] float risk, [FromRoute] string id_invest)
        {

            var entity = _context.Investisseurs.Where(x => x.ID_INVEST.Equals(id_invest)).ToList();
            foreach (var i in entity)
            {
                i.RISK = risk;
            }

            _context.SaveChanges();

        }
    }
}
