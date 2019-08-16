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
            RuleFor(x => x.MeetingId)
                .MustAsync(MeetingExists);
            RuleFor(x => x.ProposalId)
                .MustAsync((cmd, id, ct) => ProposalBelongsToMeeting(cmd, ct));
        }

        private async Task<bool> MeetingExists(Guid id, CancellationToken ct) => 
            await db.Meetings
                .AsNoTracking()
                .AnyAsync(m => m.Id == id, ct);

        private async Task<bool> ProposalBelongsToMeeting(VoteOnProposalCommand cmd, CancellationToken ct) => 
            await db.Meetings
                .Include(m => m.Proposals)
                .AnyAsync(m => m.Id == cmd.MeetingId &&
                          m.Proposals.Any(p => p.Id == cmd.ProposalId), ct);
    }
}