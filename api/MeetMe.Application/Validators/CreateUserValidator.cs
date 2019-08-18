using FluentValidation;
using MeetMe.Application.Commands;

namespace MeetMe.Application.Validators
{
    public class CreateUserValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty();
        }
    }
}