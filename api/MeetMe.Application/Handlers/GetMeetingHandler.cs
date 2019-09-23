using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            var userOidc = auth.GetUserIdentifier();
            var user = await db.Users
                .AsNoTracking()
                .SingleOrDefaultAsync(u => u.OidcIdentifier == userOidc, cancellationToken);

            if (user == null)
                throw new NotFoundException($"Unable to find user {userOidc}");

            var meeting = await db.Meetings
                .AsNoTracking()
                .Include(m => m.Creator)
                .Include(m => m.Proposals)
                .Include(m => m.Invitations)
                .ThenInclude(i => i.User)
                .Include(m => m.Invitations)
                .ThenInclude(i => i.Votes)
                .FirstOrDefaultAsync(m => m.Id == request.Id, cancellationToken);

            if (meeting == null)
                throw new NotFoundException($"Unable to find meeting {request.Id}");

            if (meeting.Creator.Id != user.Id && meeting.Invitations.All(i => i.User.Id != user.Id))
                throw new ForbiddenException($"User {user.OidcIdentifier} is no eligible to view meeting {request.Id}");

            return meeting;
        }
    }
}
