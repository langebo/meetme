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
    public class VoteOnProposalHandler : IRequestHandler<VoteOnProposalCommand, Proposal>
    {
        private readonly MeetingsContext db;

        public VoteOnProposalHandler(MeetingsContext db)
        {
            this.db = db;
        }

        public async Task<Proposal> Handle(VoteOnProposalCommand request, CancellationToken cancellationToken)
        {
            var proposal = await db.Proposals
                .Include(p => p.Votes)
                .SingleAsync(p => p.Id == request.ProposalId);

            var vote = proposal.Votes
                .FirstOrDefault(v => v.Username == request.Username);

            if (vote == null)
                proposal.Votes.Add(new Vote { Username = request.Username });
            else
            {
                proposal.Votes.Remove(vote);
                db.Votes.Remove(vote);
            }

            var result = db.Proposals.Attach(proposal);
            await db.SaveChangesAsync(cancellationToken);
            return result.Entity;
        }
    }
}