using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MeetMe.Application.Queries;
using MeetMe.Domain.Contexts;
using MeetMe.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace MeetMe.Application.Handlers
{
    public class GetUsersHandler : IRequestHandler<GetUsersQuery, List<User>>
    {
        private readonly MeetingsContext db;
        public GetUsersHandler(MeetingsContext db)
        {
            this.db = db;
        }

        public async Task<List<User>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            return await db.Users
                .AsNoTracking()
                .ToListAsync();
        }
    }
}