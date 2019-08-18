using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MeetMe.Application.Commands;
using MeetMe.Domain.Contexts;
using Microsoft.EntityFrameworkCore;

namespace MeetMe.Application.Validators
{
    public class CreateUserValidator : AbstractValidator<CreateUserCommand>
    {
        private readonly MeetingsContext db;

        public CreateUserValidator(MeetingsContext db)
        {
            this.db = db;

            RuleFor(x => x.Name)
                .NotEmpty();
            RuleFor(x => x.Email)
                .EmailAddress();
            RuleFor(x => x.Email)
                .MustAsync(EmailIsAvailable)
                .WithMessage((cmd, email) => $"Email address {email} is already taken");
        }

        private async Task<bool> EmailIsAvailable(string email, CancellationToken token) =>
            await db.Users.AsNoTracking()
                .AllAsync(u => u.Email != email.ToLower(), token);
    }
}