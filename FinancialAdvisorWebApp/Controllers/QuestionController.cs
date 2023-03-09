using FinancialAdvisorWebApp.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static FinancialAdvisorWebApp.Models.ApplicationContext;

namespace FinancialAdvisorWebApp.Controllers
{
    [Route("api/")]
    [ApiController]
    public class QuestionController : Controller
    {
        private readonly ApplicationContext _context;
        public QuestionController(ApplicationContext context)
        {
            _context = context;
        }

        // Get : api/Question
        [HttpGet]
        public IEnumerable<QUESTION> GetQuestion()
        {
            return _context.Questions;
        }
        // Get : api/Question/investId
        [Route("GetQuestionList")]
        [HttpGet("[action]/{investId}")]
        [EnableCors("AllowOrigin")]
        public ModelQuestionnaire GetQuestionList([FromRoute] string investId)
        {
            ModelQuestionnaire result = new ModelQuestionnaire() { ListQuestions = new List<Questionss>() };

            //var choiceLists = _context.Questionnaires.Where(x => x.ID_INVEST == investId);
            var questionList = _context.Questions;
            var choicesList = _context.Choices;
            foreach (var item in questionList)
            {
                //System.Diagnostics.Debug.WriteLine(item.QUEST+"****************************");
                Questionss question = new Questionss();
                question.question = item.QUEST;
                question.Code_question = item.CODE_QUESTION;
                question.choiceList = new List<ChoiceItem>();
                var choices = choicesList.Where(y => y.ID_QUESTION == item.ID_QUESTION).ToList();
                var result_checked = _context.Questionnaires;
                var check = result_checked.Where(r => r.VERSION == result_checked.Max(m => m.VERSION)).ToList();
                foreach (var choiceItem in choices)
                {
                    var x = new ChoiceItem();

                    x.Code_choice = choiceItem.CHOIX;
                    x.Choice = choiceItem.CHOIX;
                    x.Weight = choiceItem.WEIGHT;
                    foreach (var i in check)
                    {
                        if (choiceItem.ID_CHOIX == i.ID_CHOIX)
                        {
                            x.Ischecked = true;
                        }
                    }
                    question.choiceList.Add(x);
                }
                result.ListQuestions.Add(question);
            }
            return result;
        }

        //Get : api/Question/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetQuestionAsync([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var Quest = await _context.Questions.FindAsync(id);

            if (Quest == null)
            {
                return NotFound();
            }

            return Ok(Quest);

        }

    }
}
