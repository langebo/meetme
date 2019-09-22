using System.Linq;
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
    public class EjectHandler : IRequestHandler<EjectCommand, Meeting>
    {
        private readonly MeetingsContext db;
        private readonly IAuthenticationService auth;

        public EjectHandler(MeetingsContext db, IAuthenticationService auth)
        {
            this.db = db;
            this.auth = auth;
        }

        public async Task<Meeting> Handle(EjectCommand request, CancellationToken cancellationToken)
        {
            var userOidc = auth.GetUserIdentifier();
            var user = await db.Users
                .AsNoTracking()
                .SingleOrDefaultAsync(u => u.OidcIdentifier == userOidc, cancellationToken);

            if (user == null)
                throw new NotFoundException($"Unable to find user {userOidc}");

            var uninvitedUser = await db.Users
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

            if (meeting.Invitations.All(i => i.User.Id != request.UserId))
                throw new NotFoundException($"User {request.UserId} is not invited to meeting {request.MeetingId}");

            meeting.Invitations.RemoveAll(i => i.User.Id == request.UserId);

            var result = db.Meetings.Attach(meeting);
            await db.SaveChangesAsync(cancellationToken);
            return result.Entity;
        }
    }
}
