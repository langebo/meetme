using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MeetMe.Application.Exceptions;
using MeetMe.Application.Queries;
using MeetMe.Authentication.Interfaces;
using MeetMe.Domain.Contexts;
using MeetMe.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace MeetMe.Application.Handlers
{
    public class GetMeHandler : IRequestHandler<GetMeQuery, User>
    {
        private readonly MeetingsContext db;
        private readonly IAuthenticationService auth;

        public GetMeHandler(MeetingsContext db, IAuthenticationService auth)
        {
            this.db = db;
            this.auth = auth;
        }

        public async Task<User> Handle(GetMeQuery request, CancellationToken cancellationToken)
        {
            var userOidc = auth.GetUserIdentifier();
            var user = await db.Users
                .AsNoTracking()
                .SingleOrDefaultAsync(u => u.OidcIdentifier == userOidc, cancellationToken);

            if (user == null)
                throw new NotFoundException($"Unable to find user {userOidc}");

            return user;
        }
    }
}
