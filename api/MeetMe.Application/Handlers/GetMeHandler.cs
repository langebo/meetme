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
        private readonly IAuthenticationService auth;
        private readonly MeetingsContext db;
        public GetMeHandler(IAuthenticationService auth, MeetingsContext db)
        {
            this.auth = auth;
            this.db = db;
        }
        public async Task<User> Handle(GetMeQuery request, CancellationToken cancellationToken)
        {
            var oidcId = auth.GetUserIdentifier();
            var me = await db.Users
                .AsNoTracking()
                .SingleOrDefaultAsync(u => u.OidcIdentifier == oidcId);

            if (me == null)
                throw new NotFoundException($"Could not find user {oidcId}");

            return me;
        }
    }
}