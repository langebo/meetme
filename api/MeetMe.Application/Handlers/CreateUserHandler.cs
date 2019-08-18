using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MeetMe.Application.Commands;
using MeetMe.Authentication.Interfaces;
using MeetMe.Domain.Contexts;
using MeetMe.Domain.Models;

namespace MeetMe.Application.Handlers
{
    public class CreateUserHandler : IRequestHandler<CreateUserCommand, User>
    {
        private readonly MeetingsContext db;
        private readonly IAuthenticationService auth;
        public CreateUserHandler(MeetingsContext db, IAuthenticationService auth)
        {
            this.db = db;
            this.auth = auth;
        }

        public async Task<User> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var result = await db.Users.AddAsync(new User
            {
                Name = request.Name,
                Email = auth.GetUserEmail(),
                OidcIdentifier = auth.GetUserEmail()
            }, cancellationToken);

            await db.SaveChangesAsync(cancellationToken);
            return result.Entity;
        }
    }
}