using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MeetMe.Application.Commands;
using MeetMe.Application.Exceptions;
using MeetMe.Authentication.Interfaces;
using MeetMe.Domain.Contexts;
using MeetMe.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace MeetMe.Application.Handlers
{
    public class InviteHandler : IRequestHandler<InviteCommand, Meeting>
    {
        private readonly MeetingsContext db;
        private readonly IAuthenticationService auth;

        public InviteHandler(MeetingsContext db, IAuthenticationService auth)
        {
            this.db = db;
            this.auth = auth;
        }

        public async Task<Meeting> Handle(InviteCommand request, CancellationToken cancellationToken)
        {
            var userOidc = auth.GetUserIdentifier();
            var user = await db.Users
                .AsNoTracking()
                .SingleOrDefaultAsync(u => u.OidcIdentifier == userOidc, cancellationToken);

            if (user == null)
                throw new NotFoundException($"Unable to find user {userOidc}");

            var invitedUser = await db.Users
                .SingleOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

            if (user == null)
                throw new NotFoundException($"Unable to find user {request.UserId}");

            var meeting = await db.Meetings
                .Include(m => m.Creator)
                .Include(m => m.Proposals)
                .Include(m => m.Invitations)
                .ThenInclude(i => i.User)
                .Include(m => m.Invitations)
                .ThenInclude(i => i.Votes)
                .FirstOrDefaultAsync(m => m.Id == request.MeetingId, cancellationToken);

            if (meeting == null)
                throw new NotFoundException($"Unable to find meeting {request.MeetingId}");

            if (meeting.Creator.Id != user.Id)
                throw new ForbiddenException($"User {user.OidcIdentifier} is not eligible to view meeting {request.MeetingId}");

            meeting.Invitations.Add(new Invitation{User = invitedUser});

            var result = db.Meetings.Attach(meeting);
            await db.SaveChangesAsync(cancellationToken);
            return result.Entity;
        }
    }
}
