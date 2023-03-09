using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinancialAdvisorWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static FinancialAdvisorWebApp.Models.ApplicationContext;

namespace FinancialAdvisorWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChoixController : Controller
    {
        private readonly ApplicationContext _context;
        public ChoixController(ApplicationContext context)
        {
            _context = context;
        }

        // Get : api/Choix
        [HttpGet]
        public IEnumerable<CHOICE> GetChoices()
        {
            return _context.Choices;
        }

        //Get : api/Choix/5
        [HttpGet("{id}")]
        public IActionResult GetChoice([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var Choix = _context.Choices.Where(i => i.ID_CHOIX == id).Select(i=> new { i.WEIGHT });

            if (Choix == null)
            {
                return NotFound();
            }

            return Ok(Choix);

        }

    }
}
