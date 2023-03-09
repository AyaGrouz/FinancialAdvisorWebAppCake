using FinancialAdvisorWebApp.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static FinancialAdvisorWebApp.Models.ApplicationContext;

namespace FinancialAdvisorWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionnaireController : Controller
    {
        private readonly ApplicationContext _context;
        public QuestionnaireController(ApplicationContext context)
        {
            _context = context;
        }

        // Get : api/Questionnaire
        [HttpGet]
        public IEnumerable<QUESTIONNAIRE> GetQuestionnaire()
        {
            return _context.Questionnaires;
        }

        // POST: api/Questionnaire
        [EnableCors("AllowOrigin")]
        [HttpPost]
        public void PostQuestionnaires([FromBody] PostQuestionnaire Quest)
        {
            var a = 0;
            PostQuestionnaire result = new PostQuestionnaire();
            result.Id_invest = Quest.Id_invest;
            var versions = _context.Questionnaires.Where(x => x.ID_INVEST == Quest.Id_invest).ToList();
            foreach (var i in versions)
            {
                if (versions == null)
                {
                    result.Version = 0;
                }

                if (versions != null)
                {
                    a = i.VERSION;
                }

            }
            result.Version = a + 1;
            var res = _context.Choices;
            foreach (var choiceid in Quest.answer)
            {
                result.answer = new List<Choicess>();
                var x = new Choicess();
                x.Choice = choiceid.Choice;

                foreach (var j in res)
                {
                    if (x.Choice == j.CHOIX)
                    {
                        x.Id_choice = j.ID_CHOIX;
                    }
                }

                result.answer.Add(x);


                var k = new QUESTIONNAIRE();
                k.ID_INVEST = result.Id_invest;
                k.VERSION = result.Version;
                k.ID_CHOIX = x.Id_choice;
                k.CODE_CHOIX = x.Choice;

                _context.Questionnaires.Add(k);
                _context.SaveChanges();

            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuestionnaire([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var questionnaire = await _context.Questionnaires.FindAsync(id);
            if (questionnaire == null)
            {
                return NotFound();
            }

            _context.Questionnaires.Remove(questionnaire);
            await _context.SaveChangesAsync();

            return Ok(questionnaire);
        }

        //Get : api/Questionnaire/1
        [EnableCors("AllowOrigin")]
        [HttpGet("{id_invest}")]
        public List<KeyValuePair<string, string>> GetQuestionnaire([FromRoute] string id_invest)
        {
            bool exist = false;
            List<QUESTIONNAIRE> quests = _context.Questionnaires.ToList();
            foreach (var item in quests)
            {
                if (item.ID_INVEST.Equals(id_invest))
                {
                    exist = true;
                    break;
                }
            }
            if (exist)
            {
                IGrouping<int, QUESTIONNAIRE> questionnaire = _context.Questionnaires
                  .Where(x => x.ID_INVEST.Equals(id_invest)).OrderByDescending(x => x.VERSION)
                  .AsEnumerable()
                  .GroupBy(x => x.VERSION).FirstOrDefault();
                var list = new List<KeyValuePair<string, string>>();
                foreach (var i in questionnaire)
                {
                    var key = _context.Questions.Find(_context.Choices.Find(i.ID_CHOIX).ID_QUESTION).CODE_QUESTION;
                    list.Add(new KeyValuePair<string, string>(key, _context.Choices.Find(i.ID_CHOIX).CHOIX));
                }
                return (list);
            }
            else return null;
        }

        // GET: api/Questionnaire/version/id_invest
        [EnableCors("AllowOrigin")]
        [HttpGet("version/{id_invest}")]
        public IActionResult GetSpeechEmotions([FromRoute] string id_invest)
        {
            bool exist = false;
            List<QUESTIONNAIRE> quests = _context.Questionnaires.ToList();
            foreach (var item in quests)
            {
                if (item.ID_INVEST.Equals(id_invest))
                {
                    exist = true;
                    break;
                }
            }
            if (exist)
            {
                IGrouping<int, QUESTIONNAIRE> questionnaire = _context.Questionnaires
                  .Where(x => x.ID_INVEST.Equals(id_invest)).OrderByDescending(x => x.VERSION)
                  .AsEnumerable()
                  .GroupBy(x => x.VERSION).FirstOrDefault();

                return Ok(new { version = questionnaire.FirstOrDefault().VERSION });
            }
            else
                return Ok(null);
        }
    }
}
