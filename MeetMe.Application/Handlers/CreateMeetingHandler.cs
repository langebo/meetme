using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MeetMe.Application.Commands;
using MeetMe.Domain.Contexts;
using MeetMe.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace MeetMe.Application.Handlers
{
    public class CreateMeetingHandler : IRequestHandler<CreateMeetingCommand, Meeting>
    {
        private readonly MeetingsContext db;

        public CreateMeetingHandler(MeetingsContext db)
        {
            this.db = db;
        }

        public async Task<Meeting> Handle(CreateMeetingCommand request, CancellationToken cancellationToken)
        {
            var creator = await db.Users.SingleAsync(u => u.Id == request.CreatorId, cancellationToken);
            var invitations = await db.Users
                .Where(u => request.InvitedUserIds.Contains(u.Id))
                .ToListAsync(cancellationToken);

            var meeting = await db.Meetings.AddAsync(new Meeting
            {
                Title = request.Title,
                Creator = creator,
                Proposals = request.Proposals.Select(p => new Proposal {Time = p}).ToList(),
                Invitations = invitations.Select(i => new Invitation { User = i}).ToList()
            }, cancellationToken);

            await db.SaveChangesAsync(cancellationToken);
            return meeting.Entity;
        }
    }
}