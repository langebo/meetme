using MediatR;
using MeetMe.Domain.Models;

namespace MeetMe.Application.Commands
{
    public class CreateUserCommand : IRequest<User>
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }
}