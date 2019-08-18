using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MeetMe.Application.Queries;
using MeetMe.Domain.Contexts;
using Microsoft.EntityFrameworkCore;

namespace MeetMe.Application.Validators
{
    public class GetMeetingsValidator : AbstractValidator<GetMeetingsQuery>
    {
        private readonly MeetingsContext db;
        public GetMeetingsValidator(MeetingsContext db)
        {
            this.db = db;

            RuleFor(x => x.UserId)
                .MustAsync(UserExists)
                .WithMessage((qry, id) => $"User {id} does not exist");
        }

        private async Task<bool> UserExists(Guid id, CancellationToken token) =>
            await db.Users.AsNoTracking()
                .AnyAsync(u => u.Id == id, token);
    }
}