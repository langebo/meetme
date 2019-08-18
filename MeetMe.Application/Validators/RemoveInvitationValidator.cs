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
    public class RemoveInvitationValidator : AbstractValidator<RemoveInvitationCommand>
    {
        private readonly MeetingsContext db;

        public RemoveInvitationValidator(MeetingsContext db)
        {
            this.db = db;

            RuleFor(x => x.MeetingId)
                .MustAsync(MeetingExists)
                .WithMessage((cmd, id) => $"Meeting {id} does not exist")
                .DependentRules(() =>
                {
                    RuleFor(x => x.InvitationId)
                        .MustAsync(InvitationExists)
                        .WithMessage((cmd, id) => $"Invitation {id} does not exist")
                        .DependentRules(() =>
                        {
                            RuleFor(x => x.InvitationId)
                                .MustAsync((cmd, id, ct) => InvitationBelongsToMeeting(id, cmd.MeetingId, ct))
                                .WithMessage((cmd, id) => $"Invitation {id} does not belong to meeting {cmd.MeetingId}");
                        });
                });
        }

        private async Task<bool> MeetingExists(Guid id, CancellationToken ct) =>
            await db.Meetings
                .AsNoTracking()
                .AnyAsync(m => m.Id == id, ct);

        private async Task<bool> InvitationExists(Guid id, CancellationToken ct) =>
            await db.Invitations
                .AsNoTracking()
                .AnyAsync(i => i.Id == id, ct);

        private async Task<bool> InvitationBelongsToMeeting(Guid invitationId, Guid meetingId, CancellationToken ct) =>
            await db.Meetings
                .Include(m => m.Invitations)
                .ThenInclude(i => i.User)
                .AsNoTracking()
                .AnyAsync(m => m.Id == meetingId && m.Invitations.Any(i => i.Id == invitationId), ct);
    }
}