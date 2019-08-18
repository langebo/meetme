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
    public class RemoveInvitationHandler : IRequestHandler<RemoveInvitationCommand, Meeting>
    {
        private readonly MeetingsContext db;
        private readonly IAuthenticationService auth;

        public RemoveInvitationHandler(MeetingsContext db, IAuthenticationService auth)
        {
            this.db = db;
            this.auth = auth;
        }

        public async Task<Meeting> Handle(RemoveInvitationCommand request, CancellationToken cancellationToken)
        {
            var meeting = await db.Meetings
                .Include(m => m.Creator)
                .Include(m => m.Invitations)
                .ThenInclude(i => i.User)
                .Include(m => m.Proposals)
                .ThenInclude(p => p.Votes)
                .ThenInclude(v => v.User)
                .SingleOrDefaultAsync(m => m.Id == request.MeetingId, cancellationToken);

            if (meeting == null)
                throw new NotFoundException($"Meeting {request.MeetingId} does not exist");
            var oidcId = auth.GetUserIdentifier();
            if (meeting.Creator.OidcIdentifier != oidcId)
                throw new ForbiddenException($"User {oidcId} is not allowed to access meeting {request.MeetingId}");

            var invitation = meeting.Invitations.FirstOrDefault(i => i.Id == request.InvitationId);
            if (invitation == null)
                throw new NotFoundException($"Invitation {request.InvitationId} was ot found on meeting {request.MeetingId}");

            meeting.Invitations.Remove(invitation);
            db.Invitations.Remove(invitation);
            var result = db.Meetings.Attach(meeting);
            await db.SaveChangesAsync(cancellationToken);
            return result.Entity;
        }
    }
}