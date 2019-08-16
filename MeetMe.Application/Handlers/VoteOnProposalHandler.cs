using System;
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
    public class VoteOnProposalHandler : IRequestHandler<VoteOnProposalCommand, Meeting>
    {
        private readonly MeetingsContext db;

        public VoteOnProposalHandler(MeetingsContext db)
        {
            this.db = db;
        }

        public async Task<Meeting> Handle(VoteOnProposalCommand request, CancellationToken cancellationToken)
        {
            var meeting = await db.Meetings
                .Include(m => m.Proposals)
                .ThenInclude(p => p.Votes)
                .SingleAsync(m => m.Id == request.MeetingId, cancellationToken);

            var proposal = meeting.Proposals.Single(p => p.Id == request.ProposalId);
            var vote = proposal.Votes.FirstOrDefault(v => v.Username.Equals(request.Username, StringComparison.InvariantCultureIgnoreCase));
            if (vote == null)
                proposal.Votes.Add(new Vote { Username = request.Username });
            else
                proposal.Votes.Remove(vote);

            var result = db.Meetings.Attach(meeting);
            await db.SaveChangesAsync(cancellationToken);
            return result.Entity;
        }
    }
}