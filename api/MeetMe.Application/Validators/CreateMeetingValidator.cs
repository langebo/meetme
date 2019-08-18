using System;
using FluentValidation;
using MeetMe.Application.Commands;
using MeetMe.Domain.Contexts;

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
            RuleFor(x => x.Proposals)
                .NotEmpty();
            RuleForEach(x => x.Proposals)
                .GreaterThan(DateTimeOffset.Now)
                .WithMessage("Proposals must not be in the past");
        }
    }
}