using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MeetMe.Application.Commands;
using MeetMe.Domain.Contexts;
using MeetMe.Domain.Models;

namespace MeetMe.Application.Handlers
{
    public class CreateUserHandler : IRequestHandler<CreateUserCommand, User>
    {
        private readonly MeetingsContext db;
        public CreateUserHandler(MeetingsContext db)
        {
            this.db = db;
        }

        public async Task<User> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var result = await db.Users.AddAsync(new User
            {
                Name = request.Name,
                Email = request.Email
            }, cancellationToken);

            await db.SaveChangesAsync(cancellationToken);
            return result.Entity;
        }
    }
}