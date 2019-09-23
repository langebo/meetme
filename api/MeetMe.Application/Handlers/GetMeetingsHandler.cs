using System.Collections.Generic;
using System.Linq;
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
    public class GetMeetingsHandler : IRequestHandler<GetMeetingsQuery, List<Meeting>>
    {
        private readonly MeetingsContext db;
        private readonly IAuthenticationService auth;

        public GetMeetingsHandler(MeetingsContext db, IAuthenticationService auth)
        {
            this.db = db;
            this.auth = auth;
        }

        public async Task<List<Meeting>> Handle(GetMeetingsQuery request, CancellationToken cancellationToken)
        {
            var userOidc = auth.GetUserIdentifier();
            var user = await db.Users
                .AsNoTracking()
                .SingleOrDefaultAsync(u => u.OidcIdentifier == userOidc, cancellationToken);

            if (user == null)
                throw new NotFoundException($"Unable to find user with {userOidc}");

            return await db.Meetings
                .AsNoTracking()
                .Include(m => m.Creator)
                .Include(m => m.Proposals)
                .Include(m => m.Invitations)
                .ThenInclude(i => i.User)
                .Include(m => m.Invitations)
                .ThenInclude(i => i.Votes)
                .Where(m => m.Creator == user || m.Invitations.Any(i => i.User == user))
                .ToListAsync(cancellationToken);
        }
    }
}
