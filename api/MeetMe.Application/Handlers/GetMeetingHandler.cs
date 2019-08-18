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
    public class GetMeetingHandler : IRequestHandler<GetMeetingQuery, Meeting>
    {
        private readonly MeetingsContext db;
        private readonly IAuthenticationService auth;
        public GetMeetingHandler(MeetingsContext db, IAuthenticationService auth)
        {
            this.db = db;
            this.auth = auth;
        }

        public async Task<Meeting> Handle(GetMeetingQuery request, CancellationToken cancellationToken)
        {
            var meeting = await db.Meetings
                .Include(m => m.Creator)
                .Include(m => m.Proposals)
                .ThenInclude(p => p.Votes)
                .ThenInclude(v => v.User)
                .Include(m => m.Invitations)
                .ThenInclude(i => i.User)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == request.Id);

            if (meeting == null)
                throw new NotFoundException($"Meeting {request.Id} does not exist");

            var oidcId = auth.GetUserIdentifier();
            if (meeting.Creator.OidcIdentifier != oidcId || 
                !meeting.Invitations.Select(i => i.User.OidcIdentifier).Contains(oidcId))
                throw new ForbiddenException($"User {oidcId} is not allowed to access meeting {request.Id}");
            
            return meeting;
        }
    }
}