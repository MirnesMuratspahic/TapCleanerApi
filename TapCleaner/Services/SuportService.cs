using TapCleaner.Context;
using TapCleaner.Models;
using OpenAI_API;
using TapCleaner.Services.Interfaces;

namespace TapCleaner.Services
{
    public class SuportService:ISuportService
    {
        public ApplicationDbContext DbContext { get; set; }
        public IConfiguration configuration { get; set; }
        public ErrorProvider error = new ErrorProvider() { Status = false };
        public ErrorProvider defaultError = new ErrorProvider() { Status = true, Name = "Property must not be null" };
        public string EmailClaim = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress";
        readonly string apiKey = "";

        public SuportService() { }
        public SuportService(ApplicationDbContext context, IConfiguration _configuration)
        {
            DbContext = context;
            configuration = _configuration;
        }

        public async Task<(ErrorProvider, string)> GetClosestContainer(string coordinates)
        {
            if(coordinates == null || coordinates.Length == 0)
            {
                return (defaultError, null);
            }

            var authentication = new APIAuthentication(apiKey);
            var api = new OpenAIAPI(authentication);
            var conversation = api.Chat.CreateConversation();

            var allCoordinates = DbContext.Containers.Select(c => c.Coordinates).ToList();

            string input = "Tell me which coordinate from all this coordinates (" + string.Join("; ", allCoordinates) + ") is the closest one to this coordinate (" + string.Join("; ", allCoordinates) + ").Search on google maps and thes you must return me one coordinate from that bracket, in this format -> 43.356115570686555, 17.67999879831259";
            conversation.AppendUserInput(input);

            var response = await conversation.GetResponseFromChatbot();

            return (error, response);

        }
    }
}
