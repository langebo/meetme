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
    public class VoteOnMeetingValidator : AbstractValidator<VoteOnMeetingCommand>
    {
        private readonly MeetingsContext db;

        public VoteOnMeetingValidator(MeetingsContext db)
        {
            this.db = db;

            RuleFor(x => x.Username)
                .NotEmpty();

            RuleFor(x => x.MeetingId)
                .MustAsync(MeetingExists)
                .DependentRules(() => 
                {
                    RuleFor(x => x.ProposalIds)
                        .NotEmpty()
                        .MustAsync((cmd, list, ct) => MeetingHasProposals(cmd, ct));
                });
        }

        private async Task<bool> MeetingExists(Guid id, CancellationToken ct) => 
            await db.Meetings
                .AsNoTracking()
                .AnyAsync(m => m.Id == id, ct);

        private async Task<bool> MeetingHasProposals(VoteOnMeetingCommand command, CancellationToken ct)
        {
            var meeting = await db.Meetings
                .Include(m => m.Proposals)
                .Include(m => m.Votes)
                .ThenInclude(v => v.Proposal)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == command.MeetingId, ct);

            return command.ProposalIds.All(id => meeting.Proposals.Select(p => p.Id).Contains(id));
        }
    }
}