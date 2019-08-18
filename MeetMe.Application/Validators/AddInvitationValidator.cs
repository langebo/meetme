using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MeetMe.Application.Commands;
using MeetMe.Domain.Contexts;
using Microsoft.EntityFrameworkCore;

namespace MeetMe.Application.Validators
{
    public class AddInvitationValidator : AbstractValidator<AddInvitationCommand>
    {
        private readonly MeetingsContext db;

        public AddInvitationValidator(MeetingsContext db)
        {
            this.db = db;

            RuleFor(x => x.UserId)
                .MustAsync(UserExists)
                .WithMessage((cmd, id) => $"User {id} does not exist");
            RuleFor(x => x.MeetingId)
                .MustAsync(MeetingExists)
                .WithMessage((cmd, id) => $"Meeting {id} does not exist");
        }

        private async Task<bool> UserExists(Guid id, CancellationToken token) =>
            await db.Users.AsNoTracking()
                .AnyAsync(u => u.Id == id, token);

        private async Task<bool> MeetingExists(Guid id, CancellationToken ct) =>
            await db.Meetings
                .AsNoTracking()
                .AnyAsync(m => m.Id == id, ct);
    }
}