using System.Collections.Generic;
using MediatR;
using MeetMe.Domain.Models;

namespace MeetMe.Application.Queries
{
    public class GetUsersQuery : IRequest<List<User>>
    {
        
    }
}