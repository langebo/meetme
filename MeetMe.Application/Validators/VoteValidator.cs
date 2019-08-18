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
    public class VoteValidator : AbstractValidator<VoteCommand>
    {
        private readonly MeetingsContext db;

        public VoteValidator(MeetingsContext db)
        {
            this.db = db;

            RuleFor(x => x.UserId)
                .MustAsync(UserExists)
                .WithMessage((cmd, id) => $"User {id} does not exist");
            RuleFor(x => x.MeetingId)
                .MustAsync(MeetingExists)
                .WithMessage((cmd, id) => $"Meeting {id} does not exist")
                .DependentRules(() =>
                {
                    RuleFor(x => x.ProposalId)
                        .MustAsync(ProposalExists)
                        .WithMessage((cmd, id) => $"Proposal {id} does not exist");
                    RuleFor(x => x.UserId)
                        .MustAsync(IsInvitedUser)
                        .WithMessage((cmd, id) => $"User {id} is not invited to meeting {cmd.MeetingId}");
                });
        }

        private async Task<bool> MeetingExists(Guid id, CancellationToken ct) =>
            await db.Meetings
                .AsNoTracking()
                .AnyAsync(m => m.Id == id, ct);

        private async Task<bool> ProposalExists(Guid id, CancellationToken token) =>
            await db.Proposals
                .AsNoTracking()
                .AnyAsync(p => p.Id == id, token);

        private async Task<bool> UserExists(Guid id, CancellationToken token) =>
            await db.Users
                .AsNoTracking()
                .AnyAsync(u => u.Id == id, token);

        private async Task<bool> IsInvitedUser(Guid userId, CancellationToken token) =>
            await db.Meetings
                .AsNoTracking()
                .Include(m => m.Invitations)
                .ThenInclude(i => i.User)
                .AnyAsync(m => m.Invitations.Any(i => i.User.Id == userId), token);
    }
}