using FluentValidation;
using MeetMe.Application.Commands;

namespace MeetMe.Application.Validators
{
    public class CreateMeetingValidator : AbstractValidator<CreateMeetingCommand>
    {
        public CreateMeetingValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty();
            RuleFor(x => x.Proposals)
                .NotEmpty();
            RuleFor(x => x.InvitedUserIds)
                .NotEmpty();
        }
    }
}
