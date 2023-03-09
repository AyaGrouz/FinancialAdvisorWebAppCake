using System.Threading.Tasks;
using FinancialAdvisorWebApp.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using IronPython.Hosting;
using System.IO;
using System.Text;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;
using FinancialAdvisorWebApp.Models;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace FinancialAdvisorWebApp.Controllers
{
    [
        ApiController,
        Route("api/video")
    ]
    public class VideoController : ControllerBase
    {
        readonly IVideoService _videoService;
        readonly ApplicationContext _context;
        public VideoController(ApplicationContext context, IVideoService videoService)
        {
            _context = context;
            _videoService = videoService;
        }

        [HttpGet("token")]
        public IActionResult GetToken()
            => new JsonResult(new { token = _videoService.GetTwilioJwt(User.Identity.Name) });

        [HttpGet("rooms")]
        public async Task<IActionResult> GetRooms()
            => new JsonResult(await _videoService.GetAllRoomsAsync());

        [HttpGet("lastRoom")]
        public IActionResult GetLastCreatedRoom()
            => new JsonResult(new { lastRoomName = _videoService.GetLastCreatedRoom() });

        // Put: api/video
        [EnableCors("AllowOrigin")]
        [HttpPost]
        public IActionResult CompleteRoom([FromBody] PostQuestionnaire Quest)
        {
            _videoService.completeRoom(Quest.RoomSid);

            string results = ExecProcess(Quest.RoomSid, @"\ML\EmotionalRecognition\emotionalRecognition.py");
            System.Diagnostics.Debug.WriteLine(DeleteLines(results, 3));
            var obj = JObject.Parse(DeleteLines(results, 3));
            FACIAL_EMOTIONS result = new FACIAL_EMOTIONS();
            result.ID_INVEST = Quest.Id_invest;
            var versions = _context.Facial_Emotions.Where(x => x.ID_INVEST == Quest.Id_invest).ToList();

            var a = 0;
            foreach (var i in versions)
            {
                if (versions == null)
                {
                    result.VERSION = 0;
                }

                if (versions != null)
                {
                    a = i.VERSION;
                }

            }
            result.VERSION = a + 1;
            result.angry = obj.SelectToken("average").SelectToken("angry").Value<float>();
            result.disgust = obj.SelectToken("average").SelectToken("disgust").Value<float>();
            result.scared = obj.SelectToken("average").SelectToken("scared").Value<float>();
            result.happy = obj.SelectToken("average").SelectToken("happy").Value<float>();
            result.sad = obj.SelectToken("average").SelectToken("sad").Value<float>();
            result.surprised = obj.SelectToken("average").SelectToken("surprised").Value<float>();
            result.neutral = obj.SelectToken("average").SelectToken("neutral").Value<float>();
            result.deviation = obj.SelectToken("deviation").Value<float>();

            _context.Add(result);

            var nb = 0;
            SPEECH_EMOTIONS resultspeech = new SPEECH_EMOTIONS();
            resultspeech.ID_INVEST = Quest.Id_invest;
            var versionsSpeech = _context.Speech_Emotions.Where(x => x.ID_INVEST == Quest.Id_invest).ToList();
            foreach (var i in versionsSpeech)
            {
                if (versionsSpeech == null)
                {
                    resultspeech.VERSION = 0;
                }

                if (versionsSpeech != null)
                {
                    nb = i.VERSION;
                }

            }
            resultspeech.VERSION = nb + 1;
            resultspeech.emotion = obj.SelectToken("speechRecognition").Value<string>();
            _context.Add(resultspeech);

            _context.SaveChanges();
            return Ok();
        }

        // GET: api/video/emotions
        [EnableCors("AllowOrigin")]
        [HttpGet("emotions/{id_invest}")]
        public IActionResult GetFacialEmotions([FromRoute] string id_invest)
        {
            bool exist = false;
            List<FACIAL_EMOTIONS> faceEmotions = _context.Facial_Emotions.ToList();
            foreach (var item in faceEmotions)
            {
                if (item.ID_INVEST.Equals(id_invest))
                {
                    exist = true;
                    break;
                }
            }
            if (exist)
            {
                IGrouping<int, FACIAL_EMOTIONS> emotions = _context.Facial_Emotions
                    .Where(x => x.ID_INVEST.Equals(id_invest)).OrderByDescending(x => x.VERSION)
                    .AsEnumerable()
                    .GroupBy(x => x.VERSION).FirstOrDefault();
                //if (emotions.FirstOrDefault() != null)
                return Ok(emotions.FirstOrDefault());
            }
            else
                return Ok(null);

        }

        // GET: api/video/speechemotions
        [EnableCors("AllowOrigin")]
        [HttpGet("speechemotions/{id_invest}")]
        public IActionResult GetSpeechEmotions([FromRoute] string id_invest)
        {
            bool exist = false;
            List<SPEECH_EMOTIONS> speechEmotions = _context.Speech_Emotions.ToList();
            foreach (var item in speechEmotions)
            {
                if (item.ID_INVEST.Equals(id_invest))
                {
                    exist = true;
                    break;
                }
            }
            if (exist)
            {
                IGrouping<int, SPEECH_EMOTIONS> emotions = _context.Speech_Emotions
                .Where(x => x.ID_INVEST.Equals(id_invest)).OrderByDescending(x => x.VERSION)
                .AsEnumerable()
                .GroupBy(x => x.VERSION).FirstOrDefault();

                return Ok(emotions.FirstOrDefault());
            }
            else
                return Ok(null);
        }


        static string ExecProcess(string sidActiveRoom, string path)
        {
            // 1) Create Process Info
            var psi = new ProcessStartInfo();
            psi.FileName = @"C:\Users\DELL\AppData\Local\Programs\Python\Python38\python.exe";

            // 2) Provide script and arguments
            var script = Environment.CurrentDirectory + path;

            psi.Arguments = $"\"{script}\" \"{sidActiveRoom}\"";

            // 3) Process configuration
            psi.UseShellExecute = false;
            psi.CreateNoWindow = false;
            psi.WindowStyle = ProcessWindowStyle.Hidden;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;

            // 4) Execute process and get output
            var errors = "";
            var results = "";

            using (var process = Process.Start(psi))
            {
                errors = process.StandardError.ReadToEnd();
                results = process.StandardOutput.ReadToEnd();
            }

            // 5) Display output
            System.Diagnostics.Debug.WriteLine("ERRORS:");
            System.Diagnostics.Debug.WriteLine(errors);
            System.Diagnostics.Debug.WriteLine("Results:");
            System.Diagnostics.Debug.WriteLine(results);
            return results;

        }
        private static string DeleteLines(string input, int lines)
        {
            var result = input;
            for (var i = 0; i < lines; i++)
            {
                var idx = result.IndexOf('\n');
                if (idx < 0)
                {
                    // do what you want when there are less than the required lines
                    return string.Empty;
                }
                result = result.Substring(idx + 1);
            }
            return result;
        }

    }
}