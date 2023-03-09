using System.Collections.Generic;
using System.Threading.Tasks;
using FinancialAdvisorWebApp.Models;
using Twilio.Base;
using Twilio.Rest.Video.V1;

namespace FinancialAdvisorWebApp.Services
{
    public interface IVideoService
    {
        string GetTwilioJwt(string identity);
        Task<IEnumerable<RoomDetails>> GetAllRoomsAsync();
        string GetLastCreatedRoom();

        string completeRoom(string roomSid);
    }
}