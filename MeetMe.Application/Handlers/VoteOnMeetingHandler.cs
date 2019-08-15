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
    public class VoteOnMeetingHandler : IRequestHandler<VoteOnMeetingCommand, Meeting>
    {
        private readonly MeetingsContext db;

        public VoteOnMeetingHandler(MeetingsContext db)
        {
            this.db = db;
        }
        public async Task<Meeting> Handle(VoteOnMeetingCommand request, CancellationToken cancellationToken)
        {
            var meeting = await db.Meetings
                .Include(m => m.Proposals)
                .Include(m => m.Votes)
                .ThenInclude(v => v.Proposal)
                .FirstOrDefaultAsync(m => m.Id == request.MeetingId);

            meeting.Votes.AddRange(request.ProposalIds.Select(pId => new Vote 
            {
                Username = request.Username,
                Proposal = meeting.Proposals.Single(p => p.Id == pId)
            }));

            var result = db.Meetings.Attach(meeting);
            await db.SaveChangesAsync(cancellationToken);
            return result.Entity;
        }
    }
}