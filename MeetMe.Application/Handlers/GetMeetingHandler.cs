using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MeetMe.Application.Queries;
using MeetMe.Domain.Contexts;
using MeetMe.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace MeetMe.Application.Handlers
{
    public class GetMeetingHandler : IRequestHandler<GetMeetingQuery, Meeting>
    {
        private readonly MeetingsContext db;

        public GetMeetingHandler(MeetingsContext db)
        {
            this.db = db;
        }

        public async Task<Meeting> Handle(GetMeetingQuery request, CancellationToken cancellationToken)
        {
            return await db.Meetings
                .Include(m => m.Creator)
                .Include(m => m.Proposals)
                .ThenInclude(p => p.Votes)
                .ThenInclude(v => v.User)
                .Include(m => m.Invitations)
                .ThenInclude(i => i.User)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == request.Id);
        }
    }
}