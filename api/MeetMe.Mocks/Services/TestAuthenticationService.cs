using MeetMe.Authentication.Interfaces;

namespace MeetMe.Mocks.Services
{
    public class TestAuthenticationService : IAuthenticationService
    {
        public string GetUserIdentifier()
        {
            return "boris";
        }
    }
}
