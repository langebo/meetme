using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MeetMe.Application.Commands;
using MeetMe.Domain.Contexts;
using Microsoft.EntityFrameworkCore;

namespace MeetMe.Application.Validators
{
    public class CreateMeetingValidator : AbstractValidator<CreateMeetingCommand>
    {
        private readonly MeetingsContext db;

        public CreateMeetingValidator(MeetingsContext db)
        {
            this.db = db;

            RuleFor(x => x.Title)
                .NotEmpty();
            RuleFor(x => x.CreatorId)
                .MustAsync(CreatorExists)
                .WithMessage((cmd, id) => $"User {id} doesn not exist");
            RuleFor(x => x.Proposals)
                .NotEmpty();
            RuleForEach(x => x.Proposals)
                .GreaterThan(DateTimeOffset.Now)
                .WithMessage("Proposals must not be in the past");
            RuleForEach(x => x.InvitedUserIds)
                .MustAsync(InvitedUserExists)
                .WithMessage((cmd, id) => $"User {id} does not exist");
        }

        private async Task<bool> CreatorExists(Guid id, CancellationToken token) =>
            await db.Users.AsNoTracking()
                .AnyAsync(u => u.Id == id, token);

        private async Task<bool> InvitedUserExists(Guid invitedUserId, CancellationToken token) =>
            await db.Users.AsNoTracking()
                .AnyAsync(u => u.Id == invitedUserId, token);
    }
}