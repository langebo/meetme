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
    public class VoteHandler : IRequestHandler<VoteCommand, Proposal>
    {
        private readonly MeetingsContext db;
        private readonly IAuthenticationService auth;

        public VoteHandler(MeetingsContext db, IAuthenticationService auth)
        {
            this.db = db;
            this.auth = auth;
        }

        public async Task<Proposal> Handle(VoteCommand request, CancellationToken cancellationToken)
        {
            var proposal = await db.Proposals
                .Include(p => p.Votes)
                .SingleOrDefaultAsync(p => p.Id == request.ProposalId);
            
            if (proposal == null)
                throw new NotFoundException($"Proposal {request.ProposalId} does not exist");

            var oidcId = auth.GetUserIdentifier();
            var user = await db.Users
                .SingleOrDefaultAsync(u => u.OidcIdentifier == oidcId);
                
            if (user == null)
                throw new NotFoundException($"User {oidcId} does not exist");

            var vote = proposal.Votes
                .FirstOrDefault(v => v.User == user);

            if (vote == null)
                proposal.Votes.Add(new Vote { User = user });
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