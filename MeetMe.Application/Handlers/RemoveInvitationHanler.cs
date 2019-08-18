using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MeetMe.Application.Commands;
using MeetMe.Domain.Contexts;
using MeetMe.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace MeetMe.Application.Handlers
{
    public class RemoveInvitationHandler : IRequestHandler<RemoveInvitationCommand, Meeting>
    {
        private readonly MeetingsContext db;

        public RemoveInvitationHandler(MeetingsContext db)
        {
            this.db = db;
        }

        public async Task<Meeting> Handle(RemoveInvitationCommand request, CancellationToken cancellationToken)
        {
            var meeting = await db.Meetings
                .SingleAsync(m => m.Id == request.MeetingId, cancellationToken);
            var invitation = await db.Invitations
                .SingleAsync(i => i.Id == request.InvitationId, cancellationToken);

            meeting.Invitations.Remove(invitation);
            db.Invitations.Remove(invitation);
            var result = db.Meetings.Attach(meeting);
            await db.SaveChangesAsync(cancellationToken);
            return result.Entity;
        }
    }
}