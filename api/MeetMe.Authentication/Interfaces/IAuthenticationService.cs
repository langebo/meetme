using System.Security.Claims;

namespace MeetMe.Authentication.Interfaces
{
    public interface IAuthenticationService
    {
        string GetUserIdentifier();
        string GetUserEmail();
    }
}