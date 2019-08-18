using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MeetMe.Application.Queries;
using MeetMe.Domain.Contexts;
using MeetMe.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace MeetMe.Application.Handlers
{
    public class GetMeetingsHandler : IRequestHandler<GetMeetingsQuery, List<Meeting>>
    {
        private readonly MeetingsContext db;
        public GetMeetingsHandler(MeetingsContext db)
        {
            this.db = db;
        }
        public async Task<List<Meeting>> Handle(GetMeetingsQuery request, CancellationToken cancellationToken)
        {
            return await db.Meetings
                .AsNoTracking()
                .Include(m => m.Creator)
                .Include(m => m.Proposals)
                .ThenInclude(p => p.Votes)
                .ThenInclude(v => v.User)
                .Include(m => m.Invitations)
                .ThenInclude(i => i.User)
                .Where(m => m.Creator.Id == request.UserId)
                .ToListAsync(cancellationToken);
        }
    }
}