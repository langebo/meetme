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
        public readonly IAuthenticationService auth;
        public GetMeetingsHandler(MeetingsContext db, IAuthenticationService auth)
        {
            this.db = db;
            this.auth = auth;
        }
        public async Task<List<Meeting>> Handle(GetMeetingsQuery request, CancellationToken cancellationToken)
        {
            var oidcId = auth.GetUserIdentifier();
            var user = await db.Users
                .AsNoTracking()
                .SingleOrDefaultAsync(u => u.OidcIdentifier == oidcId, cancellationToken);

            if (user == null)
                throw new NotFoundException($"User {oidcId} does not exist");

            return await db.Meetings
                .AsNoTracking()
                .Include(m => m.Creator)
                .Include(m => m.Proposals)
                .ThenInclude(p => p.Votes)
                .ThenInclude(v => v.User)
                .Include(m => m.Invitations)
                .ThenInclude(i => i.User)
                .Where(m => m.Creator.OidcIdentifier == oidcId || 
                            m.Invitations.Select(i => i.User).Contains(user))
                .ToListAsync(cancellationToken);
        }
    }
}