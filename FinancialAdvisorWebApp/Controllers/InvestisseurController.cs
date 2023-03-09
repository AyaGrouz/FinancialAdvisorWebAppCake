using FinancialAdvisorWebApp.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;


namespace FinancialAdvisorWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvestisseurController : Controller
    {
        private readonly ApplicationContext _context;
        public InvestisseurController(ApplicationContext context)
        {
            _context = context;
        }

        // Get : api/Investisseur
        [HttpGet]
        public IEnumerable<INVESTISSEUR> GetInvestisseur()
        {
            return _context.Investisseurs;
        }

        //Get : api/Investisseur/5
        [HttpGet("{id}")]
        public INVESTISSEUR GetInvestisseur([FromRoute] string id)
        {
            return _context.Investisseurs.Find(id);

        }

        // Put: api/Investisseur
        [EnableCors("AllowOrigin")]
        [HttpPut("{id_invest}/{risk}")]
        public INVESTISSEUR PutInvestisseur([FromRoute] string id_invest, int risk)
        {
            var entity = _context.Investisseurs.Find(id_invest);
            entity.RISK = risk;
            _context.SaveChanges();
            return entity;
        }

        // Post: api/Investisseur
        [EnableCors("AllowOrigin")]
        [HttpPost()]
        public INVESTISSEUR AddInvestisseur([FromBody] INVESTISSEUR investisseur)
        {
            var entity = _context.Investisseurs.Find(investisseur.ID_INVEST);
            if (entity == null)
            {
                entity = new INVESTISSEUR();
                entity.ID_INVEST = investisseur.ID_INVEST;
                entity.NAME = investisseur.NAME;
                entity.LASTNAME = investisseur.LASTNAME;
                entity.RISK = investisseur.RISK;
                entity.CODE = investisseur.CODE;
                entity.Quest_Invest = null;
                _context.Add(entity);
                _context.SaveChanges();
            }
            return entity;
        }
    }
}


