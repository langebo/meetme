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
    public class VoteHandler : IRequestHandler<VoteCommand, Vote>
    {
        private readonly MeetingsContext db;

        public VoteHandler(MeetingsContext db)
        {
            this.db = db;
        }

        public async Task<Vote> Handle(VoteCommand request, CancellationToken cancellationToken)
        {
            var proposal = await db.Proposals
                .Include(p => p.Votes)
                .SingleAsync(p => p.Id == request.ProposalId);

            var voter = await db.Users
                .SingleAsync(u => u.Id == request.UserId);

            var vote = proposal.Votes
                .FirstOrDefault(v => v.User == voter);

            if (vote == null)
            {
                vote = new Vote { User = voter };
                proposal.Votes.Add(vote);
            }
            else
            {
                proposal.Votes.Remove(vote);
                db.Votes.Remove(vote);
            }

            var result = db.Proposals.Attach(proposal);
            await db.SaveChangesAsync(cancellationToken);
            return vote;
        }
    }
}