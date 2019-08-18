using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MeetMe.Application.Commands;
using MeetMe.Domain.Contexts;
using MeetMe.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace MeetMe.Application.Handlers
{
    public class AddInvitationHandler : IRequestHandler<AddInvitationCommand, Meeting>
    {
        private readonly MeetingsContext db;
        public AddInvitationHandler(MeetingsContext db)
        {
            this.db = db;
        }
        public async Task<Meeting> Handle(AddInvitationCommand request, CancellationToken cancellationToken)
        {
            var meeting = await db.Meetings
                .SingleAsync(m => m.Id == request.MeetingId, cancellationToken);
            var user = await db.Users
                .SingleAsync(u => u.Id == request.UserId, cancellationToken);

            meeting.Invitations.Add(new Invitation { User = user });       
            var result = db.Meetings.Attach(meeting);
            await db.SaveChangesAsync(cancellationToken);
            return result.Entity;
        }
    }
}