using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MeetMe.Application.Queries;
using MeetMe.Domain.Contexts;
using Microsoft.EntityFrameworkCore;

namespace MeetMe.Application.Validators
{
    public class GetMeetingValidator : AbstractValidator<GetMeetingQuery>
    {
        private readonly MeetingsContext db;

        public GetMeetingValidator(MeetingsContext db)
        {
            this.db = db;
            RuleFor(x => x.Id)
                .MustAsync(MeetingExists)
                .WithMessage((cmd, id) => $"Meeting {id} does not exist");
        }

        private async Task<bool> MeetingExists(Guid id, CancellationToken ct) => 
            await db.Meetings
                .AsNoTracking()
                .AnyAsync(m => m.Id == id, ct);
    }
}