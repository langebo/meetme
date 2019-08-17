using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MeetMe.Application.Commands;
using MeetMe.Domain.Contexts;
using Microsoft.EntityFrameworkCore;

namespace MeetMe.Application.Validators
{
    public class VoteOnProposalValidator : AbstractValidator<VoteOnProposalCommand>
    {
        private readonly MeetingsContext db;

        public VoteOnProposalValidator(MeetingsContext db)
        {
            this.db = db;

            RuleFor(x => x.Username)
                .NotEmpty();
            RuleFor(x => x.ProposalId)
                .MustAsync(ProposalExists)
                .WithMessage((cmd, id) => $"Proposal {id} does not exist");
        }

        private async Task<bool> ProposalExists(Guid id, CancellationToken token) => 
            await db.Proposals
                .AsNoTracking()
                .AnyAsync(p => p.Id == id, token);
    }
}