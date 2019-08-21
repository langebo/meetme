using System;
using System.Security.Claims;
using MeetMe.Authentication.Interfaces;
using Microsoft.AspNetCore.Http;

namespace MeetMe.Authentication.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IHttpContextAccessor accessor;

        public AuthenticationService(IHttpContextAccessor accessor)
        {
            this.accessor = accessor ?? throw new ArgumentNullException(nameof(accessor));
        }

        public string GetUserIdentifier()
        {
            var user = accessor.HttpContext.User;
            var oidcNameIdentifier = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if(!user.Identity.IsAuthenticated || string.IsNullOrEmpty(oidcNameIdentifier))
                throw new UnauthorizedAccessException("Unable retrieve nameIdentifier claim of user");

            return oidcNameIdentifier;
        }
    }
}